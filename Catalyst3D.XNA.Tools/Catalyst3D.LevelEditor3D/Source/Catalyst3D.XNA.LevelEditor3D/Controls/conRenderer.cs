using System;
using System.Windows.Forms;
using Catalyst3D.PluginSDK;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor3D.Controls
{
	public partial class conRenderer : UserControl
	{
		public IAddinHost Host;
		public InputHandler InputHandler;
		public int ScrollFactor;

		public MouseState PreviousMouseState;

		public bool IsRendering { get; set; }

		public bool IsMouseDown { get; set; }

		public conRenderer()
		{
			InitializeComponent();

			// Hook our Events
			Globals.Draw += Draw;
			Globals.Update += Update;
			Globals.ObjectAdded += OnSceneObjectAdded;
			Globals.ObjectRemoved += OnSceneObjectRemoved;
			Globals.ObjectSelected += OnSceneObjectSelected;
			Globals.Initialize += OnInitialize;

			IsRendering = true;
		}

		public void OnInitialize()
		{
			tsViewportColor.Text = string.Format("{0},{1},{2}", Globals.AppSettings.RenderSurfaceColor.R,
																					 Globals.AppSettings.RenderSurfaceColor.G,
																					 Globals.AppSettings.RenderSurfaceColor.B);

			// Get an instance of our input handler
			InputHandler = (InputHandler)Globals.Game.Services.GetService(typeof(InputHandler));
		}

		public void Update(GameTime gameTime)
		{
			if(Host.SceneManager == null || !IsRendering)
				return;

			// Update any Addin's that require it
			foreach(Addin a in AddinManager.Addins)
			{
				Addin plugin = (Addin)a.Instance;

				if(plugin != null)
					plugin.Update(gameTime);
			}
		}

		public void Draw(GameTime gameTime)
		{
			if(Host.SceneManager == null || !IsRendering)
				return;

			// Draw any Addin's that require it
			foreach(Addin a in AddinManager.Addins)
			{
				Addin plugin = (Addin)a.Instance;

				if(plugin != null)
					plugin.Draw(gameTime);
			}
		}

		public void OnSceneObjectAdded(VisualObject vo)
		{
			vo.Initialize();

			Host.SceneManager.CurrentScreen.AddVisualObject(vo);
		}

		public void OnSceneObjectRemoved(VisualObject vo)
		{
			Host.SceneManager.CurrentScreen.VisualObjects.Remove(vo);
		}

		public void OnSceneObjectSelected(VisualObject obj)
		{
			foreach(VisualObject vo in Host.SceneManager.CurrentScreen.VisualObjects)
				vo.IsSelected = false;

			if(obj != null)
			{
				obj.IsSelected = true;
				Globals.CurrentSelectedObject = obj;
			}
		}

		private void tsViewportColor_TextChanged(object sender, EventArgs e)
		{
			string[] colorString = tsViewportColor.Text.Split(',');

			if(colorString.Length == 3)
			{
				if(colorString[2] != string.Empty)
				{
					Globals.AppSettings.RenderSurfaceColor = new Color(Convert.ToInt16(colorString[0]),
																														 Convert.ToInt16(colorString[1]),
																														 Convert.ToInt16(colorString[2]));
				}
			}
		}

		private void tsColor_Click(object sender, EventArgs e)
		{
			if(rColorDialog.ShowDialog() == DialogResult.OK)
			{
				if(rColorDialog.SelectedColor.IsKnownColor)
					tsViewportColor.Text = rColorDialog.SelectedColor.Name;
				else
				{
					tsViewportColor.Text = string.Format("{0},{1},{2}", rColorDialog.SelectedColor.R, rColorDialog.SelectedColor.G, rColorDialog.SelectedColor.B);
				}

				Globals.AppSettings.RenderSurfaceColor = new Color(rColorDialog.SelectedColor.R,
																													 rColorDialog.SelectedColor.G,
																													 rColorDialog.SelectedColor.B);
			}
		}

		private void tsCameraLock_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(tsCameraLock.Text)
			{
				case "Locked":
					Host.Camera.IsCameraMovementLocked = true;
					break;
				case "UnLocked":
					Host.Camera.IsCameraMovementLocked = false;
					break;
			}
		}
	}
}