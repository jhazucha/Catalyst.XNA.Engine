using System;
using System.Collections.Generic;
using System.ComponentModel;
using AnimationEditor.TypeEditors;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;

namespace AnimationEditor.EntityClasses
{
	[Serializable]
	public class EditorActor : Actor
	{
		public BoundingBoxRenderer Picker;
		
		[Editor(typeof(SequenceEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public List<Sequence2D> Sequences
		{
			get { return ClipPlayer.Sequences; }
			set { ClipPlayer.Sequences = value; }
		}

		public EditorActor(Game game, string folderName, string filename)
			: base(game, folderName, filename)
		{
		}

		public EditorActor()
    {
    }

		public override void Initialize()
		{
      Picker = new BoundingBoxRenderer(Game);
			Picker.Initialize();

			base.Initialize();
		}

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

			if(ClipPlayer.CurrentSequence == null)
				return;

      if (Picker != null)
      {
        // Setup our Picker
        Picker.Width = ClipPlayer.Width;
        Picker.Height = ClipPlayer.Height;
        Picker.Position = Position;
        Picker.ShowBoundingBox = ShowBoundingBox;
        Picker.Update(gameTime);
      }

      if (ClipPlayer.CurrentFrameIndex > ClipPlayer.CurrentSequence.Frames.Count)
      {
        throw new Exception("Your Start and End frames are setup incorrectly for this sequence!");
      }

			if(ClipPlayer.CurrentSequence.Frames.Count > ClipPlayer.CurrentFrameIndex)
			{
				int width = ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Width;
				int height = ClipPlayer.CurrentSequence.Frames[ClipPlayer.CurrentFrameIndex].SourceRectangle.Height;

				int screenWidth = Game.Window.ClientBounds.Width;
				int screenHeight = Game.Window.ClientBounds.Height;

				CameraOffsetX = (float) screenWidth/2 - (float) width/2;
				CameraOffsetY = (float) screenHeight/2 - (float) height/2;
			}
    }

	  public override void Draw(GameTime gameTime)
		{
			if(!Visible || !Enabled)
				return;

			if(Picker != null && ShowBoundingBox)
				Picker.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
