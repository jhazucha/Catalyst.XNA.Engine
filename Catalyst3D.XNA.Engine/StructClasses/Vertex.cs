using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.StructClasses
{
	public class Vertex
	{
		public struct PositionColored : IVertexType
		{
			public Vector3 Position;
			public Vector4 Color;

			public PositionColored(Vector3 pos, Vector4 col)
			{
				Position = pos;
				Color = col;
			}

			public static int SizeInBytes = (3 + 3) * sizeof(float);

			public static VertexElement[] VertexElements = new[]
			                                               	{
			                                               		new VertexElement(0, VertexElementFormat.Vector3,
			                                               		                  VertexElementUsage.Position, 0),
			                                               		new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3,
			                                               		                  VertexElementUsage.Normal, 0),
			                                               	};

			public VertexDeclaration VertexDeclaration
			{
				get { return new VertexDeclaration(VertexElements); }
			}
		}

		public struct PositionColorTextured : IVertexType
		{
			public Vector3 Position;
			public Vector3 Color;
			public Vector2 TextureCoordinate;

			public PositionColorTextured(Vector3 pos, Vector3 col, Vector2 tex)
			{
				Position = pos;
				Color = col;

				TextureCoordinate = tex;
			}

			public static int SizeInBytes = (3 + 3 + 6) * sizeof(float);

			public static VertexElement[] VertexElements = new[]
			                                               	{
			                                               		new VertexElement(0, VertexElementFormat.Vector3,
			                                               		                  VertexElementUsage.Position, 0),
																																					new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3,
			                                               		                  VertexElementUsage.Color, 0),
			                                               		new VertexElement(sizeof (float)*6, VertexElementFormat.Vector2,
			                                               		                  VertexElementUsage.TextureCoordinate, 0),
			                                               	};

			public VertexDeclaration VertexDeclaration
			{
				get { return new VertexDeclaration(VertexElements); }
			}
		}

		public struct PositionNormal : IVertexType
		{
			public Vector3 Normal;
			public Vector3 Position;

			public static int SizeInBytes = (3 + 3) * sizeof(float);

			public static VertexElement[] VertexElements = new[]
		                                                   {
		                                                     new VertexElement(0, VertexElementFormat.Vector3,
		                                                                       VertexElementUsage.Position, 0),
		                                                     new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3,
		                                                                       VertexElementUsage.Normal, 0),
		                                                   };

			public VertexDeclaration VertexDeclaration
			{
				get { return new VertexDeclaration(VertexElements); }
			}
		}
	}
}