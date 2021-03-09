using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Camera
{
	public class RTSCamera : BasicCamera
	{
		public Vector3 Offset = new Vector3(64, 1, 64);
		public Vector3 DesiredPosition;

		public RTSCamera(Game game) : base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			// Calculate our offset
			Target = new Vector3(Target.X + Offset.X, Target.Y + Offset.Y, Target.Z + Offset.Z);
		}

		public override void Update(GameTime gameTime)
		{
			// NOTE: D3D9 Uses SRT (Scale * Rotate * Translate) for matrix's

			// get our detla shortcut
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float yaw = DesiredPosition.X*delta;

			// Calculate our slide movement
			Matrix movePosition = Matrix.CreateRotationY(yaw);

			// Update our Position
			Position += Vector3.Transform(DesiredPosition, movePosition);

			DesiredPosition = Vector3.Zero;

			base.Update(gameTime);
		}

		public void MoveLeft(float f)
		{
			DesiredPosition += new Vector3(f, 0, 0);
		}
		public void MoveRight(float f)
		{
			DesiredPosition -= new Vector3(f, 0, 0);
		}
		public void MoveForward(float f)
		{

		}
		public void MoveBackward(float f)
		{

		}
	}
}
