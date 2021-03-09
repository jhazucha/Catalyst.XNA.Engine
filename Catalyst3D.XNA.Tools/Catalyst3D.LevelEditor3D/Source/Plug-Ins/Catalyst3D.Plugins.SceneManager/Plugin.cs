using System;
using System.Diagnostics;
using System.Linq;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.AttributeClasses;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Telerik.WinControls.Docking;

namespace Plugins.SceneManager
{
	[AddinName("Scene Object Manager")]
	[AddinDescription("Provides the ability to manage scene objects")]
	public class Plugin : Addin
	{
		private IAddinHost Host;
		private conSceneObjects conSceneObjects;

		public override void Register(IAddinHost host)
		{
			// copy host to local instance
			Host = host;

			// if we didn't find a control then create one
			if(host != null)
			{
				Trace.WriteLine(DateTime.Now + " | Scene Manager Addin Loaded .. ");

				conSceneObjects = new conSceneObjects(Host);

				// Add our Property Grid to the Right Side
			  Host.DockManager.SetDock(conSceneObjects, DockPosition.Right);
			}
		}

		public override void OnProjectLoaded()
		{
			base.OnProjectLoaded();

			// Init and Register our Scene Manager
			Host.SceneManager = new Catalyst3D.XNA.Engine.SceneManager(Host.Game);
			Host.SceneManager.Add(new EditorScreen(Host.Game));
			Host.SceneManager.CurrentScreen.Content.RootDirectory = Host.ProjectPath + @"\Content\";

			Host.SceneManager.Initialize();
			
			conSceneObjects.ProjectLoaded();
		}

		public override void Draw(GameTime gameTime)
		{
			// Draw our scene objects
			var sceneObjects = (from db in Host.SceneManager.CurrentScreen.VisualObjects where db.Visible orderby db.DrawOrder select db).ToList();

			foreach(VisualObject v in sceneObjects)
				v.Draw(gameTime);
		}
		public override void Update(GameTime gameTime)
		{
			var sceneObjects = (from db in Host.SceneManager.CurrentScreen.VisualObjects orderby db.UpdateOrder select db).ToList();
			
			foreach(VisualObject v in sceneObjects)
				v.Update(gameTime);
		}
	}
}
