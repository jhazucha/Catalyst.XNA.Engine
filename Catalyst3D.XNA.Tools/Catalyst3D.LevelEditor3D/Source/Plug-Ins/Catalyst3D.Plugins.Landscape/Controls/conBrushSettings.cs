using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Catalyst3D.PluginSDK.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Telerik.WinControls.Docking;

namespace Catalyst3D.Plugins.Landscape.Controls
{
	public partial class conBrushSettings : DockWindow
	{
		private readonly IAddinHost Host;

		public EditorLandscape CurrentLandscape;

		public conBrushSettings(IAddinHost host)
		{
			SetGuid("d299c928-f001-4ad9-b85c-9b9fdd25e068");

			InitializeComponent();

			Host = host;

			// Create our Round Brush
			conBrush b1 = new conBrush();
			b1.BrushImage = Resource1.RoundBrush;
			b1.Click += OnBrushClicked;
			BrushContainer.Controls.Add(b1);

			// Create our Square Brush
			conBrush b2 = new conBrush();
			b2.BrushImage = Resource1.SquareBrush;
			b2.Click += OnBrushClicked;
			BrushContainer.Controls.Add(b2);

		}

		private void OnBrushClicked(object sender, EventArgs e)
		{
			foreach(Control c in BrushContainer.Controls)
			{
				if(c is conBrush)
				{
					conBrush b = c as conBrush;

					if(b == sender)
					{
						b.IsSelected = true;

						// Pass our brush mask to our landscape
						if(CurrentLandscape != null)
							CurrentLandscape.BrushMask = b.BrushImage;
					}
					else
						b.IsSelected = false;

					b.Invalidate();
				}
			}
		}

		private void cbBrushTexture_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbBrushTexture.SelectedIndex >= 0)
			{
				using(Stream stream = File.Open(Host.ProjectPath + @"\Content\" + cbBrushTexture.Items[cbBrushTexture.SelectedIndex], FileMode.Open))
				{
					if (stream.CanRead)
					{
						pbTexture.Image = Image.FromStream(stream);
					}
				}

				CurrentLandscape.CurrentPaintLayer = cbBrushTexture.SelectedIndex;
			}
		}

		private void tsAddTexture_Click(object sender, EventArgs e)
		{
			OpenFileDialog dia = new OpenFileDialog();
			dia.Filter = @"Image Files(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

			if(dia.ShowDialog() == DialogResult.OK)
			{
				// Move our texture into our projects Content Folder
				if (!File.Exists(Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName)))
					File.Copy(dia.FileName, Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName));

				using (Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName), FileMode.Open))
				{
					cbBrushTexture.Items.Add(Path.GetFileName(dia.FileName));

					int index = cbBrushTexture.Items.IndexOf(Path.GetFileName(dia.FileName));

					Texture2D texture = Texture2D.FromStream(CurrentLandscape.GraphicsDevice, stream);

					// Can only allow 4 textures at the moment
					switch(index)
					{
						case 0:
							CurrentLandscape.Layer1Path = Path.GetFileName(dia.FileName);
							CurrentLandscape.Layer1 = texture;
							break;
						case 1:
							CurrentLandscape.Layer2Path = Path.GetFileName(dia.FileName);
							CurrentLandscape.Layer2 = texture;
							break;
						case 2:
							CurrentLandscape.Layer3Path = Path.GetFileName(dia.FileName);
							CurrentLandscape.Layer3 = texture;
							break;
						case 3:
							CurrentLandscape.Layer4Path = Path.GetFileNameWithoutExtension(dia.FileName);
							CurrentLandscape.Layer4 = texture;
							break;
					}

					stream.Close();
				}
			}
		}

    private void btnSwap_Click(object sender, EventArgs e)
    {
      OpenFileDialog dia = new OpenFileDialog();
      dia.Filter = @"Image Files(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

      if (dia.ShowDialog() == DialogResult.OK)
      {
        // Move our texture into our projects Content Folder
        if (!File.Exists(Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName)))
          File.Copy(dia.FileName, Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName));

        using (Stream stream = File.Open(Host.ProjectPath + @"\Content\" + Path.GetFileName(dia.FileName), FileMode.Open))
        {
          // Current Index
          int index = cbBrushTexture.SelectedIndex;

          // Add this new one in its place
          cbBrushTexture.Items[index] = (Path.GetFileName(dia.FileName));

          Texture2D texture = Texture2D.FromStream(CurrentLandscape.GraphicsDevice, stream);

          // Can only allow 4 textures at the moment
          switch (index)
          {
            case 0:
              CurrentLandscape.Layer1Path = Path.GetFileName(dia.FileName);
              CurrentLandscape.Layer1 = texture;
              break;
            case 1:
              CurrentLandscape.Layer2Path = Path.GetFileName(dia.FileName);
              CurrentLandscape.Layer2 = texture;
              break;
            case 2:
              CurrentLandscape.Layer3Path = Path.GetFileName(dia.FileName);
              CurrentLandscape.Layer3 = texture;
              break;
            case 3:
              CurrentLandscape.Layer4Path = Path.GetFileNameWithoutExtension(dia.FileName);
              CurrentLandscape.Layer4 = texture;
              break;
          }

          stream.Close();
        }
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      // Can only allow 4 textures at the moment
      switch (cbBrushTexture.SelectedIndex)
      {
        case 0:
          CurrentLandscape.Layer1Path = string.Empty;
          CurrentLandscape.Layer1 = null;
          break;
        case 1:
          CurrentLandscape.Layer2Path = string.Empty;
          CurrentLandscape.Layer2 = null;
          break;
        case 2:
          CurrentLandscape.Layer3Path = string.Empty;
          CurrentLandscape.Layer3 = null;
          break;
        case 3:
          CurrentLandscape.Layer4Path = string.Empty;
          CurrentLandscape.Layer4 = null;
          break;
      }

      if (MessageBox.Show("Remove paint mask associated with this texture from the landscape too?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        Microsoft.Xna.Framework.Color[] pixels =
          new Microsoft.Xna.Framework.Color[CurrentLandscape.PaintMask1.Width*CurrentLandscape.PaintMask1.Height];
        CurrentLandscape.PaintMask1.GetData(pixels);

        switch (cbBrushTexture.SelectedIndex)
        {
          case 0:
            for (int i = 0; i < pixels.Length; i++)
              pixels[i] = new Microsoft.Xna.Framework.Color(0, pixels[i].G, pixels[i].B, pixels[i].A);
            break;
          case 1:
            for (int i = 0; i < pixels.Length; i++)
              pixels[i] = new Microsoft.Xna.Framework.Color(pixels[i].R, 0, pixels[i].B, pixels[i].A);
            break;
          case 2:
            for (int i = 0; i < pixels.Length; i++)
              pixels[i] = new Microsoft.Xna.Framework.Color(pixels[i].R, pixels[i].G, 0, pixels[i].A);
            break;
          case 3:
            for (int i = 0; i < pixels.Length; i++)
              pixels[i] = new Microsoft.Xna.Framework.Color(pixels[i].R, pixels[i].G, pixels[i].B, 0);
            break;
        }

        Host.Game.GraphicsDevice.Textures[4] = null;

        CurrentLandscape.PaintMask1.SetData(pixels);
      }

      // Remove it from our list
      cbBrushTexture.Items.RemoveAt(cbBrushTexture.SelectedIndex);

      cbBrushTexture.Text = string.Empty;

      pbTexture.Image = null;
      pbTexture.Refresh();
    }
	}
}