using System;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
  [Serializable]
  public class LedgeNodeDisplay : VisualObject
  {
  	public BoundingBoxRenderer Picker;

    private Texture2D UnSelectedNode;
    private Texture2D SelectedNode;
  	public LedgeBuilder Parent;

		[Browsable(true)]
		public float TravelSpeed { get; set; }

		[Browsable(false)]
		public Sequence2D AnimationSequence { get; set; }

		[Browsable(false)]
		public bool IsLooped { get; set; }

		[Browsable(true)]
		public SpriteEffects SpriteEffect { get; set; }

    public LedgeNodeDisplay() : base(null) { }

    public LedgeNodeDisplay(Game game)
      : base(game)
    {
    	TravelSpeed = 1;
    }

		public override void LoadContent()
		{
			if (Picker == null)
			{
				Picker = new BoundingBoxRenderer(Game);
				Picker.Initialize();
			}

			if (SelectedNode == null)
				SelectedNode = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.MediumSlateBlue, 10, 10);

			if (UnSelectedNode == null)
				UnSelectedNode = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.Orange, 10, 10);

			if (SpriteBatch == null)
				SpriteBatch = Globals.RenderWindow.RenderWindow.SpriteBatch;

			base.LoadContent();
		}

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

			if (Picker != null)
			{
				Picker.Width = (int) SelectedNode.Width;
				Picker.Height = (int) SelectedNode.Height;
				Picker.Position = new Vector2(Position.X + 5, Position.Y + 5);
				Picker.ShowBoundingBox = ShowBoundingBox;
			}

    	if (SelectedNode != null)
			{
				// Update our Bounding Box
				Vector3[] points = new Vector3[2];

				points[0] = new Vector3(Position.X + CameraOffsetX, Position.Y + CameraOffsetY, -5);
				points[1] = new Vector3((Position.X + CameraOffsetX) + (SelectedNode.Width + 10),
				                        (Position.Y + CameraOffsetY) + (SelectedNode.Height + 10), 5);

				BoundingBox = BoundingBox.CreateFromPoints(points);
			}
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

			if (!Visible)
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

		public override void UnloadContent()
		{
			base.UnloadContent();

			SpriteBatch = null;

			SelectedNode = null;
			UnSelectedNode = null;

			if (Picker != null)
			{
				Picker.UnloadContent();
				Picker = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			SpriteBatch = null;

			SelectedNode = null;
			UnSelectedNode = null;
			Picker = null;
		}
  }
}