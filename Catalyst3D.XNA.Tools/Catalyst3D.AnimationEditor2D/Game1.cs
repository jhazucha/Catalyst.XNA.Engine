using System;
using System.Windows.Forms;
using Catalyst3D.XNA.CharacterEditor.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Catalyst3D.XNA.CharacterEditor
{
  public class Game1 : Game
  {
    private readonly GraphicsDeviceManager Graphics;
    private frmMain frmMain;

    public Game1()
    {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      // Store this for our controls to utilize
      Globals.Game = this;

      // Hook the event incase the window size changes
      Globals.WindowSizeChanged += OnMainWinformSizeChanged;

      // Setup our Main Windows Form
      frmMain = new frmMain();
      frmMain.HandleDestroyed += OnMainHandleDestroyed;
      frmMain.SizeChanged += OnMainWinformSizeChanged;
      frmMain.WindowState = FormWindowState.Maximized;
      frmMain.Show();

      // Game Window
      Form gameWindow = (Form)Control.FromHandle(Window.Handle);
      gameWindow.Shown += OnGameWindowShown;
      gameWindow.FormBorderStyle = FormBorderStyle.None;

      Mouse.WindowHandle = frmMain.conRenderer.Handle;
      Globals.WindowSizeChanged.Invoke(this, null);

      base.Initialize();
    }

    private static void OnGameWindowShown(object sender, EventArgs e)
    {
      ((Form)sender).Hide();
    }

    private void OnMainWinformSizeChanged(object sender, EventArgs e)
    {
      if (frmMain == null)
        return;

      Form gameWindow = (Form)Control.FromHandle(Window.Handle);
      gameWindow.Size = frmMain.conRenderer.Size;

      Graphics.PreferredBackBufferWidth = gameWindow.Size.Width;
      Graphics.PreferredBackBufferHeight = gameWindow.Size.Height;
      Graphics.ApplyChanges();
    }

    private void OnMainHandleDestroyed(object sender, EventArgs e)
    {
      Exit();
    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      Globals.Update.Invoke(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      if (frmMain.conRenderer != null && frmMain.conRenderer.IsRendering && frmMain.WindowState != FormWindowState.Minimized)
      {
        // Clear our Back Buffer and set it to the desired render color
        Graphics.GraphicsDevice.Clear(Globals.AppSettings.GetRenderSurfaceColor());

        // Draw all sprites first
        base.Draw(gameTime);

        if (Globals.Draw != null)
          Globals.Draw.Invoke(gameTime);

        // Present all this to our Graphics Device
        Graphics.GraphicsDevice.Present(frmMain.conRenderer.Handle);
      }
    }
  }
}
