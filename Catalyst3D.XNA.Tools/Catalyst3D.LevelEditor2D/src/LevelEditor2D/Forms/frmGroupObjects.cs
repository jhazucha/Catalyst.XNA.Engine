using System.Collections.Generic;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.EntityClasses;

namespace LevelEditor2D.Forms
{
	public partial class frmGroupObjects : Form
	{
		private readonly List<VisualObject> SceneObjects;
	  private readonly conRenderer RenderWindow;

		public frmGroupObjects(List<VisualObject> obj, conRenderer renderer)
		{
      Globals.IsDialogWindowOpen = true;

      RenderWindow = renderer;

      InitializeComponent();

			SceneObjects = obj;

			rlSceneObjects.DataSource = SceneObjects;
			rlSceneObjects.DisplayMember = "Name";
		}

		private void rlSceneObjects_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Deselect all scene objects
			foreach(VisualObject vo in SceneObjects)
				vo.IsSelected = false;

			// Loop thru and select all the ones we have highlighted in the list box
			foreach(var i in rlSceneObjects.SelectedItems)
			{
				if(i is VisualObject)
				{
					VisualObject o = i as VisualObject;
					o.IsSelected = true;
				}
			}
		}

		private void btnGroup_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(tbName.Text))
			{
				MessageBox.Show(@"You must provide a name for this group");
				return;
			}

			// Create our new Group
		  EditorGroup vg = new EditorGroup(Globals.Game, RenderWindow);
			vg.Name = tbName.Text;
			vg.ShowBoundingBox = true;

			for (int i = 0; i < rlSceneObjects.SelectedItems.Count; i++)
			{
				if (rlSceneObjects.SelectedItems[i] is EditorGroup)
				{
					var vo = rlSceneObjects.SelectedItems[i] as EditorGroup;
					if (vo != null)
					{
						foreach (var v in vo.Objects)
						{
							vg.Objects.Add(v);
							SceneObjects.Remove(v);
						}
					}
				}
				else
				{
					var vo = rlSceneObjects.SelectedItems[i] as VisualObject;

					if (vo != null)
					{
						if (i == 0)
						{
							vg.Scale = vo.Scale;
							vg.Position = vo.Position;
						}

						vg.Objects.Add(vo);
						SceneObjects.Remove(vo);
					}
				}
			}

			vg.Initialize();

			Globals.ObjectAdded.Invoke(vg);

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