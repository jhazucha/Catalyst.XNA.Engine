using System.Drawing;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Environment;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Catalyst3D.Plugins.Landscape
{
	public class EditorLandscape : XNA.Engine.EntityClasses.Environment.Landscape
	{
		public delegate void HeightAdjust(Direction direction, float weight);

		[XmlIgnore]
		public HeightAdjust OnAdjustHeight;

		[XmlIgnore]
		public Globals.LandscapeAction LandscapeCreated;

		[XmlIgnore]
		public IAddinHost Host;

		[XmlIgnore]
		private Vector3 MouseCoords;

		private InputHandler InputHandler;

		[XmlIgnore]
		public int CurrentPaintLayer;

		[XmlIgnore]
		public BrushModes BrushMode;

		[XmlIgnore]
		public int BrushSize = 6;

		[XmlIgnore]
		public float BrushWeight = 2f;

		[XmlIgnore]
		public Image BrushMask;

		[XmlIgnore]
		public Texture2D Layer1;
		[XmlIgnore]
		public Texture2D Layer2;
		[XmlIgnore]
		public Texture2D Layer3;
		[XmlIgnore]
		public Texture2D Layer4;

		public Color AmbientLightColor { get; set; }

		public float AmbientLightIntensity { get; set; }

		public Color DiffuseLightColor { get; set; }

		public float DiffuseLightIntensity { get; set; }

		public Vector3 DiffuseLightDirection { get; set; }

		public string Layer1Path;
		public string Layer2Path;
		public string Layer3Path;
		public string Layer4Path;

		private Thread HeightAdjustThread;
		private bool CanAlterHeight = true;

		[XmlIgnore]
		public Texture2D PaintMask1;

		public string PaintMaskAssetName { get; set; }

		public EditorLandscape()
			: base(null, 0, 0, 0, null)
		{
			// Required for Serialization
		}

		public EditorLandscape(Game game, int blocksize, string heightmap, BasicCamera camera)
			: base(game, blocksize, heightmap, camera)
		{
		}

		public EditorLandscape(Game game, int blockSize, int width, int height, BasicCamera camera)
			: base(game, blockSize, width, height, camera)
		{
		}

		public EditorLandscape(Game game, int blockSize, Stream stream, BasicCamera camera)
			: base(game, blockSize, stream, camera)
		{
		}

		public override void Initialize()
		{
			HeightAdjustThread = new Thread(HeightAdjustWorkerThread);
			HeightAdjustThread.IsBackground = true;

			AmbientLightColor = Color.White;
			AmbientLightIntensity = 1.0f;

			DiffuseLightDirection = new Vector3(-1, -1, 0);
			DiffuseLightColor = Color.White;
			DiffuseLightIntensity = 1.0f;

			SetShaderParameters += OnLandscapeShaderParams;
			OnAdjustHeight += AdjustHeight;

			base.Initialize();
		}

		public override void LoadContent()
		{
			InputHandler = (InputHandler)Game.Services.GetService(typeof(InputHandler));
			InputHandler.Commands.Add(new InputAction("LeftAlt", Buttons.X, Keys.LeftAlt));

			CustomEffect = Host.Game.Content.Load<Effect>("LandscapeShader");

			// Setup our Texture Masks
			if(string.IsNullOrEmpty(PaintMaskAssetName))
			{
				PaintMaskAssetName = "pMask1.png";
				PaintMask1 = Utilitys.GenerateTexture(GraphicsDevice, new Color(255, 0, 0, 0), Width, Height);

				SavePaintMask();
			}
			else
			{
				if(File.Exists(Host.ProjectPath + @"\Content\" + PaintMaskAssetName))
				{
					using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + PaintMaskAssetName, FileMode.Open))
					{
						PaintMask1 = Texture2D.FromStream(Host.Game.GraphicsDevice, stream);
					}
				}
			}

			if(!string.IsNullOrEmpty(Layer1Path))
			{
				if(File.Exists(Host.ProjectPath + @"\Content\" + Layer1Path))
				{
					using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Layer1Path, FileMode.Open))
					{
						Layer1 = Texture2D.FromStream(Host.Game.GraphicsDevice, stream);
					}
				}
			}

			if(!string.IsNullOrEmpty(Layer2Path))
			{
				if(File.Exists(Host.ProjectPath + @"\Content\" + Layer2Path))
				{
					using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Layer2Path, FileMode.Open))
					{
						Layer2 = Texture2D.FromStream(Host.Game.GraphicsDevice, stream);
					}
				}
			}

			if(!string.IsNullOrEmpty(Layer3Path))
			{
				if(File.Exists(Host.ProjectPath + @"\Content\" + Layer3Path))
				{
					using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Layer3Path, FileMode.Open))
					{
						Layer2 = Texture2D.FromStream(Host.Game.GraphicsDevice, stream);
					}
				}
			}

			if(!string.IsNullOrEmpty(Layer4Path))
			{
				if(File.Exists(Host.ProjectPath + @"\Content\" + Layer4Path))
				{
					using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Layer4Path, FileMode.Open))
					{
						Layer2 = Texture2D.FromStream(Host.Game.GraphicsDevice, stream);
					}
				}
			}

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(Visible)
			{
				MouseState state = Mouse.GetState();
				MouseCoords = Utilitys.CalcPosOnPlane(GraphicsDevice, Camera, state.X, state.Y);

				if(Host.Camera.IsCameraMovementLocked)
				{
					#region Raise - Lower Brush Modes

					if(BrushMode == BrushModes.Raise || BrushMode == BrushModes.Lower)
					{
						if(state.LeftButton == ButtonState.Pressed)
						{
							switch(BrushMode)
							{
								case BrushModes.Raise:
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
									break;
								case BrushModes.Lower:
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
									break;
							}
						}
					}

					#endregion Raise - Lower Brush Modes

					#region Painting Brush Mode

					if(BrushMode == BrushModes.Paint)
					{
						if(state.LeftButton == ButtonState.Pressed)
						{
							// Need brush weight to most likely be in 0-1 range
							PaintLayer(BrushWeight);
						}
					}

					#endregion Painting Brush Mode
				}
			}
		}

		private void OnLandscapeShaderParams(GameTime gametime)
		{
			CustomEffect.Parameters["DecalViewProj"].SetValue(Utilitys.CreateDecalViewProjectionMatrix(Camera, 64));

			CustomEffect.Parameters["World"].SetValue(Matrix.Identity);
			CustomEffect.Parameters["View"].SetValue(Camera.View);
			CustomEffect.Parameters["Projection"].SetValue(Camera.Projection);

			CustomEffect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor.ToVector4());
			CustomEffect.Parameters["AmbientLightIntensity"].SetValue(AmbientLightIntensity);

			CustomEffect.Parameters["DiffuseLightDirection"].SetValue(DiffuseLightDirection);
			CustomEffect.Parameters["DiffuseLightColor"].SetValue(DiffuseLightColor.ToVector4());
			CustomEffect.Parameters["DiffuseLightIntensity"].SetValue(DiffuseLightIntensity);

			CustomEffect.Parameters["CursorPosition"].SetValue(MouseCoords);
			CustomEffect.Parameters["CursorSize"].SetValue(BrushSize);

			switch(FillMode)
			{
				case FillMode.Solid:
					CustomEffect.CurrentTechnique = CustomEffect.Techniques["Solid"];
					break;
				case FillMode.WireFrame:
					CustomEffect.CurrentTechnique = CustomEffect.Techniques["WireFrame"];
					break;
			}

			// Send our Textures to our GPU
			if(Layer1 != null)
			{
				CustomEffect.Parameters["TextureLayer1"].SetValue(Layer1);
			}
			if(Layer2 != null)
			{
				CustomEffect.Parameters["TextureLayer2"].SetValue(Layer2);
			}
			if(Layer3 != null)
			{
				CustomEffect.Parameters["TextureLayer3"].SetValue(Layer3);
			}
			if(Layer4 != null)
			{
				CustomEffect.Parameters["TextureLayer4"].SetValue(Layer4);
			}

			if(PaintMask1 != null)
				CustomEffect.Parameters["PaintMask"].SetValue(PaintMask1);
		}

		private void PaintLayer(float weight)
		{
			// Cursor Position on Landscape
			Vector2 position = new Vector2(MouseCoords.X - ((float)BrushSize / 2), MouseCoords.Z - ((float)BrushSize / 2));

			// Rectangle to draw on
			Rectangle rec = new Rectangle((int)position.X, (int)position.Y, BrushSize, BrushSize);

			// Make sure we are not larger/smaller then the actual landscapes paint mask texture
			if(rec.Right > PaintMask1.Width || rec.Left > PaintMask1.Width || rec.Top > PaintMask1.Height ||
					rec.Bottom > PaintMask1.Height || rec.Right < 0 || rec.Left < 0 || rec.Top < 0 || rec.Bottom < 0)
				return;

			Color pixel = Color.Transparent;

			// Brush texture to use
			Bitmap brush = new Bitmap(Resource1.RoundBrush, BrushSize, BrushSize);

			// Build a color array to grab our pixels off our brush texture
			Color[] col = new Color[brush.Width * brush.Height];

			for(int x = 0; x < brush.Width; x++)
			{
				for(int y = 0; y < brush.Height; y++)
				{
					int index = x + y * brush.Width;

					System.Drawing.Color c = brush.GetPixel(x, y);

					switch(CurrentPaintLayer)
					{
						case 0:
							pixel.R += (byte)(c.R / weight);
							break;
						case 1:
							pixel.G += (byte)(c.G / weight);
							break;
						case 2:
							pixel.B += (byte)(c.B / weight);
							break;
						case 3:
							pixel.A += (byte)(c.A / weight);
							break;
					}
					col[index] = new Color(pixel.R, pixel.G, pixel.B, pixel.A);
				}
			}

			#region old routine

			//for (int x = 0; x < BrushSize; x++)
			//{
			//  for (int y = 0; y < BrushSize; y++)
			//  {
			//    int index = x + y*BrushSize;

			//    int red = col[index].R + pixel.R; // Layer1
			//    int green = col[index].G + pixel.G; // Layer2
			//    int blue = col[index].B + pixel.B; // Layer3
			//    int alpha = col[index].A + pixel.A; // Layer4

			//    // Apply our new found color goodness to our pixel
			//    col[index] = new Color(red, green, blue, alpha);
			//  }
			//}

			#endregion old routine

			// Unset the [4]th samplers (0-based) texture so we can set data to it on the graphics device
			GraphicsDevice.Textures[4] = null;

			// Set the new paint mask texture data on the gpu
			PaintMask1.SetData(0, rec, col, 0, col.Length);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			if(GraphicsDevice.IsDisposed)
				return;

			SavePaintMask();

			PaintMask1 = null;
			Layer1 = null;
			Layer2 = null;
			Layer3 = null;
			Layer4 = null;
		}

		private void SavePaintMask()
		{
			if(PaintMask1 == null)
				return;

			FileStream stream = new FileStream(Host.ProjectPath + @"\Content\" + PaintMaskAssetName, FileMode.Create);
			PaintMask1.SaveAsPng(stream, PaintMask1.Width, PaintMask1.Height);
			stream.Close();
		}

		private void HeightAdjustWorkerThread(object dir)
		{
			OnAdjustHeight.Invoke((Direction)dir, BrushWeight);
		}

		private void AdjustHeight(Direction direction, float weight)
		{
			bool rebuild = false;

			for(int bx = 0; bx < numBlocksX; bx++)
			{
				for(int by = 0; by < numBlocksZ; by++)
				{
					LandscapeBlock block = Blocks[bx][by];

					// If camera is in the bounding frustum check it for a hit
					if(block.Visible)
					{
						switch(direction)
						{
							case Direction.Up:
								for(int x = 0; x < block.Width; x++)
								{
									for(int y = 0; y < block.Height; y++)
									{
										// Index of this vertice
										int index = block.GetIndex(x, y);

										// Get the distance from this veritce to the mouse position
										float distance = Vector3.Distance(block.Verts[index].Position, MouseCoords);

										if(distance <= BrushSize)
										{
											Vector3 height = new Vector3(0, weight, 0);
											height.Normalize();

											// Center
											block.Verts[index].Position += height;

											if(block.Left != null)
											{
												for(int x1 = 0; x1 < block.Left.Width; x1++)
												{
													for(int y1 = 0; y1 < block.Left.Height; y1++)
													{
														float d2 = Vector3.Distance(block.Left.Verts[block.Left.GetIndex(x1, y1)].Position,
																												block.Verts[index].Position);

														if(d2 <= 5)
															block.Left.Verts[block.Left.GetIndex(x1, y1)].Position.Y = block.Verts[index].Position.Y;
													}
												}
											}

											if(block.Right != null)
											{
												for(int x1 = 0; x1 < block.Right.Width; x1++)
												{
													for(int y1 = 0; y1 < block.Right.Height; y1++)
													{
														float d2 = Vector3.Distance(block.Right.Verts[block.Right.GetIndex(x1, y1)].Position,
																												block.Verts[index].Position);

														if(d2 <= 5)
															block.Right.Verts[block.Right.GetIndex(x1, y1)].Position.Y = block.Verts[index].Position.Y;
													}
												}
											}

											if(block.Top != null)
											{
												for(int x1 = 0; x1 < block.Top.Width; x1++)
												{
													for(int y1 = 0; y1 < block.Top.Height; y1++)
													{
														float d2 = Vector3.Distance(block.Top.Verts[block.Top.GetIndex(x1, y1)].Position,
																												block.Verts[index].Position);

														if(d2 <= 5)
															block.Top.Verts[block.Top.GetIndex(x1, y1)].Position.Y = block.Verts[index].Position.Y;
													}
												}
											}

											if(block.Bottom != null)
											{
												for(int x1 = 0; x1 < block.Bottom.Width; x1++)
												{
													for(int y1 = 0; y1 < block.Bottom.Height; y1++)
													{
														float d2 = Vector3.Distance(block.Bottom.Verts[block.Bottom.GetIndex(x1, y1)].Position,
																												block.Verts[index].Position);

														if(d2 <= 5)
															block.Bottom.Verts[block.Bottom.GetIndex(x1, y1)].Position.Y = block.Verts[index].Position.Y;
													}
												}
											}

											rebuild = true;
										}
									}
								}
								break;
							case Direction.Down:
								break;
						}

						if(rebuild)
							RebuildVertexBuffer(block);
					}
				}
			}
		}

		private void RebuildVertexBuffer(LandscapeBlock block)
		{
			CanAlterHeight = false;

			// Create the new VB
			block.VertexBuffer = new VertexBuffer(GraphicsDevice, block.Declaration, block.Verts.Length, BufferUsage.None);
			block.VertexBuffer.SetData(block.Verts);

			CanAlterHeight = true;
		}
	}
}