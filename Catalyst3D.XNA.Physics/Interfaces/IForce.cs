namespace Catalyst3D.XNA.Physics.Interfaces
{
	public interface IForce
	{
		void ApplyForce(IPhysicsBody body, float elapsedTime);
	}
}
