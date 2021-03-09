using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
	public class EditorLabel : Label
	{
	  private conRenderer RenderWindow;

		[Browsable(false)]
		public BoundingBoxRenderer Picker;

		[Browsable(true)]
		public new Color ShadowColor
		{
			get { return base.ShadowColor; }
			set { base.ShadowColor = value; }
		}

		[Browsable(true)]
		public new Color FontColor
		{
			get { return base.FontColor; }
			set { base.FontColor = value; }
		}

		[Browsable(true)]
		public new Vector2 ShadowOffset
		{
			get { return base.ShadowOffset; }
			set { base.ShadowOffset = value; }
		}

		public EditorLabel(Game game, string asset, conRenderer renderer)
			: base(game, asset)
		{
		  RenderWindow = renderer;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			IsCentered = true;

			if (Picker == null)
			{
				Picker = new BoundingBoxRenderer(Game);
				Picker.Initialize();
			}

			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
				BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

			if (IsSelected)
				ShowBoundingBox = true;
			else
				ShowBoundingBox = false;

			if (RenderWindow.FollowingObject == this)
				BoundingBoxRenderer.Color = Color.Green;
			else
				BoundingBoxRenderer.Color = Color.Yellow;
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			Picker.Draw(gameTime);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			Picker.UnloadContent();
			Picker = null;

			RenderWindow = null;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			RenderWindow = null;
			Picker = null;
		}
	}
}
