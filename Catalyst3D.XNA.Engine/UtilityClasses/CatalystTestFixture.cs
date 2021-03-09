#if !XBOX360 && !WINDOWS_PHONE

using System;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
  public class CatalystTestFixture : Game
  {
    public SceneManager SceneManager;
    public GraphicsDeviceManager GraphicsManager;
    public Color ViewportColor = Color.CornflowerBlue;
  	public InputHandler InputHandler;
  
    public CatalystTestFixture()
    {
			// Set our Content Root Directory
			Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Content\";

    	Window.Title = "Catalyst3D - Test Fixture";
    	GraphicsManager = new GraphicsDeviceManager(this);

			// Setup the scene manager incase we want to use it
    	SceneManager = new SceneManager(this);
    	
			Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Content\";
    	
			// Setup an input handler for our test fixtures to use
    	InputHandler = new InputHandler(this, PlayerIndex.One);
    	Components.Add(InputHandler);
    }

		protected override void Update(GameTime gameTime)
		{
			SceneManager.ViewportColor = ViewportColor;

			base.Update(gameTime);
		}
  }
}

#endif