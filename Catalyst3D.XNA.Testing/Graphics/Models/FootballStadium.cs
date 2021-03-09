using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System.Collections.Generic;
using Model = Catalyst3D.XNA.Engine.EntityClasses.Models.Model;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
  [TestFixture]
  public class FootballStadium : CatalystTestFixture
  {
    public Model Stadium;
    public FPSCamera Camera;

    [Test]
    public void Test()
    {
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

      Camera = new FPSCamera(this);
      Camera.Position = new Vector3(-1000, 200,1000);
      Camera.FarPlane = 12000f;
      Components.Add(Camera);

      Stadium = new Model(this, "Stadium1", Camera);
      
      Components.Add(Stadium);

      Run();
    }

    protected override void Initialize()
    {
      base.Initialize();
      


    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (InputHandler.IsActionPressed("Forward"))
      {
        Camera.MoveForward(10f);
      }

      if (InputHandler.IsActionPressed("Backward"))
      {
        Camera.MoveBackward(10f);
      }

      if (InputHandler.IsActionPressed("Left"))
      {
        Camera.StrafeLeft(10f);
      }

      if (InputHandler.IsActionPressed("Right"))
      {
        Camera.StrafeRight(10f);
      }
    }
  }
}
