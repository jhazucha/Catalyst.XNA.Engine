using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LevelEditor2D.Controls
{
	public partial class conSpriteContainer : UserControl
	{
	  public conRenderer RenderWindow;

		public int SelectedSpriteIndex = -1;

		public conSpriteContainer()
		{
			InitializeComponent();

			// Hook container events
			Globals.ContainerEvent += OnContainerEvent;
		}

		private void OnContainerEvent(int index)
		{
			SelectedSpriteIndex = index;

			for(int i = 0; i < pnContainer.Controls.Count; i++)
			{
				conSprite con = pnContainer.Controls[i] as conSprite;

				if(con == null)
					return;

				if(con.Index == index)
				{
					con.IsSelected = true;
				}
				else
					con.IsSelected = false;
			}
			Refresh();
		}

		public void AddSprite(Image img, string filename)
		{
			if(Globals.Game != null)
			{
        conSprite sprite = new conSprite(Globals.Game, img, RenderWindow);
				sprite.Tag = Globals.ContainerEvent;
				sprite.Name = Path.GetFileNameWithoutExtension(filename);
				sprite.FileName = filename;

				pnContainer.Controls.Add(sprite);
				sprite.Index = pnContainer.Controls.IndexOf(sprite);
			}
		}

		private void tsAdd_Click(object sender, EventArgs e)
		{
			if(!Globals.IsProjectLoaded)
			{
				// Invoke to save the project first
				MessageBox.Show(@"No Project Loaded! Please create a New Project or Load an Existing Project before adding Assets!");
				return;
			}

			OpenFileDialog dia = new OpenFileDialog();

			dia.Multiselect = true;
			dia.Filter = @"PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.bmp|All Files|*.*";
			dia.InitialDirectory = Globals.AppSettings.ContentPath;

			if(dia.ShowDialog() == DialogResult.OK)
			{
				foreach(string t in dia.FileNames)
					LoadSprite(t);
			}
		}

		public void LoadSprite(string filename)
		{
			// Move the texture to our project -> textures directory
			if(!File.Exists(Globals.GetDestinationTexturePath(filename)))
				File.Copy(filename, Globals.GetDestinationTexturePath(filename), true);

			// Load our Image to use in the container
			Image img = Image.FromFile(Globals.GetDestinationTexturePath(filename));

			// Load up a Sprite Control to add to our Flow Panel
      conSprite sprite = new conSprite(Globals.Game, img, RenderWindow);

			pnContainer.Controls.Add(sprite);

			sprite.Name = Path.GetFileNameWithoutExtension(filename);
			sprite.FileName = Globals.GetDestinationTexturePath(filename);

			sprite.Tag = Globals.ContainerEvent;
			sprite.Index = pnContainer.Controls.IndexOf(sprite);

			if(sprite.Index == 0)
			{
				sprite.IsSelected = true;
				Refresh();
			}
		}

		private void tsDelete_Click(object sender, EventArgs e)
		{
			if(SelectedSpriteIndex == -1)
				return;

			if(
				MessageBox.Show(@"Are you sure you want to remove the sprite from the project?", @"Sprite Deletion",
												MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				conSprite sprite = pnContainer.Controls[SelectedSpriteIndex] as conSprite;

				if (sprite != null)
				{
					sprite.SpriteImage = null;

					pnContainer.Controls.Remove(sprite);

					// Delete the texture from the texture folder
					if (File.Exists(Globals.AppSettings.ProjectPath + @"\Textures\" + sprite.FileName))
					{
						//File.Delete(Globals.AppSettings.ProjectPath + @"\Textures\" + sprite.FileName);
					}
				}

				// Update index's on sprites due to the removal of one
				foreach(Control c in pnContainer.Controls)
				{
					if (c is conSprite)
					{
						conSprite s = c as conSprite;
						s.IsSelected = false;
						s.Index = pnContainer.Controls.IndexOf(s);

						if(s.Index == 0)
						{
							s.IsSelected = true;
							Refresh();
						}
					}
				}

			Refresh();
			}
		}
	}
}