using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Primitives
{
	public class WaterPlane : VisualObject
	{
		public event CustomShader SetShaderParameters;

		public VertexPositionTexture[] _verts;
		public Matrix ReflectionViewMatrix;
		public Vector3 reflectionCameraPosition;
		public Vector3 reflectionCameraLookAt;

		public Effect CustomEffect;

		public float WaterHeight;

		// Water bump map
		public Texture2D WaterBumpMap;

		// Refraction Plane Stuff
		public RenderTarget2D rtRefraction;
		public Texture2D refractionMap;

		// Reflection Plane Stuff
		public RenderTarget2D rtReflection;
		public Texture2D reflectionMap;

		public int _width;
		public int _height;
		public int _waterHeight;

		public Vector3 WindDirection = new Vector3(0, 0, 1);
		public float WindForce = 0.002f;

		public delegate void RenderRefractionHandler(GameTime time);
		public delegate void RenderReflectionHandler(Matrix view, Vector3 reflectionCamPosition, GameTime time);

		public Viewport RenderingViewport;

		public WaterPlane(Game game, int w, int h, BasicCamera camera)
			: base(game)
		{
			Camera = camera;
			_width = w;
			_height = h;
		}

    public override void LoadContent()
		{
			base.LoadContent();

			int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
			SurfaceFormat format = Game.GraphicsDevice.DisplayMode.Format;

			// Setup our RenderTargets
      rtRefraction = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);
      rtReflection = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);

			SetUpWaterVertices();
		}

		private void SetUpWaterVertices()
		{
			_verts = new VertexPositionTexture[6];

			_verts[0] = new VertexPositionTexture(new Vector3(0, _waterHeight, 0), new Vector2(0, 1));
			_verts[1] = new VertexPositionTexture(new Vector3(_width, _waterHeight, _height), new Vector2(1, 0));
			_verts[2] = new VertexPositionTexture(new Vector3(0, _waterHeight, _height), new Vector2(0, 0));

			_verts[3] = new VertexPositionTexture(new Vector3(0, _waterHeight, 0), new Vector2(0, 1));
			_verts[4] = new VertexPositionTexture(new Vector3(_width, _waterHeight, 0), new Vector2(1, 1));
			_verts[5] = new VertexPositionTexture(new Vector3(_width, _waterHeight, _height), new Vector2(1, 0));
		}

		public void DrawRefraction(GameTime gameTime, RenderRefractionHandler render)
		{
			// Create our Refraction Plane
			Microsoft.Xna.Framework.Plane refractionPlane = Utilitys.CreatePlane(WaterHeight + 20.0f, Vector3.Down, Camera.View, Camera.Projection, false);

			// Setup the graphics Device
			GraphicsDevice.SetRenderTarget(rtRefraction); // <- render to our RT

			// Clear the back buffer
			GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

			render.Invoke(gameTime);

			// Remove our render target from our device
		  GraphicsDevice.SetRenderTarget(null);

			// Snag our Texture from our RenderTarget
		  refractionMap = rtRefraction;

		  // For diag just save it to disk
		  //refractionMap.Save(AppDomain.CurrentDomain.BaseDirectory + @"\refractionMap.jpg", ImageFileFormat.Jpg);
		}

		public void DrawReflection(GameTime gameTime, RenderReflectionHandler render)
		{
			Microsoft.Xna.Framework.Plane reflectionPlane = Utilitys.CreatePlane(WaterHeight - 0.5f, Vector3.Down, ReflectionViewMatrix, Camera.Projection, true);

			// Setup the Graphics Device
		  GraphicsDevice.SetRenderTarget(rtReflection);

			// Clear the back buffer
			GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

			// Draw anything that should reflect onto the water
			render.Invoke(ReflectionViewMatrix, Camera.Position, gameTime);

			// Remove our render target from our device
			GraphicsDevice.SetRenderTarget(null);

			// Snag our Texture from our RenderTarget
		  reflectionMap = rtReflection;

		  // For diag just save it to disk
		  //reflectionMap.Save(AppDomain.CurrentDomain.BaseDirectory + @"\reflectionMap.jpg", ImageFileFormat.Jpg);
		}

		public override void Update(GameTime gameTime)
		{
			// Get our Camera Position for our Reflection Render Target
			Vector3 reflCameraPosition = Camera.Position;
			reflCameraPosition.Y = -Camera.Position.Y + WaterHeight*2;

			// Get our Camera Look At Target for our Reflection Render Target
			Vector3 reflTargetPos = Camera.Target;
			reflTargetPos.Y = -Camera.Target.Y + WaterHeight*2;

			// Now get the Inverse Up vector which should normally be Vector3.Down
			Vector3 invUpVector = Vector3.Cross(Camera.Right, reflCameraPosition - reflTargetPos);
			
			// Create our Reflection Matrix's Look At from our new Camera Position and Look At
			ReflectionViewMatrix = Matrix.CreateLookAt(reflCameraPosition, reflTargetPos, invUpVector);

			base.Update(gameTime);

		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if(!Visible)
				return;

			// Set our Custom Effect Parameters
			if (SetShaderParameters != null)
				SetShaderParameters.Invoke(gameTime);

      if (CustomEffect != null)
      {
        foreach (EffectPass pass in CustomEffect.CurrentTechnique.Passes)
        {
          pass.Apply();
          GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _verts, 0, 2);
        }
      }
		}
	}
}