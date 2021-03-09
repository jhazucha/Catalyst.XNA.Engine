using Catalyst3D.XNA.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Physics.Forces
{
	public class Gravity : IForce
	{
		// Gravity acceleration towards the ground
	  public float Force = 450f;

		public void ApplyForce(IPhysicsBody body, float elapsed)
		{
			// Apply some velocity to the object
			body.Velocity += new Vector2(0, Force*elapsed);
		}
	}
}