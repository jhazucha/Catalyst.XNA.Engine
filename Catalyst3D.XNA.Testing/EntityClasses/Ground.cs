using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Catalyst3D.XNA.Physics;
using Catalyst3D.XNA.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Testing.EntityClasses
{
  public class Ground : Sprite, IPhysicsBody
  {
    public float Mass { get; set; }
    public Vector2 MaxVelocity { get; set; }
    public Compound Compound { get; set; }
    public PhysicsManager PhysicsManager { get; set; }
    public bool PhysicsEnabled { get; set; }

    public Ground(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      // Position it near the bottom of the screen
      Position = new Vector2(0, 465);

      // Mass of Object
      Mass = 2000f;

      // Compound the object is made of
      Compound = Compound.Solid;

      // No physics on the ground itself
      PhysicsEnabled = false;

      base.Initialize();
    }

    public override void LoadContent()
    {
      base.LoadContent();

      Texture = Utilitys.GenerateTexture(GraphicsDevice, Color.ForestGreen, 800, 100);
    }
  }
}