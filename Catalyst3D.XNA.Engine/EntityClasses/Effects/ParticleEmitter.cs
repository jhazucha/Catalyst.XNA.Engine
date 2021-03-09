using System;
using System.Collections.Generic;
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

namespace Catalyst3D.XNA.Engine.EntityClasses.Effects
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	public class ParticleEmitter : VisualObject
	{
		public delegate void ParticleEvent(Particle p);

		private TimeSpan currentElapsedTime;
		private float startLerpPosition;
		private int startPathNodeIndex;
		private Queue<Particle> freeParticles = new Queue<Particle>();

		[ContentSerializerIgnore, XmlIgnore]
		public ParticleEvent OnParticleSpawningEvent;

		[ContentSerializerIgnore, XmlIgnore]
		public Texture2D Texture;

		public int CurrentAliveParticleCount;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BoundingBoxRenderer BoundingBoxRenderer;

#if !XBOX360 && !WINDOWS_PHONE
		[TypeConverter(typeof(List<Vector2Converter>))]
#endif
		public List<Vector2> ForceObjects = new List<Vector2>();

		[ContentSerializerIgnore, XmlIgnore]
		public List<Particle> Particles = new List<Particle>();

#if !XBOX360 && !WINDOWS_PHONE
		[TypeConverter(typeof(ColorConverter))]
		[CategoryAttribute("Apperance")]
#endif
		public Color ParticleColor { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[TypeConverter(typeof(Vector2Converter))]
#endif
		public Vector2 Origin { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public int MinParticles { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public int MaxParticles { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MinAcceleration { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MaxAcceleration { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MinRotationSpeed { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MaxRotationSpeed { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MinLifeSpan { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MaxLifeSpan { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MinScale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MaxScale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MinInitialSpeed { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[CategoryAttribute("Settings")]
#endif
		public float MaxInitialSpeed { get; set; }

		private bool _respawnParticles;

		[DefaultValue(true)]
		public bool RespawnParticles
		{
			get { return _respawnParticles; }
			set
			{
				if (value)
				{
					AddParticles(MaxParticles);
				}

				_respawnParticles = value;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public new int CurrentPathNodeIndex { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		public int StartPathNodeIndex
		{
			get { return startPathNodeIndex; }
			set
			{
				startPathNodeIndex = value;
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
			get { return startLerpPosition; }
			set
			{
				startLerpPosition = value;
				CurrentPathLerpPosition = value;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public bool IsLerpingBackToFirstNode { get; set; }

		public TimeSpan DelayedStartTime;

		// Required for Serialization
		public ParticleEmitter() : base(null) { }

		public ParticleEmitter(Game game)
			: base(game)
		{
			new ParticleEmitter(game, Vector2.Zero);
		}

		public ParticleEmitter(Game game, Vector2 pos)
			: base(game)
		{
			RespawnParticles = true;

			// Store our position
			Position = pos;

			// Load Some Light Rotation
			MinRotationSpeed = -MathHelper.PiOver4 / 2.0f;
			MaxRotationSpeed = MathHelper.PiOver4 / 2.0f;
		}

		public override void Initialize()
		{
			base.Initialize();

			Particles = new List<Particle>(MaxParticles);
			freeParticles = new Queue<Particle>(MaxParticles);

			for (int i = 0; i < MinParticles; i++)
			{
				Particle part = new Particle();
				part.Color = ParticleColor;
				part.Texture = Texture;

				// first, call PickRandomDirection to figure out which way the particle
				// will be moving. velocity and acceleration's values will come from this.
				Vector2 direction = Utilitys.PickRandomDirection();

				// pick some random values for our particle
				float velocity = Utilitys.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
				float acceleration = Utilitys.RandomBetween(MinAcceleration, MaxAcceleration);
				float lifeSpan = Utilitys.RandomBetween(MinLifeSpan, MaxLifeSpan);
				float scale = Utilitys.RandomBetween(MinScale, MaxScale);
				float rotationSpeed = Utilitys.RandomBetween(MinRotationSpeed, MaxRotationSpeed);

				// then initialize it with those random values. initialize will save those,
				// and make sure it is marked as active.
				part.Initialize(Position, velocity * direction, acceleration * direction, lifeSpan, scale, rotationSpeed);

				// Enqueue it in our free particles queue
				freeParticles.Enqueue(part);

				// Load it to our particles collection
				Particles.Add(part);

				if (OnParticleSpawningEvent != null)
					OnParticleSpawningEvent.Invoke(part);
			}

			// Debug Bounding Box Renderer
			BoundingBoxRenderer = new BoundingBoxRenderer(Game);
			BoundingBoxRenderer.Initialize();
		}

    public override void LoadContent()
    {
      base.LoadContent();

      if (GameScreen.SpriteBatch != null && SpriteBatch == null)
        SpriteBatch = GameScreen.SpriteBatch;

      if (Texture == null)
      {
        if (!string.IsNullOrEmpty(AssetName) && GameScreen != null && ContentManager == null)
          Texture = GameScreen.Content.Load<Texture2D>(AssetFolder + "/" + Path.GetFileNameWithoutExtension(AssetName));
        else if (!string.IsNullOrEmpty(AssetName) && ContentManager != null)
          Texture = ContentManager.Load<Texture2D>(AssetFolder + "/" + Path.GetFileNameWithoutExtension(AssetName));
        else if (!string.IsNullOrEmpty(AssetName))
          Texture = Game.Content.Load<Texture2D>(AssetFolder + "/" + Path.GetFileNameWithoutExtension(AssetName));
      }
    }

	  public override void Update(GameTime gameTime)
		{
			if (!Enabled)
				return;

			// Allow for a delayed start time (fireworks for instance or random flames)
			if (currentElapsedTime > DelayedStartTime)
			{
				// Show our particles
				Visible = true;

				// calculate dt, the change in the since the last frame. the particle updates will use this value.
				float dt = (float)gameTime.ElapsedGameTime.TotalSeconds * 0.33f;

				#region Pathing Nodes

        if (AttachedPathingNode != null && AttachedPathingNode.Nodes.Count > 0)
        {
          //Effects = AttachedPathingNode.Nodes[CurrentPathNodeIndex].SpriteEffect;

          // Update our draw order to be the same as the current node we are on
					if (AttachedPathingNode.UseNodesDrawOrder)
						DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

          if (CurrentPathNodeIndex <= (AttachedPathingNode.Nodes.Count - 1))
          {
            if (AttachedPathingNode.IsTraveling)
            {
              // Lerp to our next node
              CurrentPathLerpPosition += (AttachedPathingNode.Nodes[CurrentPathNodeIndex].TravelSpeed * dt);
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

				#endregion

				#region Forces

				// apply force objects
				foreach (Vector2 force in ForceObjects)
				{
					foreach (Particle p in Particles)
						p.Position += force;
				}

				#endregion

				if (Origin == Vector2.Zero && Texture != null)
					Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);

				// go through all of the particles...
				foreach (Particle p in Particles)
				{
					p.Update(dt);

					if (!p.Active)
					{
						if (!RespawnParticles)
						{
							CurrentAliveParticleCount--;

							if (CurrentAliveParticleCount < 0)
								CurrentAliveParticleCount = 0;

							continue;
						}

						Particle part = freeParticles.Dequeue();
						part.Color = ParticleColor;
						part.Texture = Texture;

						// first, call PickRandomDirection to figure out which way the particle
						// will be moving. velocity and acceleration's values will come from this.
						Vector2 direction = Utilitys.PickRandomDirection();

						// pick some random values for our particle
						float velocity = Utilitys.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
						float acceleration = Utilitys.RandomBetween(MinAcceleration, MaxAcceleration);
						float lifetime = Utilitys.RandomBetween(MinLifeSpan, MaxLifeSpan);
						float scale = Utilitys.RandomBetween(MinScale, MaxScale);
						float rotationSpeed = Utilitys.RandomBetween(MinRotationSpeed, MaxRotationSpeed);

						// Init our particle with new values
						part.Initialize(Position, velocity * direction, acceleration * direction, lifetime, scale, rotationSpeed);

						CurrentAliveParticleCount++;

						freeParticles.Enqueue(part);

						if (OnParticleSpawningEvent != null)
							OnParticleSpawningEvent.Invoke(part);
					}
				}

				if (ShowBoundingBox && Texture != null && Enabled)
				{
					// Update our Bounding Box around our picker
					Vector3[] points = new Vector3[2];
					points[0] = new Vector3((Position.X + CameraOffsetX) - (Texture.Width * MaxScale) / 2,
																	Position.Y + CameraOffsetY - (Texture.Height * MaxScale) / 2, -1);

					points[1] = new Vector3((Position.X + CameraOffsetX) + (Texture.Width * MaxScale) / 2,
																	(Position.Y + CameraOffsetY) + (Texture.Height * MaxScale) / 2, -1);

					BoundingBox = BoundingBox.CreateFromPoints(points);

					// Update the Bounding Box Renderer
					BoundingBoxRenderer.ShowBoundingBox = ShowBoundingBox;
					BoundingBoxRenderer.BoundingBox = BoundingBox;

					BoundingBoxRenderer.Update(gameTime);
				}

				base.Update(gameTime);
			}
			else
			{
				// Make sure we hide it
				Visible = false;

				// Increase our elapsed run time
				currentElapsedTime += gameTime.ElapsedGameTime;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (Texture == null || SpriteBatch == null || !Enabled || !Visible || Particles.Count == 0 || Texture.IsDisposed)
				return;

			// Create our Translation Based on our Camera's Offset
			Matrix translation = Matrix.CreateTranslation(new Vector3(CameraOffsetX, CameraOffsetY, 0));

			// Begin our sprite batch
			SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend, null, null, null, null, translation);

			foreach (Particle p in Particles)
			{
				// skip inactive particles
				if (!p.Active)
					continue;

				float normalizedLifetime = p.TimeSinceStart / p.LifeSpan;

				float alpha = (2 / normalizedLifetime) * (1 - normalizedLifetime);

				Color color = ParticleColor * alpha;

				float scale = p.Scale * (.75f + .25f * normalizedLifetime);

				SpriteBatch.Draw(Texture, p.Position, null, color, p.Rotation, Origin, scale, SpriteEffects.None, 0.0f);
			}

			SpriteBatch.End();

			if (ShowBoundingBox)
				BoundingBoxRenderer.Draw(gameTime);

			base.Draw(gameTime);
		}

		public void AddParticles(int count)
		{
			CurrentAliveParticleCount += count;

			for (int i = 0; i < count; i++)
			{
				Particle p = new Particle();

				// first, call PickRandomDirection to figure out which way the particle
				// will be moving. velocity and acceleration's values will come from this.
				Vector2 direction = Utilitys.PickRandomDirection();

				// pick some random values for our particle
				float velocity = Utilitys.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
				float acceleration = Utilitys.RandomBetween(MinAcceleration, MaxAcceleration);
				float lifetime = Utilitys.RandomBetween(MinLifeSpan, MaxLifeSpan);
				float scale = Utilitys.RandomBetween(MinScale, MaxScale);
				float rotationSpeed = Utilitys.RandomBetween(MinRotationSpeed, MaxRotationSpeed);

				// then initialize it with those random values. initialize will save those,
				// and make sure it is marked as active.
				p.Initialize(Position, velocity * direction, acceleration * direction, lifetime, scale, rotationSpeed);

				freeParticles.Enqueue(p);

				Particles.Add(p);
			}
		}

		public void AddParticles(Vector2 pos, int count)
		{
			Particles.Clear();

			CurrentAliveParticleCount += count;

			for (int i = 0; i < count; i++)
			{
				Particle p = new Particle();

				// first, call PickRandomDirection to figure out which way the particle
				// will be moving. velocity and acceleration's values will come from this.
				Vector2 direction = Utilitys.PickRandomDirection();

				// pick some random values for our particle
				float velocity = Utilitys.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
				float acceleration = Utilitys.RandomBetween(MinAcceleration, MaxAcceleration);
				float lifetime = Utilitys.RandomBetween(MinLifeSpan, MaxLifeSpan);
				float scale = Utilitys.RandomBetween(MinScale, MaxScale);
				float rotationSpeed = Utilitys.RandomBetween(MinRotationSpeed, MaxRotationSpeed);

				// then initialize it with those random values. initialize will save those,
				// and make sure it is marked as active.
				p.Initialize(pos, velocity * direction, acceleration * direction, lifetime, scale, rotationSpeed);

				freeParticles.Enqueue(p);
				Particles.Add(p);
			}
		}

		public void FlushParticles()
		{
			Visible = false;

			CurrentAliveParticleCount = 0;

			freeParticles.Clear();
			Particles.Clear();

			Visible = true;
		}

		public override void UnloadContent()
		{
			freeParticles.Clear();
			
			Texture = null;
			
			base.UnloadContent();
		}
	}
}