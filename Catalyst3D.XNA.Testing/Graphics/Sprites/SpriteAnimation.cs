using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Sprites
{
	[TestFixture]
	public class SpriteAnimation : CatalystTestFixture
	{
		private SpriteBatch spriteBatch;
		private Texture2D texture;

		[Test]
		public void Test()
		{
			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our Sprite Sheet
			texture = Content.Load<Texture2D>("Test2-SpriteSheet");
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null);

			spriteBatch.Draw(texture, new Vector2(0, 0), new Rectangle(0, 0, 108, texture.Height), Color.White);

			spriteBatch.Draw(texture, new Vector2(0, 200), new Rectangle(108, 0, 108, texture.Height), Color.White);

			spriteBatch.End();
		}
	}
}
