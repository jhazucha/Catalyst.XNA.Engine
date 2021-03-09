using System.Collections.Generic;
using System.Linq;
using Catalyst3D.XNA.Engine.AbstractClasses;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
  public class VisualObjectGroup : VisualObject
	{
		private float _startLerpPosition;
		private int _startPathNodeIndex;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
    public List<VisualObject> Objects { get; set; }

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
		[ContentSerializer]
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

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public new string AssetFolder { get; set; }

    public VisualObjectGroup() : base(null)
    {
    	Objects = new List<VisualObject>();
    }

    public VisualObjectGroup(Game game)
      : base(game)
    {
      Objects = new List<VisualObject>();
    }

		public override void Initialize()
		{
			base.Initialize();

			foreach (var o in Objects)
			{
        if (!o.IsInitialized)
        {
          o.SpriteBatch = SpriteBatch;
          o.GameScreen = GameScreen;

          if (ContentManager != null)
            o.ContentManager = ContentManager;

          o.Initialize();
        }
			}
		}

  	public override void Update(GameTime gameTime)
		{
			if (!Enabled)
				return;

			base.Update(gameTime);

#if WINDOWS
				float elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds*0.15f;
#endif

#if WINDOWS_PHONE
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds*0.33f;
#endif

			var objects = (from db in Objects orderby db.UpdateOrder ascending select db).ToList();

			for (int i = 0; i < objects.Count; i++)
			{
				if (AttachedPathingNode != null)
				{
					// We are attached to a path check for nodes in the path
					if (AttachedPathingNode.Nodes.Count > 0)
					{
						// Update our draw order to be the same as the current node we are on
						if (AttachedPathingNode.UseNodesDrawOrder)
							DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

						// Update our rotation to match our nodes rotation
						Rotation = AttachedPathingNode.Nodes[CurrentPathNodeIndex].Rotation;

						// Make sure we cannot move past the last node in the chain
						if (CurrentPathNodeIndex > (AttachedPathingNode.Nodes.Count - 1))
							CurrentPathNodeIndex = 0;

						if (AttachedPathingNode.IsTraveling)
						{
							if (CurrentPathNodeIndex < (AttachedPathingNode.Nodes.Count - 1))
							{
								// Lerp to our next node

								CurrentPathLerpPosition += (AttachedPathingNode.Nodes[CurrentPathNodeIndex].TravelSpeed * elapsed);

								if (CurrentPathLerpPosition >= 1)
								{
									if (CurrentPathNodeIndex < (AttachedPathingNode.Nodes.Count - 1))
									{
										CurrentPathNodeIndex++;
										CurrentPathLerpPosition = 0;
									}
								}

								// Check to see we are on the last node in the chain
								if (CurrentPathNodeIndex == (AttachedPathingNode.Nodes.Count - 1))
								{
									// we want to respawn back to the first so reset everything
									if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.RespawnAtFirstNode)
									{
										Visible = true;

										// Reset to the begining pathing node
										CurrentPathNodeIndex = 0;

										// Reset our Lerp
										CurrentPathLerpPosition = 0;
									}
									else if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.LerpToFirstNode)
									{
										// TODO: Lerp back to the first node
									}
									else if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.StopAtLastNode)
									{
										AttachedPathingNode.IsTraveling = false;
										CurrentPathLerpPosition = 0;
									}
									else if (AttachedPathingNode.LedgeTavelAlgo == LedgeTravelAlgo.HideAfterLastNode)
									{
										Visible = false;

										// Reset to the begining pathing node
										CurrentPathNodeIndex = 0;

										// Reset our Lerp
										CurrentPathLerpPosition = 0;
									}
								}
								else
								{
									if (i > 0)
									{
										// Grab our neighbor
										Vector2 LeftPostion = Objects[i - 1].Position;

										if (Objects[i - 1] is Sprite)
										{
											Sprite s = Objects[i - 1] as Sprite;

											if (s != null && s.Texture != null)
											{
												// Match the scale of our neighbor
												Objects[i].Scale = s.Scale;

												// First node position it right ontop of our pathing node
												Objects[i].Position = new Vector2(LeftPostion.X + (s.Texture.Width * s.Scale.X), LeftPostion.Y);
											}
										}
									}
									else
									{
										// Moving
										Objects[i].Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
																											 AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Position,
																											 CurrentPathLerpPosition);

										// Scaling
										Objects[i].Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
																										AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Scale,
																										CurrentPathLerpPosition);
									}
								}
							}
						}
						else
						{
							if (i > 0)
							{
								Vector2 LeftPostion = Objects[i - 1].Position;

								if (Objects[i - 1] is Sprite)
								{
									Sprite s = Objects[i - 1] as Sprite;

									if (s != null && s.Texture != null)
									{
										// Match the scale of our neighbor
										Objects[i].Scale = s.Scale;

										// First node position it right ontop of our pathing node
										Objects[i].Position = new Vector2(LeftPostion.X + (s.Texture.Width * s.Scale.X), LeftPostion.Y);
									}
								}
							}
							else
							{
								// Moving
								Objects[i].Position = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Position,
																									 AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Position,
																									 CurrentPathLerpPosition);

								// Scaling
								Objects[i].Scale = Vector2.Lerp(AttachedPathingNode.Nodes[CurrentPathNodeIndex].Scale,
																								AttachedPathingNode.Nodes[CurrentPathNodeIndex + 1].Scale,
																								CurrentPathLerpPosition);
							}
						}
					}
					else
					{
						if (i > 0)
						{
							Vector2 LeftPostion = Objects[i - 1].Position;

							if (Objects[i - 1] is Sprite)
							{
								Sprite s = Objects[i - 1] as Sprite;

								if (s != null && s.Texture != null)
								{
									// Match the scale of our neighbor
									//Objects[i].Scale = s.Scale;

									// First node position it right ontop of our pathing node
									Objects[i].Position = new Vector2(LeftPostion.X + (s.Texture.Width * s.Scale.X), LeftPostion.Y);
								}
							}
						}
						else
						{
							// Moving
							Objects[i].Position = Position;
						}

						Objects[i].CameraOffsetX = CameraOffsetX;
						Objects[i].CameraOffsetY = CameraOffsetY;
						Objects[i].CameraZoomOffset = CameraZoomOffset;
					}
				}
				else
				{
					//Objects[i].Scale = Scale;
					Objects[i].CameraOffsetX = CameraOffsetX;
					Objects[i].CameraOffsetY = CameraOffsetY;
					Objects[i].CameraZoomOffset = CameraZoomOffset;
				}

				Objects[i].Update(gameTime);
			}
		}

  	public override void Draw(GameTime gameTime)
    {
      if(!Visible)
        return;

      base.Draw(gameTime);

      var objects = (from db in Objects orderby db.DrawOrder ascending select db).ToList();

      foreach (VisualObject vo in objects)
        vo.Draw(gameTime);
    }

		public override void UnloadContent()
		{
			base.UnloadContent();

			foreach (var v in Objects)
				v.UnloadContent();

		}

		protected override void Dispose(bool disposing)
		{
			foreach (var v in Objects)
				v.Dispose();

			base.Dispose(disposing);
		}
  }
}