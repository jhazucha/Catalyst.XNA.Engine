using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Catalyst3D.PluginSDK.Interfaces;

namespace Catalyst3D.Plugins.Landscape.Forms
{
  public partial class frmGenLandscape : Form
  {
    private readonly IAddinHost Host;

    private int hmapWidth;
    private int hmapHeight;

    public frmGenLandscape(IAddinHost host)
    {
      Host = host;

      InitializeComponent();
    }

    private void btnBrowse1_Click(object sender, EventArgs e)
    {
      OpenFileDialog dia = new OpenFileDialog();
    	dia.Filter = @"Image Files(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

      if (dia.ShowDialog() == DialogResult.OK)
      {
        tbHeightmap.Text = dia.FileName;

      	File.Copy(dia.FileName, AppDomain.CurrentDomain.BaseDirectory + @"\temp." + Path.GetExtension(dia.FileName), true);
        pbPreview.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\temp." + Path.GetExtension(dia.FileName));

        hmapWidth = pbPreview.Image.Width;
        hmapHeight = pbPreview.Image.Height;
      }
    }

    private void btnGenerate_Click(object sender, EventArgs e)
    {
			if(!ValidateForm())
				return;
    	
			EditorLandscape landscape;

			if(tbHeightmap.Text == string.Empty)
			{
				landscape = new EditorLandscape(Host.Game, Convert.ToInt16(tbBlockSize2.Text), Convert.ToInt16(tbWidth.Text), Convert.ToInt16(tbHeight.Text), Host.Camera);
			}
			else
			{
				// File stream is non-seekable so convert to memory stream
				using (Stream stream = File.Open(tbHeightmap.Text, FileMode.Open))
				{
					landscape = new EditorLandscape(Host.Game, Convert.ToInt16(tbBlockSize1.Text), stream, Host.Camera);
				}
			}

    	landscape.Host = Host;

			Host.SceneManager.CurrentScreen.AddVisualObject(landscape);

			// Fire the delegate 
    	Globals.LandscapeAction land = (Globals.LandscapeAction) Tag;
    	land.Invoke(landscape);

    	Dispose();
    }

		private bool ValidateForm()
		{
			bool validated = true;

			if (tbHeightmap.Text != string.Empty)
			{
				if (tbBlockSize1.Text == string.Empty)
				{
					validated = false;
					eProvider.SetError(tbBlockSize1, "You must specify a block size!");
				}
			}
			else
			{
				if (tbWidth.Text == string.Empty)
				{
					validated = false;
					eProvider.SetError(tbWidth, "You must specify a width!");
				}
				if (tbHeight.Text == string.Empty)
				{
					validated = false;
					eProvider.SetError(tbHeight, "You must specify a height!");
				}
				if (tbBlockSize2.Text == string.Empty)
				{
					validated = false;
					eProvider.SetError(tbBlockSize2, "You must specify a block size!");
				}
			}

			return validated;
		}

		
  }
}