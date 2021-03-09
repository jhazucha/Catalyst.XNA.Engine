using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Sprites
{
	public class SpriteMovement : CatalystTestFixture
	{
		private Actor Cow;
		public Vector2 SpawnPosition = new Vector2(200, 100);
		public Vector2 EndPosition = new Vector2(400, 400);
		public Vector2 EndScale = new Vector2(0.7f, 0.7f);

		public float lerpValue;

		[Test]
		public void Test()
		{
			GraphicsManager.PreferredBackBufferHeight = 480;
			GraphicsManager.PreferredBackBufferWidth = 800;

			Cow = new Actor(this, string.Empty, "Cow");
			Cow.IsCentered = true;
			Components.Add(Cow);

			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			// Play our Idle Sequence
			Cow.Play("Idle", true);

			Cow.Scale = Vector2.Zero;

			Cow.Position = SpawnPosition;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			lerpValue += (float) gameTime.ElapsedGameTime.TotalSeconds*0.5f;

			if(Cow.ClipPlayer.CurrentSequence != null)
			{
				Cow.Scale = Vector2.Lerp(Vector2.Zero, EndScale, MathHelper.Min(lerpValue, 1));
				Cow.Position = Vector2.Lerp(SpawnPosition - Cow.Scale, EndPosition, MathHelper.Min(lerpValue, 1));
			}
		}
	}
}
