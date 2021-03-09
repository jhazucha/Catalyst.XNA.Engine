using System.Windows.Forms;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Telerik.WinControls.Docking;

namespace Catalyst3D.Plugin.Camera
{
	public partial class conCameraSettings : DockWindow
	{
		public PropertyGrid PropertyGrid
		{
			get { return pgCamera; }
			set { pgCamera = value; }
		}

		public conCameraSettings(BasicCamera cam)
		{
			InitializeComponent();

			pgCamera.SelectedObject = cam;

			SetGuid("6ff4f13d-5442-4f1d-a433-4e4b7c39c038");
		}
	}
}
