using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.LevelEditor
{
  public class PlatfomerLevel1 : GameScreen
  {
  	public PlatfomerLevel1(Game game) : base(game, string.Empty)
    {
    }

		public override void LoadContent()
		{
			base.LoadContent();

			Load2DProject("Platformer1");
		}
  }

  public class TestPlatformer : CatalystTestFixture
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

      SceneManager.Load(new PlatfomerLevel1(this));
    }
  }
}
