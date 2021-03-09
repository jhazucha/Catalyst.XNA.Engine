using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using LevelEditor2D.EntityClasses;
using LevelEditor2D.Forms;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.Controls
{
	public partial class conKeyFrameContainer : UserControl
	{
		public Globals.SequenceAddEvent AddSequenceEvent;
		public Globals.SequenceRemoveEvent RemoveSequenceEvent;
	  public Globals.ObjectGroupEvent VisualObjectsGroupped;

		private float TimeSinceLastFrame;
		private bool isPlaying;

		public int CurrentSelectedFrame { get; set; }
		public bool IsPlaying
		{
			get { return isPlaying; }
			set
			{
				isPlaying = value;

				// If We are playing deselect all frames!
				if(value)
				{
					for(int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
					{
						conKeyFrame frame = pnKeyFrameContainer.Controls[i] as conKeyFrame;

						if(frame != null)
						{
							for(int j = 0; j < frame.VisualObjects.Count; j++)
							{
								foreach(VisualObject obj in frame.VisualObjects)
								{
									if(obj is Actor)
									{
										Actor a = obj as Actor;
										a.Play("Idle", true);
									}

									obj.IsSelected = false;
								}
							}
						}
					}
				}
				else
				{
					for(int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
					{
						conKeyFrame frame = pnKeyFrameContainer.Controls[i] as conKeyFrame;

						if(frame != null)
						{
							for(int j = 0; j < frame.VisualObjects.Count; j++)
							{
								foreach(VisualObject obj in frame.VisualObjects)
								{
									if(obj is Actor)
									{
										Actor a = obj as Actor;
										a.Stop(true);
									}
								}
							}
						}
					}
				}
			}
		}

		public Game1 Game { get; set; }

		public List<Sequence2D> Sequences = new List<Sequence2D>();

		public conKeyFrameContainer()
		{
			InitializeComponent();

			// Hook container events
			Globals.FrameSelectedEvent += OnFrameSelected;
			Globals.FrameRemovedEvent += OnItemRemovedEvent;
		  Globals.RemoveAllFramesEvent += OnFramesFlushedEvent;

		  VisualObjectsGroupped += OnCurrentFrameObjectsGroupped;
			AddSequenceEvent += OnSequenceAddedEvent;
			RemoveSequenceEvent += OnSequenceRemovedEvent;

			// Hook our Static Delegate to add sprites to our key frames
      Globals.ObjectAdded += AddObject;

			// Add a frame out the gate
			AddFrame(new conKeyFrame());
		}

	  private void OnFramesFlushedEvent()
	  {
      for (int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
      {
        conKeyFrame con = pnKeyFrameContainer.Controls[i] as conKeyFrame;

        if (con == null)
          return;

        con.Dispose();
      }

      Refresh();
	  }

    private void OnCurrentFrameObjectsGroupped(List<VisualObject> objects)
    {
      // Grab our frames
      var frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;

      if (frame != null)
      {
        // Flush the old objects out with this new version
        frame.VisualObjects = objects;
      }
    }

	  private List<Sequence2D> OnSequenceRemovedEvent(int index)
		{
			if (index != -1)
				Sequences.RemoveAt(index);

			return Sequences;
		}

		private List<Sequence2D> OnSequenceAddedEvent(Sequence2D seq)
		{
			if (seq != null)
				Sequences.Add(seq);
			
			return Sequences;
		}

		private void OnItemRemovedEvent(int frameNumber)
		{
			if (pnKeyFrameContainer.Controls.Count <= 0)
				return;

			try
			{
				pnKeyFrameContainer.Controls[frameNumber].Dispose();

				for (int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
				{
					var con = pnKeyFrameContainer.Controls[i] as conKeyFrame;

					if (con == null)
						return;

					if (con.FrameNumber == 0)
					{
						con.IsSelected = true;
					}
					else
						con.IsSelected = false;

					con.FrameNumber = pnKeyFrameContainer.Controls.IndexOf(con);
				}

				Refresh();
			}
			catch (Exception er)
			{
				MessageBox.Show(er.Message);
			}
		}

		private void OnFrameSelected(int frameNumber)
		{
			CurrentSelectedFrame = frameNumber;

			for (int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
			{
				conKeyFrame con = pnKeyFrameContainer.Controls[i] as conKeyFrame;

				if (con == null)
					return;

				if (con.FrameNumber == frameNumber)
				{
					con.IsSelected = true;
				}
				else
					con.IsSelected = false;
			}

			Refresh();
		}

		private void tsAddFrame_Click(object sender, EventArgs e)
		{
			// No frames in the contrainer add one as normal
			AddFrame(new conKeyFrame());
			CurrentSelectedFrame++;

			// Invoke to update and select the new frame
			Globals.FrameSelectedEvent.Invoke(CurrentSelectedFrame);
		}

		private void tsAddInFront_Click(object sender, EventArgs e)
		{
			ControlCollection oldCollection = pnKeyFrameContainer.Controls;
			List<conKeyFrame> frames = new List<conKeyFrame>();

			int desiredIndex = CurrentSelectedFrame;
			
			for (int i = 0; i < oldCollection.Count; i++)
			{
				// Put the new control here
				if (i == desiredIndex)
				{
					// Insert our new one here
					conKeyFrame newFrame = new conKeyFrame();
					newFrame.Tag = Globals.FrameSelectedEvent;
					newFrame.FrameNumber = desiredIndex;
					newFrame.IsSelected = true;
					frames.Add(newFrame);

					// Now Insert our previous frame infront of our desired one we just added
					conKeyFrame prevFrame = oldCollection[i] as conKeyFrame;

					if (prevFrame != null)
					{
						prevFrame.FrameNumber = desiredIndex + 1;
						prevFrame.IsSelected = false;
						frames.Add(prevFrame);
					}

					// Update our currently selected frame
					CurrentSelectedFrame = desiredIndex;
				}
				else
				{
					// This frame was not hte sele
					conKeyFrame f = oldCollection[i] as conKeyFrame;
					if (f != null)
					{
						f.IsSelected = false;
						frames.Add(f);
						f.FrameNumber = frames.IndexOf(f);
					}
				}
			}

			// Clear the container
			pnKeyFrameContainer.Controls.Clear();

			// Loop thru and add them to our control collection
			for (int i = 0; i < frames.Count; i++)
			{
				pnKeyFrameContainer.Controls.Add(frames[i]);
			}

			// Invoke to update and select the new frame
			//FrameSelectedEvent.Invoke(CurrentSelectedFrame);

			Refresh();
		}

		public void AddFrame(conKeyFrame con)
		{
			// Create Frame and add it to our container
			pnKeyFrameContainer.Controls.Add(con);

			// Display the Frame number on the frame we just added (indexof returns 0 based array so add 1)
			con.Tag = Globals.FrameSelectedEvent;
			con.FrameNumber = pnKeyFrameContainer.Controls.IndexOf(con);

			if (con.FrameNumber == -1)
			{
				if ((con.FrameNumber + 1) == 1)
				{
					con.IsSelected = true;
					Refresh();
				}
			}
		}

    public void AddObject(VisualObject obj)
    {
      var frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;

      if (frame != null)
      {
				if(string.IsNullOrEmpty(obj.Name))
					obj.Name = obj.GetType().ToString();

      	frame.VisualObjects.Add(obj);
      }
    }

    public List<VisualObject> GetObjects(int frameNumber)
    {
      var frame = pnKeyFrameContainer.Controls[frameNumber] as conKeyFrame;

      if (frame != null)
        return frame.VisualObjects;

      return null;
    }

	  public bool RemoveObject(VisualObject obj)
    {
      for (int i = 0; i < pnKeyFrameContainer.Controls.Count; i++)
      {
        conKeyFrame frame = pnKeyFrameContainer.Controls[i] as conKeyFrame;

        // Found a Key Frame check to see if our sprite is in its collection
        if (frame != null)
        {
          // loop thru all the sprites in this frame
          for (int s = 0; s < frame.VisualObjects.Count; s++)
          {
            if (frame.VisualObjects[s] == obj)
            {
              frame.VisualObjects.Remove(obj);
              return true;
            }
          }
        }
      }
      return false;
    }

		private void tsLockFrame_Click(object sender, EventArgs e)
		{
			var frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;
			if (frame != null)
				frame.IsLocked = true;

			pnKeyFrameContainer.Refresh();
		}

		private void tsUnlockFrame_Click(object sender, EventArgs e)
		{
			var frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;
			if (frame != null)
				frame.IsLocked = false;

			pnKeyFrameContainer.Refresh();
		}

		private void tsRemove_Click(object sender, EventArgs e)
		{
		  CurrentSelectedFrame--;

      if (CurrentSelectedFrame <= 0)
        CurrentSelectedFrame = 0;

		  Globals.FrameRemovedEvent.Invoke(CurrentSelectedFrame);
		}

		private void tsDuplicate_Click(object sender, EventArgs e)
		{
			// Grab our currently selected frame
			var old = pnKeyFrameContainer.Controls[Globals.CurrentSelectedObject.Index] as conKeyFrame;

			if (old == null)
			{
				MessageBox.Show("Could not grab the current frame for duplication!", "Error", MessageBoxButtons.OK);
				return;
			}

			// Create Frame and add it to our container
			conKeyFrame con = new conKeyFrame();
			pnKeyFrameContainer.Controls.Add(con);

			// Display the Frame number on the frame we just added (indexof returns 0 based array so add 1)
			con.Tag = Globals.FrameSelectedEvent;
			con.FrameNumber = pnKeyFrameContainer.Controls.IndexOf(con);

			for(int i = 0; i < con.VisualObjects.Count; i++)
			{
				if(con.VisualObjects[i] is Sprite)
				{
          Sprite original = con.VisualObjects[i] as Sprite;

          if (original != null)
          {
						EditorSprite s = new EditorSprite(Game);

          	s.Name = original.Name;
            s.AssetName = original.AssetName;
            s.Texture = original.Texture;
            s.Position = original.Position;
            s.Scale = original.Scale;
            s.Rotation = original.Rotation;
            s.BlendMode = original.BlendMode;
            s.Camera = original.Camera;
            s.CustomEffect = original.CustomEffect;
            s.Color = original.Color;
            s.DrawOrder = original.DrawOrder;
            s.UpdateOrder = original.UpdateOrder;
            s.Visible = original.Visible;
            s.Effects = original.Effects;
            s.Origin = original.Origin;
            s.LayerDepth = original.LayerDepth;
            s.Enabled = original.Enabled;
            s.Index = original.Index;

            s.Initialize();

            // Add it to our visual objects
            con.VisualObjects.Add(s);
          }
				}

				if(con.VisualObjects[i] is ParticleEmitter)
				{
					ParticleEmitter original = con.VisualObjects[i] as ParticleEmitter;

          if (original != null)
          {
						EditorEmitter emitter = new EditorEmitter(Game);
          	emitter.Name = original.Name;
            emitter.Position = original.Position;
            emitter.Texture = original.Texture;
            emitter.ParticleColor = original.ParticleColor;
            emitter.AssetName = original.AssetName;
            emitter.Camera = original.Camera;
            emitter.DrawOrder = original.DrawOrder;
            emitter.UpdateOrder = original.UpdateOrder;
            emitter.Index = original.Index;
            emitter.MaxAcceleration = original.MaxAcceleration;
            emitter.MinAcceleration = original.MinAcceleration;
            emitter.MinLifeSpan = original.MinLifeSpan;
            emitter.MaxLifeSpan = original.MaxLifeSpan;
            emitter.MinParticles = original.MinParticles;
            emitter.MaxParticles = original.MaxParticles;
            emitter.MinRotationSpeed = original.MinRotationSpeed;
            emitter.MaxRotationSpeed = original.MaxRotationSpeed;
            emitter.MinScale = original.MinScale;
            emitter.MaxScale = original.MaxScale;
            emitter.ForceObjects = original.ForceObjects;
            emitter.Visible = original.Visible;
            emitter.Index = original.Index;

            emitter.Initialize();

            // Add it to our visual objects
            con.VisualObjects.Add(emitter);
          }
				}
			}

			// Init all the sprites
//			for (int i = 0; i < con.VisualObjects.Count; i++)
//				con.VisualObjects[i].Initialize();

			con.IsLocked = old.IsLocked;
			con.IsSelected = true;
			con.FrameSpeed = old.FrameSpeed;

			Globals.FrameSelectedEvent.Invoke(con.FrameNumber);
		}

		public void Update(GameTime time)
		{
			if(CurrentSelectedFrame > (pnKeyFrameContainer.Controls.Count - 1))
				return;

			var frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;

			if(frame != null)
			{
				if(IsPlaying)
				{
					TimeSinceLastFrame += 0.1f;

					if(TimeSinceLastFrame >= frame.FrameSpeed)
					{
						if(CurrentSelectedFrame < (pnKeyFrameContainer.Controls.Count - 1))
						{
							CurrentSelectedFrame += 1;
							TimeSinceLastFrame = 0;

							Globals.FrameSelectedEvent.Invoke(CurrentSelectedFrame);
						}
						else
						{
							CurrentSelectedFrame = 0;
							TimeSinceLastFrame = 0;
						}
					}
				}

				for(int i = 0; i < frame.VisualObjects.Count; i++)
				{
					frame.VisualObjects[i].Update(time);
				}
			}
		}

    public void Draw(GameTime time)
    {
      if (CurrentSelectedFrame > (pnKeyFrameContainer.Controls.Count - 1))
        return;

      conKeyFrame prevFrame = null;
      conKeyFrame currentFrame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;

      if (CurrentSelectedFrame > 0) prevFrame = pnKeyFrameContainer.Controls[CurrentSelectedFrame - 1] as conKeyFrame;

      if (currentFrame != null)
      {
        List<VisualObject> currentFrameSprites = (from sprite in currentFrame.VisualObjects
                                                    orderby sprite.DrawOrder
                                                    select sprite).ToList();

        // Render our Previous Frame if we are not playing the animation cycle
        if (prevFrame != null && !IsPlaying)
        {
					List<VisualObject> prevFrameSprites = (from sprite in prevFrame.VisualObjects
                                                   orderby sprite.DrawOrder
                                                   select sprite).ToList();

          for (int p = 0; p < prevFrameSprites.Count; p++)
          {
            // If its a sprite alter the shader for selected or not
            if (prevFrameSprites[p] is Sprite)
            {
              Sprite s = prevFrameSprites[p] as Sprite;

              if (s != null && s.CustomEffect != null)
              {
                s.CustomEffect.Parameters["IsSelected"].SetValue(false);
                s.CustomEffect.Parameters["IsPreviousFrame"].SetValue(true);
              }
            }

            // Draw it up
            prevFrameSprites[p].Draw(time);
          }
        }

        // Render our current frame
        for (int i = 0; i < currentFrameSprites.Count; i++)
        {
          // if its a sprite alter our shader params
          if (currentFrameSprites[i] is Sprite)
          {
            Sprite s = currentFrameSprites[i] as Sprite;

            if (s != null && s.CustomEffect != null)
            {
              if (s.IsSelected && !IsPlaying)
                s.CustomEffect.Parameters["IsSelected"].SetValue(true);
              else
                s.CustomEffect.Parameters["IsSelected"].SetValue(false);

              s.CustomEffect.Parameters["IsPreviousFrame"].SetValue(false);
            }
          }

					if(currentFrameSprites[i] is EditorGroup)
          {
						EditorGroup g = currentFrameSprites[i] as EditorGroup;

            if (g != null)
            {
							foreach(VisualObject obj in g.Objects)
              {
                if (obj is Sprite)
                {
                  Sprite s = obj as Sprite;

                  if (s.CustomEffect != null)
                  {
                    if (s.IsSelected && !IsPlaying)
                      s.CustomEffect.Parameters["IsSelected"].SetValue(true);
                    else
                      s.CustomEffect.Parameters["IsSelected"].SetValue(false);

                    s.CustomEffect.Parameters["IsPreviousFrame"].SetValue(false);
                  }
                }
              }
            }
          }

          // Draw it up
          currentFrameSprites[i].Draw(time);
        }
      }
    }

	  private void tbFrameSpeed_TextChanged(object sender, EventArgs e)
		{
			// Frame speed changed
			if (IsPlaying)
				return;

			if (tbFrameSpeed.Text == string.Empty)
				return;

			conKeyFrame frame = pnKeyFrameContainer.Controls[CurrentSelectedFrame] as conKeyFrame;
			if (frame != null)
			{
				// found our frame now alter its play speed
				frame.FrameSpeed = (float)Convert.ToDouble(tbFrameSpeed.Text);
			}
		}

		private void tsLevelSequencer_Click(object sender, EventArgs e)
		{
			frmSequencer frm = new frmSequencer();
			frm.dgSequences.DataSource = Sequences;
			frm.AddSequenceEvent = AddSequenceEvent;
			frm.RemoveSequenceEvent = RemoveSequenceEvent;
			frm.Show();
		}

	}
}