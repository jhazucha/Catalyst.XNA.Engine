using System.ComponentModel;
using System.Drawing.Design;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
	public class EditorGroup : VisualObjectGroup
	{
	  private conRenderer RenderWindow;
    
    public BoundingBoxRenderer Picker;

		[Browsable(true)]
		[Editor(typeof(ObjectTypeDropDown), typeof(UITypeEditor))]
		[DisplayName(@"Object Type")]
		public new string ObjectTypeName { get; set; }

		public new LedgeBuilder AttachedPathingNode { get; set; }

		public EditorGroup(Game game, conRenderer renderer)
			: base(game)
		{
		  RenderWindow = renderer;

			ObjectTypeName = string.Empty;

			Globals.OnSceneEvent += OnSceneEvent;
		}

		private void OnSceneEvent(Enums.SceneState state)
		{
			if (state == Enums.SceneState.Stopped)
			{
				CurrentPathLerpPosition = StartPathingLerpPosition;
				CurrentPathNodeIndex = StartPathNodeIndex;
			}
		}

		public override void LoadContent()
		{
			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;

			Picker = new BoundingBoxRenderer(Game);
			Picker.Initialize();

			base.LoadContent();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			if (Picker != null)
			{
				Picker.UnloadContent();
				Picker = null;
			}

			foreach (var v in Objects)
				v.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			CameraOffsetX = Globals.CurrentCameraOffsetX;
			CameraOffsetY = Globals.CurrentCameraOffsetY;
			CameraZoomOffset = Globals.CurrentCameraZoom;

			base.Update(gameTime);

			if (Picker != null)
			{
				// Setup our Picker
				Picker.Position = Position;
				Picker.ShowBoundingBox = ShowBoundingBox;
				Picker.CameraOffsetX = CameraOffsetX;
				Picker.CameraOffsetY = CameraOffsetY;
				Picker.CameraZoomOffset = CameraZoomOffset;
			}

      if (RenderWindow.FollowingObject == this)
			{
				foreach (var n in Objects)
				{
					if (n is EditorSprite)
					{
						var s = n as EditorSprite;
						s.BoundingBoxRenderer.Color = Color.Green;
					}
					if (n is EditorEmitter)
					{
						var e = n as EditorEmitter;
						e.BoundingBoxRenderer.Color = Color.Green;
					}
					if (n is EditorActor)
					{
						var e = n as EditorActor;
						e.BoundingBoxRenderer.Color = Color.Green;
					}
				}
			}
			else
			{
				foreach (var n in Objects)
				{
					if (n is EditorSprite)
					{
						var s = n as EditorSprite;
						s.BoundingBoxRenderer.Color = Color.Yellow;
					}
					if (n is EditorEmitter)
					{
						var e = n as EditorEmitter;
						e.BoundingBoxRenderer.Color = Color.Yellow;
					}
					if (n is EditorActor)
					{
						var e = n as EditorActor;
						e.BoundingBoxRenderer.Color = Color.Yellow;
					}
				}
			}

      if (RenderWindow.CurrentSelectedObject == this)
			{
				IsSelected = true;

				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = true;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = true;
				}

				foreach (VisualObject vo in Objects)
					vo.ShowBoundingBox = true;
			}
			else
			{
				IsSelected = false;

				if (AttachedPathingNode != null)
				{
					AttachedPathingNode.IsSelected = false;
					foreach (var n in AttachedPathingNode.Nodes)
						n.IsSelected = false;
				}

				foreach (VisualObject vo in Objects)
					vo.ShowBoundingBox = false;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (Picker != null)
				Picker.Draw(gameTime);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			foreach (var v in Objects)
			{
				v.GameScreen = null;
				v.Dispose();
			}
		}
	}
}