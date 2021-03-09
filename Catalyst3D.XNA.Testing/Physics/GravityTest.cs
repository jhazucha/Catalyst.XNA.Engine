using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Catalyst3D.XNA.Physics;
using Catalyst3D.XNA.Physics.Forces;
using Catalyst3D.XNA.Testing.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Physics
{
	[TestFixture]
	public class GravityTest : CatalystTestFixture
	{
		public Box Box;
		public Ground Ground;

		public PhysicsManager PhysicsManager;

		[Test]
		public void Test()
		{
		  IsMouseVisible = true;

			// Create our Box
			Box = new Box(this);
			Components.Add(Box);

			// Create our Ground
			Ground = new Ground(this);
			Components.Add(Ground);

			// Create our Phsyics Manager
			PhysicsManager = new PhysicsManager(this);
			Components.Add(PhysicsManager);

			// Add our Bodies to our phsyics manager
			PhysicsManager.AddBody(Box);
			PhysicsManager.AddBody(Ground);

			// Add our Forces to our physics manager
			PhysicsManager.AddForce(new GravityForce());

			Run();
		}

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      MouseState state = Mouse.GetState();

      if (state.LeftButton == ButtonState.Pressed)
      {
        BoundingBox bb = new BoundingBox(new Vector3(state.X - 5, state.Y - 5, -1),
                                         new Vector3(state.X + 5, state.Y + 5, 1));

        // Check if the mouse bounding box hits the box
        if (bb.Intersects(Box.BoundingBox))
        {
          Box.Velocity += new Vector2(0, -50);
        }
      }
    }
	}
}