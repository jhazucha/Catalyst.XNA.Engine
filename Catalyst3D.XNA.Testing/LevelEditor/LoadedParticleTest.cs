using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.LevelEditor
{
  public class TestScreen : GameScreen
  {
    public TestScreen(Game game) : base(game, string.Empty)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();

      // Load our 2D Level Editor Particle Emitter Test Scene
    	Load2DProject("EmitterTest");

      // Grab our object out of our pre-loaded scene
			var emitter = Get<ParticleEmitter>("Star1.png");
      emitter.ParticleColor = Color.Blue;
    	emitter.MaxParticles = 25;
    	emitter.MinAcceleration = 50;
    	emitter.MaxAcceleration = 60;
    	emitter.MinScale = 0.5f;
    	emitter.MaxScale = 0.8f;
			emitter.RespawnParticles = true;
    	emitter.MaxLifeSpan = 5;
    	emitter.MinLifeSpan = 2;
    }
  }

  [TestFixture]
	public class LoadedParticleTest : CatalystTestFixture
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

      // Add our Test Screen to the Scene Manager for rendering
      SceneManager.Load(new TestScreen(this));
    }
	}
}