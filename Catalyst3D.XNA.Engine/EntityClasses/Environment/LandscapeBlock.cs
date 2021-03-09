using System.Collections.Generic;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.StructClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Environment
{
	public class LandscapeBlock : QuadTreeNode
	{
		[XmlIgnore]
		public VertexBuffer VertexBuffer;

		[XmlIgnore]
		public CustomLandscapeVertex[] Verts;

		[XmlIgnore]
		public List<ushort> Indices = new List<ushort>();

		[XmlIgnore]
		public VertexDeclaration Declaration;

		[XmlIgnore]
		private DynamicIndexBuffer ib;

		private int BaseTriangleCount;
		private int EdgeTriangleCount;

		public Vector3 Center;

		public Color Color = Color.White;
		public int Width;
		public int Height;

		public Color[] HeightColorData;

		public int MipLevel;
		public int MaxMipLevel = 2;

		public float Smoothness = 1.0f;

		public float MaxHeight;

		public LandscapeBlock Top;
		public LandscapeBlock Bottom;
		public LandscapeBlock Left;
		public LandscapeBlock Right;

		private readonly int TotalWidth;
		private readonly int TotalHeight;
		private readonly int OffsetX;
		private readonly int OffsetY;

		public LandscapeBlock() : base(null) { /* Required for serialization */ }

		public LandscapeBlock(Game game, int totalWidth, int totalHeight, int offsetX, int offsetY)
			: base(game)
		{
			TotalWidth = totalWidth;
			TotalHeight = totalHeight;
			OffsetX = offsetX;
			OffsetY = offsetY;
		}

		public override void Initialize()
		{
			base.Initialize();

			Declaration = new VertexDeclaration(CustomLandscapeVertex.VertexElements);

			// Create our Vertices
			Verts = new CustomLandscapeVertex[(Width + 1) * (Height + 1)];

			if(VertexBuffer != null)
				VertexBuffer.Dispose();

			for(int x = 0; x <= Width; x++)
			{
				for(int z = 0; z <= Height; z++)
				{
					float width = Position.X + x;
					float height = Position.Y + z;

					Vector3 position;

					if(HeightColorData.Length > 0)
						position = new Vector3(width, HeightColorData[GetIndex(x, z)].R, height);
					else
						position = new Vector3(width, Verts[GetIndex(x, z)].Position.Y, height);

					// Calculate our texture cords
					Vector2 texCoord = new Vector2((float)(x + OffsetX) / TotalWidth, (float)(z + OffsetY) / TotalHeight);

					// Create our vert array
					Verts[GetIndex(x, z)] = new CustomLandscapeVertex(position, Vector3.Zero, texCoord);
				}
			}

			// Calculate our blocks bounding box
			Vector3 bbMin = new Vector3(Position.X, -MaxHeight, Position.Y);
			Vector3 bbMax = new Vector3(Position.X + (Width + 1), MaxHeight, Position.Y + (Height + 1));

			BoundingBox = new BoundingBox(bbMin, bbMax);
			Center = (bbMin + bbMax) * 0.5f;

			BuildBaseIndices(Indices);
			BuildEdgeIndices(Indices, LandscapeBlockEdge.Top);
			BuildEdgeIndices(Indices, LandscapeBlockEdge.Bottom);
			BuildEdgeIndices(Indices, LandscapeBlockEdge.Left);
			BuildEdgeIndices(Indices, LandscapeBlockEdge.Right);

			if(Indices.Count > 0)
			{
				ib = new DynamicIndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, Indices.Count, BufferUsage.WriteOnly);
				ib.SetData(Indices.ToArray());
			}

			// Setup our Normals
			BuildNormals();
		}

		public void BuildNormals()
		{
			for(int i = 0; i < Indices.Count / 3; i++)
			{
				int index1 = Indices[i * 3];
				int index2 = Indices[i * 3 + 1];
				int index3 = Indices[i * 3 + 2];

				Vector3 side1 = Verts[index1].Position - Verts[index3].Position;
				Vector3 side2 = Verts[index1].Position - Verts[index2].Position;
				Vector3 normal = Vector3.Cross(side1, side2);

				Verts[index1].Normal += normal;
				Verts[index2].Normal += normal;
				Verts[index3].Normal += normal;
			}

			for(int v = 0; v < Verts.Length; v++)
				Verts[v].Normal.Normalize();

			// Dispose the VB or memory will leak all over the place!
			if(VertexBuffer != null)
				VertexBuffer.Dispose();

			// Create our Vertex Buffer for the Whole Landscape
			VertexBuffer = new VertexBuffer(GraphicsDevice, Declaration, Verts.Length, BufferUsage.None);
			VertexBuffer.SetData(Verts);
		}

		protected void BuildEdgeIndices(List<ushort> indexes, LandscapeBlockEdge edge)
		{
			LandscapeBlock neighbor = GetBlockFromEdge(edge);

			BuildNormalEdge(indexes, edge);

			if(neighbor != null && neighbor.MipLevel < MipLevel)
			{
				ApplyStitchedEdge(indexes, edge);
			}
		}

		public void BuildBaseIndices(List<ushort> indices)
		{
			EdgeTriangleCount = 0;
			BaseTriangleCount = 0;

			// Setup Vert Stepping
			int stepping = 1 << MipLevel;

			for(int x = stepping; x < Width - stepping; x += stepping)
			{
				for(int z = stepping; z < Height - stepping; z += stepping)
				{
					// triangle 1
					indices.Add(GetIndex(x + stepping, z + stepping));
					indices.Add(GetIndex(x + stepping, z));
					indices.Add(GetIndex(x, z));
					BaseTriangleCount++;

					// triangle 2
					indices.Add(GetIndex(x + stepping, z + stepping));
					indices.Add(GetIndex(x, z));
					indices.Add(GetIndex(x, z + stepping));
					BaseTriangleCount++;
				}
			}
		}

		public void BuildEdgeIndices(List<ushort> indices)
		{
			BuildNormalEdge(indices, LandscapeBlockEdge.Top);
			BuildNormalEdge(indices, LandscapeBlockEdge.Bottom);
			BuildNormalEdge(indices, LandscapeBlockEdge.Left);
			BuildNormalEdge(indices, LandscapeBlockEdge.Right);

			if(Top != null && Top.MipLevel < MipLevel)
			{
				ApplyStitchedEdge(indices, LandscapeBlockEdge.Top);
			}

			if(Bottom != null && Bottom.MipLevel > MipLevel)
			{
				ApplyStitchedEdge(indices, LandscapeBlockEdge.Bottom);
			}

			if(Left != null && Left.MipLevel < MipLevel)
			{
				ApplyStitchedEdge(indices, LandscapeBlockEdge.Left);
			}

			if(Right != null && Right.MipLevel > MipLevel)
			{
				ApplyStitchedEdge(indices, LandscapeBlockEdge.Right);
			}
		}

		private void BuildNormalEdge(ICollection<ushort> Indices, LandscapeBlockEdge edge)
		{
			int step = 1 << MipLevel;
			int startX = 0;
			int startZ = 0;
			int endX = 0;
			int endZ = 0;

			switch(edge)
			{
				case LandscapeBlockEdge.Top:
					startX = 0;
					endX = startX;
					startZ = 0;
					endZ = Width - step;
					break;

				case LandscapeBlockEdge.Bottom:
					startX = Width - step;
					endX = startX;
					startZ = 0;
					endZ = Width - step;
					break;

				case LandscapeBlockEdge.Left:
					startX = 0;
					endX = Width - step;
					startZ = 0;
					endZ = startZ;
					break;

				case LandscapeBlockEdge.Right:
					startX = 0;
					endX = Width - step;
					startZ = Width - step;
					endZ = startZ;
					break;
			}

			for(int x = startX; x <= endX; x += step)
			{
				for(int z = startZ; z <= endZ; z += step)
				{
					#region West

					if(edge == LandscapeBlockEdge.Top)
					{
						if(z == startZ)
						{
							Indices.Add(GetIndex(x, z + step));
							Indices.Add(GetIndex(x + step, z + step));
							Indices.Add(GetIndex(x, z));

							EdgeTriangleCount++;
							continue;
						}
						if(z == endZ)
						{
							Indices.Add(GetIndex(x, z + step));
							Indices.Add(GetIndex(x + step, z));
							Indices.Add(GetIndex(x, z));

							EdgeTriangleCount++;
							continue;
						}
					}

					#endregion West

					#region East

					if(edge == LandscapeBlockEdge.Bottom)
					{
						if(z == startZ)
						{
							// Tri 1
							ushort i1 = GetIndex(x, z + step);
							ushort i2 = GetIndex(x + step, z + step);
							ushort i3 = GetIndex(x + step, z);

							Indices.Add(i1);
							Indices.Add(i2);
							Indices.Add(i3);
							EdgeTriangleCount++;

							continue;
						}

						if(z == endZ)
						{
							Indices.Add(GetIndex(x + step, z + step));
							Indices.Add(GetIndex(x + step, z));
							Indices.Add(GetIndex(x, z));

							EdgeTriangleCount++;

							continue;
						}
					}

					#endregion East

					#region North

					if(edge == LandscapeBlockEdge.Left)
					{
						if(x == startX)
						{
							Indices.Add(GetIndex(x + step, z + step));
							Indices.Add(GetIndex(x + step, z));
							Indices.Add(GetIndex(x, z));
							EdgeTriangleCount++;

							continue;
						}

						if(x == endX)
						{
							Indices.Add(GetIndex(x, z + step));
							Indices.Add(GetIndex(x + step, z));
							Indices.Add(GetIndex(x, z));
							EdgeTriangleCount++;

							continue;
						}
					}

					#endregion North

					#region South

					if(edge == LandscapeBlockEdge.Right)
					{
						if(x == startX)
						{
							Indices.Add(GetIndex(x, z + step));
							Indices.Add(GetIndex(x + step, z + step));
							Indices.Add(GetIndex(x + step, z));
							EdgeTriangleCount++;

							continue;
						}

						if(x == endX)
						{
							Indices.Add(GetIndex(x, z + step));
							Indices.Add(GetIndex(x + step, z + step));
							Indices.Add(GetIndex(x, z));

							EdgeTriangleCount++;

							continue;
						}
					}

					#endregion South

					// Tri 1
					Indices.Add(GetIndex(x, z + step));
					Indices.Add(GetIndex(x + step, z));
					Indices.Add(GetIndex(x, z));
					EdgeTriangleCount++;

					// Tri 2
					Indices.Add(GetIndex(x, z + step));
					Indices.Add(GetIndex(x + step, z + step));
					Indices.Add(GetIndex(x + step, z));
					EdgeTriangleCount++;
				}
			}
		}

		private void ApplyStitchedEdge(ICollection<ushort> Indices, LandscapeBlockEdge edge)
		{
			int step = 1 << MipLevel;
			int startX = 0;
			int startZ = 0;
			int endX = 0;
			int endZ = 0;

			switch(edge)
			{
				case LandscapeBlockEdge.Top:
					startX = 0;
					endX = startX;
					startZ = 0;
					endZ = Width - step;
					break;

				case LandscapeBlockEdge.Bottom:
					startX = Width - step;
					endX = startX;
					startZ = 0;
					endZ = Width - step;
					break;

				case LandscapeBlockEdge.Left:
					startX = 0;
					endX = Width - step;
					startZ = 0;
					endZ = startZ;
					break;

				case LandscapeBlockEdge.Right:
					startX = 0;
					endX = Width - step;
					startZ = Width - step;
					endZ = startZ;
					break;
			}

			for(int x = startX; x <= endX; x += step)
			{
				for(int z = startZ; z <= endZ; z += step)
				{
					if(MipLevel == 1)
					{
						#region Top

						if(edge == LandscapeBlockEdge.Top)
						{
							if(z == startZ)
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add((ushort)(i1 + 2));
								Indices.Add((ushort)(i2 + (endZ + 1)));
								Indices.Add(i3); // start

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 + (endZ + 1)));
								Indices.Add((ushort)(i3 + 2));

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Top

						#region Bottom

						if(edge == LandscapeBlockEdge.Bottom)
						{
							if(z == endZ)
							{
								ushort i1 = GetIndex(x + step, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 + (endX + 3)));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x + step, z);

								Indices.Add(i1);
								Indices.Add(i2);
								Indices.Add((ushort)(i3 + endX + 3));

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Bottom

						#region Right

						if(edge == LandscapeBlockEdge.Right)
						{
							if(x == endX)
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 1));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x + step, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 1));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Right

						#region Left

						if(edge == LandscapeBlockEdge.Left)
						{
							if(x == startX)
							{
								ushort i1 = GetIndex(x + step, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add(i2);
								Indices.Add((ushort)(i3 + 1));

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 1));
								Indices.Add(i3);

								EdgeTriangleCount++;
							}
						}

						#endregion Left
					}
					else if(MipLevel == 2)
					{
						#region Top

						if(edge == LandscapeBlockEdge.Top)
						{
							if(z == startZ)
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add(i2);
								Indices.Add((ushort)(i3 + (endZ * 2) + 10));

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add((ushort)(i1 - ((endZ * 2) + 10)));
								Indices.Add(i2);
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Top

						#region Bottom

						if(edge == LandscapeBlockEdge.Bottom)
						{
							if(z == endZ)
							{
								ushort i1 = GetIndex(x + step, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add((ushort)(i1 - ((Width + 1) + (Width + 1))));
								Indices.Add(i2);
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								// Tri 1
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x + step, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - ((Width + 1) + (Width + 1))));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Bottom

						#region Right

						if(edge == LandscapeBlockEdge.Right)
						{
							if(x == endX)
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 2));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z + step);
								ushort i3 = GetIndex(x + step, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 2));
								Indices.Add(i3);

								EdgeTriangleCount++;

								continue;
							}
						}

						#endregion Right

						#region Left

						if(edge == LandscapeBlockEdge.Left)
						{
							if(x == startX)
							{
								ushort i1 = GetIndex(x + step, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add(i2);
								Indices.Add((ushort)(i3 + 2));

								EdgeTriangleCount++;

								continue;
							}
							else
							{
								ushort i1 = GetIndex(x, z + step);
								ushort i2 = GetIndex(x + step, z);
								ushort i3 = GetIndex(x, z);

								Indices.Add(i1);
								Indices.Add((ushort)(i2 - 2));
								Indices.Add(i3);

								EdgeTriangleCount++;
							}
						}

						#endregion Left
					}
					else if(MipLevel == 3)
					{
					}
				}
			}
		}

		public ushort GetIndex(int x, int z)
		{
			return (ushort)(x + z * (Width + 1));
		}

		public LandscapeBlock GetBlockFromEdge(LandscapeBlockEdge edge)
		{
			switch(edge)
			{
				case LandscapeBlockEdge.Top:
					return Top;
				case LandscapeBlockEdge.Bottom:
					return Bottom;
				case LandscapeBlockEdge.Left:
					return Left;
				case LandscapeBlockEdge.Right:
					return Right;
			}
			return null;
		}

		public void ChangeMipLevel(int level)
		{
			if(level > MaxMipLevel)
				level = MaxMipLevel;

			if(MipLevel == level)
				return;

			MipLevel = level;

			Indices = new List<ushort>();

			BuildBaseIndices(Indices);
			BuildEdgeIndices(Indices);

			//if(Top != null)
			//  Top.ChangeMipLevel(Top.MipLevel);

			//if(Bottom != null)
			//  Bottom.ChangeMipLevel(Bottom.MipLevel);

			if(Indices.Count > 0)
			{
				ib = new DynamicIndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, Indices.Count, BufferUsage.WriteOnly);
				ib.SetData(Indices.ToArray());
			}
		}

		public override void Update(GameTime gameTime)
		{
			int level = 0;
			float distance = Vector3.Distance(new Vector3(Camera.Position.X, 0, Camera.Position.Z),
																				new Vector3(Center.X, 0, Center.Z));

			float ratio = (float)Width / MaxMipLevel;

			if(distance < Width - 1)
				level = 0;

			if(distance > Width + 1)
				level = 1;

			if(distance > Width * 2)
				level = 2;

			if(distance > Width * 3)
				level = 3;

			if(distance > Width * 4)
				level = 4;

			ChangeMipLevel(level);
		}

		public int DrawBlock(GameTime gameTime)
		{
			// Count our Triangles
			int TotalTriCount = BaseTriangleCount + EdgeTriangleCount;

			if(TotalTriCount > 0)
			{
				if(!VertexBuffer.IsDisposed)
				{
					// Set our Index Buffer and Vertex Buffer on our Graphics Device
					GraphicsDevice.Indices = ib;
					GraphicsDevice.SetVertexBuffer(VertexBuffer);

					// Draw our Indices
					GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Verts.Length, 0, TotalTriCount);
				}
			}

			return TotalTriCount;
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			if(VertexBuffer != null)
				VertexBuffer.Dispose();

			if(ib != null)
				ib.Dispose();

			VertexBuffer = null;
			ib = null;
		}

		public bool Intersects(Ray ray)
		{
			// If its not visible then dont test
			if(!Visible)
				return false;

			BoundingBox box = new BoundingBox(new Vector3(Position.X, -MaxHeight, Position.Y),
																				new Vector3(Position.X + Width, MaxHeight, Position.Y + Height));

			return ray.Intersects(box) > 0;
		}
	}
}