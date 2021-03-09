using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Input
{
	[TestFixture]
	public class InputHandlerTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			Run();
		}

		protected override void Initialize()
		{
			base.Initialize();

			// Now assign these actions to our Input Handler
			InputHandler.AddCommand(new InputAction("Red", Buttons.A, Keys.F1));
			InputHandler.AddCommand(new InputAction("Green", Buttons.B, Keys.F2));
			InputHandler.AddCommand(new InputAction("Blue", Buttons.Y, Keys.F3));
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			/* Now to check if an input action was pressed we do the following */

			if(InputHandler.IsActionPressed("Red"))
			{
				// We pushed the F1 key assigned to the RED change the viewports color to it
				ViewportColor = Color.Red;
			}

			if(InputHandler.IsActionPressed("Green"))
			{
				// We pushed the F2 key assigned to the Green change the viewports color to it
				ViewportColor = Color.Green;
			}

			if(InputHandler.IsActionPressed("Blue"))
			{
				// We pushed the F3 key assigned to the Blue change the viewports color to it
				ViewportColor = Color.Blue;
			}
		}
	}
}
