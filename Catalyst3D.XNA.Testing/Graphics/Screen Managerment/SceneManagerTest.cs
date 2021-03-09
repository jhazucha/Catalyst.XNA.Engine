using System;
using Catalyst3D.XNA.Engine.UtilityClasses;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Screen_Managerment
{
	[TestFixture]
	public class GameScreenTest : CatalystTestFixture
	{
		[Test]
		public void Test()
		{
			// Add the screen to our SceneManager
			SceneManager.Load(new MainMenu(this));

			Run();
		}
	}
}
