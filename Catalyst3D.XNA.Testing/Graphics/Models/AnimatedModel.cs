using System;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Models;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
	[TestFixture]
	public class AnimatedModel : CatalystTestFixture
	{
		public Model SoldierBodyUpper;
		public BasicCamera Camera;

		[Test]
		public void Test()
		{
			Camera = new BasicCamera(this);
			Camera.Position = new Vector3(0, 5, -20);
			Components.Add(Camera);

		
			Run();
		}
	}
}
