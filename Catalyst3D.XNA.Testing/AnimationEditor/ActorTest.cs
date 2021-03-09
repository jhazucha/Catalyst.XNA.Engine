using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.AnimationEditor
{
  public class MyGameScreen : GameScreen
  {
    public Actor Cow;

    public MyGameScreen(Game game)
      : base(game, string.Empty)
    {
    }

		public override void LoadContent()
		{
			base.LoadContent();

			// Load our Cow
			Cow = LoadActor("Cow");

			Cow.Play("Scared", true);
		}
  }

  public class ActorTest : CatalystTestFixture
  {
    [Test]
    public void Test()
    {
      IsMouseVisible = true;

      Run();
    }

    protected override void Initialize()
    {
      base.Initialize();

			SceneManager.Load(new MyGameScreen(this));
    }
  }
}
