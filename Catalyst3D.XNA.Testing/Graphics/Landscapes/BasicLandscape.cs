using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using Plane = Catalyst3D.XNA.Engine.EntityClasses.Primitives.Plane;
using System.Collections.Generic;

namespace Catalyst3D.XNA.Testing.Graphics.Landscapes
{
	[TestFixture]
	public class BasicLandscape : CatalystTestFixture
	{
		private FPSCamera Camera;
		private Plane groundPlane;

		[Test]
		public void Generate()
		{
			// Create our camera
			Camera = new FPSCamera(this);
			Camera.Position = new Vector3(128, 10, 128);
			Components.Add(Camera);

			// Create our Plane Primitive
			groundPlane = new Plane(this, 256, 256, Camera);
			groundPlane.FillMode = FillMode.Solid;
			Components.Add(groundPlane);

			// Create some input actions to move around the ground with
			var inputActions = new List<InputAction>
			                   	{
			                   		new InputAction("Left", Buttons.LeftThumbstickLeft, Keys.A),
			                   		new InputAction("Right", Buttons.LeftThumbstickLeft, Keys.D),
			                   		new InputAction("Forward", Buttons.LeftThumbstickLeft, Keys.W),
			                   		new InputAction("Backward", Buttons.LeftThumbstickLeft, Keys.S),
			                   	};

			// Add these input actions to our Input Handler
			InputHandler.Commands.AddRange(inputActions);

			Run();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			groundPlane.Texture = Content.Load<Texture2D>("desert1");
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(InputHandler.IsActionPressed("Forward"))
			{
				Camera.MoveForward(2f);
			}

			if(InputHandler.IsActionPressed("Backward"))
			{
				Camera.MoveBackward(2f);
			}

			if(InputHandler.IsActionPressed("Left"))
			{
				Camera.StrafeLeft(2f);
			}

			if(InputHandler.IsActionPressed("Right"))
			{
				Camera.StrafeRight(2f);
			}
		}
	}
}