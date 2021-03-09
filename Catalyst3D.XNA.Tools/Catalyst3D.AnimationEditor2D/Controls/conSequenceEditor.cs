using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace AnimationEditor.Controls
{
	public partial class conSequenceEditor : UserControl
	{
    private readonly List<Sequence2D> Sequences = new List<Sequence2D>();
		private readonly IWindowsFormsEditorService Service;
    private Sequence2D CurrentSequence;
		private KeyFrame2D CurrentFrame;

		private bool HasFramesBeenAdded;

    public conSequenceEditor(List<Sequence2D> value, IWindowsFormsEditorService service)
		{
			Service = service;

			Sequences = value;

			InitializeComponent();

			lsSequences.DataSource = Sequences;
			lsSequences.DisplayMember = "Name";
		}
	
		private void tbName_TextChanged(object sender, EventArgs e)
		{
      if (!string.IsNullOrEmpty(tbName.Text))
        CurrentSequence.Name = tbName.Text;
		}

		private void lsFrames_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lsFrames.SelectedIndex == -1)
				return;

			CurrentFrame = CurrentSequence.Frames[lsFrames.SelectedIndex];

		  CurrentSequence.StartFrame = 0;
		  CurrentSequence.EndFrame = lsFrames.Items.Count - 1;

			tbFrameDuration.Text = CurrentFrame.Duration.ToString();
		}

		private void tbFrameDuration_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if(CurrentFrame != null)
				{
					CurrentFrame.Duration = (float)Convert.ToDouble(tbFrameDuration.Text);
				}
			}
			catch
			{
				return;
			}
		}

		private void tsRemoveSequence_Click(object sender, EventArgs e)
		{
			if(CurrentSequence != null)
			{
				Sequences.Remove(CurrentSequence);

				CurrentFrame = null;
				CurrentSequence = null;

				tbName.Text = string.Empty;

				tbFrameDuration.Text = string.Empty;

        lsFrames.DataSource = null;
			  
        lsSequences.DataSource = null;
        lsSequences.DataSource = Sequences;
        lsSequences.DisplayMember = "Name";
			}
		}

		private void tsRemoveFrame_Click(object sender, EventArgs e)
		{
			if(CurrentFrame != null)
			{
				if(
					MessageBox.Show("Are you sure you want to remove this frame?", "Frame Removal Confirmation",
													MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					CurrentSequence.Frames.Remove(CurrentFrame);
					CurrentFrame = null;

          lsFrames.DataSource = null;
          lsFrames.DataSource = CurrentSequence.Frames;
          lsFrames.DisplayMember = "AssetFilename";
				}
			}
		}

		private void lsSequences_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			if(lsSequences.SelectedIndex == -1)
				return;

			CurrentSequence = Sequences[lsSequences.SelectedIndex];

			tbName.Text = CurrentSequence.Name;

			lsFrames.DataSource = CurrentSequence.Frames;
			lsFrames.DisplayMember = "AssetFilename";
		}

		private void tsAdd_Click(object sender, EventArgs e)
		{
      Sequences.Add(new Sequence2D("unnamed"));

			lsSequences.DataSource = null;
			lsSequences.DataSource = Sequences;

			lsSequences.DisplayMember = "Name";
		}

		private void tsAddFrame_Click(object sender, EventArgs e)
		{
			HasFramesBeenAdded = true;

			if(CurrentSequence == null)
				return;

			OpenFileDialog dia = new OpenFileDialog();
			dia.Multiselect = true;
			dia.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.bmp|All Files|*.*";
			dia.InitialDirectory = Globals.AppSettings.ContentPath;

			if(dia.ShowDialog() == DialogResult.OK)
			{
				for (int i = 0; i < dia.FileNames.Length; i++)
				{
					if (!Directory.Exists(Globals.AppSettings.ProjectPath + @"\Temp\"))
						Directory.CreateDirectory(Globals.AppSettings.ProjectPath + @"\Temp\");

					// Move the texture to our project -> textures directory
					File.Copy(dia.FileNames[i], Utilitys.GetDestinationTexturePath(dia.FileNames[i]), true);

          KeyFrame2D frame = new KeyFrame2D();
					frame.AssetFilename = Path.GetFileName(dia.FileNames[i]);
					frame.Duration = 1;

					CurrentSequence.Frames.Add(frame);
				}
			}

			lsFrames.DataSource = null;
			lsFrames.DataSource = CurrentSequence.Frames;
			lsFrames.DisplayMember = "AssetFilename";
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if(HasFramesBeenAdded)
			{
				// Bake new sprite sheet!
				Globals.ReBakeSpriteSheet.Invoke(true);
			}

			Globals.RefreshSequenceDropDown.Invoke(false);
		  Globals.UpdateActorSequences.Invoke(Sequences);

			Service.CloseDropDown();
		}

	}
}