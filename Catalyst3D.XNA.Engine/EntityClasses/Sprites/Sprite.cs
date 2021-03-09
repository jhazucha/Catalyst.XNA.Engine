using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#if !XBOX360 && !WINDOWS_PHONE
using Microsoft.Xna.Framework.Design;
#endif

namespace Catalyst3D.XNA.Engine.EntityClasses.Sprites
{
	[XmlType(IncludeInSchema = true)]
	public class Sprite : VisualObject
	{
		public event CustomShader SetShaderParameters;

		private const float Circle = MathHelper.Pi * 2;
		private BlendState _BlendMode = BlendState.AlphaBlend;
		private SpriteEffects _SpriteEffect = SpriteEffects.None;
		private Color _Color = Color.White;
		private float _Depth = 1;
		private float _startLerpPosition;
		private int _startPathNodeIndex;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public Vector2 Center { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BoundingBoxRenderer BoundingBoxRenderer;

		[ContentSerializerIgnore, XmlIgnore]
		public Vector2 BoundingBoxPadding { get; set; }

		[ContentSerializerIgnore, XmlIgnore]
		public Texture2D Texture;

		public float ScrollSpeed { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public Vector2 StartPosition { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public Effect CustomEffect { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public Vector2 Origin { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public float LayerDepth
		{
			get { return _Depth; }
			set { _Depth = value; }
		}

#if !XBOX360 && !WINDOWS_PHONE
		[TypeConverter(typeof(ColorConverter))]
#endif
		public Color Color
		{
			get { return _Color; }
			set { _Color = value; }
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore]
		public BlendState BlendMode
		{
			get { return _BlendMode; }
			set { _BlendMode = value; }
		}

		public SpriteEffects Effects
		{
			get { return _SpriteEffect; }
			set { _SpriteEffect = value; }
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public int StartPathNodeIndex
		{
			get { return _startPathNodeIndex; }
			set
			{
				_startPathNodeIndex = value;
				CurrentPathNodeIndex = value;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public float CurrentPathLerpPosition { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public float StartPathingLerpPosition
		{
			get { return _startLerpPosition; }
			set
			{
				_startLerpPosition = value;
				CurrentPathLerpPosition = value;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public bool IsLerpingBackToFirstNode { get; set; }

		public Sprite() : base(null) { }

		public Sprite(Game game)
			: base(game)
		{
			Scale = Vector2.One;
		}

		public Sprite(Game game, bool Centered)
			: base(game)
		{
			Scale = Vector2.One;
			IsCentered = Centered;
		}

		public override void LoadContent()
		{
			base.LoadContent();

      if (GameScreen != null && GameScreen.SpriteBatch != null && SpriteBatch == null)
        SpriteBatch = GameScreen.SpriteBatch;
      
      if (Texture == null || Texture.IsDisposed)
      {
        if (!string.IsNullOrEmpty(AssetName) && GameScreen != null && ContentManager == null)
          Texture = GameScreen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
        else if (!string.IsNullOrEmpty(AssetName) && ContentManager != null)
          Texture = ContentManager.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
        else if (!string.IsNullOrEmpty(AssetName))
          Texture = Game.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
      }

		  // Debug Bounding Box Renderer
			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
				BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Texture != null && Enabled)
			{
				float tw = Texture.Width * Scale.X;
				float th = Texture.Height * Scale.Y;

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 0.33f;

        if (AttachedPathingNode != null && AttachedPathingNode.Nodes.Count > 0)
        {
          Effects = AttachedPathingNode.Nodes[CurrentPathNodeIndex].SpriteEffect;

          // Update our draw order to be the same as the current node we are on
					if (AttachedPathingNode.UseNodesDrawOrder)
						DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

          if (CurrentPathNodeIndex <= (AttachedPathingNode.Nodes.Count - 1))
          {
            if (AttachedPathingNode.IsTraveling)
            {
              // Lerp to our next node
              CurrentPathLerpPosition += (AttachedPathingNode.Nodes[CurrentPathNodeIndex].TravelSpeed * elapsed);
            }

            if (CurrentPathLerpPosition >= 1)
            {
              if (IsLerpingBackToFirstNode)
              {
                CurrentPathNodeIndex--;
                CurrentPathLerpPosition = 0;
              }
              else
              {
                if (CurrentPathNodeIndex < (AttachedPathingNode.Nodes.Count - 1))
                {
                  if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].RespawnDelay > TimeSpan.Zero)
                  {
                    if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime >
                        AttachedPathingNode.Nodes[CurrentPathNodeIndex].RespawnDelay)
                    {
                      // purge the last node elapsed time
                      AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime = TimeSpan.Zero;

                      AttachedPathingNode.IsTraveling = true;

                      CurrentPathNodeIndex++;
                      CurrentPathLerpPosition = 0;
                    }
                    else
                    {
                      AttachedPathingNode.IsTraveling = false;
                      CurrentPathLerpPosition = 1;
                      AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime += gameTime.ElapsedGameTime;
                    }
                  }
                  else
                  {
                    CurrentPathNodeIndex++;
                    CurrentPathLerpPosition = 0;
                  }
                }
              }
            }

            // Check to see we are on the last node in the chain
            if (CurrentPathNodeIndex == (AttachedPathingNode.Nodes.Count - 1) && !IsLerpingBackToFirstNode && AttachedPathingNode.IsTraveling)
            {
              CurrentPathLerpPosition = 0;

              // we want to respawn back to the first so reset everything
              switch (AttachedPathingNode.LedgeTavelAlgo)
              {
                case LedgeTravelAlgo.RespawnAtFirstNode:
                  CurrentPathNodeIndex = 0;
                  break;
                case LedgeTravelAlgo.LerpToFirstNode:
                  IsLerpingBackToFirstNode = true;
                  break;
                case LedgeTravelAlgo.StopAtLastNode:
                  AttachedPathingNode.IsTraveling = false;
                  break;
              }
            }
            else if (CurrentPathNodeIndex == 0 && IsLerpingBackToFirstNode)
            {
              IsLerpingBackToFirstNode = false;
              CurrentPathLerpPosition = 0;
            }
            else
            {
              // Continue to move along the path
              if (IsLerpingBackToFirstNode)
              {
                // Lerping Rotation Calculation ** value1 + (value2 - value1) * amount **
                float n1 = AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Rotation;
                float n2 = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;
                Rotation = n1 + (n2 - n1) * CurrentPathLerpPosition;

                // Moving Back towards the last node
                Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
                                        AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Position,
                                        CurrentPathLerpPosition);

                // Scaling
                Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
                                     AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Scale, CurrentPathLerpPosition);
              }
              else
              {
                if ((CurrentPathNodeIndex + 1) <= (AttachedPathingNode.Nodes.Count - 1))
                {
                  // Lerping Rotation Calculation ** value1 + (value2 - value1) * amount **
                  float n1 = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;
                  float n2 = AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Rotation;
                  Rotation = n1 + (n2 - n1) * CurrentPathLerpPosition;


                  // Moving
                  Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
                                          AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Position,
                                          CurrentPathLerpPosition);

                  // Scaling
                  Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
                                       AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Scale, CurrentPathLerpPosition);
                }
              }
            }
          }
        }

			  if (IsCentered)
				{
					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(Position.X + CameraOffsetX - (tw / 2) - BoundingBoxPadding.X,
																	Position.Y + CameraOffsetY - (th / 2) - BoundingBoxPadding.Y, -5);

					points[1] = new Vector3(Position.X + CameraOffsetX + (tw / 2) + BoundingBoxPadding.X,
																	Position.Y + CameraOffsetY + (th / 2) + BoundingBoxPadding.Y, 5);

					// Calculate the center of our sprite
					Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																			(int)(points[1].X - points[0].X),
																			(int)(points[1].Y - points[0].Y));

					Center = new Vector2(r.Center.X, r.Center.Y);

					// Calculate our Origin
					Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);


					// Create our Bounding Box
					BoundingBox = BoundingBox.CreateFromPoints(points);
				}
				else
				{
					Origin = Vector2.Zero;

					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(Position.X + CameraOffsetX - BoundingBoxPadding.X,
																	Position.Y + CameraOffsetY - BoundingBoxPadding.Y, -5);

					points[1] = new Vector3(Position.X + CameraOffsetX + tw + BoundingBoxPadding.X,
																	Position.Y + CameraOffsetY + th + BoundingBoxPadding.Y, 5);

					// Create our Bounding Box
					BoundingBox = BoundingBox.CreateFromPoints(points);

					// Calculate the center of our sprite
					Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																			(int)(points[1].X - points[0].X),
																			(int)(points[1].Y - points[0].Y));

					Center = new Vector2(r.Center.X, r.Center.Y);
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
			if (!Visible || !Enabled || SpriteBatch == null)
				return;

			if (Texture != null && !Texture.IsDisposed)
			{
				// Create our Translation Based on our Camera's Offset factoring a speed value
				float x = (CameraOffsetX + (CameraOffsetX * ScrollSpeed));

				// Create our Translation Matrix Based off the camera's X offset
				Matrix translation = Matrix.CreateTranslation(new Vector3(x, CameraOffsetY, 0));

				if (SetShaderParameters != null)
					SetShaderParameters.Invoke(gameTime);

				// Render with a custom effect
				if (CustomEffect != null)
				{
					// Begin our Sprite Batch
					SpriteBatch.Begin(SpriteSortMode, BlendMode, null, null, null, CustomEffect, translation);

					// Loop thru Each Pass in our Shader
					foreach (EffectPass pass in CustomEffect.CurrentTechnique.Passes)
					{
						pass.Apply();

						// Begin rendering with this sprite mode
						SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height),
															Color, Rotation % Circle, Origin, Scale, _SpriteEffect, _Depth);
					}

					// End the sprite batch
					SpriteBatch.End();
				}
				else
				{
					// Begin our Sprite Batch | WHY IS SAMPLER STATE, DEPTH AND RASTER STATE PROPERTYS?
					SpriteBatch.Begin(SpriteSortMode, BlendMode, null, null, null, null, translation);
					
					SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color,
														Rotation % Circle, Origin, Scale, _SpriteEffect, _Depth);

					SpriteBatch.End();
				}
			}

			if (ShowBoundingBox)
				BoundingBoxRenderer.Draw(gameTime);

			base.Draw(gameTime);
		}

		public override void UnloadContent()
		{
			Texture = null;
			CustomEffect = null;

			if (BoundingBoxRenderer != null)
				BoundingBoxRenderer.UnloadContent();

			base.UnloadContent();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Texture = null;
			AttachedPathingNode = null;
		}

		~Sprite()
		{
			Texture = null;
			AttachedPathingNode = null;
		}
	}
}