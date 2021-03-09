using Catalyst3D.XNA.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Physics.Forces
{
	public class Friction : IForce
	{
		public FrictionType FrictionType = FrictionType.Air;

		public void ApplyForce(IPhysicsBody body, float elapsed)
		{
			float friction = 0;

			switch(FrictionType)
			{
				case FrictionType.Air:
					friction = body.Velocity.Y + body.Mass;
					break;

				case FrictionType.Surface:
					friction = body.Velocity.Y + body.Mass;
					break;
			}

			// If the body is moving add some friction to it
			body.Velocity -= new Vector2(0, friction*elapsed);
		}
	}
}
