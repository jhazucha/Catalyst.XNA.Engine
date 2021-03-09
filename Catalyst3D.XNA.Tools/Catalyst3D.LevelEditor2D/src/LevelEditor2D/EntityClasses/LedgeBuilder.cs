using System;
using System.Collections.Generic;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EnumClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.TypeEditors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
	public class LedgeBuilder : VisualObject
	{
		private conRenderer RenderWindow;
		private BasicEffect basicEffect;

		[Browsable(false)]
		public new Ledge AttachedPathingNode { get; set; }

		[Browsable(false)]
		public new string AttachedPathingNodeName { get; set; }

		[Editor(typeof(NodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public List<LedgeNodeDisplay> Nodes { get; set; }

		[Browsable(false)]
		public List<Sequence2D> Sequences { get; set; }

		[Browsable(false)]
		public new Vector2 BoundingBoxScale { get; set; }

		[Browsable(false)]
		public new float Rotation { get; set; }

		[Browsable(false)]
		public new int DrawOrder { get; set; }

		[Browsable(false)]
		public new float UpdateOrder { get; set; }

		[Browsable(false)]
		public new string ObjectType { get; set; }

		[Browsable(false)]
		public new Vector2 Position { get; set; }

		[Browsable(false)]
		public new Vector2 Scale { get; set; }

		[Browsable(true)]
		public LedgeRole Role { get; set; }

		[Browsable(false)]
		public float Width = 16;

		[Browsable(false)]
		public float Height = 16;

		[Browsable(true)]
		public new string Name
		{
			get { return base.Name; }
			set
			{
				// TODO: REWORK THIS NO MORE STATIC VARS FOR RENDERING WINDOW
				//// already has a name need to update any attached pathing nodes with the name change
				//foreach (var vo in Globals.RenderWindow.VisualObjects.Where(vo => !string.IsNullOrEmpty(vo.AttachedPathingNodeName) && vo.AttachedPathingNodeName == base.Name))
				//{
				//  vo.AttachedPathingNodeName = value;
				//}

				base.Name = value;
			}
		}

		public bool IsAxisYLocked { get; set; }
		public bool IsAxisXLocked { get; set; }

		[Browsable(true)]
		public LedgeTravelAlgo LedgeTavelAlgo { get; set; }

		[Browsable(true)]
		public bool IsTraveling { get; set; }

		public LedgeBuilder(Game game, conRenderer renderer)
			: base(game)
		{
			Sequences = new List<Sequence2D>();
			Nodes = new List<LedgeNodeDisplay>();
			Role = LedgeRole.Ground;

			RenderWindow = renderer;

			Globals.OnSceneEvent += OnSceneEventTriggered;
		}

		private void OnSceneEventTriggered(Enums.SceneState state)
		{
			if (state == Enums.SceneState.Playing)
				IsTraveling = true;
			else if (state == Enums.SceneState.Stopped)
				IsTraveling = false;
		}

		public override void Initialize()
		{
			base.Initialize();

			if (string.IsNullOrEmpty(Name))
				Name = "Unnamed Path" + RenderWindow.RenderWindow.Paths.Count;

			foreach (var n in Nodes)
			{
				n.Parent = this;
				n.Initialize();
			}
		}

		public override void LoadContent()
		{
			if (basicEffect == null)
			{
				basicEffect = new BasicEffect(GraphicsDevice);
				basicEffect.VertexColorEnabled = true;
			}

			foreach (var n in Nodes)
			{
				if (n != null)
					n.LoadContent();
			}

			if (RenderWindow == null)
				RenderWindow = Globals.RenderWindow;

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (RenderWindow.CurrentSelectedObject == this)
			{
				IsSelected = true;
			}
			else
			{
				IsSelected = false;
			}

			foreach (LedgeNodeDisplay n in Nodes)
			{
				n.IsSelected = IsSelected;
				n.ShowBoundingBox = ShowBoundingBox;
				n.CameraOffsetX = CameraOffsetX;
				n.CameraOffsetY = CameraOffsetY;
				n.CameraZoomOffset = CameraZoomOffset;
				n.IsLocked = IsLocked;

				n.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (!Visible && !IsSelected)
				return;

			for (int i = 0; i < Nodes.Count; i++)
			{
				if (i > 0)
				{
					// Center Point of Cursor
					float width = Width / 2;
					float height = Height / 2;

					// Setup our Basic Effect
					basicEffect.View = Matrix.CreateTranslation(CameraOffsetX, CameraOffsetY, 0);
					basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, Game.Window.ClientBounds.Width,
																																			Game.Window.ClientBounds.Height, 0, 0, 1);
					// Begin rendering with our basic effect
					foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
					{
						pass.Apply();

						// Create our Vertex Position Colored Array
						VertexPositionColor[] verts = new VertexPositionColor[2];

						switch (Role)
						{
							case LedgeRole.Path:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Green);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Green);
								break;
							case LedgeRole.Ground:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Lime);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Lime);
								break;
							case LedgeRole.Ledge:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Yellow);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Yellow);
								break;
							case LedgeRole.Boundry:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Red);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Red);
								break;
							case LedgeRole.Stairs:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Orange);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Orange);
								break;
							case LedgeRole.HitBox:
								verts[0] =
									new VertexPositionColor(new Vector3(Nodes[i - 1].Position.X + width, Nodes[i - 1].Position.Y + height, 0),
																					Color.Violet);
								verts[1] = new VertexPositionColor(new Vector3(Nodes[i].Position.X + width, Nodes[i].Position.Y + height, 0),
																									 Color.Violet);
								break;
						}

						GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, verts, 0, 1);
					}
				}

				// Draw our node
				Nodes[i].Draw(gameTime);
			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();

			basicEffect = null;

			foreach (var l in Nodes)
				l.UnloadContent();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			basicEffect = null;
			Nodes = null;
			RenderWindow = null;
		}
	}
}