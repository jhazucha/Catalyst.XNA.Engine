using System;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.UtilityClasses;

namespace AnimationEditor.Forms
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

      tbContentPath.Text = Globals.AppSettings.ContentPath;
      tbProjectPath.Text = Globals.AppSettings.ProjectPath;
      cbRenderSurfaceColor.Text = Globals.AppSettings.RenderSurfaceColor;
    }

    private void btnProjectBrowse_Click(object sender, EventArgs e)
    {
      var dia = new FolderBrowserDialog();
      dia.ShowNewFolderButton = true;

      if (dia.ShowDialog() == DialogResult.OK)
      {
        tbProjectPath.Text = dia.SelectedPath;
      }
    }

    private void btnContentBrowse_Click(object sender, EventArgs e)
    {
      var dia = new FolderBrowserDialog();
      dia.ShowNewFolderButton = true;

      if (dia.ShowDialog() == DialogResult.OK)
      {
        tbContentPath.Text = dia.SelectedPath;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      Globals.AppSettings.ContentPath = tbContentPath.Text;
      Globals.AppSettings.ProjectPath = tbProjectPath.Text;
      Globals.AppSettings.RenderSurfaceColor = cbRenderSurfaceColor.Text;

      Serializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml", Globals.AppSettings);

      Dispose();
    }
  }
}