using System;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.UtilityClasses;

namespace LevelEditor2D.Forms
{
	public partial class frmOptions : Form
	{
		public frmOptions()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Globals.IsDialogWindowOpen = true;

			tbContentPath.Text = Globals.AppSettings.ContentPath;
			tbProjectPath.Text = Globals.AppSettings.ProjectPath;
			cbRenderSurfaceColor.Text = Globals.AppSettings.RenderSurfaceColor;
			tbGameObjects.Text = Globals.AppSettings.ObjectTypesPath;

			cbResolution.SelectedIndex = Globals.AppSettings.Resolution;
		}

		private void btnProjectBrowse_Click(object sender, EventArgs e)
		{
			var dia = new FolderBrowserDialog();
			dia.ShowNewFolderButton = true;

			if (dia.ShowDialog() == DialogResult.OK)
				tbProjectPath.Text = dia.SelectedPath;
		}

		private void btnContentBrowse_Click(object sender, EventArgs e)
		{
			var dia = new FolderBrowserDialog();
			dia.ShowNewFolderButton = true;

			if (dia.ShowDialog() == DialogResult.OK)
				tbContentPath.Text = dia.SelectedPath;
		}

		private void btnGameObjects_Click(object sender, EventArgs e)
		{
			var dia = new OpenFileDialog();

			if (dia.ShowDialog() == DialogResult.OK)
			{
				tbGameObjects.Text = dia.FileName;

				Globals.LoadAssembly(dia.FileName);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Globals.AppSettings.ContentPath = tbContentPath.Text;
			Globals.AppSettings.ProjectPath = tbProjectPath.Text;
			Globals.AppSettings.RenderSurfaceColor = cbRenderSurfaceColor.Text;
			Globals.AppSettings.ObjectTypesPath = tbGameObjects.Text;

			switch (cbResolution.Text)
			{
				case "1920x1080":
					Globals.AppSettings.Resolution = 0;
					break;
				case "1280x720":
					Globals.AppSettings.Resolution = 1;
					break;

				case "800x480":
					Globals.AppSettings.Resolution = 2;
					break;

				case "480x800":
					Globals.AppSettings.Resolution = 3;
					break;

				default:
					Globals.AppSettings.Resolution = 2;
					break;
			}

			IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User |
																																				IsolatedStorageScope.Assembly |
																																				IsolatedStorageScope.Domain, null, null);

			// Save this default config to disk
			Serializer.IsoSerialize(isoStore, "settings.xml", Globals.AppSettings);

			Globals.IsDialogWindowOpen = false;

			Dispose();
		}

		protected override void DestroyHandle()
		{
			Globals.IsDialogWindowOpen = false;

			base.DestroyHandle();
		}
	}
}