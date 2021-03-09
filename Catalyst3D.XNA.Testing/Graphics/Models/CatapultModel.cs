using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using Model = Catalyst3D.XNA.Engine.EntityClasses.Models.Model;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
  public class CatapultModel : CatalystTestFixture
  {
    public Model Catapult;
    public Model Ball;
    public BasicCamera Camera;
    
    public bool IsFiring;
    public bool CanFire;
    
    public float angle;

    [Test]
    public void Test()
    {
      // Create our Camera
      Camera = new BasicCamera(this);
      Camera.Position = new Vector3(0, 3,25);
      Camera.Target = new Vector3(0,5,0);
      Components.Add(Camera);

      Catapult = new Model(this, "Catapult1_rev2", Camera);
      Catapult.DiffuseColor = new Color(50,50,50,255);
      Components.Add(Catapult);

      InputHandler.AddCommand(new InputAction("Left", Buttons.LeftStick, Keys.Left));
      InputHandler.AddCommand(new InputAction("Right", Buttons.RightStick, Keys.Right));
      InputHandler.AddCommand(new InputAction("Fire", Buttons.A, Keys.Space));

      Run();
    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      float wheelSpeed = 0;

      ModelBone arm;

      if(Catapult.Content.Bones.TryGetValue("jArm", out arm))
      {
        if (InputHandler.IsActionPressed("Fire"))
        {
          if (!IsFiring && CanFire)
          {
            IsFiring = true;
            CanFire = false;
          }
        }

        if(InputHandler.IsActionPressed("Left"))
        {
          Catapult.Position -= new Vector3(0.08f, 0, 0);
          wheelSpeed = (float) gameTime.TotalGameTime.TotalSeconds*200f;
        }

        if (InputHandler.IsActionPressed("Right"))
        {
          Catapult.Position += new Vector3(0.08f, 0, 0);
          wheelSpeed = -(float) gameTime.TotalGameTime.TotalSeconds*200f;
        }

        if (IsFiring)
          angle += 8.8f;
        else
          angle -= 0.4f;

        if (angle >= 90)
        {
          IsFiring = false;
          angle = 90;
        }
        if (angle <= 0)
        {
          CanFire = true;

          angle = 0;
        }

        arm.Transform = Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(arm.Transform.Translation);
      }

      ModelBone frontAxels;
      if(Catapult.Content.Bones.TryGetValue("jFrontAxel", out frontAxels))
      {
        frontAxels.Transform = Matrix.CreateRotationZ(MathHelper.ToRadians(wheelSpeed)) * Matrix.CreateTranslation(frontAxels.Transform.Translation);
      }

      ModelBone rearAxels;
      if (Catapult.Content.Bones.TryGetValue("jRearAxel", out rearAxels))
      {
        rearAxels.Transform = Matrix.CreateRotationZ(MathHelper.ToRadians(wheelSpeed)) * Matrix.CreateTranslation(rearAxels.Transform.Translation);
      }
    }
  }
}
