using System.Collections.Generic;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Physics
{
  public class PhysicsManager : UpdatableObject
	{
		public readonly List<IPhysicsBody> Bodies = new List<IPhysicsBody>();
		public List<IForce> ForceObjects = new List<IForce>();

		public delegate void Collision();

		public Collision CheckCollision;

		public PhysicsManager(Game game)
			: base(game)
		{
			// Add this as a game service
			game.Services.AddService(typeof (PhysicsManager), this);
			
			// Add this to the component collection for rendering
			game.Components.Add(this);
		}

  	public void AddBody(IPhysicsBody b)
		{
			if(b.PhysicsManager == null)
				b.PhysicsManager = this;

			Bodies.Add(b);
		}
		public void RemoveBody(IPhysicsBody b)
		{
			if(b.PhysicsManager == null)
				b.PhysicsManager = this;

			Bodies.Remove(b);
		}

		public void AddForce(IForce force)
		{
			ForceObjects.Add(force);
		}
		public void RemoveForce(IForce force)
		{
			ForceObjects.Remove(force);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// calculate dt, the change in the since the last frame. the particle updates will use this value.
			float dt = (float) gameTime.ElapsedGameTime.TotalSeconds*0.33f;

			// apply force objects
			foreach(IForce force in ForceObjects)
			{
				// apply force to bodies
				foreach(IPhysicsBody b in Bodies)
				{
					if (b.PhysicsEnabled)
						force.ApplyForce(b, dt);
				}
			}
		}
	}
}