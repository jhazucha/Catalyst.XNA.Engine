using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
	[Category("Button")]
	public class EditorButton : Button
	{
	  private conRenderer RenderWindow;

		public EditorButton(Game game, conRenderer renderer)
			: base(game)
		{
		  RenderWindow = renderer;
			SetShaderParameters += OnShaderParams;
		}

		private void OnShaderParams(GameTime gametime)
		{
			CustomEffect.Parameters["Color"].SetValue(Color.ToVector4());
			CustomEffect.Parameters["IsSelected"].SetValue(IsSelected);
		}

		public override void LoadContent()
		{
			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;

			CustomEffect = GameScreen.Content.Load<Effect>("SpriteShader");

			OnClick += OnButtonClicked;

			if (Texture == null)
				Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																					 Globals.GetDestinationTexturePath(AssetName));

			BoundingBoxRenderer = new BoundingBoxRenderer(Game);
			BoundingBoxRenderer.Initialize();

			base.LoadContent();
		}

		private void OnButtonClicked()
		{
			CustomEffect.Parameters["IsClicked"].SetValue(true);
		}

		public override void Update(GameTime gameTime)
		{
			CustomEffect.Parameters["IsClicked"].SetValue(false);

			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

      if (RenderWindow.FollowingObject == this)
				BoundingBoxRenderer.Color = Color.Green;
			else
				BoundingBoxRenderer.Color = Color.Yellow;

			base.Update(gameTime);
		}
	}
}