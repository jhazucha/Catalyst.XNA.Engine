using System.ComponentModel;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Text
{
	[XmlType(IncludeInSchema = true)]
	public class Label : VisualObject
	{
		private Vector2 CenteredPosition;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BoundingBoxRenderer BoundingBoxRenderer;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public SpriteFont Font;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BasicEffect SimpleEffect;

		public string Text = string.Empty;

		public BlendState BlendState = BlendState.AlphaBlend;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public SpriteEffects SpriteEffects = SpriteEffects.None;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public Color FontColor = Color.Black;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public Color ShadowColor = Color.DarkSlateGray;

		public float LayerDepth;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public Vector2 Length = Vector2.Zero;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public Vector2 ShadowOffset;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public bool IsShadowVisible { get; set; }

		public Label()
			: base(null)
		{
		}

		public Label(Game game, string asset)
			: base(game)
		{
			AssetName = asset;
		}

    public Label(Game game, SpriteFont font)
      : base(game)
    {
      Font = font;
    }

    public override void LoadContent()
    {
      base.LoadContent();

      if (GameScreen != null && GameScreen.SpriteBatch != null && SpriteBatch == null)
        SpriteBatch = GameScreen.SpriteBatch;

      if (Font == null)
      {
        if (!string.IsNullOrEmpty(AssetName) && GameScreen != null && ContentManager == null)
          Font = GameScreen.Content.Load<SpriteFont>(AssetName);
        else if (!string.IsNullOrEmpty(AssetName) && ContentManager != null)
          Font = ContentManager.Load<SpriteFont>(AssetName);
        else if (!string.IsNullOrEmpty(AssetName) && GameScreen == null)
          Font = Game.Content.Load<SpriteFont>(AssetName);
      }

      // Debug Bounding Box Renderer
      BoundingBoxRenderer = new BoundingBoxRenderer(Game);
      BoundingBoxRenderer.Initialize();
    }

	  public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Get our fonts length
			if (Font != null)
			{
				Length = Font.MeasureString(Text);

				// Store our origin
				if (IsCentered)
				{
					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(Position.X + CameraOffsetX - (Length.X/2)*Scale.X,
					                        Position.Y + CameraOffsetY - (Length.Y/2)*Scale.Y, -5);

					points[1] = new Vector3(Position.X + CameraOffsetX + Length.X*Scale.X,
																	Position.Y + CameraOffsetY + Length.Y*Scale.Y, 5);


					// Calculate the center of our sprite
					Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																			(int)(points[1].X - points[0].X),
																			(int)(points[1].Y - points[0].Y));

					CenteredPosition = new Vector2(r.Center.X, r.Center.Y);

					// Create our Bounding Box
					BoundingBox = BoundingBox.CreateFromPoints(points);
				}
				else
				{
					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(Position.X + CameraOffsetX*Scale.X,
					                        Position.Y + CameraOffsetY*Scale.Y, -5);

					points[1] = new Vector3(Position.X + CameraOffsetX + (Length.X*Scale.X),
					                        Position.Y + CameraOffsetY + (Length.Y*Scale.Y), 5);

					// Create our Bounding Box
					BoundingBox = BoundingBox.CreateFromPoints(points);

					// Calculate the center of our sprite
					Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																			(int)(points[1].X - points[0].X),
																			(int)(points[1].Y - points[0].Y));

					CenteredPosition = new Vector2(r.Center.X, r.Center.Y);
				}
			}

			if (ShowBoundingBox && Enabled)
			{
				// Update the Bounding Box Renderer
				BoundingBoxRenderer.ShowBoundingBox = ShowBoundingBox;
				BoundingBoxRenderer.BoundingBox = BoundingBox;

				BoundingBoxRenderer.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (GameScreen.SpriteBatch != null && Font != null && Visible)
			{
				// Store our previous renderstate
				RasterizerState previousState = GraphicsDevice.RasterizerState;

				// Make sure this is rendered in solid mode
				RasterizerState newState = new RasterizerState
				                           	{
				                           		CullMode = previousState.CullMode,
				                           		DepthBias = previousState.DepthBias,
				                           		FillMode = FillMode.Solid,
				                           		MultiSampleAntiAlias = previousState.MultiSampleAntiAlias,
				                           		Name = previousState.Name,
				                           		ScissorTestEnable = previousState.ScissorTestEnable,
				                           		SlopeScaleDepthBias = previousState.SlopeScaleDepthBias,
				                           		Tag = previousState.Tag
				                           	};

				// Set our fillmode to solid so if wireframe is enabled it does not destroy our text!
				GraphicsDevice.RasterizerState = newState;

				// Begin the Sprite Batch and tell it our blending mode to use
				GameScreen.SpriteBatch.Begin(SpriteSortMode, BlendState);

				if (IsCentered)
				{
					// Draw the drop shadow if its visible
					if (IsShadowVisible)
						GameScreen.SpriteBatch.DrawString(Font, Text,
						                       new Vector2(CenteredPosition.X + ShadowOffset.X, CenteredPosition.Y - ShadowOffset.Y),
						                       ShadowColor, Rotation, new Vector2(Length.X/2, Length.Y/2), Scale, SpriteEffects,
						                       LayerDepth + 1);

					// Draw our text string
					GameScreen.SpriteBatch.DrawString(Font, Text, CenteredPosition, FontColor, Rotation, new Vector2(Length.X / 2, Length.Y / 2),
					                       Scale, SpriteEffects, LayerDepth);
				}
				else
				{
					// Draw the drop shadow if its visible
					if (IsShadowVisible)
						GameScreen.SpriteBatch.DrawString(Font, Text, new Vector2(Position.X + ShadowOffset.X, Position.Y - ShadowOffset.Y),
						                       ShadowColor, Rotation, new Vector2(Length.X/2, Length.Y/2), Scale, SpriteEffects,
						                       LayerDepth + 1);

					// Draw our text string
					GameScreen.SpriteBatch.DrawString(Font, Text, Position, FontColor, Rotation, Vector2.Zero, Scale,
					                       SpriteEffects, LayerDepth);
				}

				// Done stop drawing our sprite batch
				GameScreen.SpriteBatch.End();

				// Reset our device with our previous renderstate
				GraphicsDevice.RasterizerState = previousState;

				if (ShowBoundingBox)
					BoundingBoxRenderer.Draw(gameTime);
			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			Font = null;
			BoundingBoxRenderer = null;
		}
	}
}