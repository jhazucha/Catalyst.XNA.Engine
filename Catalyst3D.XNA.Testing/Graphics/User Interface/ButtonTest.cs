using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.User_Interface
{
	public class MainMenu : GameScreen
	{
		private Button btnNewGame;
		private Button btnExitGame;

		public MainMenu(Game game)
			: base(game, string.Empty)
		{
			EnabledGestures = GestureType.Tap;
		}

		public override void Initialize()
		{
			// Create the Button and wire the onclick event
			btnNewGame = new Button(Game);
			btnNewGame.Position = new Vector2(100, 100);
			btnNewGame.OnClick += OnNewGameClicked;

			// Add it to our Scene Manager for Rendering
			AddVisualObject(btnNewGame);

			// Create the Button and wire the onclick event
			btnExitGame = new Button(Game);
			btnExitGame.Position = new Vector2(100, 300);
			btnExitGame.OnClick += OnExitGameClicked;

			// Add it to our Scene Manager for Rendering
			AddVisualObject(btnExitGame);

			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();
		
			// Assign our Texture to our Buttons
			btnNewGame.Texture = Content.Load<Texture2D>("btnNewGame1");

			btnExitGame.Texture =Content.Load<Texture2D>("btnExitGame1");

		}

		private void OnExitGameClicked()
		{
			Game.Exit();
		}

		private void OnNewGameClicked()
		{
			throw new Exception("Not yet Implemented");
		}
	}

	[TestFixture]
	public class ButtonTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();
			
			// Add our main menu screen to our scene manager
			SceneManager.Load(new MainMenu(this));
		}
	}
}
