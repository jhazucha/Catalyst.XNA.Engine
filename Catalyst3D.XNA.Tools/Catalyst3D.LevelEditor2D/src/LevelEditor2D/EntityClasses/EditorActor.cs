using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
	[Category("Animated")]
	public class EditorActor : Actor
	{
	  private conRenderer RenderWindow;

		[Category("Animation Sequences")]
		[Browsable(true)]
		[Editor(typeof(SequenceEditor), typeof(UITypeEditor))]
		public List<Sequence2D> Sequences
		{
			get { return ClipPlayer.Sequences; }
			set { ClipPlayer.Sequences = value; }
		}

		[Browsable(true)]
		[Editor(typeof(ObjectTypeDropDown), typeof(UITypeEditor))]
		[DisplayName(@"Object Type")]
		public new string ObjectTypeName { get; set; }

		[Browsable(true)]
		public string CurrentSequence
		{
			get { return ClipPlayer.CurrentSequence != null ? ClipPlayer.CurrentSequence.Name : string.Empty; }
			set { Play(value, true); }
		}

		public new LedgeBuilder AttachedPathingNode { get; set; }

		public EditorActor(Game game, string folderName, string name, conRenderer renderer)
			: base(game, folderName, name)
		{
		  RenderWindow = renderer; 

			ObjectTypeName = string.Empty;
		}

		public override void Initialize()
		{
			Globals.OnSceneEvent += OnSceneEvent;

			base.Initialize();
		}

		public override void LoadContent()
		{
			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;

			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
				BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}

			if (ClipPlayer.Texture == null)
				ClipPlayer.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(SpriteSheetFileName));

			base.LoadContent();
		}

		private void OnSceneEvent(Enums.SceneState state)
		{
			if (ClipPlayer.CurrentSequence != null)
			{
				switch (state)
				{
					case Enums.SceneState.Playing:
						ClipPlayer.Play(ClipPlayer.CurrentSequence.Name, ClipPlayer.IsLooped);
						break;
					case Enums.SceneState.Stopped:
						{
							CurrentPathLerpPosition = StartPathingLerpPosition;
							CurrentPathNodeIndex = StartPathNodeIndex;

							ClipPlayer.Stop(true);
						}
						break;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Globals.IsScenePaused)
				return;

			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 0.15f;

			if (ClipPlayer.CurrentSequence != null)
			{
				if (BoundingBoxRenderer != null)
				{
          if (RenderWindow.FollowingObject == this)
						BoundingBoxRenderer.Color = Color.Green;
					else
						BoundingBoxRenderer.Color = Color.Yellow;
				}

				if (AttachedPathingNode != null)
				{
					// We are attached to a path check for nodes in the path
					if (AttachedPathingNode.Nodes.Count > 0)
					{
						ClipPlayer.SpriteEffects = AttachedPathingNode.Nodes[CurrentPathNodeIndex].SpriteEffect;

						// Update our draw order to be the same as the current node we are on
						DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

						// See if we should alter our animation sequence
						if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].AnimationSequence != null && !Globals.IsScenePaused && !Globals.IsSceneStopped)
						{
							// Update our Clip Player
							ClipPlayer.Play(AttachedPathingNode.Nodes[CurrentPathNodeIndex].AnimationSequence.Name,
														 AttachedPathingNode.Nodes[CurrentPathNodeIndex].IsLooped);
						}

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
									// Lerping Rotation Calculation ** value1 + (value2 - value1) * amount **
									float n1 = AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Rotation;
									float n2 = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;
									Rotation = n1 + (n2 - n1) * CurrentPathLerpPosition;

									// Moving Back towards the last node
									Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
																					AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Position, CurrentPathLerpPosition);

									// Scaling
									Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
																			 AttachedPathingNode.Nodes[CurrentPathNodeIndex - 1].Scale, CurrentPathLerpPosition);
								}
								else
								{
									// Lerping Rotation Calculation ** value1 + (value2 - value1) * amount **
									float n1 = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;
									float n2 = AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Rotation;
									Rotation = n1 + (n2 - n1) * CurrentPathLerpPosition;


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
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			RenderWindow = null;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			RenderWindow = null;
		}

	}
}
