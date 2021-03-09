using System.Threading;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.StructClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace Catalyst3D.XNA.Testing.Graphics.Landscapes
{
	public enum BrushMode
	{
		Sculpting,
		Paint,
	}

	[TestFixture]
	public class LandscapeCursor : CatalystTestFixture
	{
		public delegate void HeightAdjust(Direction direction, float weight);

		public HeightAdjust OnAdjustHeight;

		private Effect Effect;
		private VertexDeclaration Declaration;

		private VertexBuffer vb;
		private IndexBuffer ib;

		private CustomLandscapeVertex[] Verts;

		private const int Width = 512;
		private const int Height = 512;

		private int TriangleCount;

		private Texture2D Layer1;
		private Texture2D Layer2;
		private Texture2D Layer3;
		private Texture2D Layer4;

		private Texture2D PaintMask1;

		private const int MinBrushSize = 4;
	  private int BrushSize = 32;
		private int CurrentLayer;

		private BasicCamera Camera;

		private Vector3 CursorPosition = Vector3.Zero;

		private BrushMode BrushMode;
		private int[] Indices;

		private Thread HeightAdjustThread;
		private bool CanAlterHeight = true;

		[Test]
		public void Test()
		{
			IsMouseVisible = true;
			
      InputHandler.AddCommand(new InputAction("Forward", Buttons.LeftThumbstickUp, Keys.Up));
			InputHandler.AddCommand(new InputAction("Back", Buttons.LeftThumbstickUp, Keys.Down));
			InputHandler.AddCommand(new InputAction("Left", Buttons.LeftThumbstickUp, Keys.Left));
			InputHandler.AddCommand(new InputAction("Right", Buttons.LeftThumbstickUp, Keys.Right));
			InputHandler.AddCommand(new InputAction("bsIncrease", Buttons.LeftThumbstickUp, Keys.PageUp));
			InputHandler.AddCommand(new InputAction("bsDecrease", Buttons.LeftThumbstickUp, Keys.PageDown));
			InputHandler.AddCommand(new InputAction("Sculpting", Buttons.LeftThumbstickUp, Keys.F1));
			InputHandler.AddCommand(new InputAction("PaintLayer1", Buttons.LeftThumbstickUp, Keys.F2));
			InputHandler.AddCommand(new InputAction("PaintLayer2", Buttons.LeftThumbstickUp, Keys.F3));
			InputHandler.AddCommand(new InputAction("PaintLayer3", Buttons.LeftThumbstickUp, Keys.F4));
			InputHandler.AddCommand(new InputAction("PaintLayer4", Buttons.LeftThumbstickUp, Keys.F5));

			Camera = new BasicCamera(this);
		  Camera.Position = new Vector3(420, 100, 420);
			Camera.Target = new Vector3(0, -50, 0);
			Components.Add(Camera);

			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			HeightAdjustThread = new Thread(HeightAdjustWorkerThread);
			HeightAdjustThread.IsBackground = true;

			Declaration = new VertexDeclaration(CustomLandscapeVertex.VertexElements);

			// Create our vertices
			Verts = new CustomLandscapeVertex[Width*Height];

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					Vector3 position = new Vector3(x, 0, y);
					Vector2 texCoords = new Vector2((float) x/Width, (float) y/Height);
					Vector3 normal = new Vector3(0, 1, 0);

					Verts[x + y*Width] = new CustomLandscapeVertex(position, normal, texCoords);
				}
			}

			// Create our vertex buffer
			vb = new VertexBuffer(GraphicsDevice, Declaration, Verts.Length, BufferUsage.None);
			vb.SetData(Verts);

			// Create our indices
			Indices = new int[(Width - 1) * (Height - 1) * 6];

			for (int x = 0; x < Width - 1; x++)
			{
				for (int y = 0; y < Height - 1; y++)
				{
					// Triangle 1
					Indices[(x + y*(Width - 1))*6] = (x + 1) + (y + 1)*Width;
					Indices[(x + y*(Width - 1))*6 + 1] = (x + 1) + y*Width;
					Indices[(x + y*(Width - 1))*6 + 2] = x + y*Width;

					// Triangle 2
					Indices[(x + y*(Width - 1))*6 + 3] = (x + 1) + (y + 1)*Width;
					Indices[(x + y*(Width - 1))*6 + 4] = x + y*Width;
					Indices[(x + y*(Width - 1))*6 + 5] = x + (y + 1)*Width;
				}
			}

			TriangleCount = Indices.Length/3;

			// Create and Set our Index Buffer
			ib = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, (Width - 1)*(Height - 1)*6, BufferUsage.WriteOnly);
			ib.SetData(Indices);

			OnAdjustHeight += AdjustHeight;
		}
	
		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our Custom Shader
			Effect = Content.Load<Effect>("EditorLandscapeShader");

			// Load our Actual Textures
			Layer1 = Content.Load<Texture2D>("desert1");
			Layer2 = Content.Load<Texture2D>("dirt1");
			Layer3 = Content.Load<Texture2D>("grass1");
			Layer4 = Content.Load<Texture2D>("snow1");

			// Setup our Texture Masks
			PaintMask1 = Utilitys.GenerateTexture(GraphicsDevice, new Color(255, 0, 0, 0), Width, Height);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			MouseState state = Mouse.GetState();

			// Calculate our cursors position on the plane in 3D space
			CursorPosition = Utilitys.CalcPosOnPlane(GraphicsDevice, Camera, state.X, state.Y);

			if (InputHandler.IsActionPressed("bsIncrease"))
				BrushSize += 1;

			if (InputHandler.IsActionPressed("bsDecrease"))
			{
				if (BrushSize >= MinBrushSize)
					BrushSize -= 1;
			}

			if (InputHandler.IsActionPressed("Sculpting"))
				BrushMode = BrushMode.Sculpting;

			if (InputHandler.IsActionPressed("PaintLayer1"))
			{
				BrushMode = BrushMode.Paint;
				CurrentLayer = 0;
			}

			if (InputHandler.IsActionPressed("PaintLayer2"))
			{
				BrushMode = BrushMode.Paint;
				CurrentLayer = 1;
			}

			if (InputHandler.IsActionPressed("PaintLayer3"))
			{
				BrushMode = BrushMode.Paint;
				CurrentLayer = 2;
			}

			if (InputHandler.IsActionPressed("PaintLayer4"))
			{
				BrushMode = BrushMode.Paint;
				CurrentLayer = 3;
			}

			#region Sculpting Brush Mode

			if(BrushMode == BrushMode.Sculpting)
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					if(HeightAdjustThread.ThreadState == ThreadState.Stopped)
						CanAlterHeight = true;

					if(CanAlterHeight && HeightAdjustThread.ThreadState != ThreadState.Running)
					{
						CanAlterHeight = false;

						HeightAdjustThread = new Thread(HeightAdjustWorkerThread);
						HeightAdjustThread.IsBackground = true;

						HeightAdjustThread.Start(Direction.Up);
					}
				}

				if (state.RightButton == ButtonState.Pressed)
				{
					if(HeightAdjustThread.ThreadState == ThreadState.Stopped)
						CanAlterHeight = true;

					if(CanAlterHeight && HeightAdjustThread.ThreadState != ThreadState.Running)
					{
						CanAlterHeight = false;

						HeightAdjustThread = new Thread(HeightAdjustWorkerThread);
						HeightAdjustThread.IsBackground = true;

						HeightAdjustThread.Start(Direction.Down);
					}
				}
			}

			#endregion

			#region Painting Mode

			if (BrushMode == BrushMode.Paint)
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					PaintLayer(CurrentLayer, 100);
				}
			}

			#endregion

			// Update our Shader Var's
			Effect.Parameters["World"].SetValue(Matrix.Identity);
			Effect.Parameters["View"].SetValue(Camera.View);
			Effect.Parameters["Projection"].SetValue(Camera.Projection);

			Effect.Parameters["AmbientLightColor"].SetValue(Color.White.ToVector4());
			Effect.Parameters["AmbientLightIntensity"].SetValue(0.1f);

			Effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(1, 1, 1));
			Effect.Parameters["DiffuseLightColor"].SetValue(Color.Gray.ToVector4());
			Effect.Parameters["DiffuseLightIntensity"].SetValue(0.9f);

			Effect.Parameters["CursorPosition"].SetValue(CursorPosition);
			Effect.Parameters["CursorSize"].SetValue(BrushSize);

			// Send our Textures to our GPU
			if (Layer1 != null)
				Effect.Parameters["TextureLayer1"].SetValue(Layer1);

			if (Layer2 != null)
				Effect.Parameters["TextureLayer2"].SetValue(Layer2);

			if (Layer3 != null)
				Effect.Parameters["TextureLayer3"].SetValue(Layer3);

			if (Layer4 != null)
				Effect.Parameters["TextureLayer4"].SetValue(Layer4);

			if (PaintMask1 != null)
				Effect.Parameters["PaintMask"].SetValue(PaintMask1);
		}

		private void PaintLayer(int layerID, float weight)
		{
			Color pixel = Color.Transparent;

			Vector2 position = new Vector2(CursorPosition.X - ((float) BrushSize/2), CursorPosition.Z - ((float) BrushSize/2));

			Rectangle rec = new Rectangle((int)position.X, (int)position.Y, BrushSize, BrushSize);

			if(rec.Right > PaintMask1.Width || rec.Left > PaintMask1.Width || rec.Top > PaintMask1.Height ||
				rec.Bottom > PaintMask1.Height || rec.Right < 0 || rec.Left < 0 || rec.Top < 0 || rec.Bottom < 0)
				return;

				switch(layerID)
				{
					case 0:
						pixel.R += (byte)weight;
						break;
					case 1:
						pixel.G += (byte)weight;
						break;
					case 2:
						pixel.B += (byte)weight;
						break;
					case 3:
						pixel.A += (byte)weight;
						break;
				}

			Color[] col = new Color[BrushSize*BrushSize];

			for(int i = 0; i < col.Length; i++)
				col[i] = new Color(col[i].R + pixel.R, col[i].G + pixel.G, col[i].B + pixel.B, col[i].A + pixel.A);
			
			// Flush the texture off the graphics device first
			GraphicsDevice.Textures[4] = null;
			
			// Set the new paint mask texture data on the gpu
			PaintMask1.SetData(0, rec, col, 0, col.Length);
		}

		private void HeightAdjustWorkerThread(object dir)
		{
			OnAdjustHeight.Invoke((Direction) dir, 4);
		}

		private void AdjustHeight(Direction direction, float weight)
		{
			bool rebuild = false;

			switch(direction)
			{
				case Direction.Up:
					for(int x = 0; x < Width; x++)
					{
						for (int y = 0; y < Height; y++)
						{
							int index = x + y*Width;

							float distance = Vector3.Distance(Verts[index].Position, CursorPosition);

							if (distance <= BrushSize)
							{
								Vector3 height = new Vector3(0, weight, 0);
								height.Normalize();

								if (x > 0)
									Verts[(x - 1) + y*Width].Position += height/distance;

								// Center
								Verts[index].Position += height;

								if (x < Width)
									Verts[(x + 1) + y*Width].Position += height/distance;

								if (x > 0 && y > 0)
								{
									Verts[(x - 1) + (y - 1)*Width].Position += height/distance;
								}

								if (y > 0)
								{
									Verts[x + (y - 1)*Width].Position += height/distance;
								}

								if (x > 0 && y < Height)
								{
									Verts[(x - 1) + (y + 1)*Width].Position += height/distance;
								}

								if (y < Height)
								{
									Verts[x + (y - 1)*Width].Position += height/distance;
								}

								if (x < Width && y < Height)
								{
									Verts[(x + 1) + (y + 1)*Width].Position += height/distance;
								}

								rebuild = true;
							}
						}
					}
					break;
				case Direction.Down:
					for(int x = 0; x < Width; x++)
					{
						for(int y = 0; y < Height; y++)
						{
							int index = x + y * Width;

							float distance = Vector3.Distance(Verts[index].Position, CursorPosition);

							if(distance <= BrushSize)
							{
								Vector3 height = new Vector3(0, weight, 0);
								height.Normalize();

								// Center
								Verts[index].Position -= height;

								// Left
								if(x > 0)
									Verts[(x - 1) + y * Width].Position -= height / distance;

								// Right
								if(x < Width)
									Verts[(x + 1) + y * Width].Position -= height / distance;

								// Top
								if(x > 0 && y > 0)
								{
									Verts[(x - 1) + (y - 1) * Width].Position -= height / distance;
								}

								// Top left
								if(y > 0)
								{
									Verts[x + (y - 1) * Width].Position -= height / distance;
								}

								// Bottom Right
								if(x > 0 && y < Height)
								{
									Verts[(x - 1) + (y + 1) * Width].Position -= height / distance;
								}

								// Top Right
								if(y < Height)
								{
									Verts[x + (y - 1) * Width].Position -= height / distance;
								}

								// Bottom Left
								if(x < Width && y < Height)
								{
									Verts[(x + 1) + (y + 1) * Width].Position -= height / distance;
								}

								rebuild = true;
							}
						}
					}
					break;
			}

			if(rebuild)
				RebuildVertexBuffer();
		}

		private void RebuildVertexBuffer()
		{
			// Create the new VB
			vb = new VertexBuffer(GraphicsDevice, Declaration, Verts.Length, BufferUsage.None);
			vb.SetData(Verts);

			CanAlterHeight = true;
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			GraphicsDevice.Indices = ib;
			GraphicsDevice.SetVertexBuffer(vb);

			foreach (EffectPass p in Effect.CurrentTechnique.Passes)
			{
				p.Apply();
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Verts.Length, 0, TriangleCount);
			}
		}

	}
}
