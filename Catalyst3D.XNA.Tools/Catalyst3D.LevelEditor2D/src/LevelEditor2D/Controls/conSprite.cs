using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = System.Drawing.Rectangle;

namespace LevelEditor2D.Controls
{
  public partial class conSprite : UserControl
  {
    private readonly Game Game;
    private readonly conRenderer RenderWindow;

  	public Image SpriteImage;
    public bool IsSelected;
    public string FileName { get; set; }
    public int Index { get; set; }
    public int SpriteType { get; set; }

    public conSprite(Game game, Image img, conRenderer renderer)
    {
      InitializeComponent();

      RenderWindow = renderer;
      Game = game;
      SpriteImage = img;
    }

    protected override void OnClick(System.EventArgs e)
    {
      base.OnClick(e);

      // Alert the Container that this is the selected sprite
      ((Globals.SpriteContainerEvent) Tag).Invoke(Index);
    }

    protected override void OnDoubleClick(System.EventArgs e)
    {
      if (MessageBox.Show(@"Are you sure you want to add this to the Scene?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        // Add our sprite
      	EditorSprite sprite = new EditorSprite(Game);

				// Open a Memory Stream
      	MemoryStream stream = new MemoryStream();
      	
				// Save the current sprite image to a stream
				SpriteImage.Save(stream, ImageFormat.Png);
      	
				// Load the stream into a texture2D object
      	sprite.Texture = Texture2D.FromStream(Game.GraphicsDevice, stream);

				// Close the Stream
      	stream.Close();

      	sprite.AssetName = FileName;
      	sprite.Position = new Vector2(-Globals.CurrentCameraOffsetX, 0);
      	sprite.Name = Name;

        // Add it to our key frame container
        Globals.ObjectAdded.Invoke(sprite);

        // Select it out the gate
        Globals.ObjectSelected.Invoke(sprite);
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
    	e.Graphics.Clear(System.Drawing.Color.Transparent);

      if (SpriteImage != null)
      {
        // Draw our Image FIRST
      	Rectangle rect = new Rectangle(0, 0, 64, 64);
      	e.Graphics.DrawImage(SpriteImage, rect);

        Pen pen;

        switch (SpriteType)
        {
          case 0:
            // Background
            pen = new Pen(System.Drawing.Color.Blue, 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, 63, 63));
            break;

          case 1:
            // Enemy
            pen = new Pen(System.Drawing.Color.Green, 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, 63, 63));
            break;

          case 2:
            // Player
            pen = new Pen(System.Drawing.Color.Orange, 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, 63, 63));
            break;

          case 3:
            // Prop
            pen = new Pen(System.Drawing.Color.Red, 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, 63, 63));
            break;
        }

        if (IsSelected)
        {
          pen = new Pen(System.Drawing.Color.Yellow, 3);
          e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, 63, 63));
        }
      }
    }
  }
}