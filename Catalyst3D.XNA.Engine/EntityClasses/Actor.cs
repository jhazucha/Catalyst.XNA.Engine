using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
#if !WINDOWS_PHONE
  [Serializable]
#endif
  public class Actor : VisualObject
  {
		private float _startLerpPosition;
		private int _startPathNodeIndex;

#if !XBOX360 && !WINDOWS_PHONE
    [Browsable(false)]
#endif
    [ContentSerializer(SharedResource = true)]
    public AnimationPlayer2D ClipPlayer { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public int Width
		{
			get
			{
				if(ClipPlayer.CurrentSequence != null)
				{
					if(ClipPlayer.CurrentSequence.Frames.Count <= 0)
						return 0;

					return ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Width;
				}
				return 0;
			}
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
    public int Height
    {
			get
			{
				if (ClipPlayer.CurrentSequence != null)
				{
					if(ClipPlayer.CurrentSequence.Frames.Count <= 0)
						return 0;

					return ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Height;
				}
				return 0;
			}
    }

    [ContentSerializerIgnore]
    public ActorRole Role { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
    [ContentSerializerIgnore]
    public PlayerIndex PlayerIndex { get; set; }

    [ContentSerializerIgnore]
    public Direction Direction { get; set; }

		[ContentSerializerIgnore]
		public SpriteEffects SpriteEffects { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
  	[Browsable(true)]
#endif
  	public bool IsLooped
  	{
			get
			{
				if(ClipPlayer != null)
					return ClipPlayer.IsLooped;
				return false;
			}
			set
			{
				if(ClipPlayer != null)
					ClipPlayer.IsLooped = value;
			}
  	}

		[ContentSerializerIgnore, XmlIgnore]
    public BoundingBoxRenderer BoundingBoxRenderer;

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

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public string SpriteSheetFileName { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BasicEffect BasicEffect { get { return ClipPlayer.BasicEffect; } set { ClipPlayer.BasicEffect = value; } }

    public Actor() : base(null) { }

		public Actor(Game game, string assetFolder, string assetName, ContentManager contentManager)
			: base(game)
		{
			AssetName = assetName;
			AssetFolder = assetFolder;
			Scale = Vector2.One;
			Direction = Direction.Right;

			// Create a new clip player
			ClipPlayer = new AnimationPlayer2D(Game);

			SpriteEffects = SpriteEffects.None;

			ContentManager = contentManager;
		}

  	public override void Initialize()
		{
			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
			  BoundingBoxRenderer.GameScreen = GameScreen;
			  BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}

		

  		base.Initialize();
		}

    public override void LoadContent()
    {
      base.LoadContent();

			if (!string.IsNullOrEmpty(AttachedPathingNodeName) && AttachedPathingNode == null)
				AttachedPathingNode = GameScreen.GetPath(AttachedPathingNodeName);

      // Check to see if this was pre-loaded
			if (string.IsNullOrEmpty(SpriteSheetFileName))
			{
				Actor original;

				if (ContentManager != null)
					original = ContentManager.Load<Actor>(AssetName);
				else
					original = GameScreen.Content.Load<Actor>(AssetName);

				if (original == null)
					throw new Exception("Error: Could not load the Actor!");

				Direction = original.Direction;
				DrawOrder = original.DrawOrder;
				UpdateOrder = original.UpdateOrder;
				Name = original.Name;
				PlayerIndex = original.PlayerIndex;
				Role = original.Role;
				Enabled = original.Enabled;
				Visible = original.Visible;
				SpriteSheetFileName = original.SpriteSheetFileName;

				ClipPlayer = new AnimationPlayer2D(Game)
				             	{
				             		AssetName = original.ClipPlayer.AssetName,
				             		Sequences = original.ClipPlayer.Sequences
				             	};
			}

			ClipPlayer.GameScreen = GameScreen;
			ClipPlayer.SetShaderParameters += OnSetShaderParams;
			ClipPlayer.AssetPath = AssetFolder;
    	ClipPlayer.AssetName = SpriteSheetFileName;
			ClipPlayer.Game = Game;
			ClipPlayer.GameScreen = GameScreen;
			ClipPlayer.SpriteBatch = SpriteBatch;
			ClipPlayer.ContentManager = ContentManager;
			ClipPlayer.Initialize();

      if (BoundingBoxRenderer == null)
      {
        BoundingBoxRenderer = new BoundingBoxRenderer(Game);
        BoundingBoxRenderer.GameScreen = GameScreen;
        BoundingBoxRenderer.SpriteBatch = SpriteBatch;
        BoundingBoxRenderer.Initialize();
      }
    }

    public virtual void OnSetShaderParams(GameTime gametime)
    {
    }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!Enabled)
				return;

			float elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds*0.33f;

			switch (Direction)
			{
				case Direction.Left:
					ClipPlayer.IsFlipped = true;
					break;
				case Direction.Right:
					ClipPlayer.IsFlipped = false;
					break;
			}

			if (ClipPlayer.Texture != null && ClipPlayer.CurrentSequence != null && ClipPlayer.CurrentSequence.Frames.Count > 0)
			{
				#region Pathing Nodes

				if (AttachedPathingNode != null)
				{
					// We are attached to a path check for nodes in the path
					if (AttachedPathingNode.Nodes.Count > 0)
					{
						SpriteEffects = AttachedPathingNode.Nodes[CurrentPathNodeIndex].SpriteEffect;

						// Update our draw order to be the same as the current node we are on
						if (AttachedPathingNode.UseNodesDrawOrder)
							DrawOrder = AttachedPathingNode.Nodes[CurrentPathNodeIndex].DrawOrder;

						// See if we should alter our animation sequence
						if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].AnimationSequence != null && CurrentPathLerpPosition == 0)
						{
							// Update our Clip Player
							ClipPlayer.Play(AttachedPathingNode.Nodes[CurrentPathNodeIndex].AnimationSequence.Name,
							                AttachedPathingNode.Nodes[CurrentPathNodeIndex].IsLooped);
						}

						if (CurrentPathNodeIndex <= (AttachedPathingNode.Nodes.Count - 1))
						{
							if (AttachedPathingNode.IsTraveling)
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
                switch (AttachedPathingNode.LedgeTavelAlgo)
                {
                  case LedgeTravelAlgo.RespawnAtFirstNode:
                    if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].RespawnDelay > TimeSpan.Zero)
                    {
                      if (AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime >
                          AttachedPathingNode.Nodes[CurrentPathNodeIndex].RespawnDelay)
                      {
                        // purge the last node elapsed time
                        AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime = TimeSpan.Zero;

                        // Reset to the begining pathing node
                        CurrentPathNodeIndex = 0;

                        // Reset our Lerp
                        CurrentPathLerpPosition = 0;
                      }
                      else
                      {
                        AttachedPathingNode.Nodes[CurrentPathNodeIndex].NodeElapsedTime += gameTime.ElapsedGameTime;
                      }
                    }
                    else
                    {
                      // Reset to the begining pathing node
                      CurrentPathNodeIndex = 0;

                      // Reset our Lerp
                      CurrentPathLerpPosition = 0;
                    }
                    break;
                  case LedgeTravelAlgo.LerpToFirstNode:
                    IsLerpingBackToFirstNode = true;
                    CurrentPathLerpPosition = 0;
                    break;
                  case LedgeTravelAlgo.StopAtLastNode:
                    AttachedPathingNode.IsTraveling = false;
                    CurrentPathLerpPosition = 0;
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

				#endregion

				// Incase it walks past the max frame count on the sequences
				if (ClipPlayer.CurrentFrameIndex >= ClipPlayer.CurrentSequence.Frames.Count)
				{
					if (ClipPlayer.IsLooped)
						ClipPlayer.CurrentFrameIndex = 0;
					else
						ClipPlayer.CurrentFrameIndex = ClipPlayer.CurrentSequence.Frames.Count - 1;
				}

				float tw = (ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Width +
				            BoundingBoxScale.X)*Scale.X;

				float th = (ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Height +
				            BoundingBoxScale.Y)*Scale.Y;

				if (IsCentered)
				{
					// Center Aligned means the position is actually in the center of the actor and needs to remove that
					float widthHalf = tw/2;
					float heightHalf = th/2;

					float posX1 = Position.X - widthHalf;
					float posY1 = Position.Y - heightHalf;

					float posX2 = Position.X + widthHalf;
					float posY2 = Position.Y + heightHalf;

					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(posX1 + CameraOffsetX,
					                        posY1 + CameraOffsetY, -5);

					points[1] = new Vector3(posX2 + CameraOffsetX,
					                        posY2 + CameraOffsetY, 5);

					BoundingBox = BoundingBox.CreateFromPoints(points);
				}
				else
				{
					// Update our Bounding Box
					Vector3[] points = new Vector3[2];

					points[0] = new Vector3(Position.X + CameraOffsetX,
					                        Position.Y + CameraOffsetY, -5);

					points[1] = new Vector3(Position.X + CameraOffsetX + tw,
					                        Position.Y + CameraOffsetY + th, 5);

					// Create our Bounding Box
					BoundingBox = BoundingBox.CreateFromPoints(points);
				}

        if (BoundingBoxRenderer != null)
        {
          BoundingBoxRenderer.ShowBoundingBox = ShowBoundingBox;
          BoundingBoxRenderer.BoundingBox = BoundingBox;
          BoundingBoxRenderer.Update(gameTime);
        }

			  // Update our Clip Player
				ClipPlayer.IsCentered = IsCentered;
				ClipPlayer.Position = Position;
				ClipPlayer.Velocity = Velocity;
				ClipPlayer.Scale = Scale;
				ClipPlayer.Rotation = Rotation;
				ClipPlayer.SpriteEffects = SpriteEffects;
				ClipPlayer.CameraOffsetX = CameraOffsetX;
				ClipPlayer.CameraOffsetY = CameraOffsetY;
				ClipPlayer.CameraZoomOffset = CameraZoomOffset;
				ClipPlayer.BoundingBox = BoundingBox;
			  ClipPlayer.ShowBoundingBox = ShowBoundingBox;
				ClipPlayer.Update(gameTime);
			}
		}

  	public override void Draw(GameTime gameTime)
    {
			if(!Visible || !Enabled)
				return;

      ClipPlayer.Draw(gameTime);

			if (BoundingBoxRenderer != null && ShowBoundingBox)
				BoundingBoxRenderer.Draw(gameTime);

  	  base.Draw(gameTime);
    }

    public void Play(string sequence, bool loop)
    {
    	ClipPlayer.Play(sequence, loop);
    }

    public void Stop(bool reset)
    {
    	ClipPlayer.Stop(reset);
    }

		public override void UnloadContent()
		{
			base.UnloadContent();

			ClipPlayer.UnloadContent();
			BoundingBoxRenderer.UnloadContent();
		}

		protected override void Dispose(bool disposing)
		{
			if (ClipPlayer != null)
				ClipPlayer.Dispose();

			base.Dispose(disposing);
		}
  }
}