using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Physics.Interfaces
{
  public interface IPhysicsBody
  {
    float Mass { get; set; }

		bool PhysicsEnabled { get; set; }

		Vector2 Velocity { get; set; }
    Vector2 MaxVelocity { get; set; }

    Compound Compound { get; set; }

		BoundingBox BoundingBox { get; set; }

		PhysicsManager PhysicsManager { get; set; }

		Vector2 Position { get; set; }

		Vector2 Center { get; set; }
  }
}