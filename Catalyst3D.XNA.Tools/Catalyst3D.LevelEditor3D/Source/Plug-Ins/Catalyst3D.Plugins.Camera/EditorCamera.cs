using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Microsoft.Xna.Framework;

namespace Catalyst3D.Plugin.Camera
{
	public class EditorCamera : BasicCamera
	{
		private Vector3 DesiredPosition;

		public float Yaw { get; set; }

		public float Pitch { get; set; }

		public bool EnableMouse = true;

		public float Sensitivity { get; set; }

		public EditorCamera() : base(null) { /* Required for Serialization */ }

		public EditorCamera(Game game) : base(game) { }

		public override void Initialize()
		{
			base.Initialize();

			// init our Target Target
			Target = Vector3.Backward * Position;

			IsCameraMovementLocked = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Clamp this to deal with gimbal locking problems
			Pitch = MathHelper.Clamp(Pitch, MathHelper.ToRadians(-89.9f), MathHelper.ToRadians(89.9f));

			// setup our rotartion matrix
			Matrix rotMatrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);

			// setup our new movement matrix
			Matrix movMatrix = Matrix.CreateRotationY(Yaw);

			// Transform the reference and the rotation matrix
			Vector3 tReference = Vector3.Transform(Vector3.Backward, rotMatrix);

			// Transform our new movement vector against our yaw's matrix
			Position += Vector3.Transform(DesiredPosition, movMatrix);

			// Setup our new Target
			Target = tReference + Position;

			// Reset our Position
			DesiredPosition = Vector3.Zero;
		}

		public void MoveForward(float distance)
		{
			DesiredPosition += new Vector3(0, 0, distance);
		}

		public void MoveBackward(float distance)
		{
			DesiredPosition -= new Vector3(0, 0, distance);
		}

		public void StrafeRight(float distance)
		{
			DesiredPosition -= new Vector3(distance, 0, 0);
		}

		public void StrafeLeft(float distance)
		{
			DesiredPosition += new Vector3(distance, 0, 0);
		}

		public void Rotate(float distance)
		{
			Yaw += distance;
		}

		public void MoveUp(float distance)
		{
			DesiredPosition.Y += distance;
		}

		public void MoveDown(float distance)
		{
			DesiredPosition.Y -= distance;
		}
	}
}