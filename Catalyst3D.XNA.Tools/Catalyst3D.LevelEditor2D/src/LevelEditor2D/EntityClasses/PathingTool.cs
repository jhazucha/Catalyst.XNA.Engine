using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor2D.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
  public class PathingTool : VisualObject
  {
    private conRenderer RenderWindow;
    private Texture2D Cursor;

		// Cap the click time on placing nodes so we dont slam the scene with un-wanted nodes!
  	private readonly TimeSpan nodePlacementDelay = new TimeSpan(0, 0, 0, 0, 100);
		private TimeSpan elapsedNodePlacement;

		public LedgeBuilder CurrentPath;

    public PathingTool(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
			if(Game != null)
			{
				base.Initialize();

				// Grab a ref of our sprite batch
				if (SpriteBatch == null)
					SpriteBatch = Globals.RenderWindow.RenderWindow.SpriteBatch;

				// Load our textures
				Cursor = Globals.Game.Content.Load<Texture2D>("PathingCursor");

				// Hook the global event in case we are told to place a node somewhere
				Globals.OnPathingNodePlaced += OnPathingNodePlaced;
				Globals.OnSaveCurrentPath += OnSavePathingNodes;
				Globals.OnCanceledCurrentPath += OnCanceledPath;
			}

			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;
    }

		private void OnCanceledPath()
		{
			// Flush the current path we were making
			CurrentPath = null;

			// Hide the pathing tool once saved
			Globals.IsPathingToolVisible = false;
		}

		private void OnSavePathingNodes()
		{
			Globals.Paths.Add(CurrentPath);
			Globals.ObjectSelected.Invoke(CurrentPath);

			// Drop the current path
			CurrentPath = null;

			// Hide the pathing tool once saved
			Globals.IsPathingToolVisible = false;
		}

  	private void OnPathingNodePlaced()
    {
			if (CurrentPath == null)
			{
				CurrentPath = new LedgeBuilder(Game, RenderWindow);
				CurrentPath.Initialize();

				Globals.ObjectSelected.Invoke(CurrentPath);
				elapsedNodePlacement = TimeSpan.Zero;
			}
			else if (elapsedNodePlacement <= nodePlacementDelay)
				return;

			elapsedNodePlacement = TimeSpan.Zero;

      // Create our path node
      LedgeNodeDisplay node = new LedgeNodeDisplay(Globals.Game);
      node.Position = new Vector2(Position.X - Globals.CurrentCameraOffsetX,
                                  Position.Y - Globals.CurrentCameraOffsetY);
  		
      node.CameraOffsetX = Globals.CurrentCameraOffsetX;
      node.CameraOffsetY = Globals.CurrentCameraOffsetY;
      node.DrawOrder = 100;
    	node.Visible = true;

      node.Initialize();

      CurrentPath.Nodes.Add(node);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      // Update our Pathing Tools Position
      Position = new Vector2(Globals.CurrentMouseX - 8, Globals.CurrentMouseY - 8);

			if (CurrentPath != null)
			{
				elapsedNodePlacement += gameTime.ElapsedGameTime;

				CurrentPath.CameraOffsetX = CameraOffsetX;
				CurrentPath.CameraOffsetY = CameraOffsetY;
				CurrentPath.Update(gameTime);
			}
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

			if (SpriteBatch != null && Cursor != null && Globals.IsPathingToolVisible)
			{
				// Draw our Cursor
				SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
				SpriteBatch.Draw(Cursor, new Vector2(Position.X, Position.Y), Color.White);
				SpriteBatch.End();

				if (CurrentPath != null)
				{
					CurrentPath.Visible = true;
					CurrentPath.Draw(gameTime);
				}
			}
    }

		public override void UnloadContent()
		{
			base.UnloadContent();

			SpriteBatch = null;
			Cursor = null;
			CurrentPath = null;
		}
  }
}