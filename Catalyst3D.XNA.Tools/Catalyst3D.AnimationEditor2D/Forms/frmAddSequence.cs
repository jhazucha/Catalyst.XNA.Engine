using System;
using System.Windows.Forms;

namespace AnimationEditor.Forms
{
  public partial class frmAddSequence : Form
  {
    public frmAddSequence()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      tbName.Select();
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      eProvider.SetError(tbName, null);

      if(string.IsNullOrEmpty(tbName.Text))
      {
        eProvider.SetError(tbName, "You must specify a name");
        return;
      }

      // Fire the event so that the actor form adds it
      Globals.AddSequence.Invoke(tbName.Text);

      Dispose();
    }
  }
}
