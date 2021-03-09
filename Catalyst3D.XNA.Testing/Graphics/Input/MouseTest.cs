using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Input
{
	[TestFixture]
	public class MouseTest : CatalystTestFixture
	{
		private Label myText;

		[Test]
		public void Test()
		{
			// Create some text to render
      myText = new Label(this, "Arial");
			myText.Text = "<-";
			Components.Add(myText);

			Run();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Get our Mouses current state
			MouseState state = Mouse.GetState();

			// Make the Text Follow the Mouse Cursor
			myText.Position = new Vector2(state.X, state.Y);
		}
	}
}
