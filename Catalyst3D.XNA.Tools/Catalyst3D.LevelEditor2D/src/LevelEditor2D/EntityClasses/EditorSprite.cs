using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
	public class EditorSprite : Sprite
	{
		public List<Vector2> PreviousState = new List<Vector2>();

		[Browsable(true)]
		[Editor(typeof(ObjectTypeDropDown), typeof(UITypeEditor))]
		[DisplayName(@"Object Type")]
		public new string ObjectTypeName { get; set; }

		public new LedgeBuilder AttachedPathingNode { get; set; }

		[Browsable(false)]
		public bool ToggleBoundingBox;

		public new Vector2 Position
		{
			get { return base.Position; }
			set
			{
				PreviousState.Add(value);
				base.Position = value;
			}
		}

		public EditorSprite(Game game)
			: base(game)
		{
			ObjectTypeName = string.Empty;

			SetShaderParameters += OnShaderParams;

			Globals.ObjectUndo += OnObjectUndo;
			Globals.OnSceneEvent += OnSceneEvent;
		}

		private void OnSceneEvent(Enums.SceneState state)
		{
			if (state == Enums.SceneState.Stopped)
			{
				CurrentPathLerpPosition = StartPathingLerpPosition;
				CurrentPathNodeIndex = StartPathNodeIndex;
			}
		}

		private void OnObjectUndo(VisualObject sprite)
		{
			if (sprite != this)
				return;

			if (PreviousState.Count > 0)
			{
				// Reset the position back
				Position = PreviousState[PreviousState.Count - 1];
				PreviousState.RemoveAt(PreviousState.Count - 1);
			}
		}

		private void OnShaderParams(GameTime gametime)
		{
			CustomEffect.Parameters["Color"].SetValue(Color.ToVector4());
			CustomEffect.Parameters["IsSelected"].SetValue(IsSelected);
		}

		public override void LoadContent()
		{
			CustomEffect = GameScreen.Content.Load<Effect>("SpriteShader");

			if (Texture == null)
				Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(AssetName));

			BoundingBoxRenderer = new BoundingBoxRenderer(Game);
			BoundingBoxRenderer.Initialize();

			if (AttachedPathingNode != null)
				AttachedPathingNode.Initialize();

			base.LoadContent();
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

			if (AttachedPathingNode != null)
			{
				// We are attached to a path check for nodes in the path
				if (AttachedPathingNode.Nodes.Count > 0)
				{
					Effects = AttachedPathingNode.Nodes[CurrentPathNodeIndex].SpriteEffect;

					// Update our draw order to be the same as the current node we are on
					DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

					if (CurrentPathNodeIndex <= (AttachedPathingNode.Nodes.Count - 1))
					{
						if (AttachedPathingNode.IsTraveling && !Globals.IsScenePaused)
						{
							// Lerp to our next node
							CurrentPathLerpPosition += (AttachedPathingNode.Nodes[CurrentPathNodeIndex].TravelSpeed*elapsed);
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
								Rotation = n1 + (n2 - n1)*CurrentPathLerpPosition;

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
								Rotation = n1 + (n2 - n1)*CurrentPathLerpPosition;

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

			if (BoundingBoxRenderer != null)
			{
				BoundingBoxRenderer.ShowBoundingBox = ShowBoundingBox;

				if (Globals.RenderWindow.FollowingObject != null && Globals.RenderWindow.FollowingObject == this)
					BoundingBoxRenderer.Color = Color.Green;
				else
					BoundingBoxRenderer.Color = Color.Yellow;

			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			if (AttachedPathingNode != null)
				AttachedPathingNode.UnloadContent();
		}
	}
}