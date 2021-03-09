using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Catalyst3D.XNA.Engine.EntityClasses.Camera
{
	public class FPSCamera : BasicCamera
	{
		private Vector3 NewPosition = Vector3.Zero;

#if !XBOX360
		private MouseState MouseCache;
#endif

		private GamePadState GamePadCache;
		private float Yaw;
		private float Pitch;
		public bool EnableMouse = true;

		public float Sensitivity { get; set; }
		
		public FPSCamera(Game game)
			: base(game)
		{
			Sensitivity = 0.06f;
		}

		public override void Initialize()
		{
			base.Initialize();

#if !XBOX360

			// If mouse is visible hide it
			if (Game.IsMouseVisible)
				Game.IsMouseVisible = false;

			int centerX = Game.GraphicsDevice.Viewport.Width / 2;
			int centerY = Game.GraphicsDevice.Viewport.Height / 2;

			Mouse.SetPosition(centerX, centerY);

			// init our mouse cache
			MouseCache = Mouse.GetState();
#endif

			GamePadCache = GamePad.GetState(PlayerIndex.One);

			// init our Target Target
			Target = Vector3.Backward * Position;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

      if (!Game.IsActive)
        return;

			// get our detla shortcut
			float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

#if !XBOX360
			// Mouse state can stay in here thats bland
			MouseState mState = Mouse.GetState();
			float lookX = mState.X - MouseCache.X;
			float lookY = mState.Y - MouseCache.Y;
      
      Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/2);
#endif

			GamePadState gState = GamePad.GetState(PlayerIndex.One);
			if (gState.IsConnected)
			{
				lookX = gState.ThumbSticks.Right.X*15 - GamePadCache.ThumbSticks.Right.X;
				lookY = (-gState.ThumbSticks.Right.Y*15) - -GamePadCache.ThumbSticks.Right.Y;
			}

			// Setup our Pitch and Yaw values
			Pitch += (lookY*Sensitivity)*delta;
			Yaw -= (lookX*Sensitivity)*delta;

			// Clamp this to deal with gimbal locking problems
			Pitch = MathHelper.Clamp(Pitch, MathHelper.ToRadians(-89.9f), MathHelper.ToRadians(89.9f));

			// setup our rotartion matrix
			Matrix rotMatrix = Matrix.CreateRotationX(Pitch)*Matrix.CreateRotationY(Yaw);

			// setup our new movement matrix
			Matrix movMatrix = Matrix.CreateRotationY(Yaw);

			// Transform the reference and the rotation matrix
			Vector3 tReference = Vector3.Transform(Vector3.Backward, rotMatrix);

			// Transform our new movement vector against our yaw's matrix
			Position += Vector3.Transform(NewPosition, movMatrix);

			// Setup our new Target
			Target = tReference + Position;

			// Setup our new View matrix for our camera
			View = Matrix.CreateLookAt(Position, Target, Vector3.Up);

			// Reset our Position
			NewPosition = Vector3.Zero;
		}

		public void MoveForward(float distance)
		{
			Vector3 dir = Position - Target;
			dir.Z += distance;
			dir.Y = 0; // Clamp Y for now

			NewPosition += dir;
		}
		public void MoveBackward(float distance)
		{
			Vector3 dir = Position - Target;
			dir.Z -= distance;
			dir.Y = 0; // Clamp Y for now

			NewPosition += dir;
		}

		public void StrafeRight(float distance)
		{
			Vector3 dir = Position - Target;
			dir.X -= distance;
			dir.Y = 0;

			NewPosition += dir;
		}
		public void StrafeLeft(float distance)
		{
			Vector3 dir = Position - Target;
			dir.X += distance;
			dir.Y = 0;

			NewPosition += dir;
		}

		public void MoveUp(float distance)
		{
			NewPosition.Y += distance;
		}
		public void MoveDown(float distance)
		{
			NewPosition.Y -= distance;
		}
	}
}