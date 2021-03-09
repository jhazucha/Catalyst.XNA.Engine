using System;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Camera
{
	public class ChaseCamera : BasicCamera
	{
		private Vector3 _ChasePosition = Vector3.Zero;
		private Vector3 _ChaseDirection = Vector3.Forward;
  	
		private Vector3 _DesiredCamPosition = Vector3.Zero;

		private Vector3 _PositionOffset = new Vector3(0, 8f, 10f);
		private Vector3 _LookAtOffset = new Vector3(0, 1.5f, 0);
  	
		private Vector3 _CamVelocity = Vector3.Zero;

		private const float _Stiffness = 1000;
		private const float _Dampening = 500;
		private const float _Speed = 5;

		public Vector3 ChasePosition
		{
			get { return _ChasePosition; }
			set { _ChasePosition = value; }
		}
		public Vector3 ChaseDirection
		{
			get { return _ChaseDirection; }
			set { _ChaseDirection = value; }
		}
		public Vector3 PositionOffset
		{
			get { return _PositionOffset; }
			set { _PositionOffset = value; }
		}
		public Vector3 LookAtOffset
		{
			get { return _LookAtOffset; }
			set { _LookAtOffset = value; }
		}

		public ChaseCamera(Game game)
			: base(game)
		{
		}

		private void UpdateWorldPositions()
		{
			// Construct a matrix to transform from object space to worldspace
			Matrix transform = Matrix.Identity;
			transform.Forward = ChaseDirection;
			transform.Up = Up;
			transform.Right = Vector3.Cross(Up, ChaseDirection);

			// Calculate desired camera properties in world space
			_DesiredCamPosition = ChasePosition + Vector3.TransformNormal(_PositionOffset, transform);
			Target = ChasePosition + Vector3.TransformNormal(_LookAtOffset, transform);
		}

		public override void Update(GameTime gameTime)
		{
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			UpdateWorldPositions();

			// Calculate some springage
			Vector3 pos = Position - _DesiredCamPosition;

			// Get our forces to graviate our camera too
			Vector3 posForce = -_Stiffness*pos - _Dampening*_CamVelocity;

			// Apply some acceleration to our velocity
			Vector3 posAccel = posForce / _Speed;

			// Calculate our Velocity;
			_CamVelocity += posAccel*elapsed;

			// Basically if the camera's Current Position == the Reference Objects Position we need to push the camera back or lock it
			// Looking straight down so it does not flip signs on us. Making the keyboard input backwards for forward and back movement!

			if (Position != Vector3.Backward)
				ChaseDirection = Vector3.Backward;
			else
				ChaseDirection = Vector3.Forward;


			// Update our Position and Look At
			Position += _CamVelocity*elapsed;

			// Setup our aspect ratio
			Aspect = (float)Game.GraphicsDevice.Viewport.Width / Game.GraphicsDevice.Viewport.Height;

			View = Matrix.CreateLookAt(Position, Target, Up);
			Projection = Matrix.CreatePerspectiveFieldOfView(FOV, Aspect, NearPlane, FarPlane);
		}
	}
}