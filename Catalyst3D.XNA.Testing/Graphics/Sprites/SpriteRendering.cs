using System;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Sprites
{
	[TestFixture]
	public class SpriteRendering : CatalystTestFixture
	{
		private Sprite sprite;

		[Test]
		public void Render()
		{
			// Create a Sprite Object
			sprite = new Sprite(this);
		  sprite.Position = new Vector2(0, -30);

			// Add it to our games Component Collection so XNA can Update and Draw it automatically
			Components.Add(sprite);

			Run();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our Sprite Objects Texture
			sprite.Texture = Content.Load<Texture2D>("blaster");
		}
	}
}