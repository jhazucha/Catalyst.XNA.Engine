using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Sprites
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	public class AnimationPlayer2D : VisualObject
	{
		#region Variables and Propertys

		private float CurrentElapsedTime;
		private List<Sequence2D> sequences = new List<Sequence2D>();

		[ContentSerializerIgnore]
		public SpriteEffects SpriteEffects { get; set; }

		public string AssetPath;

		private const float Circle = MathHelper.Pi * 2;

		public Vector2 Origin;
		public Vector2 BoundingBoxPadding;
		public Color Color = Color.White;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new Vector2 Position { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new Vector2 Scale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new float Rotation { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new int UpdateOrder { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new int DrawOrder { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new bool Visible { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new bool Enabled { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new Vector2 Velocity { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new string Name { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[XmlIgnore, ContentSerializerIgnore]
		public Texture2D Texture;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[XmlIgnore, ContentSerializerIgnore]
		public Effect CustomEffect;

		public event CustomShader SetShaderParameters;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public List<Sequence2D> Sequences
		{
			get { return sequences; }
			set { sequences = value; }
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializerIgnore]
		public Sequence2D CurrentSequence;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore]
		public int CurrentFrameIndex;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[XmlIgnore, ContentSerializerIgnore]
		public int Width
		{
			get
			{
				if (CurrentSequence != null && CurrentSequence.Frames.Count < CurrentFrameIndex)
				{
					return (int)(CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle.Width * Scale.X);
				}
				return 0;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[XmlIgnore, ContentSerializerIgnore]
		public int Height
		{
			get
			{
				if (CurrentSequence != null && CurrentSequence.Frames.Count < CurrentFrameIndex)
				{
					return (int)(CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle.Height * Scale.Y);
				}
				return 0;
			}
		}

		public bool IsFlipped;
		public bool IsLooped;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore]
		public bool IsPlaying = true;

		#endregion

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[XmlIgnore, ContentSerializerIgnore]
		public BasicEffect BasicEffect;

		public AnimationPlayer2D() : base(null) { }

		public AnimationPlayer2D(Game game)
			: base(game)
		{
			SpriteEffects = SpriteEffects.None;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			if (BasicEffect == null)
			{
				BasicEffect = new BasicEffect(Game.GraphicsDevice);
				BasicEffect.World = Matrix.Identity;
				BasicEffect.PreferPerPixelLighting = true;
			}

			// Load our Sprite Sheet)
			if (Texture == null || Texture.IsDisposed)
			{
				if (!string.IsNullOrEmpty(AssetName) && GameScreen != null && ContentManager == null)
					Texture = GameScreen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
				else if (!string.IsNullOrEmpty(AssetName) && ContentManager != null)
					Texture = ContentManager.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
				else if (!string.IsNullOrEmpty(AssetName))
					Texture = Game.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
			}

			// Display our first frame of our first sequence out the gate
			if (Sequences.Count > 0 && CurrentSequence == null)
				CurrentSequence = Sequences[0];
		}

		public void Play(string name, bool loop)
		{
			if (CurrentSequence != null)
			{
				if (!IsPlaying)
					IsPlaying = true;

				if (name == CurrentSequence.Name && IsLooped == loop)
					return;
			}

			IsLooped = loop;

			var sequence = (from s in Sequences where s.Name == name select s).SingleOrDefault();

			if (sequence != null)
			{
				IsPlaying = true;

				CurrentSequence = sequence;
				CurrentElapsedTime = 0;
				CurrentFrameIndex = sequence.StartFrame;

			}
			else
			{
				CurrentSequence = null;

				Stop(true);
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Calculates 0.0333f * 30 = 1 Frame per second!!
			CurrentElapsedTime += elapsedTime * 30f;

			if (IsPlaying && CurrentSequence != null && CurrentSequence.Frames.Count > 0)
			{
				// Are we passed the frame duration?
				if (CurrentElapsedTime > CurrentSequence.Frames[CurrentFrameIndex].Duration)
				{
					if (IsLooped)
					{
						if (CurrentFrameIndex == CurrentSequence.EndFrame)
						{
							IsPlaying = true;

							// Reset our elapsed frame time
							CurrentElapsedTime = 0;

							// Start back at the beginning
							CurrentFrameIndex = CurrentSequence.StartFrame;
						}
						else
						{
							IsPlaying = true;

							// Reset our elapsed frame time
							CurrentElapsedTime = 0;

							// Move to the next frame
							CurrentFrameIndex++;
						}
					}
					else
					{
						if (CurrentFrameIndex == CurrentSequence.EndFrame)
						{
							CurrentElapsedTime = 0;
							CurrentFrameIndex = CurrentSequence.EndFrame;

							IsPlaying = false;
						}
						else
						{
							CurrentElapsedTime = 0;
							IsPlaying = true;

							CurrentFrameIndex++;
						}
					}
				}
			}

			if (Texture != null && CurrentSequence != null && CurrentSequence.Frames.Count > 0)
			{
				// Store our origin
				if (IsCentered)
					Origin = new Vector2((float)CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle.Width / 2,
															 (float)CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle.Height / 2);
				else
					Origin = Vector2.Zero;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (Texture != null && GameScreen != null && GameScreen.SpriteBatch != null && CurrentSequence != null && CurrentSequence.Frames.Count > 0 && CurrentFrameIndex < CurrentSequence.Frames.Count)
			{
				if (Texture.IsDisposed)
					return;

				Vector3 pos = Vector3.Add(new Vector3(Position.X, Position.Y, 0),
																	new Vector3(CameraOffsetX, CameraOffsetY, 0));

				// Create our Translation Based on our Camera's Offset
				Matrix translation = Matrix.CreateTranslation(pos);

				if (SetShaderParameters != null)
					SetShaderParameters.Invoke(gameTime);

				if (CustomEffect != null)
				{
					// Begin our Sprite Batch
					GameScreen.SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend, null, null, null, CustomEffect, translation);

					// Loop thru Each Pass in our Shader
					foreach (EffectPass pass in CustomEffect.CurrentTechnique.Passes)
					{
						// Begin the pass
						pass.Apply();

						GameScreen.SpriteBatch.Draw(Texture, Vector2.Zero, CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle,
														 Color.White, Rotation % Circle, Origin, Scale, SpriteEffects, 0f);
					}

					// End the sprite batch
					GameScreen.SpriteBatch.End();
				}
				else
				{

					// Begin our Sprite Batch
					GameScreen.SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend, null, null, null, null, translation);

					GameScreen.SpriteBatch.Draw(Texture, Vector2.Zero,
													 CurrentSequence.Frames[CurrentFrameIndex].SourceRectangle, Color,
													 Rotation % Circle, Origin, Scale, SpriteEffects, 0f);

					GameScreen.SpriteBatch.End();
				}
			}
		}

		public void Stop(bool reset)
		{
			if (CurrentSequence == null)
				return;

			IsPlaying = false;
			CurrentFrameIndex = CurrentSequence.StartFrame;
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			Texture = null;
			CustomEffect = null;
			BasicEffect = null;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Texture = null;
		}
	}
}