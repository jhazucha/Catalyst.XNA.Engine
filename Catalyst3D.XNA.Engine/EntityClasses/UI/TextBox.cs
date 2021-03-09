using System.Linq;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.EntityClasses.UI
{
	public class TextBox : VisualObject
	{
		public delegate void TextboxEvent();

		public event TextboxEvent OnClick;

		[ContentSerializerIgnore, XmlIgnore] private InputHandler InputHandler;

		public int Width = 100;
		public int Height = 45;

		public int BorderSize;

		public string Text;

		public Color BorderColor = Color.White;
		public Color InteriorColor = Color.Black;
		public Color FontColor = Color.Black;

		public Sprite BorderTexture;
		public Sprite InsideTexture;

		private Label label;
		private readonly string fontName;

		public TextBox(Game game, string FontName)
			: base(game)
		{
			fontName = FontName;
		}

		public override void Initialize()
		{
			base.Initialize();

			// Hook our input handler
			InputHandler = (InputHandler)Game.Services.GetService(typeof(InputHandler));

			// Setup the border
			BorderTexture = new Sprite(Game);
			BorderTexture.Texture = Utilitys.GenerateTexture(Game.GraphicsDevice, BorderColor, Width + BorderSize, Height + BorderSize);
			BorderTexture.Initialize();

			// Setup the Interior of the text box
			InsideTexture = new Sprite(Game);
			InsideTexture.Texture = Utilitys.GenerateTexture(Game.GraphicsDevice, BorderColor, Width, Height);
			InsideTexture.Initialize();

			// Inside label text
			label = new Label(Game, fontName);
			label.FontColor = FontColor;
			label.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			BorderTexture.Position = new Vector2(Position.X - BorderSize, Position.Y - BorderSize);
			BorderTexture.Update(gameTime);

			InsideTexture.Position = new Vector2(Position.X + BorderSize, Position.Y + BorderSize);
			InsideTexture.Update(gameTime);

			label.Position = new Vector2(Position.X + BorderSize, Position.Y + BorderSize);
			label.Text = Text;
			label.Update(gameTime);

			if (InputHandler != null)
			{
				// Check for Button Press
				foreach (Point p in from gesture in InputHandler.Gestures
				                    where gesture.GestureType == GestureType.Tap
				                    select new Point((int) gesture.Position.X, (int) gesture.Position.Y)
				                    into tapLocation
				                    let rec =
				                    	new Rectangle((int) Position.X, (int) Position.Y, Width*(int) Scale.X, Height*(int) Scale.Y)
				                    where rec.Contains(tapLocation)
				                    select tapLocation)
				{
					OnClick.Invoke();
				}

				// Check for Button Press by mouse
				MouseState state = Mouse.GetState();
				if (state.LeftButton == ButtonState.Pressed)
				{
					BoundingBox mouseBB = new BoundingBox(new Vector3(state.X - 1, state.Y - 1, 0),
					                                      new Vector3(state.X + 1, state.Y + 1, 0));
					if (mouseBB.Intersects(InsideTexture.BoundingBox))
					{
						if (OnClick != null)
							OnClick.Invoke();
					}
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			BorderTexture.Draw(gameTime);
			InsideTexture.Draw(gameTime);

			label.Draw(gameTime);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			InputHandler = null;

			label.UnloadContent();
			BorderTexture.UnloadContent();
			InsideTexture.UnloadContent();
		}
	}
}
