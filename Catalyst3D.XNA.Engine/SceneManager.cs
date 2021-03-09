using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace Catalyst3D.XNA.Engine
{
	public class SceneManager : DrawableGameComponent
	{
		private readonly List<GameScreen> ScreensToUpdate = new List<GameScreen>();

		private Texture2D sceneTexture;
		private bool IsSwaping;

		private RenderTarget2D renderTarget1;
		private RenderTarget2D renderTarget2;
		private RenderTarget2D renderTarget3;
		private RenderTarget2D renderTarget4;

		public delegate void ObjectEvent(VisualObject sprite);

		public delegate void CustomShader(GameTime gameTime);

		public GameScreen[] GetScreens()
		{
			return Screens.ToArray();
		}

		[ContentSerializerIgnore, XmlIgnore] public CustomShader OnSetPostProcessShaderParams;

		[ContentSerializerIgnore, XmlIgnore] public Effect PostProcessShader;

		[ContentSerializerIgnore, XmlIgnore] public List<GameScreen> Screens = new List<GameScreen>();

		[ContentSerializerIgnore, XmlIgnore] public bool IsInitialized;
		[ContentSerializerIgnore, XmlIgnore] public bool IsExiting;
		[ContentSerializerIgnore, XmlIgnore] public bool IsSceneHardwareBlurred;
		[ContentSerializerIgnore, XmlIgnore] public bool IsPhoneLocked;

		[ContentSerializerIgnore, XmlIgnore] public GameScreen CurrentScreen;

		[ContentSerializerIgnore, XmlIgnore] public SpriteBatch SpriteBatch;

		[ContentSerializerIgnore, XmlIgnore] public bool IsPaused;

		[ContentSerializerIgnore, XmlIgnore] public int HardwareBlurFactor = 2;
		[ContentSerializerIgnore, XmlIgnore] public int TitleSafeWidth;
		[ContentSerializerIgnore, XmlIgnore] public int TitleSafeHeight;

		[ContentSerializerIgnore, XmlIgnore] public ObjectEvent ObjectAdded;
		[ContentSerializerIgnore, XmlIgnore] public ObjectEvent ObjectRemoved;

		public Color ViewportColor = Color.CornflowerBlue;

		public SpriteSortMode SpriteSortMode = SpriteSortMode.Immediate;

		public SceneManager() : base(null) { }
		
		public SceneManager(Game game)
			: base(game)
		{
			// Register this as a service
			game.Services.AddService(typeof (SceneManager), this);

			// Load it to the games component collection
			game.Components.Add(this);

			Game.InactiveSleepTime = new TimeSpan(0, 0, 1);

#if WINDOWS_PHONE
			PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
			PhoneApplicationService.Current.Deactivated += OnPhoneLocking;
			PhoneApplicationService.Current.Activated += OnPhoneUnLocked;
#endif
		}

		public override void Initialize()
		{
			base.Initialize();

			// Get our Title Safe Area to render in
			TitleSafeWidth = Game.GraphicsDevice.Viewport.TitleSafeArea.Width;
			TitleSafeHeight = Game.GraphicsDevice.Viewport.TitleSafeArea.Height;

			// Create our render target
			int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
			int height = GraphicsDevice.PresentationParameters.BackBufferHeight;

			renderTarget1 = new RenderTarget2D(GraphicsDevice, width, height);
			renderTarget2 = new RenderTarget2D(GraphicsDevice, width/HardwareBlurFactor, height/HardwareBlurFactor);
			renderTarget3 = new RenderTarget2D(GraphicsDevice, renderTarget2.Width/HardwareBlurFactor, renderTarget2.Height/HardwareBlurFactor);
			renderTarget4 = new RenderTarget2D(GraphicsDevice, renderTarget3.Width/HardwareBlurFactor, renderTarget3.Height/HardwareBlurFactor);

			// Tell each of the screens to load their content.
			foreach (GameScreen screen in Screens)
			{
				screen.SpriteBatch = SpriteBatch;
				screen.Initialize();
			}

			IsInitialized = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			if (SpriteBatch == null)
				SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
		}

		public override void Update(GameTime gameTime)
		{
			if (IsExiting || IsPhoneLocked)
				return;

			base.Update(gameTime);

			if (OnSetPostProcessShaderParams != null)
				OnSetPostProcessShaderParams.Invoke(gameTime);

			// Clear our list of screens to update
			ScreensToUpdate.Clear();

			// Grab our objects to update
			ScreensToUpdate.AddRange(Screens);

			// Loop as long as there are screens waiting to be updated.
			while (ScreensToUpdate.Count > 0)
			{
				// Pop the topmost screen off the waiting list.
				GameScreen screen = ScreensToUpdate[ScreensToUpdate.Count - 1];

				if (screen != null)
				{
					ScreensToUpdate.RemoveAt(ScreensToUpdate.Count - 1);

					// Update the screen.
					screen.Update(gameTime);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (IsExiting || IsPhoneLocked || GraphicsDevice == null)
				return;

			if (PostProcessShader != null)
			{
				// Tell graphics device to render to target
				GraphicsDevice.SetRenderTarget(renderTarget1);

				// Render our scene
				GraphicsDevice.Clear(ViewportColor);

				base.Draw(gameTime);

				foreach (GameScreen o in Screens)
					o.Draw(gameTime);

				// Stop rendering to the target
				GraphicsDevice.SetRenderTarget(null);

				// Grab the texture out our render target
				sceneTexture = renderTarget1;

				// Cleare the back buffer again
				GraphicsDevice.Clear(ViewportColor);

				SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend, null, null, null, PostProcessShader);
				SpriteBatch.Draw(sceneTexture, Vector2.Zero, Color.White);
				SpriteBatch.End();
			}
			else
			{
				if (IsSceneHardwareBlurred)
				{
					// Get which screens are able to be drawn
					GameScreen[] screens = (Screens.Where(db => db.State != ScreenState.Hidden).OrderBy(db => db.DrawOrder)).ToArray();

					List<VisualObject> blurrables = new List<VisualObject>();
					List<VisualObject> nonBlurrables = new List<VisualObject>();

					foreach (var s in screens)
					{
						foreach (var v in s.VisualObjects)
						{
							if (v.IsBlurrEnabled)
								blurrables.Add(v);
							else
								nonBlurrables.Add(v);
						}
					}

					#region Render Target 1

					// Set our first render target to render to
					GraphicsDevice.SetRenderTarget(renderTarget1);

					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					foreach (var o in blurrables.Where(db => db.Visible).OrderBy(db => db.DrawOrder))
						o.Draw(gameTime);

					#endregion

					#region Render Target 2

					// Set the 2nd render target which is a down sampled texture
					GraphicsDevice.SetRenderTarget(renderTarget2);

					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					SpriteBatch.Begin();
					SpriteBatch.Draw(renderTarget1, GraphicsDevice.Viewport.Bounds, Color.White);
					SpriteBatch.End();

					#endregion

					#region Render Target 3

					// Set the 2nd render target which is a down sampled texture
					GraphicsDevice.SetRenderTarget(renderTarget3);

					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					SpriteBatch.Begin();
					SpriteBatch.Draw(renderTarget2, GraphicsDevice.Viewport.Bounds, Color.White);
					SpriteBatch.End();

					#endregion

					#region Render Target 4

					// Set the 2nd render target which is a down sampled texture
					GraphicsDevice.SetRenderTarget(renderTarget4);

					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					SpriteBatch.Begin();
					SpriteBatch.Draw(renderTarget3, GraphicsDevice.Viewport.Bounds, Color.White);
					SpriteBatch.End();

					#endregion

					// Set the 2nd render target which is a down sampled texture
					GraphicsDevice.SetRenderTarget(null);

					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					// Draw the first one back
					SpriteBatch.Begin();
					SpriteBatch.Draw(renderTarget1, GraphicsDevice.Viewport.Bounds, Color.White);
					SpriteBatch.End();

					// Render our last one scalled up
					SpriteBatch.Begin();
					SpriteBatch.Draw(renderTarget4, GraphicsDevice.Viewport.Bounds, Color.White);
					SpriteBatch.End();

					foreach (var o in nonBlurrables.Where(db => db.Visible).OrderBy(db => db.DrawOrder))
						o.Draw(gameTime);
				}
				else
				{
					if (CurrentScreen != null)
						GraphicsDevice.Clear(CurrentScreen.BackgroundColor);
					else
						GraphicsDevice.Clear(ViewportColor);

					// Get which screens are able to be drawn
					GameScreen[] screens = (Screens.Where(db => db.State != ScreenState.Hidden).OrderBy(db => db.DrawOrder)).ToArray();

					foreach (var o in screens)
						o.Draw(gameTime);
				}
			}

			base.Draw(gameTime);
		}

		public void SwapScreen(GameScreen toScreen)
		{
			// Prevent from calling this more than once
			if (IsSwaping)
				return;

			IsSwaping = true;

			WaitCallback cb = OnSwapScreen;
			ThreadPool.QueueUserWorkItem(cb, toScreen);
		}

		internal void OnSwapScreen(object swap)
		{
			// Swap from the current scene into this new screen
			var screen = (GameScreen) swap;

			screen.Game = Game;
			screen.SceneManager = this;
			screen.State = ScreenState.Active;

			if (IsInitialized)
				screen.Initialize();

			TouchPanel.EnabledGestures = screen.EnabledGestures;

			var removals = new List<VisualObject>();

			// Find the ones that are already in the current scene
			foreach (var o in screen.VisualObjects)
			{
				// Check to see if its in the scene, no reason to add it its already in the scene ..
				if (CurrentScreen.Contains(o.Name) != null)
				{
					o.Enabled = false;
					o.Visible = false;

					removals.Add(o);

					continue;
				}

				/* -- NOTES --
					Check to see if its attached to a pathing node
					This is going to be tricky on respawns because it will pop them onto the pathing node at birth and that is not desired
					when their is a chain of these objects. Need to find a way to transition them on 1 by 1 down the chain once all on screen
					its good to go
				*/

				// Check to see if there attached to a pathing node
				if (o.AttachedPathingNode != null)
				{
					// Need a way to respawn the others in the chain after this one reaches the next node! :)
					//if (o.CurrentPathNodeIndex == 0)
					//{
					//  //o.CurrentObjectState = ObjectState.Alive;
					//  o.Visible = true;
					//  o.Enabled = true;
					//}
					//else
					//{
					//  //o.CurrentObjectState = ObjectState.Dead;
					//  o.Visible = true;
					//  o.Enabled = true;
					//}

					o.Visible = true;
					o.Enabled = true;

					// Flag it as traveling
					o.AttachedPathingNode.IsTraveling = true;
				}
				else
				{
					// Normal sprite in the scene
					o.CurrentObjectState = ObjectState.Alive;
					o.Visible = true;
					o.Enabled = true;
				}
			}

			// Flush the ones already in the scene out
			foreach (var o in removals)
				screen.VisualObjects.Remove(o);

			// Tell all objects not locked in the current scene to transition off
			foreach (var o in CurrentScreen.VisualObjects)
			{
				screen.AddVisualObject(o);

				// Loop thru the current scene's visual objects and mark them to transition off
				if (!o.IsLocked)
					o.CurrentObjectState = ObjectState.TransitioningOff;
				else
				{
					o.CurrentObjectState = ObjectState.Alive;
					o.Enabled = true;
					o.Visible = true;
				}
			}

			// Swap the current screens visual objects with the new one
			int index = Screens.IndexOf(CurrentScreen);

			// Swap to our new screen
			Screens[index] = screen;

			CurrentScreen = screen;
			CurrentScreen.Index = index;

			IsSwaping = false;
		}

    public void Load(GameScreen screen, bool flush)
    {
      GameScreen previous = null;

      // Prevent re-loading the same screen type twice
      if ((from s in Screens where s.GetType() == screen.GetType() select s).Count() > 0)
        return;

      if (CurrentScreen != null)
      {
        if (screen.IsDialogWindow)
        {
          screen.AudioManager = CurrentScreen.AudioManager;
          CurrentScreen.State = ScreenState.Behind;
        }
        else
        {
          previous = CurrentScreen;
        }
      }

      // Pass our local vars over to the screen
      screen.Game = Game;
      screen.SceneManager = this;
      screen.SpriteBatch = SpriteBatch;

      // Set this as the current screen
      CurrentScreen = screen;

      // If they have the input handler wired up pass it to the game screen so we dont have to pull it outta the service collection
      var InputHandler = (InputHandler) Game.Services.GetService(typeof (InputHandler));

      if (InputHandler != null)
        screen.InputHandler = InputHandler;

      if (IsInitialized)
        screen.Initialize();

      TouchPanel.EnabledGestures = screen.EnabledGestures;

      // Load it to our screen collection
      Screens.Add(screen);

      if (flush)
      {
        foreach (var s in GetScreens())
        {
          if (s == screen)
            continue;

          s.State = ScreenState.Exiting;
          s.UnloadContent();
          Screens.Remove(s);
          ScreensToUpdate.Remove(s);
        }

        previous = null;
      }

      if (previous != null)
      {
        previous.State = ScreenState.Exiting;

        // If we have a graphics device, tell the screen to unload content.
        previous.UnloadContent();

        // Remove the screen
        Screens.Remove(previous);

        // Remove it from our pending update screen collection
        ScreensToUpdate.Remove(previous);
      }
    }

	  public void Remove(GameScreen screen)
		{
			screen.State = ScreenState.Exiting;

			// If we have a graphics device, tell the screen to unload content.
			screen.UnloadContent();

			// Remove the screen
			Screens.Remove(screen);

			// Remove it from our pending update screen collection
			ScreensToUpdate.Remove(screen);

			// if the count is higher than 0 then walk over the new screens gestures
			if (Screens.Count > 0)
			{
				TouchPanel.EnabledGestures = Screens[Screens.Count - 1].EnabledGestures;

				CurrentScreen = Screens[Screens.Count - 1];

				// Reset the screen as active
				Screens[Screens.Count - 1].State = ScreenState.TransitionOn;
			}
			else
				CurrentScreen = null;
		}

#if WINDOWS_PHONE
		private void OnPhoneUnLocked(object sender, ActivatedEventArgs e)
		{
			IsPhoneLocked = false;

			PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
		}

		private void OnPhoneLocking(object sender, DeactivatedEventArgs e)
		{
			IsPhoneLocked = true;

			//Game.InactiveSleepTime = new TimeSpan(0, 0, 3, 0);
			PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
		}
#endif

    public override void UnloadContent()
    {
      base.UnloadContent();

      ScreensToUpdate.Clear();

      foreach (GameScreen screen in Screens)
        screen.UnloadContent();

      sceneTexture = null;
      SpriteBatch = null;
      renderTarget1 = null;
      PostProcessShader = null;
    }
	}
}