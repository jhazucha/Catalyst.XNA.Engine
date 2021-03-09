using System.ComponentModel;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Camera
{
	public class BasicCamera : UpdatableObject
	{

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Projection Matrix")]
		[Category("Matrices")]
#endif
		public Matrix Projection { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("View Matrix")]
		[Category("Matrices")]
#endif
		public Matrix View { get; set; }
	
#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Look At Vector")]
		[Category("Orientation")]
#endif
		public Vector3 Target { get; set; }
	
#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Up Vector")]
		[Category("Orientation")]
#endif
		public Vector3 Up { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Right Vector")]
		[Category("Orientation")]
#endif
		public Vector3 Right { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Position Vector")]
		[Category("Orientation")]
#endif
		public new Vector3 Position { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[ReadOnly(true)]
		[Description("Aspect Ratio")]
		[Category("Settings")]
#endif
		public float Aspect { get; set; }
	
#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[ReadOnly(true)]
		[Description("Field of View")]
		[Category("Settings")]
#endif
		public float FOV { get; set; }
	
#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Near Plane")]
		[Category("Settings")]
#endif
		public float NearPlane { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Far Plane")]
		[Category("Settings")]
#endif
		public float FarPlane { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Camera Name")]
		[Category("Content")]
#endif
			public new string Name { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)] [Description("Camera Frustum")] [Category("Information")]
#endif
			public BoundingFrustum BoundingFrustum;

	  public bool IsCameraMovementLocked;

	  public BasicCamera(Game game) : base(game)
		{
			Right = Vector3.Right;
			Up = Vector3.Up;
			Position = new Vector3(0, 0, 1);

			FOV = MathHelper.ToRadians(45.0f);
			NearPlane = 0.05f;
			FarPlane = 1000f;
			
			BoundingFrustum = new BoundingFrustum(Matrix.Identity);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Setup our aspect ratio
			Aspect = (float) Game.GraphicsDevice.Viewport.Width/Game.GraphicsDevice.Viewport.Height;

			// Setup our Projection Matrix
			Projection = Matrix.CreatePerspectiveFieldOfView(FOV, Aspect, NearPlane, FarPlane);

			// Setup our View Matrix and Update it incase we moved the camera
			View = Matrix.CreateLookAt(Position, Target, Up);

			// Update our Bounding Frustum
			BoundingFrustum = new BoundingFrustum(View*Projection);
		}
	}
}