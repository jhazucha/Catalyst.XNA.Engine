using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using LevelEditor2D.EntityClasses;
using LevelEditor2D.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace LevelEditor2D
{
	public class Game1 : Game
	{
		private MouseState prevMouseState;

		private Enums.PickingState pickingState = Enums.PickingState.Idle;
		private Enums.CameraState cameraState = Enums.CameraState.Idle;

		private InputHandler InputHandler;
		private LedgeNodeDisplay SelectedNode;
		private frmMain frmMain;

		private TimeSpan elapsedSelectDelay;

		// This is for Mouse Picking Diagnostics reasons do not remove
		public bool ShowMouseLocation;

		public Game1()
		{
			Globals.GraphicsDeviceManager = new GraphicsDeviceManager(this);

			// TODO: THIS NORMALLY WILL BE CHANGED FOR XBOX 360 OR PC GAMES BUT FOR NOW DEFAULT TO WINDOWS PHONE!
			TargetElapsedTime = TimeSpan.FromTicks(333333);

			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			// Store this for our controls to utilize
			Globals.Game = this;

			// Hook the event incase the window size changes
			Globals.WindowSizeChanged += OnMainWinformSizeChanged;

			// Setup our Main Windows Form
			frmMain = new frmMain();
			frmMain.HandleDestroyed += OnMainHandleDestroyed;
			frmMain.SizeChanged += OnMainWinformSizeChanged;
			frmMain.Show();

			// Game Window
			Form gameWindow = (Form)Control.FromHandle(Window.Handle);
			gameWindow.Shown += OnGameWindowShown;
			gameWindow.FormBorderStyle = FormBorderStyle.None;

			Mouse.WindowHandle = frmMain.conRenderer.Handle;
			
			Globals.WindowSizeChanged.Invoke(this, null);

			// Input Handler
			if (InputHandler == null)
			{
				InputHandler = new InputHandler(this, PlayerIndex.One);
				InputHandler.Commands.Add(new InputAction("Delete", Buttons.X, Keys.Delete));
				InputHandler.Commands.Add(new InputAction("SpaceBar", Buttons.LeftStick, Keys.Space));
				InputHandler.Commands.Add(new InputAction("Cntrl", Buttons.A, Keys.LeftControl));
				Components.Add(InputHandler);
			}

			Globals.CurrentCameraOffsetY = 50;
			Globals.CurrentCameraOffsetX = 50;

			base.Initialize();
		}

		private static void OnGameWindowShown(object sender, EventArgs e)
		{
			((Form)sender).Hide();
		}

		private void OnMainWinformSizeChanged(object sender, EventArgs e)
		{
			if (frmMain == null)
				return;

			frmMain.conRenderer.IsRendering = false;

			Form gameWindow = (Form) Control.FromHandle(Window.Handle);
			gameWindow.Size = frmMain.conRenderer.Size;

			frmMain.conRenderer.SceneMarkers.UnloadContent();
			frmMain.conRenderer.SceneManager.UnloadContent();

			Globals.GraphicsDeviceManager.PreferredBackBufferWidth = gameWindow.Size.Width;
			Globals.GraphicsDeviceManager.PreferredBackBufferHeight = gameWindow.Size.Height;
			Globals.GraphicsDeviceManager.ApplyChanges();

			frmMain.conRenderer.SceneMarkers.Initialize();
			frmMain.conRenderer.SceneManager.Initialize();

			frmMain.conRenderer.IsRendering = true;
		}

		private void OnMainHandleDestroyed(object sender, EventArgs e)
		{
			Exit();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (GraphicsDevice == null || GraphicsDevice.IsDisposed)
				return;

			// Update our key frame animation container
			if (Globals.Update != null)
				Globals.Update.Invoke(gameTime);

			#region Check for Mouse Input

			if (!Globals.IsDialogWindowOpen)
			{
				elapsedSelectDelay += gameTime.ElapsedGameTime;

				// Grab our current mouse state
				MouseState currentMouseState = Mouse.GetState();

				// Get our current mouse x/y
				Globals.CurrentMouseX = currentMouseState.X;
				Globals.CurrentMouseY = currentMouseState.Y;

				// Calculate the distance from the current to the last states
				float distanceX = currentMouseState.X - prevMouseState.X;
				float distanceY = currentMouseState.Y - prevMouseState.Y;

				float cameraZoom = currentMouseState.ScrollWheelValue;

				// Get the current rendering window x/y size
				int rendSizeX = Globals.CurrentRenderWindowWidth;
				int rendSizeY = Globals.CurrentRenderWindowHeight;

				// If we are outside the rendering window with the mouse ignore the input (could be in menus, ect)
				if (Globals.CurrentMouseX > rendSizeX || Globals.CurrentMouseY > rendSizeY || Globals.CurrentMouseX <= 0 ||
				    Globals.CurrentMouseY <= 0)
					return;

				// Generate a bounding box with some padding around the mouse cursor
				Vector3 min = new Vector3(Globals.CurrentMouseX - 0.1f, Globals.CurrentMouseY - 0.1f, -50);
				Vector3 max = new Vector3(Globals.CurrentMouseX + 0.1f, Globals.CurrentMouseY + 0.1f, 50);

				BoundingBox mouseBoundingBox = new BoundingBox(min, max);

				if (pickingState == Enums.PickingState.Idle && currentMouseState.LeftButton == ButtonState.Pressed &&
						!InputHandler.IsHeldDown("SpaceBar"))
				{
					if (Globals.IsPathingToolVisible && currentMouseState.LeftButton == ButtonState.Pressed)
					{
						if (prevMouseState.LeftButton != ButtonState.Pressed)
						{
							Globals.OnPathingNodePlaced.Invoke();
							return;
						}
					}

					var vPickables = new List<VisualObject>();

					bool foundNodes = false;

					// Check pathing nodes first!
					var pathingNodes = Globals.Paths;
					foreach (var p in pathingNodes)
					{
						foreach (var n in p.Nodes)
						{
							if (n.BoundingBox.Intersects(mouseBoundingBox))
							{
								pickingState = Enums.PickingState.PickingNode;
								SelectedNode = n;
								SelectedNode.IsSelected = true;
								foundNodes = true;
							}
						}
					}

					if (!foundNodes && frmMain.conRenderer.RenderWindow.SceneManager.CurrentScreen != null)
					{
						// Check for scene objects
            var sceneObjects = frmMain.conRenderer.RenderWindow.VisualObjects;
						foreach (var vo in sceneObjects)
						{
							if (vo is EditorGroup)
							{
								EditorGroup g = vo as EditorGroup;
								if (g.Objects.Any(v => v.BoundingBox.Intersects(mouseBoundingBox)))
								{
									vPickables.Add(vo);
								}
							}
							else if (vo.BoundingBox.Intersects(mouseBoundingBox))
							{
								vPickables.Add(vo);
							}
						}

						// Found some pickable objects get the one with the highest draw order
						var pickable = (from v in vPickables orderby v.DrawOrder descending select v).ToList();
						if (pickable.Count > 0)
						{
							if (InputHandler.IsHeldDown("Cntrl"))
							{
								if (frmMain.conRenderer.CurrentSelectedObject != null)
                  frmMain.conRenderer.CurrentlySelectedObjects.Add(frmMain.conRenderer.CurrentSelectedObject);

								// Deleselct any selected objects
								Globals.ObjectSelected.Invoke(null);

                if (!frmMain.conRenderer.CurrentlySelectedObjects.Contains(pickable.First()))
                  frmMain.conRenderer.CurrentlySelectedObjects.Add(pickable.First());
							}
							else
							{
                frmMain.conRenderer.CurrentlySelectedObjects.Clear();

								pickingState = Enums.PickingState.PickingObject;
								Globals.ObjectSelected.Invoke(pickable.First());
							}
						}
						else
						{
              frmMain.conRenderer.CurrentlySelectedObjects.Clear();

							cameraState = Enums.CameraState.Idle;
							Globals.ObjectSelected.Invoke(null);
							return;
						}
					}
				}
				else
				{
					if (!Globals.IsScenePaused && pickingState == Enums.PickingState.PickingObject &&
							!InputHandler.IsHeldDown("SpaceBar"))
					{
						// Latched on a scene object
						if (currentMouseState.LeftButton == ButtonState.Pressed)
						{
              if (frmMain.conRenderer.CurrentSelectedObject is EditorGroup)
							{
                EditorGroup g = frmMain.conRenderer.CurrentSelectedObject as EditorGroup;

								if (g != null && !g.IsLocked)
								{
									foreach (var vo in g.Objects)
										vo.Position += new Vector2(distanceX, distanceY);

									g.Position += new Vector2(distanceX, distanceY);
								}
							}
              else if (frmMain.conRenderer.CurrentSelectedObject is LedgeBuilder)
							{
								SelectedNode.Position += new Vector2(distanceX, distanceY);
							}
							else
							{
                if (!frmMain.conRenderer.CurrentSelectedObject.IsLocked)
                  frmMain.conRenderer.CurrentSelectedObject.Position += new Vector2(distanceX, distanceY);
							}
						}
					}
					else if (pickingState == Enums.PickingState.PickingNode && !InputHandler.IsHeldDown("SpaceBar"))
					{
						if (currentMouseState.LeftButton == ButtonState.Pressed)
						{
							if (SelectedNode != null)
							{
								// Nodes are Locked move all them together
								if (SelectedNode.IsLocked)
								{
									// MOVE ALL OTHER NODES IN THE CHAIN
									var firstNode = SelectedNode.Parent.Nodes[0];

									if (firstNode != null)
									{
										// Move the first node
										firstNode.Position += new Vector2(distanceX, distanceY);
										firstNode.IsSelected = true;

										for (int i = 1; i < SelectedNode.Parent.Nodes.Count; i++)
										{
											// move the other nodes too	
											SelectedNode.Parent.Nodes[i].Position += new Vector2(distanceX, distanceY);
										}
									}
								}
								else
								{
									if (SelectedNode.Parent != null)
									{
										foreach (var nodes in SelectedNode.Parent.Nodes)
											nodes.IsSelected = false;

										SelectedNode.IsSelected = true;

										if (SelectedNode.Parent.IsAxisYLocked)
										{
											SelectedNode.Position += new Vector2(distanceX, 0);
										}
										else if (SelectedNode.Parent.IsAxisXLocked)
										{
											SelectedNode.Position += new Vector2(0, distanceY);
										}
										else
											SelectedNode.Position += new Vector2(distanceX, distanceY);
									}
									else
										SelectedNode.Position += new Vector2(distanceX, distanceY);
								}
							}
						}
					}
					else if (cameraState == Enums.CameraState.Idle)
					{
						// Camera Panning
						if (Globals.IsProjectLoaded)
						{
							if (InputHandler.IsHeldDown("SpaceBar"))
							{
								if (currentMouseState.LeftButton == ButtonState.Pressed)
								{
									cameraState = Enums.CameraState.Panning;
								}

								// Camera Zoomage
								Globals.CurrentCameraZoom += (float)Math.Ceiling(cameraZoom / 1000f);
							}
						}
					}

					if (cameraState == Enums.CameraState.Panning)
					{
						// Move everything
						Globals.CurrentCameraOffsetX += distanceX / Globals.CameraSensitivity;
						Globals.CurrentCameraOffsetY += distanceY / Globals.CameraSensitivity;

						cameraState = Enums.CameraState.Idle;
					}

					if (InputHandler.IsActionPressed("Delete"))
					{
            if (frmMain.conRenderer.CurrentSelectedObject != null)
						{
							if (
								MessageBox.Show(@"Remove selected sprite from the scene?", @"Sprite Remove Confirmation",
																MessageBoxButtons.YesNo,
																MessageBoxIcon.Asterisk) == DialogResult.Yes)
							{
								// Remove the currently selected sprite from our scene collection
                Globals.ObjectRemoved.Invoke(frmMain.conRenderer.CurrentSelectedObject);
							}
						}
					}
				}

				if (currentMouseState.LeftButton == ButtonState.Released)
				{
					// Done dragging
					pickingState = Enums.PickingState.Idle;
				}

				// Update our mouse state
				prevMouseState = currentMouseState;
			}

			#endregion
		}

		protected override void Draw(GameTime gameTime)
		{
			if (frmMain.conRenderer != null && frmMain.conRenderer.IsRendering &&
					frmMain.WindowState != FormWindowState.Minimized)
			{
				// Draw all sprites first
				base.Draw(gameTime);

				if (Globals.Draw != null)
					Globals.Draw.Invoke(gameTime);

				//// Present all this to our Graphics Device
				Globals.GraphicsDeviceManager.GraphicsDevice.Present(null, new Rectangle(0, 0, frmMain.conRenderer.Width, frmMain.conRenderer.Height), frmMain.conRenderer.Handle);
			}
		}
	}
}