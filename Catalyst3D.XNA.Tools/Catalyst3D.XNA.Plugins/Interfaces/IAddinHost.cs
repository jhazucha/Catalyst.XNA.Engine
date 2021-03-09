using System.Windows.Forms;
using Catalyst3D.XNA.Engine;
using Microsoft.Xna.Framework;

namespace Catalyst3D.PluginSDK.Interfaces
{
	public interface IAddinHost
	{
		MenuStrip MenuStrip { get; set; }
		ToolStrip ToolStrip { get; set; }
		Control.ControlCollection HostControls { get; }
		SceneManager SceneManager { get; set; }
		Game Game { get; }

		bool IsProjectLoaded { get; }
		string ProjectPath { get; }
  }
}
