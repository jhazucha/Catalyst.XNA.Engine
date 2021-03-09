using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Environment;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System.Collections.Generic;

namespace Catalyst3D.XNA.Testing.Graphics.Landscapes
{
	[TestFixture]
	public class LargeLandscape : CatalystTestFixture
	{
		private Landscape Landscape;
		private BasicCamera Camera;
    private Label Status;
		private Texture2D cursor;
		private Texture2D ground;
		private Vector3 CursorPosition;

		[Test]
		public void Test()
		{
		  ViewportColor = Color.CornflowerBlue;
		  GraphicsManager.PreferredBackBufferWidth = 1024;
		  GraphicsManager.PreferredBackBufferHeight = 768;

      // Create our camera
      Camera = new BasicCamera(this);
			Camera.Position = new Vector3(2100, 2200, 2100);
			Camera.Target = new Vector3(0, -2400, 0);
		  Camera.FarPlane = 4000;
      Components.Add(Camera);

			// Create some input actions to move around the ground with
			var inputActions = new List<InputAction>
			                   	{
			                   		new InputAction("Exit", Buttons.Back, Keys.Escape),
			                   		new InputAction("Forward", Buttons.LeftThumbstickLeft, Keys.W),
			                   		new InputAction("Backward", Buttons.LeftThumbstickLeft, Keys.S),
			                   		new InputAction("Left", Buttons.LeftThumbstickLeft, Keys.A),
			                   		new InputAction("Right", Buttons.LeftThumbstickLeft, Keys.D),
			                   		new InputAction("Up", Buttons.LeftTrigger, Keys.PageUp),
			                   		new InputAction("Down", Buttons.RightTrigger, Keys.PageDown),
			                   		new InputAction("F1", Buttons.LeftShoulder, Keys.F1),
			                   		new InputAction("F2", Buttons.LeftShoulder, Keys.F2)
			                   	};

			// Add these input actions to our Input Handler
      InputHandler.Commands.AddRange(inputActions);

      // Create our Landscape
		  Landscape = new Landscape(this, 128, "hmap2", Camera);
			Landscape.Smoothness = 1.5f;
		  Landscape.SetShaderParameters += OnShaderParams;
			Components.Add(Landscape);

      Status = new Label(this, "Arial");
      Status.Position = new Vector2(100, 10);
      Status.FontColor = Color.Yellow;
      Components.Add(Status);

			Run();
		}

	  protected override void LoadContent()
		{
			base.LoadContent();

			Landscape.CustomEffect = Content.Load<Effect>("EditorLandscapeShader");

	  	cursor = Content.Load<Texture2D>("cursor1");
	  	ground = Content.Load<Texture2D>("hmap2texture");
		}

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

			MouseState state = Mouse.GetState();

      Ray ray = Utilitys.CalculateRayFromCursor(GraphicsDevice, Camera, state.X, state.Y);

      CursorPosition = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);

      Status.Text = "Triangles : " + Landscape.TotalVisibleTriangles;

      if (InputHandler.IsActionPressed("Exit"))
        Exit();

			//if (InputHandler.IsActionPressed("Forward"))
			//{
			//  Camera.MoveForward(4f);
			//}

			//if (InputHandler.IsActionPressed("Backward"))
			//{
			//  Camera.MoveBackward(4f);
			//}

			//if (InputHandler.IsActionPressed("Left"))
			//{
			//  Camera.StrafeLeft(4f);
			//}

			//if (InputHandler.IsActionPressed("Right"))
			//{
			//  Camera.StrafeRight(4f);
			//}

      if (InputHandler.IsActionPressed("Down"))
      {
        Camera.Position = Vector3.Transform(Camera.Position, Matrix.CreateTranslation(0, -2f, 0));
      }

      if (InputHandler.IsActionPressed("Up"))
      {
        Camera.Position = Vector3.Transform(Camera.Position, Matrix.CreateTranslation(0, 2f, 0));
      }

      if (InputHandler.IsActionPressed("F1"))
      {
        Landscape.CustomEffect.CurrentTechnique = Landscape.CustomEffect.Techniques["RenderSolid"];
      }

      if (InputHandler.IsActionPressed("F2"))
        Landscape.CustomEffect.CurrentTechnique = Landscape.CustomEffect.Techniques["RenderWireframe"];
    }

    private void OnShaderParams(GameTime gametime)
    {
      //Landscape.CustomEffect.Parameters["DecalViewProj"].SetValue(Utilitys.CreateDecalViewProjectionMatrix(Camera, 32));

      Landscape.CustomEffect.Parameters["World"].SetValue(Landscape.World);
      Landscape.CustomEffect.Parameters["View"].SetValue(Camera.View);
      Landscape.CustomEffect.Parameters["Projection"].SetValue(Camera.Projection);

      Landscape.CustomEffect.Parameters["AmbientLightColor"].SetValue(Color.White.ToVector4());
      Landscape.CustomEffect.Parameters["AmbientLightIntensity"].SetValue(0.6f);

      Landscape.CustomEffect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(-1, 1, 0));
      Landscape.CustomEffect.Parameters["DiffuseLightColor"].SetValue(Color.Blue.ToVector4());
      Landscape.CustomEffect.Parameters["DiffuseLightIntensity"].SetValue(50.0f);

      Landscape.CustomEffect.Parameters["CursorPosition"].SetValue(CursorPosition);

      if (cursor != null)
        Landscape.CustomEffect.Parameters["TextureCursor"].SetValue(cursor);

      Landscape.CustomEffect.Parameters["CursorColor"].SetValue(Color.Red.ToVector3());
      Landscape.CustomEffect.Parameters["CursorSize"].SetValue(12);
      Landscape.CustomEffect.Parameters["CursorScale"].SetValue(12);

      if (ground != null)
        Landscape.CustomEffect.Parameters["TextureLayer1"].SetValue(ground);

    }

	}
}
