using System.ComponentModel;
using System.Drawing.Design;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
	public class EditorSpriteBox : SpriteBox
	{
	  private conRenderer RenderWindow;

		[Browsable(true)]
		[Editor(typeof(ObjectTypeDropDown), typeof(UITypeEditor))]
		[DisplayName(@"Object Type")]
		public new string ObjectTypeName { get; set; }

		public new LedgeBuilder AttachedPathingNode { get; set; }

		[Browsable(false)]
		public bool ToggleBoundingBox;

		public EditorSpriteBox(Game game, conRenderer renderer)
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
			CustomEffect = GameScreen.Content.Load<Effect>("SpriteShader");

			if (BoundingBoxRenderer == null)
			{
				BoundingBoxRenderer = new BoundingBoxRenderer(Game);
				BoundingBoxRenderer.SpriteBatch = SpriteBatch;
				BoundingBoxRenderer.Initialize();
			}

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Globals.IsScenePaused)
				return;

			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

			if (IsSelected)
			{
				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = true;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = true;
				}

				ShowBoundingBox = true;
			}
			else
			{
				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = false;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = false;
				}

				ShowBoundingBox = false;
			}

			if (RenderWindow.FollowingObject == this)
				BoundingBoxRenderer.Color = Color.Green;
			else
				BoundingBoxRenderer.Color = Color.Yellow;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			RenderWindow = null;
		}
	}
}