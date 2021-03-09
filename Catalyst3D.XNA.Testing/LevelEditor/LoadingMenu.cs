using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Catalyst3D.XNA.Engine.UtilityClasses;

namespace Catalyst3D.XNA.Testing.LevelEditor
{
	public class MenuTest : GameScreen
	{
		public MenuTest(Game game)
			: base(game,string.Empty)
		{
		}

		public override void LoadContent()
		{
			base.LoadContent();

      // Create the Project
			Load2DProject("mainmenu");

			// Hook into our Scene's Exit / New Game Buttons
			Get<Button>("btnExitGame1").OnClick += OnExitClicked;
			Get<Button>("btnExitGame1").OnClick += OnNewGameClicked;
		}

		private void OnNewGameClicked()
		{
			SceneManager.Remove(this);
		}

		private void OnExitClicked()
		{
			Game.Exit();
		}
	}

	[TestFixture]
	public class Loading : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			IsMouseVisible = true;

		  GraphicsManager.PreferredBackBufferHeight = 800;
		  GraphicsManager.PreferredBackBufferWidth = 480;

			Run();
		}

    protected override void Initialize()
    {
      base.Initialize();

      SceneManager.Load(new MenuTest(this));
    }
	}
}
