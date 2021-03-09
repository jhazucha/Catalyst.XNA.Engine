using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
	[Serializable]
	public class PathNode : VisualObject
	{
		private SpriteBatch SpriteBatch;
		private Texture2D UnSelectedNode;
		private Texture2D SelectedNode;
		private BoundingBoxRenderer Picker;

		public PathNode() : base(null) { }

		public PathNode(Game game)
			: base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			Picker = new BoundingBoxRenderer(Game);
			Picker.Initialize();

			SelectedNode = Utilitys.GenerateTexture(GraphicsDevice, Color.White, 10, 10);
			UnSelectedNode = Utilitys.GenerateTexture(GraphicsDevice, Color.Orange, 10, 10);

			// Init our sprite batch
			SpriteBatch = new SpriteBatch(GraphicsDevice);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Update our Bounding Box
			Vector3[] points = new Vector3[2];
			points[0] = new Vector3(Position.X + CameraOffsetX, Position.Y + CameraOffsetY, -5);
			points[1] = new Vector3((Position.X + CameraOffsetX) + 20,
			                        (Position.Y + CameraOffsetY) + 20, 5);

			BoundingBox = BoundingBox.CreateFromPoints(points);

			if (Picker != null)
			{
				Picker.Position = Position;
				Picker.BoundingBox = BoundingBox;
				Picker.ShowBoundingBox = ShowBoundingBox;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if(!Enabled || !Visible)
				return;

			if(SelectedNode != null && UnSelectedNode != null)
			{
				// Create our Translation Based on our Camera's Offset for nodes we have placed (cursor is exempt from this!)
				Matrix translation = Matrix.CreateTranslation(new Vector3(CameraOffsetX, CameraOffsetY, 0));

				// Begin our sprite batch
				SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, translation);

				if(IsSelected)
					SpriteBatch.Draw(SelectedNode, new Vector2(Position.X + 5, Position.Y + 5), Color.White);
				else
					SpriteBatch.Draw(UnSelectedNode, new Vector2(Position.X + 5, Position.Y + 5), Color.White);

				SpriteBatch.End();
			}
		}
	}
}