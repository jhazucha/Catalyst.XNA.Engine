using System;
using System.Diagnostics;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.AttributeClasses;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Telerik.WinControls.Docking;

namespace Catalyst3D.Plugin.Camera
{
	[AddinName("Editor Camera")]
	[AddinDescription("Provides a basic editor style camera ..")]
	public class Plugin : Addin
	{
		private IAddinHost Host;
		private EditorCamera Camera;
		private MouseState PreviousMouseState;
		private InputHandler InputHandler;
		private conCameraSettings conSettings;

		public override void Register(IAddinHost host)
		{
			// copy host to local instance
			Host = host;

			// if we didn't find a control then create one
			if(host != null)
			{
				Trace.WriteLine(DateTime.Now + " | Editor Camera Addin Loaded .. ");

				// Create our Camera
				Camera = new EditorCamera(Host.Game);
				Camera.Position = new Vector3(1, 20, 0);
				Camera.Pitch = 0.5f;
				Camera.FarPlane = 3000;

				Host.Camera = Camera;

				// Retreive our Input Handler Service
				InputHandler = (InputHandler) Host.Game.Services.GetService(typeof (InputHandler));

				InputHandler.AddCommand(new InputAction("Left", Buttons.LeftStick, Keys.A));
				InputHandler.AddCommand(new InputAction("Right", Buttons.LeftStick, Keys.D));
				InputHandler.AddCommand(new InputAction("Forward", Buttons.LeftStick, Keys.W));
				InputHandler.AddCommand(new InputAction("Backward", Buttons.LeftStick, Keys.S));
				InputHandler.AddCommand(new InputAction("MoveUpward", Buttons.LeftStick, Keys.PageUp));
				InputHandler.AddCommand(new InputAction("MoveDownward", Buttons.LeftStick, Keys.PageDown));

				conSettings = new conCameraSettings(Camera);
				conSettings.Enabled = false;

				Host.DockManager.SetDock(conSettings, DockPosition.Right);
			}
		}

		public override void Update(GameTime gameTime)
		{
		  // Update our Camera
		  Camera.Update(gameTime);

		  MouseState currentState = Mouse.GetState();

		  // Dont do anything if our mouse is outside our renders viewport
		  if (currentState.X > Host.Game.Window.ClientBounds.Width || currentState.Y > Host.Game.Window.ClientBounds.Height ||
		      currentState.X <= 0 || currentState.Y <= 0)
		    return;

      if (!Camera.IsCameraMovementLocked)
      {
        // Rotate the Camera
        if (currentState.LeftButton == ButtonState.Pressed)
        {
          // get our detla shortcut to smooth out the movement
          float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

          float rotDistance = PreviousMouseState.X - currentState.X;
          rotDistance = rotDistance*delta;

          Camera.Rotate(rotDistance);

          float pitchDistance = PreviousMouseState.Y - currentState.Y;
          pitchDistance = pitchDistance*delta;

          Camera.Pitch -= pitchDistance;
        }

        if (InputHandler.IsActionPressed("LeftCtrl"))
        {
          if (InputHandler.IsActionPressed("Left"))
            Camera.StrafeLeft(5f);

          if (InputHandler.IsActionPressed("Right"))
            Camera.StrafeRight(5f);

          if (InputHandler.IsActionPressed("Forward"))
            Camera.MoveForward(5f);

          if (InputHandler.IsActionPressed("Backward"))
            Camera.MoveBackward(5f);

          if (InputHandler.IsActionPressed("MoveUpward"))
            Camera.MoveUp(5f);

          if (InputHandler.IsActionPressed("MoveDownward"))
            Camera.MoveDown(5f);
        }
      }

		  PreviousMouseState = currentState;
		}

		public override void Load(string path)
		{
			base.Load(path);
		
			string dir = System.IO.Path.GetDirectoryName(path);

			if(System.IO.File.Exists(dir + @"\camera.xml"))
			{
				EditorCamera cam = Serializer.Deserialize<EditorCamera>(dir + @"\camera.xml");

				if(cam != null)
				{
					Camera = new EditorCamera(Host.Game);
					Camera.Position = cam.Position;
					Camera.Target = cam.Target;
					Camera.Aspect = cam.Aspect;
					Camera.FOV = cam.FOV;
					Camera.Yaw = cam.Yaw;
					Camera.Pitch = cam.Pitch;
					Camera.FarPlane = cam.FarPlane;
					Camera.NearPlane = cam.NearPlane;
					Camera.Name = cam.Name;
					Camera.Enabled = cam.Enabled;
				  Camera.IsCameraMovementLocked = cam.IsCameraMovementLocked;

					Host.Camera = Camera;

					conSettings.PropertyGrid.SelectedObject = Camera;
				}
			}
		}

		public override void Save(string path)
		{
			base.Save(path);

			if(Camera != null)
				Serializer.Serialize(path + @"\camera.xml", Camera);
		}

		public override void OnProjectLoaded()
		{
			base.OnProjectLoaded();

			conSettings.Enabled = true;
		}
	}
}
