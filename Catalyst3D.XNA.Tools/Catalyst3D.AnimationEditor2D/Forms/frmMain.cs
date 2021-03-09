using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using AnimationEditor.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Catalyst3D.XNA.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationEditor.Forms
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();

			// Show the mouse cursor
			Globals.Game.IsMouseVisible = true;

			// Wire up some of our global events
			Globals.LoadProject += LoadProject;
			Globals.SaveProject += SaveProject;
			Globals.ActorAdded += OnActorAdded;
			Globals.ReBakeSpriteSheet += OnReBakeSpriteSheet;
			Globals.RefreshSequenceDropDown += OnRefreshSequenceDropDown;
			Globals.Update += OnFrameUpdate;
		}

		private void OnFrameUpdate(GameTime gametime)
		{
			if(conRenderer.Actor != null)
				lbFrame.Text = conRenderer.Actor.ClipPlayer.CurrentFrameIndex.ToString();
		}

		private void OnRefreshSequenceDropDown(bool showresult)
		{
			// Clear the list
			tsSequence.Items.Clear();

			// Update our Available sequence display drop down with our sequences
			foreach (Sequence2D s in conRenderer.Actor.Sequences)
				tsSequence.Items.Add(s.Name);
		}

		private void OnReBakeSpriteSheet(bool showresult)
		{
			try
			{
				// Generate our Sprite Sheet
				SpriteTools.GenerateSpriteSheet(conRenderer.Actor, Globals.AppSettings.ProjectPath, conRenderer.Actor.Name);

				Image img = Image.FromFile(Globals.AppSettings.ProjectPath + @"\" + conRenderer.Actor.SpriteSheetFileName);

				MemoryStream stream = new MemoryStream();

				img.Save(stream, ImageFormat.Png);

				// Load the stream into a texture2D object
				conRenderer.Actor.ClipPlayer.Texture = Texture2D.FromStream(Globals.Game.GraphicsDevice, stream);

				// Close the Stream
				stream.Close();

				if (showresult)
					MessageBox.Show("Sprite sheet was successfully rebaked ..");
			}
			catch(Exception er)
			{
				throw new Exception(er.Message);
			}
		}

		private void OnActorAdded(EditorActor obj)
		{
		  conRenderer.Actor = obj;

			pgGrid.SelectedObject = obj;

			Globals.SaveProject.Invoke(false);
			Globals.RefreshSequenceDropDown.Invoke(false);
		}

		private void SaveProject(bool showresult)
		{
			// If no project is loaded prompt to create/save it
			if(!Globals.IsProjectLoaded && conRenderer.Actor != null)
			{
				SaveFileDialog dia = new SaveFileDialog();

				dia.Filter = "Catalyst3D Actor File|*.a2d";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					// Store the path\filename in our global settings
					Globals.AppSettings.ProjectPath = Path.GetDirectoryName(dia.FileName);
					Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.FileName);

					Actor actor = new Actor
					              	{
					              		AssetName = conRenderer.Actor.AssetName,
														SpriteSheetFileName = conRenderer.Actor.SpriteSheetFileName,
					              		Direction = conRenderer.Actor.Direction,
					              		DrawOrder = conRenderer.Actor.DrawOrder,
					              		UpdateOrder = conRenderer.Actor.UpdateOrder,
					              		Name = conRenderer.Actor.Name,
					              		PlayerIndex = conRenderer.Actor.PlayerIndex,
					              		Position = Vector2.Zero,
					              		Role = conRenderer.Actor.Role,
					              		Scale = conRenderer.Actor.Scale,
					              		Enabled = conRenderer.Actor.Enabled,
					              		Visible = conRenderer.Actor.Visible
					              	};

					actor.ClipPlayer = new AnimationPlayer2D
					                   	{
					                   		AssetName = conRenderer.Actor.ClipPlayer.AssetName,
					                   		Sequences = conRenderer.Actor.ClipPlayer.Sequences
					                   	};

					// Save the project to disk
					Serializer.Serialize(Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename, actor);

					// Flag that a project is loaded as well
					Globals.IsProjectLoaded = true;

					if (showresult)
						MessageBox.Show("Successfully saved the Actor!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					return;
				}
			}
			else
			{
        if (conRenderer.Actor == null)
          return;

				// Convert our current Actor Object to a CTBaseActor to pass to our game
				Actor actor = new Actor
												{
													AssetName = conRenderer.Actor.AssetName,
													SpriteSheetFileName = conRenderer.Actor.SpriteSheetFileName,
													Direction = conRenderer.Actor.Direction,
													DrawOrder = conRenderer.Actor.DrawOrder,
													UpdateOrder = conRenderer.Actor.UpdateOrder,
													Name = conRenderer.Actor.Name,
													PlayerIndex = conRenderer.Actor.PlayerIndex,
													Position = Vector2.Zero,
													Role = conRenderer.Actor.Role,
													Scale = conRenderer.Actor.Scale,
													Enabled = conRenderer.Actor.Enabled,
													Visible = conRenderer.Actor.Visible
												};

				actor.ClipPlayer = new AnimationPlayer2D
														 {
															 AssetName = conRenderer.Actor.ClipPlayer.AssetName,
															 Sequences = conRenderer.Actor.ClipPlayer.Sequences
														 };

				// Save the project to disk
				Serializer.Serialize(Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename, actor);

				// Flag that a project is loaded as well
				Globals.IsProjectLoaded = true;

				if(showresult)
					MessageBox.Show("Successfully saved the Actor!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void LoadProject(bool showresult)
		{
			// Create our Open File Dialog
			OpenFileDialog dia = new OpenFileDialog();
			dia.Filter = "Catalyst3D Actor File|*.a2d";
			dia.InitialDirectory = Globals.AppSettings.ProjectPath;

			if(dia.ShowDialog() == DialogResult.OK)
			{
        try
        {
          if (Globals.IsProjectLoaded)
          {
            // Flush the sequencer drop down for available sequences
            tsSequence.Items.Clear();
            conRenderer.Manager.CurrentScreen.VisualObjects.Clear();
          }

          // Store our current projects path and file name
          Globals.AppSettings.ProjectPath = Path.GetDirectoryName(dia.FileName);
          Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.FileName);

          Actor original = Serializer.Deserialize<Actor>(dia.FileName);

        	EditorActor actor = new EditorActor(Globals.Game, string.Empty, original.AssetName)
        	                    	{
        	                    		AssetName = original.AssetName,
																	SpriteSheetFileName = original.SpriteSheetFileName,
        	                    		Direction = original.Direction,
        	                    		DrawOrder = original.DrawOrder,
        	                    		UpdateOrder = original.UpdateOrder,
        	                    		Name = original.Name,
        	                    		PlayerIndex = original.PlayerIndex,
        	                    		Position = Vector2.Zero,
        	                    		Role = original.Role,
        	                    		Scale = original.Scale,
        	                    		Enabled = original.Enabled,
        	                    		Visible = original.Visible,
        	                    		GameScreen = Globals.Renderer
                          };

        	actor.ClipPlayer = new AnimationPlayer2D(Globals.Game)
        	                   	{
        	                   		AssetName = original.ClipPlayer.AssetName,
        	                   		Sequences = original.ClipPlayer.Sequences,
																GameScreen = Globals.Renderer
        	                   	};

          actor.ClipPlayer.Initialize();

					actor.ClipPlayer.Texture = Catalyst3D.XNA.Engine.UtilityClasses.Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.AppSettings.ProjectPath + @"\" + original.SpriteSheetFileName);

          // Play the first sequence by default after loading
          if (actor.ClipPlayer.Sequences.Count > 0)
            actor.Play(actor.ClipPlayer.Sequences[0].Name, true);

          actor.Initialize();

          conRenderer.Actor = actor;

          pgGrid.SelectedObject = actor;

					Globals.Renderer.AddVisualObject(actor);

          Globals.RefreshSequenceDropDown.Invoke(false);

          Globals.IsProjectLoaded = true;
        }
        catch
        {
          MessageBox.Show("Error: Failed to load the sprite sheet for this actor project!");

          Globals.IsProjectLoaded = false;

          conRenderer.Actor = null;

          if (conRenderer.Manager.CurrentScreen != null)
            conRenderer.Manager.CurrentScreen.VisualObjects.Clear();
        }
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmOptions frm = new frmOptions();
			frm.Show();
		}

		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(conRenderer.Actor != null)
			{
				if(MessageBox.Show("Save Current Project?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
						DialogResult.Yes)
				{
					SaveProject(true);
				}
			}

			conRenderer.Actor = null;

			// Create our Folder Browse
			FolderBrowserDialog dia = new FolderBrowserDialog();
			dia.SelectedPath = Globals.AppSettings.ProjectPath;
			dia.ShowNewFolderButton = true;
			dia.RootFolder = Environment.SpecialFolder.DesktopDirectory;

      if (dia.ShowDialog() == DialogResult.OK)
      {
        // Create our fresh collection for holding scene objects
        Globals.AppSettings.ProjectPath = dia.SelectedPath;
        Directory.CreateDirectory(Globals.AppSettings.ProjectPath + @"\Temp");

        Globals.Game.Content.RootDirectory = Globals.AppSettings.ProjectPath + @"\";

        Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.SelectedPath);

        frmAddActor frm = new frmAddActor();
        frm.Show();
      }
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Prompt to save if actor object is dirty?

			Globals.Game.Exit();
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Globals.LoadProject.Invoke(true);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveProject(true);
		}

		private void frmMain_SizeChanged(object sender, EventArgs e)
		{
			Globals.WindowSizeChanged.Invoke(sender, e);
		}

		private void tsPlay_Click(object sender, EventArgs e)
		{
			if (conRenderer.Actor != null)
			{
				if (tsSequence.Text != string.Empty)
				{
					conRenderer.Actor.Play(tsSequence.Text, conRenderer.Actor.ClipPlayer.IsLooped);
				}
				else
					MessageBox.Show("Please a sequence to play", "Sequence Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void btnLoop_Click(object sender, EventArgs e)
		{
			if(conRenderer.Actor == null)
				return;

			if(conRenderer.Actor.ClipPlayer.IsLooped)
			{
				conRenderer.Actor.ClipPlayer.IsLooped = false;
			  btnLoop.Text = "Looping Disabled";
			}
			else
			{
				conRenderer.Actor.ClipPlayer.IsLooped = true;
				btnLoop.Text = "Looping Enabled";
			}
		}

		private void tsStop_Click(object sender, EventArgs e)
		{
			if(conRenderer.Actor != null)
			{
				conRenderer.Actor.ClipPlayer.Stop(true);
			}
		}

		private void tsSequence_TextChanged(object sender, EventArgs e)
		{
			if(tsSequence.Text != string.Empty)
			{
				if(conRenderer.Actor != null)
				{
					conRenderer.Actor.Play(tsSequence.Text, conRenderer.Actor.ClipPlayer.IsLooped);
				}
			}
		}

		private void tsPreviousFrame_Click(object sender, EventArgs e)
		{
			if(conRenderer.Actor.ClipPlayer.CurrentFrameIndex > conRenderer.Actor.ClipPlayer.CurrentSequence.StartFrame)
			{
				conRenderer.Actor.ClipPlayer.IsPlaying = false;
				conRenderer.Actor.ClipPlayer.CurrentFrameIndex--;
			}
		}

		private void tsNextFrame_Click(object sender, EventArgs e)
		{
			if(conRenderer.Actor.ClipPlayer.CurrentFrameIndex < conRenderer.Actor.ClipPlayer.CurrentSequence.EndFrame)
			{
				conRenderer.Actor.ClipPlayer.IsPlaying = false;
				conRenderer.Actor.ClipPlayer.CurrentFrameIndex++;
			}
		}

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (conRenderer.Actor != null)
      {
        if (MessageBox.Show("Save Current Project?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
            DialogResult.Yes)
        {
          SaveProject(true);
        }
      }

      Globals.IsProjectLoaded = false;
      
      conRenderer.Actor = null;

      if (conRenderer.Manager.CurrentScreen != null)
        conRenderer.Manager.CurrentScreen.VisualObjects.Clear();

      pgGrid.SelectedObject = null;
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (conRenderer.Actor == null)
        return;

      SaveFileDialog dia = new SaveFileDialog();

      dia.Filter = "Catalyst3D Actor File|*.a2d";
      dia.InitialDirectory = Globals.AppSettings.ProjectPath;

      if (dia.ShowDialog() == DialogResult.OK)
      {
        // Store the path\filename in our global settings
        Globals.AppSettings.ProjectPath = Path.GetDirectoryName(dia.FileName);
        Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.FileName);

        Actor actor = new Actor
        {
          AssetName = conRenderer.Actor.AssetName,
					SpriteSheetFileName = conRenderer.Actor.SpriteSheetFileName,
          Direction = conRenderer.Actor.Direction,
          DrawOrder = conRenderer.Actor.DrawOrder,
          UpdateOrder = conRenderer.Actor.UpdateOrder,
          Name = conRenderer.Actor.Name,
          PlayerIndex = conRenderer.Actor.PlayerIndex,
          Position = Vector2.Zero,
          Role = conRenderer.Actor.Role,
          Scale = conRenderer.Actor.Scale,
          Enabled = conRenderer.Actor.Enabled,
          Visible = conRenderer.Actor.Visible
        };

        actor.ClipPlayer = new AnimationPlayer2D
        {
          AssetName = conRenderer.Actor.ClipPlayer.AssetName,
          Sequences = conRenderer.Actor.ClipPlayer.Sequences
        };

        // Save the project to disk
        Serializer.Serialize(Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename, actor);

        // Flag that a project is loaded as well
        Globals.IsProjectLoaded = true;

        MessageBox.Show("Successfully saved the Actor!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
        return;
      }
    }
	}
}