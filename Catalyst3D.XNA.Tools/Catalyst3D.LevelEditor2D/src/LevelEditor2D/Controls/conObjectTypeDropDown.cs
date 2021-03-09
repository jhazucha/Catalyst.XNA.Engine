using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using LevelEditor2D.EntityClasses;

namespace LevelEditor2D.Controls
{
	public partial class conObjectTypeDropDown : UserControl
	{
		private readonly IWindowsFormsEditorService Service;

		public conObjectTypeDropDown(IWindowsFormsEditorService editorService)
		{
			InitializeComponent();
			
			Service = editorService;

			Globals.IsDialogWindowOpen = true;

			lsTypes.DataSource = Globals.CurrentGameObjectTypes;
			lsTypes.DisplayMember = "Name";
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			Globals.IsDialogWindowOpen = false;

      if (Globals.RenderWindow.CurrentSelectedObject is EditorSprite)
			{
				EditorSprite s = Globals.RenderWindow.CurrentSelectedObject as EditorSprite;
			  {
			    s.ObjectTypeName = lsTypes.Text;
			    s.ObjectType = lsTypes.SelectedItem.GetType();
			  }

			  Service.CloseDropDown();
			}

			if (Globals.RenderWindow.CurrentSelectedObject is EditorActor)
			{
				EditorActor s = Globals.RenderWindow.CurrentSelectedObject as EditorActor;
			  {
			    s.ObjectTypeName = lsTypes.Text;
			    s.ObjectType = lsTypes.SelectedItem.GetType();
			  }

			  Service.CloseDropDown();
			}

			if (Globals.RenderWindow.CurrentSelectedObject is EditorEmitter)
			{
				EditorEmitter s = Globals.RenderWindow.CurrentSelectedObject as EditorEmitter;
			  {
			    s.ObjectTypeName = lsTypes.Text;
			    s.ObjectType = lsTypes.SelectedItem.GetType();
			  }

			  Service.CloseDropDown();
			}

			if (Globals.RenderWindow.CurrentSelectedObject is EditorGroup)
			{
				EditorGroup s = Globals.RenderWindow.CurrentSelectedObject as EditorGroup;
			  {
			    s.ObjectTypeName = lsTypes.Text;
			    s.ObjectType = lsTypes.SelectedItem.GetType();
			  }

			  Service.CloseDropDown();
			}
		}

		private void conObjectTypeDropDown_Leave(object sender, EventArgs e)
		{
			Globals.IsDialogWindowOpen = false;
		}
	}
}
