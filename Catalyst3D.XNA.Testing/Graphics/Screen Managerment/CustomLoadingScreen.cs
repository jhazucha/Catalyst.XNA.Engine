using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Screen_Managerment
{
	public class CustomLoadingScreen : LoadingScreen
	{
		public Label loadingText;

		public CustomLoadingScreen(Game game, GameScreen[] screens)
			: base(game, screens, string.Empty)
		{
		}
		public override void Initialize()
		{
			base.Initialize();

			// Load up our Text to display saying we are loading a screen
      loadingText = new Label(Game, "Arial");
			loadingText.Text = "Please wait while we load the Main Menu Screen!!";
			loadingText.Position = new Vector2(380, 150);
			loadingText.ShadowOffset = new Vector2(6, -6);
			loadingText.IsShadowVisible = true;

			// Add it to our Game Screen's Scene Object Collection so it gets drawn and updated
			AddVisualObject(loadingText);
		}
	}
}