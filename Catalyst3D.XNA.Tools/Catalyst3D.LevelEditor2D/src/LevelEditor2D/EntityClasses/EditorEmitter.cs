using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
	[Serializable]
	public class EditorEmitter : ParticleEmitter
	{
	  private conRenderer RenderWindow;

		[Category("Forces")]
		[Browsable(true)]
		[Editor(typeof(EmitterForceEditor), typeof(UITypeEditor))]
		public new List<Vector2> ForceObjects
		{
			get { return base.ForceObjects; }
			set { base.ForceObjects = value; }
		}

		public new LedgeBuilder AttachedPathingNode { get; set; }

		[Browsable(true)]
		[Editor(typeof(ObjectTypeDropDown), typeof(UITypeEditor))]
		[DisplayName(@"Object Type")]
		public new string ObjectTypeName { get; set; }

		public new int MaxParticles
		{
			get { return base.MaxParticles; }
			set
			{
				if (value != base.MaxParticles)
				{
					base.MaxParticles = value;
					Initialize();
				}
			}
		}
		public new int MinParticles
		{
			get { return base.MinParticles; }
			set
			{
				if (value != base.MinParticles)
				{
					base.MinParticles = value;
					Initialize();
				}
			}
		}

		public EditorEmitter(Game game, conRenderer renderer)
			: base(game)
		{
		  RenderWindow = renderer;

			ShowBoundingBox = true;
			RespawnParticles = true;

			ParticleColor = Color.Gray;
			MinParticles = 6;
			MaxParticles = 10;
			MinAcceleration = 0.5f;
			MaxAcceleration = 2.0f;
			MinLifeSpan = 0.1f;
			MaxLifeSpan = 1.0f;
			MinInitialSpeed = 0.5f;
			MaxInitialSpeed = 2.0f;
			MinScale = 0.1f;
			MaxScale = 1.0f;
		}

		public override void LoadContent()
		{
			Globals.OnSceneEvent += OnSceneEvent;

			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;

			if (Texture == null)
				Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																					 Globals.GetDestinationTexturePath(AssetName));

			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
				BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}

			base.LoadContent();
		}

		private void OnSceneEvent(Enums.SceneState state)
		{
			switch (state)
			{
				case Enums.SceneState.Playing:
					AddParticles(MaxParticles);
					break;
				case Enums.SceneState.Stopped:
					{
						Particles.Clear();

						CurrentPathLerpPosition = StartPathingLerpPosition;
						CurrentPathNodeIndex = StartPathNodeIndex;
					}
					break;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Globals.IsScenePaused)
				return;

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 0.15f;

			// Update our Camera's Offset
			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

			if (RenderWindow.FollowingObject == this)
				BoundingBoxRenderer.Color = Color.Green;
			else
				BoundingBoxRenderer.Color = Color.Yellow;

			if (AttachedPathingNode != null)
			{
				// We are attached to a path check for nodes in the path
				if (AttachedPathingNode.Nodes.Count > 0)
				{
					// Update our draw order to be the same as the current node we are on
					DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

					// Update our rotation to match our nodes rotation
					Rotation = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;

					if (CurrentPathNodeIndex <= (AttachedPathingNode.Nodes.Count - 1))
					{
						if (AttachedPathingNode.IsTraveling && !Globals.IsScenePaused)
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
									CurrentPathNodeIndex++;
									CurrentPathLerpPosition = 0;
								}
							}
						}

						// Check to see we are on the last node in the chain
						if (CurrentPathNodeIndex == (AttachedPathingNode.Nodes.Count - 1) && !IsLerpingBackToFirstNode)
						{
							// we want to respawn back to the first so reset everything
							if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.RespawnAtFirstNode)
							{
								// Reset to the begining pathing node
								CurrentPathNodeIndex = 0;

								// Reset our Lerp
								CurrentPathLerpPosition = 0;
							}
							else if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.LerpToFirstNode)
							{
								// Flag we are lerping back to the last node
								IsLerpingBackToFirstNode = true;
								CurrentPathLerpPosition = 0;
							}
							else if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.StopAtLastNode)
							{
								AttachedPathingNode.IsTraveling = false;
								CurrentPathLerpPosition = 0;
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
								// Moving Back towards the last node
								Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
																				AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Position, CurrentPathLerpPosition);

								// Scaling
								Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
																		 AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Scale, CurrentPathLerpPosition);
							}
							else
							{
								// Moving
								Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
																				AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Position, CurrentPathLerpPosition);

								// Scaling
								Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
																		 AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Scale, CurrentPathLerpPosition);
							}
						}
					}
				}
			}

			if (IsSelected)
			{
				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = true;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = true;
				}

				ShowBoundingBox = true;
			}
			else
			{
				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = false;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = false;
				}

				ShowBoundingBox = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			RenderWindow = null;
		}
	}
}