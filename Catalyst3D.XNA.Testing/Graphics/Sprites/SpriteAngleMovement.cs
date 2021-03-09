using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Sprites
{
	[TestFixture]
	public class SpriteAngleMovement : CatalystTestFixture
	{
		private Sprite sprite;

		private const int Angle = 30;
		private const float Speed = 2f;

		[Test]
		public void Test()
		{
			// Create a Sprite Object
			sprite = new Sprite(this);
			sprite.Position = new Vector2(50, 50);
			sprite.Scale = new Vector2(0.2f, 0.2f);
			sprite.Scale = Vector2.Zero;

			// Add it to our games Component Collection so XNA can Update and Draw it automatically
			Components.Add(sprite);

			Run();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our Sprite Objects Texture
			sprite.Texture = Content.Load<Texture2D>("Tree01");
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			float elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds*0.33f;

			sprite.Scale += new Vector2(0.2f*elapsed, 0.2f*elapsed);
			sprite.Position += Utilitys.VelocityFromAngleSpeed(Angle, Speed * sprite.Scale.X);

			if (sprite.Position.Y + (sprite.Texture.Height * sprite.Scale.Y) > Window.ClientBounds.Height)
			{
				sprite.Scale = Vector2.Zero;
				sprite.Position = new Vector2(50, 20);
			}
		}
	}
}
