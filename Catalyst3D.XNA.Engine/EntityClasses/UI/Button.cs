using System.Linq;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.EntityClasses.UI
{
	public class Button : Sprite
	{
		public delegate void ButtonAction();

		[ContentSerializerIgnore, XmlIgnore]
		public InputHandler InputHandler;

		[ContentSerializerIgnore, XmlIgnore]
		public ButtonAction OnClick;

		public int Depth = 1;

		// Parameterless constructor for serialization!
		public Button() : base(null) { }

		public Button(Game game)
			: base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			InputHandler = (InputHandler)Game.Services.GetService(typeof(InputHandler));
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Texture == null)
				return;

			float tw = Texture.Width * Scale.X;
			float th = Texture.Height * Scale.Y;

			if (IsCentered)
			{
				// Update our Bounding Box
				Vector3[] points = new Vector3[2];

				points[0] = new Vector3(Position.X + CameraOffsetX - (tw / 2) - BoundingBoxPadding.X,
																Position.Y + CameraOffsetY - (th / 2) - BoundingBoxPadding.Y, -5);

				points[1] = new Vector3(Position.X + CameraOffsetX + (tw / 2) + BoundingBoxPadding.X,
																Position.Y + CameraOffsetY + (th / 2) + BoundingBoxPadding.Y, 5);

				// Calculate the center of our sprite
				Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																		(int)(points[1].X - points[0].X),
																		(int)(points[1].Y - points[0].Y));

				Center = new Vector2(r.Center.X, r.Center.Y);

				// Calculate our Origin
				Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);


				// Create our Bounding Box
				BoundingBox = BoundingBox.CreateFromPoints(points);
			}
			else
			{
				Origin = Vector2.Zero;

				// Update our Bounding Box
				Vector3[] points = new Vector3[2];

				points[0] = new Vector3(Position.X + CameraOffsetX - BoundingBoxPadding.X,
																Position.Y + CameraOffsetY - BoundingBoxPadding.Y, -5);

				points[1] = new Vector3(Position.X + CameraOffsetX + tw + BoundingBoxPadding.X,
																Position.Y + CameraOffsetY + th + BoundingBoxPadding.Y, 5);

				// Create our Bounding Box
				BoundingBox = BoundingBox.CreateFromPoints(points);

				// Calculate the center of our sprite
				Rectangle r = new Rectangle((int)points[0].X, (int)points[0].Y,
																		(int)(points[1].X - points[0].X),
																		(int)(points[1].Y - points[0].Y));

				Center = new Vector2(r.Center.X, r.Center.Y);
			}

			if (ShowBoundingBox && Enabled)
			{
				// Update the Bounding Box Renderer
				BoundingBoxRenderer.ShowBoundingBox = ShowBoundingBox;
				BoundingBoxRenderer.BoundingBox = BoundingBox;

				BoundingBoxRenderer.Update(gameTime);
			}

			if (InputHandler != null)
			{
        // Check for Button Press
        foreach (Point p in (from gesture in InputHandler.Gestures
                             where gesture.GestureType == GestureType.Tap
                             select new Point((int)gesture.Position.X, (int)gesture.Position.Y)
                               into tapLocation
                               let rec = new Rectangle((int)BoundingBox.Min.X, (int)BoundingBox.Min.Y, (int)(BoundingBox.Max.X - BoundingBox.Min.X), (int)(BoundingBox.Max.Y - BoundingBox.Min.Y))
                               where rec.Contains(tapLocation)
                               select tapLocation).Where(p => OnClick != null))
        {
          OnClick.Invoke();
        }

#if WINDOWS
				// Check for Button Press by mouse
				MouseState state = Mouse.GetState();
				if (state.LeftButton == ButtonState.Pressed)
				{
					BoundingBox mouseBB = new BoundingBox(new Vector3(state.X - 1, state.Y - 1, 0),
																								new Vector3(state.X + 1, state.Y + 1, 0));
					if (mouseBB.Intersects(BoundingBox))
					{
						if (OnClick != null)
							OnClick.Invoke();
					}
				}
#endif

			}
		}
	}
}