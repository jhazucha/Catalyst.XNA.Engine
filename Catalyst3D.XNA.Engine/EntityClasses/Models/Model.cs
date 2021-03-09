using System;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models
{
	public class Model : VisualObject
	{
		public event CustomShader SetShaderParameters;

    public  Microsoft.Xna.Framework.Graphics.Model Content;

		public Effect CustomEffect;
	
		private readonly string _assetName;
		
		private BoundingBox _boundingBox;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Position of Model")]
		[Category("Orientation")]
#endif
		public new Vector3 Position { get; set; }

		public new Vector3 Rotation { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Scale of Model")]
		[Category("Orientation")]
#endif
		public new Vector3 Scale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Direction of Model")]
		[Category("Orientation")]
#endif
		public Vector3 Direction { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Models Right Vector")]
		[Category("Orientation")]
#endif
		public Vector3 Right { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Models Up Vector")]
		[Category("Orientation")]
#endif
	  public Vector3 Up { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("HLSL Effect Paramaters")]
		[Category("Effects")]
#endif
			public EffectParameter[] EffectParams { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Diffuse Color")]
		[Category("Materials")]
#endif
			public Color DiffuseColor { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Category("Materials")]
		[Description("Specular Color")]
#endif
			public Color SpecularColor { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Ambient Color")]
		[Category("Materials")]
#endif
      public Color AmbientColor { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Category("Materials")]
		[Description("Specular Light Power")]
#endif
      public float SpecularPower { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Category("Materials")]
		[Description("Emissive Light Color")]
#endif
      public Color EmissiveColor { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Renderers Fill Mode")]
		[Category("Rendering")]
#endif
      public FillMode FillMode { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Cull Mode used by the renderer")]
		[Category("Rendering")]
#endif
      public CullMode CullMode { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
		[Description("Bounding Type")]
#endif
      public BoundingType BoundingType { get; set; }

      public Model(Game game, BasicCamera cam)
        : base(game)
      {
        Right = Vector3.Right;
        Direction = Vector3.Forward;
        Scale = Vector3.One;
        Rotation = Vector3.Zero;
        Camera = cam;

        Up = Vector3.Up;
        AmbientColor = Color.White;
        DiffuseColor = Color.White;
        SpecularColor = Color.Black;
        EmissiveColor = Color.Black;
        SpecularPower = 100;
        FillMode = FillMode.Solid;
        CullMode = CullMode.CullCounterClockwiseFace;
        BoundingType = BoundingType.Box;

      }

	  public Model(Game game, string asset, BasicCamera cam)
			: base(game)
		{
			Right = Vector3.Right;
			Direction = Vector3.Forward;
			Scale = Vector3.One;
			Rotation = Vector3.Zero;
			_assetName = asset;
			Camera = cam;

      Up = Vector3.Up;
      AmbientColor = Color.White;
      DiffuseColor = Color.White;
      SpecularColor = Color.Black;
      EmissiveColor = Color.Black;
      SpecularPower = 100;
      FillMode = FillMode.Solid;
      CullMode = CullMode.CullCounterClockwiseFace;
      BoundingType = BoundingType.Box;
		}

    public override void LoadContent()
    {
      base.LoadContent();

      if (!string.IsNullOrEmpty(_assetName) && Content == null)
      {
        // Create a name for our object
        Name = _assetName + Game.Components.Count;

        if (GameScreen != null)
        {
          // load using our scene manager
          Content = GameScreen.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(_assetName);
        }
        else
        {
          // Load without our scene manager
          Content = Game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(_assetName);
        }
      }

      if (Content != null)
      {
        // Be sure the Device is not currently holding any DATA!
        //GraphicsDevice.Vertices[0].SetSource(null, 0, 0);
        GraphicsDevice.Indices = null;

        // Generate our Bounding Box / Why are we not factoring in Scale here?
        foreach (ModelMesh mesh in Content.Meshes)
        {
          foreach (ModelMeshPart part in mesh.MeshParts)
          {
            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[part.NumVertices];
            part.VertexBuffer.GetData(verts);
            //mesh.VertexBuffer.GetData(verts);

            // Loop thru and get our points
            Vector3[] pos = new Vector3[verts.Length];
            for (int i = 0; i < verts.Length; i++)
              pos[i] = verts[i].Position;

            // Create our Bounding Box from our models vertice positions
            _boundingBox = BoundingBox.CreateFromPoints(pos);
          }
        }
      }
    }

	  public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Set our Custom Effect Parameters
			if (SetShaderParameters != null)
				SetShaderParameters.Invoke(gameTime);
		}

    public override void Draw(GameTime gameTime)
    {
      if (Content != null && Camera != null && Enabled && Visible)
      {
        // Copy any parent transforms.
        Matrix[] transforms = new Matrix[Content.Bones.Count];
        Content.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in Content.Meshes)
        {
          // Update our World Matrix for this Model
          World = transforms[mesh.ParentBone.Index]*
                   Matrix.CreateRotationX(Rotation.X)*
                   Matrix.CreateRotationY(Rotation.Y)*
                   Matrix.CreateRotationZ(Rotation.Z)*
                   Matrix.CreateScale(Scale)*
                   Matrix.CreateTranslation(Position);

          if (CustomEffect != null)
          {
            #region Custom HLSL Effect Rendering

            /*********************************************************************************************************
										-- Please do not fuck with this to much haha unless ya ask me (AR-50 LOCKED AND LOADED!) --
						 *********************************************************************************************************/

            // Pass our meshes Index buffer to our graphics device
            //GraphicsDevice.Indices = mesh.IndexBuffer;

            // Loop thru the Custom Effect's passes
            foreach (EffectPass pass in CustomEffect.CurrentTechnique.Passes)
            {
              // Begin our First pass
              pass.Apply();

              // Render this pass over the parts inside the mesh
              foreach (ModelMeshPart part in mesh.MeshParts)
              {
                // Pass our mesh part's Vertex declaration to our graphics device
                //GraphicsDevice.VertexDeclaration = part.VertexDeclaration;

                // Manually pass our vertices to our graphics device
                //GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);

                // TODO: Need to rework this .. the double casts will kill performance! *Refactor OUT*

                // Check and see if the part has an attached texture
                if (((BasicEffect) part.Effect).Texture != null)
                {
                  // If it does pass the texture our graphics device instead of manually in our shader
                  GraphicsDevice.Textures[0] = ((BasicEffect) part.Effect).Texture;
                }

                // Finally Draw our Index Primitives (Triangles) for this part of our mesh
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.NumVertices, 0, part.NumVertices,
                                                     part.StartIndex, part.PrimitiveCount);
              }
            }

            #endregion
          }
          else
          {
            #region Basic Effect Rendering Loop

            // Setup culling / fill 
            RasterizerState rasterizerState1 = new RasterizerState();
            rasterizerState1.CullMode = CullMode;
            rasterizerState1.FillMode = FillMode;
            GraphicsDevice.RasterizerState = rasterizerState1;

            // Set our alpha blending states if enabled
            if (EnableAlphaBlending)
            {
              GraphicsDevice.BlendState = BlendState.AlphaBlend;

              //GraphicsDevice.RenderState.AlphaBlendEnable = true;
              //GraphicsDevice.RenderState.AlphaTestEnable = true;

              //GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
              //GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
              //GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
              //GraphicsDevice.RenderState.BlendFunction = BlendFunction.Load;
            }

            foreach (BasicEffect effect in mesh.Effects)
            {
              effect.EnableDefaultLighting();
              
              effect.AmbientLightColor = AmbientColor.ToVector3();
              effect.DiffuseColor = DiffuseColor.ToVector3();
              effect.EmissiveColor = EmissiveColor.ToVector3();
              effect.SpecularColor = SpecularColor.ToVector3();
             
              effect.World = World;
              effect.View = Camera.View;
              effect.Projection = Camera.Projection;
            }

            mesh.Draw();

            // Reset our alpha blending states
            if (EnableAlphaBlending)
            {
              //GraphicsDevice.RenderState.AlphaBlendEnable = false;
              //GraphicsDevice.RenderState.AlphaTestEnable = false;
              //GraphicsDevice.RenderState.SourceBlend = Blend.Zero;
              //GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
            }

            #endregion
          }
          base.Draw(gameTime);
        }
      }
    }

	  public bool RayIntersectsModel(Ray ray)
		{
      foreach (ModelMesh mesh in Content.Meshes)
			{
				if (BoundingType == BoundingType.Sphere)
				{
					// Calculate the bounding sphere
					BoundingSphere sphere = TransformBoundingSphere(mesh.BoundingSphere);

					if (sphere.Intersects(ray) != null)
						return true;
				}

				if (BoundingType == BoundingType.Box)
				{
					// Calculate the bounding box
					BoundingBox box = TransformBoundingBox(_boundingBox, World);

					if (box.Intersects(ray) != null)
						return true;
				}
			}
			return false;
		}

		public BoundingSphere TransformBoundingSphere(BoundingSphere sphere)
		{
			BoundingSphere transformedSphere;

			// Be sure to use the radius incase the sphere has been scaled at all!
			Vector3 scale = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);

			// Now transform it
      scale = Vector3.TransformNormal(scale, World);

			transformedSphere.Radius = Math.Max(scale.X, Math.Max(scale.Y, scale.Z));
      transformedSphere.Center = Vector3.Transform(sphere.Center, World);

			return transformedSphere;
		}

		public BoundingBox TransformBoundingBox(BoundingBox box, Matrix transform)
		{
			BoundingBox transformedBox;

			//Vector3 min = new Vector3(Position.X + box.Max.X, Position.Y + box.Max.Y, Position.Z + box.Max.Z);
			//Vector3 max = new Vector3(Position.X + box.Min.X, Position.Y + box.Min.Y, Position.Z + box.Min.Z);
			
			Vector3 min = new Vector3(box.Max.X, box.Max.Y, box.Max.Z);
			Vector3 max = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);

			// now transform it
      transformedBox.Max = Vector3.Transform(max, World);
      transformedBox.Min = Vector3.Transform(min, World);

			return transformedBox;
		}
	}
}