using System;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Models;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
	[TestFixture]
	public class WoodBox : CatalystTestFixture
	{
		private Model woodBox;

		[Test]
		public void Test()
		{
			// Create our camera
			BasicCamera camera = new BasicCamera(this);
      camera.Position = new Vector3(250, 150, 250);
		  camera.Target = new Vector3(0, 50, 0);
			Components.Add(camera);

			// Create our model
			woodBox = new Model(this, "Cart1", camera);
			Components.Add(woodBox);

			Run();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

		  float rot = (float) gameTime.ElapsedGameTime.TotalSeconds;
			woodBox.Rotation += new Vector3(0, rot, 0);
		}
	}
}
