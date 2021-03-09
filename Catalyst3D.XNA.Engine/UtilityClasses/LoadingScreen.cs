using System.Linq;
using System.Threading;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class LoadingScreen : GameScreen
	{
		private readonly GameScreen[] screensToLoad;
    private new bool IsLoaded { get; set; }

		public LoadingScreen(Game game, GameScreen[] screens, string assetFolder)
			: base(game, assetFolder)
		{
			screensToLoad = screens;
		}

		public override void Initialize()
		{
			base.Initialize();

			WaitCallback cb = OnLoadScreens;
			ThreadPool.QueueUserWorkItem(cb, screensToLoad);
		}

		private void OnLoadScreens(object screens)
		{
			var screenCollection = (GameScreen[]) screens;

			foreach (var s in screenCollection.Where(s => s != this))
			  SceneManager.Load(s, false);

			IsLoaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (IsLoaded)
			{
				SceneManager.Remove(this);
			}
		}
	}
}