using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2=Microsoft.Xna.Framework.Vector2;

namespace Catalyst3D.XNA.CharacterEditor.EntityClasses
{
	[Serializable]
	public class CTKeyFrameSprite2D
	{
		public bool IsSelected { get; set; }
		public bool Enabled { get; set; }
		public bool Visible { get; set; }
		public int UpdateOrder { get; set; }
		public int DrawOrder { get; set; }
		public string AssetName { get; set; }
		public BoundingBox BoundingBox { get; set; }
		public Vector2 Origin { get; set; }
		public float LayerDepth { get; set; }
		public Vector4 Color { get; set; }
		public SpriteBlendMode BlendMode { get; set; }
		public SpriteEffects Effects { get; set; }
		public float Alpha { get; set; }
		public float Rotation { get; set; }
		public Vector2 Scale { get; set; }
		public Vector2 Position { get; set; }
		public string AssetFilename { get; set; }
		public float FrameSpeed { get; set; }
		public int FrameNumber { get; set; }
	}
}
