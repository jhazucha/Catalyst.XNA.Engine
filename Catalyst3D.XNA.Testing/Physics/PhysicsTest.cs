using Catalyst3D.XNA.Engine.UtilityClasses;
using Catalyst3D.XNA.Physics;
using Catalyst3D.XNA.Physics.Forces;
using Catalyst3D.XNA.Testing.EntityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Physics
{
	[TestFixture]
	public class PhysicsTest : CatalystTestFixture
	{
		public Ground Ground;

		public PhysicsManager PhysicsManager;

		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			// Create our Phsyics Manager
			PhysicsManager = new PhysicsManager(this);
			Components.Add(PhysicsManager);

			// Add our Forces to our physics manager
			PhysicsManager.AddForce(new Gravity());
			PhysicsManager.AddForce(new Friction());

			#region Box Generation

			for(int i = 0; i < 12; i++)
			{
				// Create our Box
				Box box = new Box(this);
				box.Position = new Vector2(60*i + 35f, 300);
				Components.Add(box);

				// Add it to our physics manager
				PhysicsManager.AddBody(box);
			}

			for(int i = 0; i < 11; i++)
			{
				// Create our Box
				Box box = new Box(this);
				box.Position = new Vector2(60*i + 70f, 100);
				Components.Add(box);

				// Add it to our physics manager
				PhysicsManager.AddBody(box);
			}

			for(int i = 0; i < 10; i++)
			{
				// Create our Box
				Box box = new Box(this);
				box.Position = new Vector2(60 * i + 105f, 20);
				Components.Add(box);

				// Add it to our physics manager
				PhysicsManager.AddBody(box);
			}

			for(int i = 0; i < 9; i++)
			{
				// Create our Box
				Box box = new Box(this);
				box.Position = new Vector2(60 * i + 140f, -40);
				Components.Add(box);

				// Add it to our physics manager
				PhysicsManager.AddBody(box);
			}

#endregion

			// Create our Ground
			Ground = new Ground(this);
			Components.Add(Ground);

			// Add it to our physics manager
			PhysicsManager.AddBody(Ground);

			Run();
		}
	}
}