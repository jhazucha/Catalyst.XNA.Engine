using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.StructClasses
{
  [StructLayout(LayoutKind.Sequential)]
  public struct CustomLandscapeVertex : IVertexType
  {
    public Vector3 Position;
    public Vector2 TextureCoordinate;
    public Vector3 Normal;

    public static readonly VertexElement[] VertexElements;

    public CustomLandscapeVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
    {
      Position = position;
      Normal = normal;
      TextureCoordinate = textureCoordinate;
    }

		static CustomLandscapeVertex()
		{
			VertexElements = new[]
			                 	{
			                 		new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			                 		new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			                 		new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
			                 	};
		}

  	public VertexDeclaration VertexDeclaration { get { return null;  } }
  }
}