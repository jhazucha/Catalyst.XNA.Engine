using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Screen_Managerment
{
	[TestFixture]
	public class LoadingScreenTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			// Create our Custom Loading Screen and pass in a GameScreen[] array of all the screens you want to load
			// for now we will just pass in our Main Menu Screen
			CustomLoadingScreen ls = new CustomLoadingScreen(this, new GameScreen[] { new MainMenu(this) });

			// Add the loading screen to our game managers screen collection
			SceneManager.Load(ls);

			Run();
		}
	}
}
