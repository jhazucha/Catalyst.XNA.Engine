using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Catalyst3D.PluginSDK.Interfaces;
using Microsoft.Xna.Framework;

namespace Catalyst3D.PluginSDK.AbstractClasses
{
	public class Addin
	{
		public string File { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[DefaultValue(true)]
		public bool LoadOnStartUp { get; set; }

		public Guid Guid { get; set; }

		[XmlIgnore]
		public object Instance { get; set; }

		public Addin()
		{
			LoadOnStartUp = true;
		}

		public virtual void Register(IAddinHost host) { }
		public virtual void UpdateControls() { }
		public virtual void DockClose() { }

		public virtual void Draw(GameTime gameTime) { }
		public virtual void Update(GameTime gameTime) { }

		public virtual void OnProjectLoaded() { }
		public virtual void OnProjectUnloaded() { }

		public virtual void Save(string path) { }
		public virtual void Load(string path) { }
	}
}