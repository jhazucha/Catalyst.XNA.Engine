using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.Plugins.Landscape
{
	public class TextureLayer
	{
		[XmlIgnore] public Texture2D Texture;
		[XmlIgnore] public Color[] PaintMask;

		public string AssetName;
	}
}
