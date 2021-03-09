using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.EntityClasses.UI
{
	public class VirtualThumbStick : Sprite
	{
		private int touchId;

		// the distance in screen pixels that represents a thumbstick value of 1f.
		public float MaxThumbstickDistance = 5f;

		public Vector2? ThumbstickCenter { get; private set; }

		public Vector2 TouchPosition { get; set; }

		public Vector2 Thumbstick
		{
			get
			{
				// if there is no thumbstick center, return a value of (0, 0)
				if(!ThumbstickCenter.HasValue)
					return Vector2.Zero;

				// calculate the scaled vector from the touch position to the center,
				// scaled by the maximum thumbstick distance
				Vector2 location = (TouchPosition - ThumbstickCenter.Value) / MaxThumbstickDistance;

				// if the length is more than 1, normalize the vector
				if(location.LengthSquared() > 1f)
					location.Normalize();

				return location;
			}
		}

		public VirtualThumbStick(Game game) : base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			TouchLocation? Touch = null;
			TouchCollection touches = TouchPanel.GetState();

			foreach(var touch in touches)
			{
				if(touch.Id == touchId)
				{
					// This is a motion of a left-stick touch that we're already tracking
					Touch = touch;
					continue;
				}

				// We're didn't continue an existing thumbstick gesture; see if we can start a new one.
				// We'll use the previous touch position if possible, to get as close as possible to where
				// the gesture actually began.
				TouchLocation earliestTouch;
				if(!touch.TryGetPreviousLocation(out earliestTouch))
					earliestTouch = touch;

				if(touchId == -1)
				{
					// if we are not currently tracking a left thumbstick and this touch is on the left
					// half of the screen, start tracking this touch as our left stick
					if(earliestTouch.Position.X < (float)TouchPanel.DisplayWidth / 2)
					{
						Touch = earliestTouch;
						continue;
					}
				}
			}

			// if we have a left touch
			if(Touch.HasValue)
			{
				//  ThumbstickCenter = Touch.Value.Position;
				// if we have no center, this position is our center
				if(!ThumbstickCenter.HasValue)
					ThumbstickCenter = Center;

				// save the position of the touch
				TouchPosition = Touch.Value.Position;

				// save the ID of the touch
				touchId = Touch.Value.Id;
			}
			else
			{
				// otherwise reset our values to not track any touches
				// for the left thumbstick
				ThumbstickCenter = null;
				touchId = -1;
			}
		}
	}
}
