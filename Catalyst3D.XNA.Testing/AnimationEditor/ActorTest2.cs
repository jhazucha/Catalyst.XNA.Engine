using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.AnimationEditor
{
	public class ActorTest2 : CatalystTestFixture
	{
		public Actor Player;

		[Test]
		public void Test()
		{
			Player = new Actor(this, string.Empty, "Cow");
		  Player.Position = new Vector2(200, 200);
		  Player.IsCentered = true;

			Components.Add(Player);

			Run();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(!Player.ClipPlayer.IsPlaying)
			{
        
			
				Player.ClipPlayer.Play("Scared", true);
			}
		}
	}
}
