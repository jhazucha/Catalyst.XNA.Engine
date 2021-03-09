using System;
using System.Windows.Forms;
using LevelEditor2D.Controls;
using LevelEditor2D.EntityClasses;

namespace LevelEditor2D.Forms
{
	public partial class frmAddLabel : Form
	{
	  private conRenderer RenderWindow;

		public frmAddLabel(conRenderer renderer)
		{
			InitializeComponent();

		  RenderWindow = renderer;

			Globals.IsDialogWindowOpen = true;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				// Create our editor label
				EditorLabel label = new EditorLabel(Globals.Game, cbFontType.Text + cbFontSize.Text, RenderWindow);
				label.Text = tbText.Text;
				label.Name = "lb" + tbText.Text.TrimStart(' ').Trim();

				// init it
				label.Initialize();

				// Add it to our scene
				Globals.ObjectAdded.Invoke(label);

				// Select it
				Globals.ObjectSelected.Invoke(label);

				Globals.IsDialogWindowOpen = false;

				// Close the dialog window
				Close();
			}
			catch (Exception er)
			{
				throw new Exception(er.Message);
			}
		}

		protected override void DestroyHandle()
		{
			Globals.IsDialogWindowOpen = false;

			base.DestroyHandle();
		}
	}
}