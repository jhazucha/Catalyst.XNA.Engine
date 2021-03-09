using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.User_Interface
{
	public class SliderMenu : GameScreen
	{
		public Slider conSlider1;
		public Slider conSlider2;

		public SliderMenu(Game game)
			: base(game, string.Empty)
		{
			EnabledGestures = GestureType.FreeDrag | GestureType.HorizontalDrag | GestureType.Tap | GestureType.Hold;
		}

		public override void Initialize()
		{
			base.Initialize();

			// Create our Slider
			conSlider1 = new Slider(Game, string.Empty, "sliderBall", string.Empty, SliderType.Horizontal);
			conSlider1.Position = new Vector2(50, 50);
			conSlider1.CurrentSliderPosition = 50; // 50%
			AddVisualObject(conSlider1);

			conSlider2 = new Slider(Game, string.Empty, "sliderBall", string.Empty, SliderType.Vertical);
			conSlider2.Position = new Vector2(50, 90);
			AddVisualObject(conSlider2);
		}
	}

	[TestFixture]
	public class SliderTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			GraphicsManager.PreferredBackBufferHeight = 300;
			GraphicsManager.PreferredBackBufferWidth = 200;

			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			// Add our main menu screen to our scene manager
			SceneManager.Load(new SliderMenu(this));

		}
	}
}