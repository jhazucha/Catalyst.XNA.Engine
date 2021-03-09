using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Text
{
	[TestFixture]
	public class Text2DTest : CatalystTestFixture
	{
    private Label myText;

		[Test]
		public void Test()
		{
			// Create our Text Object
      myText = new Label(this, "Arial");
			myText.Text = "Catalyst3D ROCKS!";

			// Add it to XNA's Component Colleciton so it gets Updated and Drawn automatically
			Components.Add(myText);

			Run();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			myText.Position = new Vector2((float) GraphicsDevice.Viewport.Width/2, (float) GraphicsDevice.Viewport.Height/2);
		}
	}
}
