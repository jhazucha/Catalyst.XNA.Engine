using System.Windows.Forms;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace LevelEditor2D.Forms
{
  public partial class frmSequencer : Form
  {
    public Globals.SequenceAddEvent AddSequenceEvent;
    public Globals.SequenceRemoveEvent RemoveSequenceEvent;

    public frmSequencer()
    {
      InitializeComponent();

    	Globals.IsDialogWindowOpen = true;
    }

    private void tsAdd_Click(object sender, System.EventArgs e)
    {
      if (AddSequenceEvent != null)
      {
        dgSequences.DataSource = null;
        dgSequences.DataSource = AddSequenceEvent.Invoke(new Sequence2D { Name = "", StartFrame =1, EndFrame = 1 });
      }
    }

    private void tsRemove_Click(object sender, System.EventArgs e)
    {
      if(RemoveSequenceEvent != null)
      {
        if (dgSequences.SelectedRows.Count > 0)
          RemoveSequenceEvent.Invoke(dgSequences.SelectedRows[0].Index);
      }
    }

    private void tsSave_Click(object sender, System.EventArgs e)
    {
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