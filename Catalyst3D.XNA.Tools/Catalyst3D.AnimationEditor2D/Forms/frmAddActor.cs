using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AnimationEditor.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Tools;

namespace AnimationEditor.Forms
{
  public partial class frmAddActor : Form
  {
    private readonly List<string> loadedTextures = new List<string>();

    public EditorActor Actor;

    public frmAddActor()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      Globals.AddSequence += OnAddSequenceEvent;
    }

    private void OnAddSequenceEvent(string name)
    {
      if(tvSequences.Nodes.ContainsKey(name))
      {
        MessageBox.Show("Sequence already exists!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return;
      }

      // Add it
      tvSequences.Nodes.Add(name, name);

      // Select it
      tvSequences.SelectedNode = tvSequences.Nodes[name];
    }

    private void tsAddSprite_Click(object sender, EventArgs e)
    {
      if (tvSequences.SelectedNode == null)
        return;

      OpenFileDialog dia = new OpenFileDialog();
      dia.Multiselect = true;
      dia.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.bmp|All Files|*.*";
      dia.InitialDirectory = Globals.AppSettings.ContentPath;

      if(dia.ShowDialog() == DialogResult.OK)
      {
        for(int i = 0; i < dia.FileNames.Length; i++)
        {
          string filename = Path.GetFileNameWithoutExtension(dia.FileNames[i]);

          TreeNode node = new TreeNode();
          node.Tag = Path.GetFileName(dia.FileNames[i]);
          node.ImageKey = dia.FileNames[i];
          if (filename != null) node.Text = filename;

          if (tvSequences.SelectedNode.Parent != null)
            tvSequences.SelectedNode.Parent.Nodes.Add(node);
          else
            tvSequences.SelectedNode.Nodes.Add(node);

          // Move the texture to our project -> textures directory
          File.Copy(dia.FileNames[i], Utilitys.GetDestinationTexturePath(dia.FileNames[i]), true);

          loadedTextures.Add(Utilitys.GetDestinationTexturePath(dia.FileNames[i]));
        }
      }
    }

    private void tvSequences_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if(e.Node.ImageKey == string.Empty)
        return;

      pbPreview.Image = Image.FromFile(e.Node.ImageKey);
    }

		private void btnAddActor_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show("Are you sure you want to add this to the scene?", "Add Actor Confirmation", MessageBoxButtons.YesNo,
				                MessageBoxIcon.Question) == DialogResult.No)
				return;

		  // Disable all thse
      tbName.Enabled = false;
		  cbRole.Enabled = false;
		  gbSequences.Enabled = false;
		  gbPreview.Enabled = false;
			btnSave.Enabled = false;
      
		  pgBar.Style = ProgressBarStyle.Continuous;

      // Create our actor
      Actor = new EditorActor(Globals.Game, string.Empty, string.Empty);
			
			if (tvSequences.Nodes.Count >= 0)
			{
				// Set our Actors Role
				switch (cbRole.Text)
				{
					case "Player":
						Actor.Role = ActorRole.Player;
						break;
					case "Enemy":
						Actor.Role = ActorRole.Enemy;
						break;
					case "NPC":
						Actor.Role = ActorRole.NPC;
						break;
					case "Prop":
						Actor.Role = ActorRole.Prop;
						break;
				}

				Actor.Name = tbName.Text;

				// Init our clip player
				Actor.ClipPlayer = new AnimationPlayer2D(Globals.Game);
				Actor.ClipPlayer.Initialize();

				// Loop thru the Tree view controls Nodes adding our Key Frames to our Clip Player
				foreach (TreeNode node in tvSequences.Nodes)
				{
					// Reset our frame index
					{
						int endFrameIndex = (node.Nodes.Count - 1);

						Sequence2D sequence = new Sequence2D(node.Name);
						sequence.StartFrame = 0;
						sequence.EndFrame = endFrameIndex;

						// Add our Frames
						foreach (TreeNode child in tvSequences.Nodes[node.Name].Nodes)
						{
							KeyFrame2D frame = new KeyFrame2D();
							frame.Duration = 1f;
							frame.AssetFilename = child.Tag.ToString();

							sequence.Frames.Add(frame);
						}

						// Add our Sequence
						Actor.ClipPlayer.Sequences.Add(sequence);
					}
				}

				// Generate our Sprite Sheet for the Clip Player
				SpriteTools.GenerateSpriteSheet(Actor, Globals.AppSettings.ProjectPath, tbName.Text);

				Actor.ClipPlayer.Texture = Catalyst3D.XNA.Engine.UtilityClasses.Utilitys.TextureFromFile(
					Globals.Game.GraphicsDevice, Globals.AppSettings.ProjectPath + @"\" + Actor.SpriteSheetFileName);
			}

			// Default to the idle sequence
      if (Actor.Sequences.Count > 0)
        Actor.Play(Actor.ClipPlayer.Sequences[0].Name, true);

			// Add it to our Scene
		  Globals.ActorAdded.Invoke(Actor);

			tvSequences.Nodes.Clear();

			Close();
		}

  	private void tsRemove_Click(object sender, EventArgs e)
    {
      // Only delete child nodes
      if(tvSequences.SelectedNode != null && tvSequences.SelectedNode.Parent != null)
      {
        loadedTextures.Remove(tvSequences.SelectedNode.Text);
        pbPreview.Image = null;
        tvSequences.Nodes.Remove(tvSequences.SelectedNode);
      }
    }

    private void tsAddSequence_Click(object sender, EventArgs e)
    {
      frmAddSequence frm = new frmAddSequence();
      frm.Show();
    }

    private void tsRemoveSequence_Click(object sender, EventArgs e)
    {
      if (
        MessageBox.Show("Are you sure you want to remove this sequence?", "Remove Sequence Confirmation",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        tvSequences.Nodes.RemoveByKey(tvSequences.SelectedNode.Name);
    }
  }
}