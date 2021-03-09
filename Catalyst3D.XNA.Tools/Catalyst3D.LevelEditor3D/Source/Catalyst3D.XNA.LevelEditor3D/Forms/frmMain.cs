using System;
using System.IO;
using System.Windows.Forms;
using Catalyst3D.PluginSDK;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Telerik.WinControls.Docking;

namespace LevelEditor3D.Forms
{
	public partial class frmMain : Form, IAddinHost
	{
		#region IAddinHost Members

		public DockingManager DockManager
		{
			get { return dockManager; }
			set { dockManager = value; }
		}

		public StatusStrip StatusStrip
		{
			get { return statusStrip1; }
			set { statusStrip1 = value; }
		}

		public MenuStrip MenuStrip
		{
			get { return menuStrip1; }
			set { menuStrip1 = value; }
		}

		public ToolStrip ToolStrip
		{
			get { return toolStrip; }
			set { toolStrip = value; }
		}

		public Control.ControlCollection HostControls
		{
			get { return Controls; }
		}

		public FlowLayoutPanel ToolPanel
		{
			get { return flToolPanel; }
			set { flToolPanel = value; }
		}

		public UserControl Renderer
		{
			get { return conRenderer; }
		}

		public SceneManager SceneManager { get; set; }

		public BasicCamera Camera { get; set; }

		public InputHandler InputHandler { get; set; }

		public Game Game
		{
			get { return Globals.Game; }
		}

		public bool IsProjectLoaded { get; set; }

		public string ProjectPath { get; set; }

		#endregion

		public frmMain()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Set the global var
			AddinManager.Host = this;

			// Load Addins
			if(File.Exists(Application.StartupPath + @"\addins.xml"))
			{
				AddinManager.Addins = AddinManager.LoadPlugins(Application.StartupPath + @"\addins.xml");

				foreach(Addin addin in AddinManager.Addins)
				{
					if(addin.LoadOnStartUp)
					{
						AddinManager.Register(addin);
					}
				}
			}

			// Kept this in the base app due to the XNA hook on it ..
			conRenderer.Host = this;

			// Load our layout
			if(File.Exists(Application.StartupPath + @"\layout.xml"))
			{
				dockManager.LoadXML(Application.StartupPath + @"\layout.xml");
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if(AddinManager.Addins != null && AddinManager.Addins.Count > 0)
			{
				AddinManager.SavePlugins(Application.StartupPath + @"\addins.xml");
			}

			// Save Layout
			dockManager.SaveXML(Application.StartupPath + @"\layout.xml");

			// Save Application Preferences
			Serializer.Serialize(Application.StartupPath + @"\settings.xml", Globals.AppSettings);
		}

		private void tsPreferences_Click(object sender, EventArgs e)
		{
			frmPreferences frm = new frmPreferences();
			frm.Show();
		}

		private void pluginsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmPluginManager manager = new frmPluginManager();
			manager.Show();
		}

    public void newProjectgToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveProject(false);
		}

    public void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveProject(true);
		}

    public void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if(IsProjectLoaded)
      {
        //TODO: Project is loaded prompt to save it before loading a new one
      }

      // Create our Open File Dialog
      OpenFileDialog dia = new OpenFileDialog();
      dia.Filter = "Catalyst3D Project File|*.c3d";
      dia.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

      if (dia.ShowDialog() == DialogResult.OK)
        LoadProject(dia.FileName);
    }

	  private void SaveProject(bool showResult)
		{
			// If no project is loaded prompt to create/save it
			if(!IsProjectLoaded)
			{
				// Create our Folder Browse
				SaveFileDialog dia = new SaveFileDialog();
				dia.Filter = "Catalyst3D Project File|*.c3d";
				dia.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

				if(dia.ShowDialog() == DialogResult.OK)
				{
					// Store our current projects path and file name
					ProjectPath = Path.GetDirectoryName(dia.FileName);

					// Create our fresh collection for holding scene objects
					Directory.CreateDirectory(ProjectPath + @"\Content");

					// For now just serialize a bs file .c3d file
					Serializer.Serialize(dia.FileName, new CatalystProject3D());
				}
				else
				{
					return;
				}
			}

			// Tell all our Plug-in's to save any resources to disk
			foreach(Addin addin in AddinManager.Addins)
			{
				Addin plugin = (Addin) addin.Instance;

				if (plugin != null)
				{
					// Tell our plug-in's we have loaded a project
					if (!IsProjectLoaded)
						plugin.OnProjectLoaded();

					// Tell our plug-in's to save any data they require for this project
					plugin.Save(ProjectPath);
				}
			}

			// Flag that a project is loaded as well
			IsProjectLoaded = true;

			if(showResult)
				MessageBox.Show("Successfully Saved the Project!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(IsProjectLoaded)
			{
				if(MessageBox.Show("Would you like to save the current project before exiting the application?", "Save Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					SaveProject(true);
					Close();
				}
			}
			else
				Close();

		}

    public void LoadProject(string filename)
    {
      IsProjectLoaded = true;

      // Store our current projects path and file name
      ProjectPath = Path.GetDirectoryName(filename);

      foreach (Addin a in AddinManager.Addins)
      {
        Addin plugin = (Addin)a.Instance;

        if (plugin != null)
        {
          plugin.OnProjectLoaded();

          plugin.Load(filename);
        }
      }
    }
	}
}