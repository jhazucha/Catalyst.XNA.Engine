using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor3D.EntityClasses;
using LevelEditor3D.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace LevelEditor3D
{
	public class LevelEditor3D : Game
	{
	  private readonly string ProjectFilename;

    private readonly GraphicsDeviceManager Graphics;
		private Form gameWindow;

		private frmMain frmMain;

		// This is for Mouse Picking Diagnostics reasons do not remove
		public bool ShowMouseLocation;

    public LevelEditor3D(string[] filename)
    {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      // Input Handler
      InputHandler InputHandler = new InputHandler(this, PlayerIndex.One);

      // Store some MASTER controls in the collection
      InputHandler.Commands.Add(new InputAction("LeftCtrl", Buttons.LeftShoulder, Keys.LeftControl));
      Components.Add(InputHandler);

      ProjectFilename = filename[0];
    }

	  protected override void Initialize()
		{
			// Load our saved application settings
			if(File.Exists(Application.StartupPath + @"\settings.xml"))
				Globals.AppSettings = Serializer.Deserialize<AppSettings>(Application.StartupPath + @"\settings.xml");
			else
				Globals.AppSettings = new AppSettings();

			// Store this for our controls to utilize
			Globals.Game = this;

			// Hook the event incase the window size changes
			Globals.WindowSizeChanged += OnMainWinformSizeChanged;

			// Setup our Main Windows Form
			frmMain = new frmMain();
			frmMain.HandleDestroyed += OnMainHandleDestroyed;
			frmMain.conRenderer.SizeChanged += OnMainWinformSizeChanged;
			frmMain.Show();

			// Game Window
			gameWindow = (Form)Control.FromHandle(Window.Handle);
			gameWindow.Shown += OnGameWindowShown;

			gameWindow.FormBorderStyle = FormBorderStyle.None;

			// Update xna with our Render Window Size
			OnMainWinformSizeChanged(null, null);

			// Mouse Handle
			Mouse.WindowHandle = frmMain.conRenderer.RenderWindow.Handle;

			base.Initialize();

			// Initialize anything else that we have manually wired up
			Globals.Initialize.Invoke();
		}

    protected override void LoadContent()
    {
      base.LoadContent();

      if (!string.IsNullOrEmpty(ProjectFilename))
        frmMain.LoadProject(ProjectFilename);
    }

		private static void OnGameWindowShown(object sender, EventArgs e)
		{
			((Form)sender).Hide();
		}

		private void OnMainWinformSizeChanged(object sender, EventArgs e)
		{
			if(frmMain == null || gameWindow == null)
				return;

			gameWindow = (Form)Control.FromHandle(Window.Handle);

			frmMain.conRenderer.IsRendering = false;

			if(frmMain.conRenderer.Size != new Size(0, 0))
			{
				gameWindow.Size = frmMain.conRenderer.Size;

				Graphics.PreferredBackBufferWidth = frmMain.conRenderer.Size.Width;
				Graphics.PreferredBackBufferHeight = frmMain.conRenderer.Size.Height;
				Graphics.ApplyChanges();
			}

			frmMain.conRenderer.IsRendering = true;
		}

		private void OnMainHandleDestroyed(object sender, EventArgs e)
		{
			Exit();
		}

		protected override void Update(GameTime gameTime)
		{
			if(frmMain.conRenderer.RenderWindow != null && frmMain.conRenderer.IsRendering)
			{
				base.Update(gameTime);

				// Update our key frame animation container
				if(Globals.Update != null)
					Globals.Update.Invoke(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			if(frmMain.conRenderer.RenderWindow != null && frmMain.conRenderer.IsRendering)
			{
				// Clear our Back Buffer and set it to the desired render color
				Graphics.GraphicsDevice.Clear(Globals.AppSettings.RenderSurfaceColor);

				// Draw all sprites first
				base.Draw(gameTime);

				if(Globals.Draw != null)
					Globals.Draw.Invoke(gameTime);

				// Present all this to our Graphics Device
				Graphics.GraphicsDevice.Present(null, new Rectangle(0, 0, frmMain.conRenderer.RenderWindow.Width, frmMain.conRenderer.RenderWindow.Height), frmMain.conRenderer.RenderWindow.Handle);
			}
		}
	}
}