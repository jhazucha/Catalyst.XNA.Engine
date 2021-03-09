using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace Catalyst3D.XNA.Engine.EntityClasses.UI
{
	public class GameScreenSlider : GameScreen
	{
		private bool isSliding;

		private Vector2 previousMouseDownPosition;
		private const float requiredDragDistance = 5;
		private float currentDragDistance;
		private SlidingDirection slideDirection = SlidingDirection.Right;

		public float SlideSpeed = 60.0f;
		public int ScreenWidth = 800;
		public int ScreenHeight = 480;
		public SliderType SliderType;
		public int CurrentScreenIndex;
		public int StartScreenIndex;
		public int TotalScreenCount;

		public GameScreenSlider(Game game, string assetFolder, string assetName, int totalScreens, SliderType type)
			: base(game, assetFolder)
		{
			AssetName = assetName;
			SliderType = type;
			TotalScreenCount = (totalScreens - 1); // 0 based indexing

			EnabledGestures = GestureType.HorizontalDrag | GestureType.Tap | GestureType.DragComplete;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (InputHandler != null)
			{
#if WINDOWS || XBOX360
				var mouseState = Mouse.GetState();
				var mousePosition = new Vector2(mouseState.X, mouseState.Y);

				if (mouseState.LeftButton == ButtonState.Pressed && !isSliding)
				{
					if (currentDragDistance >= requiredDragDistance)
					{
						// figure out which way they are dragging
						if (previousMouseDownPosition.X > mouseState.X)
						{
							if (CurrentScreenIndex < TotalScreenCount)
							{
								isSliding = true;
								currentDragDistance = 0;

								// dragging to the left
								slideDirection = SlidingDirection.Right;

								CurrentScreenIndex++;
							}
						}
						else if (previousMouseDownPosition.X < mouseState.X)
						{
							if (CurrentScreenIndex > 0)
							{
								isSliding = true;
								currentDragDistance = 0;

								// dragging to the right
								slideDirection = SlidingDirection.Left;

								CurrentScreenIndex--;
							}
						}
					}
					else
					{
						currentDragDistance += Vector2.Distance(previousMouseDownPosition, mousePosition);
						previousMouseDownPosition = mousePosition;
					}
				}
#endif

#if WINDOWS_PHONE

				if (!isSliding)
				{
					foreach (var g in InputHandler.Gestures)
					{
						// Check for bounding box collision first!
						if (g.GestureType == GestureType.HorizontalDrag)
						{
							if (g.Delta.X > 0)
							{
								isSliding = true;
								slideDirection = SlidingDirection.Left;

								if (CurrentScreenIndex > 0)
									CurrentScreenIndex--;
							}
							else
							{
								isSliding = true;
								slideDirection = SlidingDirection.Right;

								if (CurrentScreenIndex < TotalScreenCount)
									CurrentScreenIndex++;
							}
						}
					}
				}
#endif
			}

			if (isSliding)
			{
				float camOffset = (CurrentScreenIndex*ScreenWidth);

				switch (slideDirection)
				{
					case SlidingDirection.Right:
						if (CameraOffsetX <= -camOffset)
						{
							isSliding = false;
							CameraOffsetX = -camOffset;
						}
						else
						{
							CameraOffsetX -= SlideSpeed;
						}
						break;
					case SlidingDirection.Left:
						if (CameraOffsetX >= camOffset)
						{
							isSliding = false;
							CameraOffsetX = camOffset;
						}
						else
						{
							CameraOffsetX += SlideSpeed;
						}
						break;
				}
			}

			// Set our camera offset for our visual objects :]
			foreach (var vo in VisualObjects)
			{
				vo.CameraOffsetX = CameraOffsetX;
				vo.CameraOffsetY = CameraOffsetY;
			}
		}
	}
}