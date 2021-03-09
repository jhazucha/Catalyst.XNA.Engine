using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using LevelEditor2D.CollectionClasses;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.Controls
{
  public partial class conEmitterForces : UserControl
  {
    private readonly Vector2Collection SceneObjects = new Vector2Collection();
    private readonly IWindowsFormsEditorService Service;

    public List<Vector2> Forces = new List<Vector2>();

    public conEmitterForces(List<Vector2> value, IWindowsFormsEditorService editor)
    {
      InitializeComponent();
      
			Service = editor;

    	Globals.IsDialogWindowOpen = true;

			if (value != null)
			{
				Forces = value;
				SceneObjects.AddRange(Forces);
			}

    	lbForces.DataSource = SceneObjects;
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      var emitter = Globals.RenderWindow.CurrentSelectedObject as EditorEmitter;
    	if (emitter != null) emitter.ForceObjects = Forces;

    	Globals.IsDialogWindowOpen = false;

			Service.CloseDropDown();
    }

		private void btnAdd_Click(object sender, EventArgs e)
		{
			lbForces.DataSource = null;

			Forces.Add(new Vector2((float) Convert.ToDouble(tbX.Text), (float) Convert.ToDouble(tbY.Text)));

			lbForces.DataSource = null;
			lbForces.DataSource = Forces;
		}

  	private void tsRemove_Click(object sender, EventArgs e)
    {
      if (Forces.Count <= 0 || lbForces.SelectedItems.Count <= 0)
        return;

      Forces.RemoveAt(lbForces.SelectedIndex);

      lbForces.DataSource = null;
      lbForces.DataSource = Forces;
    }

		private void conEmitterForces_Leave(object sender, EventArgs e)
		{
			Globals.IsDialogWindowOpen = false;
		}
  }
}