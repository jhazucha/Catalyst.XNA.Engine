using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D
{
	public class RendererWindow : GameScreen
	{
    public PathingTool PathingDisplay;
		public Effect ShaderEffect;

		public RendererWindow(Game game)
			: base(game, string.Empty)
		{
		}

    public override void Initialize()
    {
      base.Initialize();

    	Globals.WindowSizeChanged += OnWindowSizeChanged;

      // Create our Pathing Display for Actor constraining
    	PathingDisplay = new PathingTool(Globals.Game);
      PathingDisplay.Visible = false;
      PathingDisplay.Initialize();
    }

		private void OnWindowSizeChanged(object sender, EventArgs e)
		{
		}

		public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      PathingDisplay.CameraOffsetX = Globals.CurrentCameraOffsetX;
      PathingDisplay.CameraOffsetY = Globals.CurrentCameraOffsetY;
      PathingDisplay.CameraZoomOffset = Globals.CurrentCameraZoom;
      PathingDisplay.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

      PathingDisplay.Draw(gameTime);
    }

		public override void UnloadContent()
		{
			base.UnloadContent();

			ShaderEffect = null;
		}
	}
}
