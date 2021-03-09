using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Testing.Graphics.Screen_Managerment
{
	public class MainMenu : GameScreen
	{
		public Sprite Logo;

		public MainMenu(Game game)
			: base(game, string.Empty)
		{
		}

		public override void LoadContent()
		{
			base.LoadContent();
			
			// Load our Catalyst3D Logo
			Logo = new Sprite(Game, true);
			Logo.Position = new Vector2(400, 200);
			Logo.Texture = Content.Load<Texture2D>("CatalystLogo1");
			AddVisualObject(Logo);
		}

	}
}
