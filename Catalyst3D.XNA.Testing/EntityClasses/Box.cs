using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Physics;
using Catalyst3D.XNA.Physics.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Testing.EntityClasses
{
  public class Box : Sprite, IPhysicsBody
  {
    public float Mass { get; set; }
    public bool PhysicsEnabled { get; set; }
    public Vector2 MaxVelocity { get; set; }
    public Compound Compound { get; set; }
    public PhysicsManager PhysicsManager { get; set; }
		public float Width { get; set; }

    public Box(Game game) : base(game) { }

    public override void Initialize()
    {
      // Define the Mass of this Object
    	Mass = 25.0f;

      // Define what this objects compound consists of
      Compound = Compound.Solid;

      PhysicsEnabled = true;

      base.Initialize();
    }

    public override void LoadContent()
    {
      // Generate a 50x50px texture on the fly
      //Texture = Utilitys.GenerateTexture(GraphicsDevice, Color.Blue, 50, 50);
			Texture = Game.Content.Load<Texture2D>("WoodBoxTexture");
    	Scale = new Vector2(0.3f, 0.3f);

      base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
			// Check to see if anything is colliding with our Ground and reflect it away
			foreach(IPhysicsBody b in PhysicsManager.Bodies)
			{
				if(b == this)
					continue;

				// If anything hits teh ground reflect it away
				if(b.BoundingBox.Intersects(BoundingBox))
				{
					// Calculate which direction/angle its coming in on
					float angleX = MathHelper.Clamp(Velocity.X, 0, 1);
					float angleY = MathHelper.Clamp(Velocity.Y, 0, 1);

					Velocity = Vector2.Reflect(Velocity, new Vector2(angleX, angleY));
				}
			}

      base.Update(gameTime);
    }
	}
}