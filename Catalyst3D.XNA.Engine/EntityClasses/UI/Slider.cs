using System.IO;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.EntityClasses.UI
{
	public class Slider : VisualObject
	{
		public delegate void SliderAction(int percentage);

		private bool isSliding;

		private InputHandler InputHandler;
		private readonly string sliderTextureName;
		private Sprite SliderTexture;

		[ContentSerializerIgnore, XmlIgnore]
		public SliderAction OnSliderMoved;

		public Sprite Button;
		public int Width = 115;
		public int Height = 2;

		public Color SliderColor = Color.Black;
		public Color ButtonColor = Color.White;

		public int CurrentSliderPosition;

		public SliderType SliderType = SliderType.Horizontal;

		public Slider(Game game, SliderType type)
			: base(game)
		{
			SliderType = type;
		}

		public Slider(Game game, string assetFolder, string buttonTexName, string sliderTexName, SliderType type)
			: base(game)
		{
			AssetName = buttonTexName;
			sliderTextureName = sliderTexName;
			AssetFolder = assetFolder;
			SliderType = type;

			BoundingBoxScale = new Vector2(5, 5);
		}

		public override void LoadContent()
		{
			base.LoadContent();

			InputHandler = (InputHandler) Game.Services.GetService(typeof (InputHandler));

			// Load these gestures to the game screen that this control requires
			GameScreen.EnabledGestures = GameScreen.EnabledGestures | GestureType.FreeDrag | GestureType.DragComplete;

			// Setup our slider texture
			SliderTexture = new Sprite(Game, false);
			SliderTexture.Position = Position;
		  SliderTexture.GameScreen = GameScreen;

			if (!string.IsNullOrEmpty(sliderTextureName))
				GameScreen.Content.Load<Texture2D>(AssetFolder + "/" + Path.GetFileNameWithoutExtension(AssetName));
			else
			{
				if (SliderType == SliderType.Horizontal)
					SliderTexture.Texture = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.White, Width, Height);
				else
					SliderTexture.Texture = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.White, Height, Width);
			}

			SliderTexture.Initialize();

			// Button Texture
			Button = new Sprite(Game, false);
			Button.Color = ButtonColor;
		  Button.GameScreen = GameScreen;

			if (!string.IsNullOrEmpty(AssetName))
			{
			  Button.Texture = GameScreen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(AssetName));
			}
			else
			{
				Button.Texture = Utilitys.GenerateTexture(GraphicsDevice, Color.White, 15, 15);
			}

			switch (SliderType)
			{
				case SliderType.Horizontal:
					{
						// Width of slider bar in pixels
						float sliderWidth = (SliderTexture.Texture.Width*SliderTexture.Scale.X);

						// Percentage of slider position
						float percent = (CurrentSliderPosition/sliderWidth)*100;

						// Position of current button on slider factoring in slider percentage
						float x = (Position.X - ((Button.Texture.Width*Button.Scale.X)/2)) + percent;

						Button.Position = new Vector2(x, Position.Y - ((Button.Texture.Height*Button.Scale.Y)/2));
					}
					break;
				case SliderType.Vertical:
					{
						float y = Position.Y - ((Button.Texture.Height*Button.Scale.Y)/2);

						Button.Position = new Vector2(Position.X - ((Button.Texture.Width*Button.Scale.X)/2), y);
					}
					break;
			}



			Button.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (SliderTexture != null)
			{
				SliderTexture.Color = SliderColor;
				SliderTexture.BoundingBoxScale = BoundingBoxScale;
        SliderTexture.Update(gameTime);

        BoundingBox = SliderTexture.BoundingBox;
			}

			if (Button != null)
			{
				Button.Color = ButtonColor;
				Button.Update(gameTime);
			}

			if (InputHandler != null)
			{
			  Vector2 position = Vector2.Zero;

#if WINDOWS || XBOX360
				var mouseState = Mouse.GetState();

				position = new Vector2(mouseState.X, mouseState.Y);

				if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
				{
					BoundingBox bb =
						new BoundingBox(new Vector3(position.X - BoundingBoxScale.X, position.Y - BoundingBoxScale.Y, 0),
						                new Vector3(position.X + BoundingBoxScale.X, position.Y + BoundingBoxScale.Y, 0));

					if (bb.Intersects(BoundingBox))
						isSliding = true;
				}
				else
				{
					isSliding = false;
				}
#endif

#if WINDOWS_PHONE
				foreach (var g in InputHandler.Gestures)
				{
					// Check for bounding box collision first!
					if (g.GestureType == GestureType.FreeDrag)
					{
						position = new Vector2(g.Position.X, g.Position.Y);

						BoundingBox bb =
							new BoundingBox(new Vector3(position.X - BoundingBoxScale.X, position.Y - BoundingBoxScale.Y, 0),
							                new Vector3(position.X + BoundingBoxScale.X, position.Y + BoundingBoxScale.Y, 0));

						if (bb.Intersects(BoundingBox))
							isSliding = true;
					}

					if (g.GestureType == GestureType.DragComplete)
						isSliding = false;
				}
#endif

				if (isSliding)
				{
					int slWidth = SliderTexture.Texture.Width;
					int slHeight = SliderTexture.Texture.Height;

					int btnHeight = Button.Texture.Height/2;
					int btnWidth = Button.Texture.Width/2;

					if (SliderType == SliderType.Horizontal)
					{
						float maxPosX = Position.X + slWidth;
						float minPosX = Position.X - ((Button.Texture.Width*Button.Scale.X)/2);

						// Cap it so it cannot pass the right side of the slider
						Button.Position = new Vector2(position.X, Button.Position.Y);

						// Cap it so it cannot pass the left/right side of the slider
						if (Button.Position.X > maxPosX)
							Button.Position = new Vector2(maxPosX - btnWidth, Button.Position.Y);
						else if (Button.Position.X < minPosX)
							Button.Position = new Vector2(minPosX, Button.Position.Y);
					}
					else
					{
						float maxPosY = Position.Y + slHeight;
						float minPosY = Position.Y - ((Button.Texture.Height*Button.Scale.Y)/2);

						// Cap it so it cannot pass the right side of the slider
						Button.Position = new Vector2(Button.Position.X, position.Y);

						// Cap it so it cannot pass the left/right side of the slider
						if (Button.Position.Y > maxPosY)
							Button.Position = new Vector2(Button.Position.X, maxPosY - btnHeight);
						else if (Button.Position.Y < minPosY)
							Button.Position = new Vector2(Button.Position.X, minPosY);
					}

					int btnPosX = (int) ((int) Button.Position.X + ((Button.Texture.Width*Button.Scale.X)/2));
					int btnPosY = (int) ((int) Button.Position.Y + ((Button.Texture.Height*Button.Scale.Y)/2));

					switch (SliderType)
					{
						case SliderType.Horizontal:
							{
								int pos = btnPosX - (int) Position.X;
								float percent = ((float) pos/slWidth)*100;

								if (percent >= 100)
									percent = 100;

								CurrentSliderPosition = (int) percent;

								if (OnSliderMoved != null)
									OnSliderMoved.Invoke((int) percent);
							}
							break;
						case SliderType.Vertical:
							{
								int pos = btnPosY - (int) Position.Y;
								float percent = ((float) pos/slHeight)*100;

								if (percent <= 0)
									percent = 0;

								CurrentSliderPosition = (int) percent;

								if (OnSliderMoved != null)
									OnSliderMoved.Invoke((int) percent);
							}
							break;
					}
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (SliderTexture != null)
				SliderTexture.Draw(gameTime);

			if (Button != null)
				Button.Draw(gameTime);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			InputHandler = null;
			Button.UnloadContent();
			SliderTexture.UnloadContent();
		}
	}
}