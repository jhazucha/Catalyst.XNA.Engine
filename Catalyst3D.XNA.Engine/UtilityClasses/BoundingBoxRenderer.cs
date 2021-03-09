using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class BoundingBoxRenderer : VisualObject
	{
		private Color _color = Color.Yellow;

		public Texture2D BoxTexture;

		public Color Color
		{
			get { return _color; }
			set { _color = value; }
		}

		public float Width;
		public float Height;

		public bool IsPositionCentered;

		public BoundingBoxRenderer(Game game)
			: base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!Visible || !ShowBoundingBox)
				BoxTexture = null;

			// Build it off our bounding box we passed in
			Width = BoundingBox.Max.X - BoundingBox.Min.X;
			Height = BoundingBox.Max.Y - BoundingBox.Min.Y;

			Position = new Vector2(BoundingBox.Min.X, BoundingBox.Min.Y);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (ShowBoundingBox && BoxTexture == null)
				BoxTexture = Utilitys.GenerateTexture(Game.GraphicsDevice, Color, 1, 1);

			if(SpriteBatch != null && ShowBoundingBox && !SpriteBatch.IsDisposed && BoxTexture != null)
			{
				// Translate it into place
				Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, 0);

				// Begin the sprite batch
				SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend, null, null, null, null, translation);

				// Top Line
				SpriteBatch.Draw(BoxTexture, new Rectangle(0, 0, (int)Width, 1), Color.White);

				// Left Line
				SpriteBatch.Draw(BoxTexture, new Rectangle(0, 0, 1, (int)Height), Color.White);

				// Right Line
				SpriteBatch.Draw(BoxTexture, new Rectangle((int)Width, 0, 1, (int)Height), Color.White);

				// Bottom Line
				SpriteBatch.Draw(BoxTexture, new Rectangle(0, (int)Height, (int)Width, 1), Color.White);

				// End our sprite batch
				SpriteBatch.End();
			}
		}

		public override void UnloadContent()
		{
			BoxTexture = null;

			base.UnloadContent();
		}
	}
}