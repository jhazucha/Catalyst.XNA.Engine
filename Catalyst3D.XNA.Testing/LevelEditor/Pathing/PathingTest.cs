using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.LevelEditor.Pathing
{
	public class TravelingBall : GameScreen
	{
		public Sprite SoccerBall;

		public const float TravelSpeed = 0.025f;

		public TravelingBall(Game game)
			: base(game, string.Empty)
		{
		}

		public override void LoadContent()
		{
			base.LoadContent();

			// Load our 2D Project
			Load2DProject("TravelingBall1");

			// Get a copy of our Soccer Ball Sprite
			SoccerBall = Get<Sprite>("Soccerball");
			SoccerBall.AttachedPathingNode.IsTraveling = true;
			SoccerBall.AttachedPathingNode.TravelSpeed = 0.025f;
			SoccerBall.ShowBoundingBox = false;
			SoccerBall.AttachedPathingNode.LedgeTavelAlgo = LedgeTravelAlgo.StopAtLastNode;
		}
	}

	[TestFixture]
	public class PathingTest : CatalystTestFixture
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

			SceneManager.Load(new TravelingBall(this));
		}
	}
}
