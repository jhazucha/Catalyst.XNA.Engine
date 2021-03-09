using System;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing
{
	[TestFixture]
	public class ColorLerpingTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			Run();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Calculate Sky Color
			Color sky = Utilitys.GetColorBasedOnTimeOfDay(DateTime.Now.Hour, Color.CornflowerBlue, Color.DarkGray);

			// Change viewport
			ViewportColor = sky;
		}
	}
}
