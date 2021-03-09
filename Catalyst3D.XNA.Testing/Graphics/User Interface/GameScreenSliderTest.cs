using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Catalyst3D.XNA.Engine.EnumClasses;

namespace Catalyst3D.XNA.Testing.Graphics.User_Interface
{
	public class SliderScreen : GameScreenSlider
	{
		public SliderScreen(Game game)
			: base(game, "", "GameScreenSlider1", 3, SliderType.Horizontal)
		{
		}
	}

	public class GameScreenSliderTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			GraphicsManager.PreferredBackBufferWidth = 800;
			GraphicsManager.PreferredBackBufferHeight = 480;

			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			// Add our main menu screen to our scene manager
			SceneManager.Load(new SliderScreen(this));
		}
	}
}