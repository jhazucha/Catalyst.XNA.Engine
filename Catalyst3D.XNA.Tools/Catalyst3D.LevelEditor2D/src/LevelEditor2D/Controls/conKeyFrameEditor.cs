using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace LevelEditor2D.Controls
{
  public partial class conKeyFrameEditor : UserControl
  {
		public List<KeyFrame2D> Frames { get; set; }
    private readonly IWindowsFormsEditorService Service;

    public conKeyFrameEditor()
    {
      InitializeComponent();
    }

		public conKeyFrameEditor(List<KeyFrame2D> value, IWindowsFormsEditorService service)
    {
      InitializeComponent();

      Service = service;
      Frames = value;

			Globals.IsDialogWindowOpen = true;

			foreach(KeyFrame2D frame in Frames)
			{
				lsFrames.Items.Add("[" + frame.FrameNumber + "] " + frame.AssetFilename);
			}
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
    	Globals.IsDialogWindowOpen = false;

      Service.CloseDropDown();
    }

    private void tbDuration_TextChanged(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(tbDuration.Text))
        Frames[lsFrames.SelectedIndex].Duration = (float) Convert.ToDouble(tbDuration.Text);
    }

    private void lsFrames_SelectedIndexChanged(object sender, EventArgs e)
    {
			//tbDuration.Text = Frames[lsFrames.SelectedIndex].Duration.ToString();

			//Sprite sprite = Frames[lsFrames.SelectedIndex] as Sprite;
			//if(sprite != null)
			//{
			//  pbFramePic.Image = Image.FromFile(Globals.AppSettings.ProjectPath + sprite.AssetFilePath);
			//}
    }

    private void tsRemove_Click(object sender, EventArgs e)
    {
			//if (lsFrames.SelectedIndex != -1)
			//{
			//  Frames.RemoveAt(lsFrames.SelectedIndex);

			//  lsFrames.Items.Clear();

			//  foreach(KeyFrame2D frame in Frames)
			//  {
			//    lsFrames.Items.Add("[" + frame.FrameNumber + "] " + frame.VisualObjects[0].AssetName);
			//  }
			//}
    }
  }
}
