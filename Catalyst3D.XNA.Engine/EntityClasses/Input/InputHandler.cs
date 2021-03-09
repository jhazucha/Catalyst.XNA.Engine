using System;
using System.Collections.Generic;
using System.Linq;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#if WINDOWS_PHONE
using Microsoft.Devices.Sensors;
#endif

namespace Catalyst3D.XNA.Engine.EntityClasses.Input
{
	public class InputHandler : UpdatableObject
	{
		private KeyboardState keyState;
		private GamePadState padState;

#if WINDOWS_PHONE
		public Accelerometer Accelerometer;
#endif

		public Vector3 RawAcceleration;
		public Vector3 SmoothedAcceleration;
		public DateTimeOffset LastSensorTime;
		public const float Smoothing = .1f;

		private KeyboardState prevKeyState;
		private GamePadState prevPadState;

		// Default this to the First Player
		private readonly PlayerIndex playerIndex = PlayerIndex.One;

		public List<InputAction> Commands = new List<InputAction>();

		public Vector2 LeftThumbStickValue;
		public Vector2 RightThumbStickValue;

		private float VibrationDurationLeft;
		private float VibrationDurationRight;

		public float PollDelay;
		public float PollElapsed;

		public readonly TimeSpan BufferFlush = TimeSpan.FromMilliseconds(500);
		public readonly TimeSpan MergeTime = TimeSpan.FromMilliseconds(100);

		public TimeSpan LastInputTime { get; private set; }

		public List<InputAction> Buffer = new List<InputAction>();

		// Is this going away I dont like the naming convention by MS on this ..
		public readonly List<GestureSample> Gestures = new List<GestureSample>();

		public InputHandler(Game game)
			: base(game)
		{
			new InputHandler(game, PlayerIndex.One, new List<InputAction>());
		}

		public InputHandler(Game game, PlayerIndex player)
			: base(game)
		{
			new InputHandler(game, player, new List<InputAction>());
		}

		public InputHandler(Game game, PlayerIndex player, List<InputAction> actions)
			: base(game)
		{
			TouchPanel.EnabledGestures = GestureType.None;

			// Grab our player index
			playerIndex = player;
			Commands = actions;

			// Load this to our games component Collection
			game.Components.Add(this);

			// Register this as a game service as well
			game.Services.AddService(typeof(InputHandler), this);
		}

#if WINDOWS_PHONE
		public override void Initialize()
		{
			base.Initialize();

			Accelerometer = new Accelerometer();
			Accelerometer.ReadingChanged += OnAccelData;
			Accelerometer.Start();
		}

		private void OnAccelData(object sender, AccelerometerReadingEventArgs args)
		{
			// Raw un-smoothed accel from sensor
			RawAcceleration = new Vector3((float)args.X, (float)args.Y, (float)args.Z);

			// Empirically determined error in accelerometer readings. It seems to be off by a constant offset.
			Vector3 SensorError = new Vector3(-0.09f, -0.02f, 0.04f);

			// reduce the data by an allocated error factor
			RawAcceleration -= SensorError;

			float dt = (float)args.Timestamp.Subtract(LastSensorTime).TotalSeconds;
			LastSensorTime = args.Timestamp;
			dt = MathHelper.Clamp(dt, 0.0f, 1.0f);

			float p = (float)Math.Exp(-dt / Smoothing);
			SmoothedAcceleration = Vector3.Lerp(RawAcceleration, SmoothedAcceleration, p);
		}
#endif

		public void AddCommand(InputAction action)
		{
			bool canAdd = true;
      foreach (var c in Commands.Where(c => c.Name == action.Name))
        canAdd = false;

			if (canAdd)
				Commands.Add(action);
		}

		public override void Update(GameTime gameTime)
		{
			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 30;

			// Get our Keyboard and GamePad States
			keyState = Keyboard.GetState();
			padState = GamePad.GetState(playerIndex);

			// Store these incase we need em
			LeftThumbStickValue = new Vector2(padState.ThumbSticks.Left.X, padState.ThumbSticks.Left.Y);
			RightThumbStickValue = new Vector2(padState.ThumbSticks.Right.X, padState.ThumbSticks.Right.Y);

			// Expire old input.
			TimeSpan time = gameTime.TotalGameTime;
			TimeSpan timeSinceLast = time - LastInputTime;

			// Flush the buffer if its passed our elapsed buffer flush time
			if (timeSinceLast > BufferFlush)
				Buffer.Clear();

			// Check for keyboard input
			foreach (InputAction act in Commands)
			{
				if (keyState.IsKeyDown(act.Key))
				{
					// Key was up previously so its a new key press
					act.IsPressed = true;
					act.PressTime += 1;
				}
				else if (keyState.IsKeyUp(act.Key))
				{
					// was not held down
					act.IsPressed = false;
					act.PressTime = 0;
				}

#if XBOX
				if (padState.IsConnected)
				{
					if (padState.IsButtonDown(act.Button))
					{
						act.IsPressed = true;
						act.PressTime += 1f;
					}
					else if (padState.IsButtonUp(act.Button))
					{
						act.IsPressed = false;
						act.PressTime = 0;
					}
				}
#endif

#if WINDOWS_PHONE
				if (padState.IsButtonDown(act.Button))
				{
					act.IsPressed = true;
					act.PressTime += 1f;
				}
				else if (padState.IsButtonUp(act.Button))
				{
					act.IsPressed = false;
					act.PressTime = 0;
				}
#endif
			}

			// Handle our Controller Vibration
			VibrationDurationLeft -= elapsedTime;
			VibrationDurationRight -= elapsedTime;

			VibrationDurationLeft = MathHelper.Max(0, VibrationDurationLeft);
			VibrationDurationRight = MathHelper.Max(0, VibrationDurationRight);

			GamePad.SetVibration(playerIndex, VibrationDurationLeft, VibrationDurationRight);

			if (TouchPanel.GetCapabilities().IsConnected)
			{
				Gestures.Clear();

				while (TouchPanel.IsGestureAvailable)
				{
					Gestures.Add(TouchPanel.ReadGesture());
				}
			}

			// Update our previous states
			prevKeyState = keyState;
			prevPadState = padState;

			PollElapsed += elapsedTime;

			base.Update(gameTime);
		}

		public bool IsActionPressed(string name)
		{
			if (PollElapsed >= PollDelay)
			{
				if (Commands.Where(a => a.Name == name).Any(a => a.IsPressed))
				{
					Commands.Where(a => a.Name == name).FirstOrDefault().IsPressed = false;
					PollElapsed = 0;

					return true;
				}
			}
			return false;
		}

#if !WINDOWS_PHONE

		public bool IsHeldDown(string name)
		{
			if(GetPressedTime(name) >= 1f)
				return true;

			return false;
		}

		public float GetPressedTime(string name)
		{
			if (Commands.Count == 0)
				return 0;

			var action = Commands.Find(a => a.Name == name);
			return action != null ? Commands.Find(a => a.Name == name).PressTime : 0;
		}

#endif

		public void Vibrate(PlayerIndex index, float leftMotor, float rightMotor, float duration)
		{
			if (padState.IsConnected)
			{
				GamePad.SetVibration(index, leftMotor, rightMotor);

				VibrationDurationLeft = rightMotor;
				VibrationDurationRight = leftMotor;
			}
		}

#if WINDOWS_PHONE
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Accelerometer.Stop();

			base.Dispose(disposing);
		}
#endif

		public bool IsGestureActive(GestureType gestureType)
		{
			return (from g in Gestures where g.GestureType == gestureType select g).Count() > 0;
		}

		public bool IsGestureActive(out List<GestureSample> samples)
		{
			samples = (from g in Gestures select g).ToList();

			if (samples.Count() > 0)
				return true;

			return false;
		}
	}
}