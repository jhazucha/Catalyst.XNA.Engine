using System.IO;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Primitives
{
	public class Plane : VisualObject
	{
		public event CustomShader SetShaderParameters;

		public delegate void PickOperation(int index);
		public event PickOperation PickingOperation;

		[XmlIgnore]
		[ContentSerializerIgnore]
		public Texture2D Texture;

		private int _width;
		private int _height;
		private float[,] _heightData;

	  public Effect CustomEffect { get; set; }
		public BasicEffect SimpleEffect { get; set; }

		public IndexBuffer IndexBuffer;
		public VertexBuffer VertexBuffer;
		public VertexPositionColorTexture[] Vertices;
		public VertexDeclaration VertexDeclaration { get; set; }

    public MemoryStream HeightMapStream;

		public int Width { get; set; }
		public int Height { get; set; }
	  public int TriangleCount { get; private set; }

	  public Color MaterialColor { get; set; }
		public Color AmbientColor { get; set; }
		public Color DiffuseColor { get; set; }
		public Color SpecularColor { get; set;  }
		public Color EmissiveColor { get; set; }

		public float SpecularPower { get; set; }
		public float[,] HeightData { get; set; }
		public string HeightmapAsset { get; set; }

		public new Vector3 Position { get; set; }
		public new Vector3 Rotation { get; set; }
		public new Vector3 Scale = Vector3.One;
		
		public FillMode FillMode = FillMode.WireFrame;
	  public CullMode CullMode = CullMode.None;

		public Plane(Game game, int w, int h, BasicCamera cam)
			: base(game)
		{
			// Hook our camera ref
			Camera = cam;

			// Set our width and height values
			_width = w;
			_height = h;
			
			_heightData = new float[_width, _width];

			SpecularPower = 100;

			MaterialColor = Color.White;
			AmbientColor = Color.Black;
			DiffuseColor = Color.White;
			SpecularColor = Color.Black;
			EmissiveColor = Color.Black;
		}

		public Plane(Game game, string heightmap, BasicCamera cam)
			: base(game)
		{
			Camera = cam;
			SpecularPower = 100;
			HeightmapAsset = heightmap;

			MaterialColor = Color.White;
			AmbientColor = Color.Black;
			DiffuseColor = Color.White;
			SpecularColor = Color.Black;
			EmissiveColor = Color.Black;
		}

    public Plane(Game game, MemoryStream heightmap, BasicCamera cam)
      : base(game)
    {
      HeightMapStream = heightmap;
      Camera = cam;

      MaterialColor = Color.White;
      AmbientColor = Color.Black;
      DiffuseColor = Color.White;
      SpecularColor = Color.Black;
      EmissiveColor = Color.Black;
    }

	  public override void LoadContent()
		{
			base.LoadContent();

			// If we are using a heightmap load it
			if(!string.IsNullOrEmpty(HeightmapAsset))
			{
			  LoadHeightmap(Game.Content.Load<Texture2D>(HeightmapAsset));
			}
      else if (HeightMapStream != null)
      {
        // Load our texture up from stream
        LoadHeightmap(Texture2D.FromStream(Game.GraphicsDevice, HeightMapStream));
        HeightMapStream.Seek(0, SeekOrigin.Begin);
      }
		}

		public override void Initialize()
		{
			base.Initialize();

			// Init our Basic Effect
			SimpleEffect = new BasicEffect(GraphicsDevice);
			SimpleEffect.VertexColorEnabled = true;

			// Create vert array
			SetupVertices();

			// Create indices for our plane
			SetupIndices();
		}

    private void LoadHeightmap(Texture2D HeightMap)
    {
      // Store our Width and Height of our local heightmap
      _width = HeightMap.Width;
      _height = HeightMap.Height;

      // Create a Color[] Array to hold read our heights from the loaded texture
      Color[] TextureColors = new Color[_width * _height];

      // Copy our Textures Data into a Color[] Array
      HeightMap.GetData(TextureColors);

      // Create our Local Height Data Array
      _heightData = new float[(_width), (_width)];

      for (int x = 0; x < _width; x++)
      {
        for (int y = 0; y < _height; y++)
        {
          // Get our Height value from our Textures color array
          _heightData[x, y] = TextureColors[(x + y * _width)].R +
                              TextureColors[(x + y * _width)].G +
                              TextureColors[(x + y * _width)].B / 3;
        }
      }

      // Dump our heightmap now that were done with it
      HeightMap.Dispose();
    }

		public void SetupVertices()
		{
			// Create our Vert Array
			Vertices = new VertexPositionColorTexture[_width * _width];

			// Calculate our Vertice Array
			for (int x = 0; x < _width; x++)
			{
				for (int z = 0; z < _height; z++)
				{
					// Calc our verts
					Vertices[x + z*_width].Position = new Vector3(x, _heightData[x, z], z);
					Vertices[x + z*_width].Color = MaterialColor;
					//VertArray[x + z*_width].Normal = Vector3.Up;
					Vertices[x + z*_width].TextureCoordinate.X = (float) x/_width;
					Vertices[x + z*_width].TextureCoordinate.Y = (float) z/_height;
				}
			}

			// Create and Fill our Vertex Buffer
			VertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionColorTexture), _width * _height, BufferUsage.WriteOnly);
			VertexBuffer.SetData(Vertices);

		}

		public void SetupIndices()
		{
			int[] indices = new int[(_width - 1) * (_height - 1) * 6];

			for (int x = 0; x < _width - 1; x++)
			{
				for (int y = 0; y < _height - 1; y++)
				{
					indices[(x + y * (_width - 1)) * 6] = (x + 1) + (y + 1) * _width;
					indices[(x + y * (_width - 1)) * 6 + 1] = (x + 1) + y * _width;
					indices[(x + y * (_width - 1)) * 6 + 2] = x + y * _width;

					indices[(x + y * (_width - 1)) * 6 + 3] = (x + 1) + (y + 1) * _width;
					indices[(x + y * (_width - 1)) * 6 + 4] = x + y * _width;
					indices[(x + y * (_width - 1)) * 6 + 5] = x + (y + 1) * _width;
				}
			}

			// Calculate our Triangle Count for this Node
			TriangleCount = indices.Length / 3;

			// Create and Set our Index Buffer
			IndexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), (_width - 1) * (_height - 1) * 6, BufferUsage.WriteOnly);
			IndexBuffer.SetData(indices);
		}

		public bool IsPlanePicked(int mouseX, int mouseY)
		{
			// Construct our ray 
			Ray ray = Utilitys.CalculateRayFromCursor(Game.GraphicsDevice, Camera, mouseX, mouseY);

			Vector3 min = new Vector3(Vertices[0].Position.X, Vertices[0].Position.Y, Vertices[0].Position.Z);
			Vector3 max = new Vector3(Vertices[Vertices.Length - 1].Position.X,
			                          Vertices[Vertices.Length - 1].Position.Y,
			                          Vertices[Vertices.Length - 1].Position.Z);

			BoundingBox bb = new BoundingBox(min, max);

			if(bb.Intersects(ray) > 0)
			{
				// TODO: This needs reworking because we should almost segregate the difference between a Vertice Picking operation and the whole Object Picking operation!

				// Invoke our delegate so we can manipulate this from outside our class
				if (PickingOperation != null)
					PickingOperation.Invoke(0);
			}

			return false;
		}

		public bool IsVertPicked(int mouseX, int mouseY)
		{
			// Construct our ray 
			Ray ray = Utilitys.CalculateRayFromCursor(Game.GraphicsDevice, Camera, mouseX, mouseY);

			// Calculate our Vertice Array
			for (int x = 0; x < _width; x++)
			{
				for (int z = 0; z < _height; z++)
				{
					// Calc our Bounding Boxes
					Vector3 min = new Vector3(Vertices[x + z * _width].Position.X,
					                          Vertices[x + z * _width].Position.Y,
					                          Vertices[x + z * _width].Position.Z);

					Vector3 max = new Vector3(Vertices[x + z * _width].Position.X + 1,
					                          Vertices[x + z * _width].Position.Y + 1,
					                          Vertices[x + z * _width].Position.Z + 1);

					BoundingBox bb = new BoundingBox(min, max);

					// If we found a hit return it
					if (bb.Intersects(ray) > 0)
					{
						// Invoke our delegate so we can manipulate this from outside our class
						if (PickingOperation != null)
							PickingOperation.Invoke(x + z*_width);

						// Rebuild our vert array
						VertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof (VertexPositionColorTexture), _width*_height, BufferUsage.WriteOnly);
						VertexBuffer.SetData(Vertices);

						return true;
					}
				}
			}

			return false;
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			World =  Matrix.CreateScale(Scale) *
			          Matrix.CreateRotationY(Rotation.Y) *
			          Matrix.CreateRotationX(Rotation.X) *
			          Matrix.CreateRotationX(Rotation.Z) *
			          Matrix.CreateTranslation(Position);

			if (VertexBuffer != null && IndexBuffer != null)
			{
				// Pass our Index Buffer and our Vertex Buffer to the Graphics Device
				GraphicsDevice.Indices = IndexBuffer;
				GraphicsDevice.SetVertexBuffer(VertexBuffer);

				if(CustomEffect != null)
				{
					// Setup our Shader
					if(SetShaderParameters != null)
						SetShaderParameters.Invoke(gameTime);

					foreach(EffectPass p in CustomEffect.CurrentTechnique.Passes)
					{
						p.Apply();
						GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _width * _height, 0, TriangleCount);
					}
				}
				else
				{
					RasterizerState state = new RasterizerState();

				  state.FillMode = FillMode;
				  state.CullMode = CullMode;

					GraphicsDevice.RasterizerState = state;

					// Pass in our Lighting Colors
					SimpleEffect.AmbientLightColor = AmbientColor.ToVector3();
					SimpleEffect.DiffuseColor = DiffuseColor.ToVector3();
					SimpleEffect.EmissiveColor = EmissiveColor.ToVector3();
					SimpleEffect.SpecularColor = SpecularColor.ToVector3();
					SimpleEffect.SpecularPower = SpecularPower;

					// Update the world, view and projection matrices on the basic effect
					SimpleEffect.World = World;
					SimpleEffect.View = Camera.View;
					SimpleEffect.Projection = Camera.Projection;

					// Pass our texture to our graphics device for rendering
					if (Texture != null)
					{
						SimpleEffect.Texture = Texture;
						SimpleEffect.TextureEnabled = true;
					}

					foreach(EffectPass pass in SimpleEffect.CurrentTechnique.Passes)
					{
						pass.Apply();
						GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _width*_height, 0, TriangleCount);
					}
				}
			}
		}
	}
}