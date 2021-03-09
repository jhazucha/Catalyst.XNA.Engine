using System;
using System.Windows.Forms;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.CollectionClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Models;
using Telerik.WinControls.Docking;

namespace Plugins.SceneManager
{
	public partial class conSceneObjects : DockWindow
	{
		private readonly IAddinHost Host;

    public conSceneObjects(IAddinHost host)
    {
      SetGuid("9f1d2834-56ad-4dd2-8507-5e5549fa85ea");

      InitializeComponent();

      Host = host;

      tsAdd.Image = Resource1.add;
      tsRemove.Image = Resource1.delete;

    	Enabled = false;
    }

	  public void OnObjectAddedToScene(VisualObject vo)
		{
			VisualObjectCollection col = new VisualObjectCollection();
			col.AddRange(Host.SceneManager.CurrentScreen.VisualObjects);

			pgSceneObjects.SelectedObject = col;
		}

    private void tsRemove_Click(object sender, System.EventArgs e)
    {
      VisualObject vo = pgSceneObjects.SelectedGridItem.Value as VisualObject;

      if (vo != null)
        Host.SceneManager.CurrentScreen.VisualObjects.Remove(vo);

      RefreshPropertyGrid();
    }

    private void tsAdd_Click(object sender, EventArgs e)
    {
      OpenFileDialog dia = new OpenFileDialog();

      dia.Filter = @"Models(*.FBX)|*.FBX|All files (*.*)|*.*";

      if (dia.ShowDialog() == DialogResult.OK)
      {
        Model model = new Model(Host.Game, dia.FileName, Host.Camera);
        Host.SceneManager.CurrentScreen.AddVisualObject(model);
        
        RefreshPropertyGrid();
      }
    }

    private void RefreshPropertyGrid()
    {
      VisualObjectCollection col = new VisualObjectCollection();
      col.AddRange(Host.SceneManager.CurrentScreen.VisualObjects);

      pgSceneObjects.SelectedObject = col;
    }

		public void ProjectLoaded()
		{
			Host.SceneManager.ObjectAdded += OnObjectAddedToScene;
			RefreshPropertyGrid();

			Enabled = true;
		}
	}
}
