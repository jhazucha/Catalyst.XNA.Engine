using System.ComponentModel;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Sprites
{
	[XmlType(IncludeInSchema = true)]
	public class SpriteBox : Sprite
	{
		private int width;
		private int height;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public int Width
		{
			get { return width; }
			set
			{
				if (value != Width && Texture != null)
					Texture = Utilitys.GenerateTexture(GraphicsDevice, Color, value, Height);

				width = value;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public int Height
		{
			get { return height; }
			set
			{
				if (value != Height && Texture != null)
					Texture = Utilitys.GenerateTexture(GraphicsDevice, Color, Width, value);

				height = value;
			}
		}

		public SpriteBox() : base(null) { }

		public SpriteBox(Game game)
			: base(game)
		{
			Color = Color.White;

			ObjectTypeName = string.Empty;

			width = 100;
			height = 25;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			if (Texture == null)
				Texture = Utilitys.GenerateTexture(GraphicsDevice, Color, Width, Height);
		}
	}
}