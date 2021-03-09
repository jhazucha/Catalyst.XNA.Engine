using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.CollectionClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Label = Catalyst3D.XNA.Engine.EntityClasses.Text.Label;

namespace LevelEditor2D.Forms
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Show the mouse cursor
			Globals.Game.IsMouseVisible = true;

			// Wire up some of our global events
			Globals.LoadProject += LoadProject;
			Globals.SaveProject += SaveProject;
			Globals.ObjectsGroupped += OnObjectsGroupped;
			Globals.ObjectAdded += OnObjectAdded;
			Globals.ObjectRemoved += OnObjectRemoved;
			Globals.ObjectSelected += OnObjectSelected;
			Globals.OnSaveCurrentPath += OnSaveCurrentPath;
			Globals.Update += OnSceneUpdate;

			// Add our custom buttons to the property grid			
			foreach (Control c in pgGrid.Controls)
			{
				if (c is ToolStrip)
				{
					ToolStrip b = c as ToolStrip;

					// Add a seperator
					ToolStripSeparator sep = new ToolStripSeparator();
					b.Items.Add(sep);

					// Duplicate Object
					Image img1 = Image.FromHbitmap(Resource1.add_page.GetHbitmap());
					ToolStripButton dupe = new ToolStripButton("Duplicate", img1, OnDuplicateClicked);
					b.Items.Add(dupe);

					// Remove Object
					Image img2 = Image.FromHbitmap(Resource1.delete_page.GetHbitmap());
					ToolStripButton remove = new ToolStripButton("Remove", img2, OnRemoveClicked);
					b.Items.Add(remove);

				}
			}

			// If they passed in a command line argument (project .c2d file and path) load it up!
			if (!string.IsNullOrEmpty(Globals.AppSettings.ProjectFilename))
			{
				LoadProject(false, Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename);
			}
		}

		private void OnSceneUpdate(GameTime gametime)
		{
      if (conRenderer.FollowingObject == null && conRenderer.CurrentSelectedObject != null)
			{
				tsFollow.Text = @"Follow Selected";
			}
      else if (conRenderer.FollowingObject != null)
			{
        if (!string.IsNullOrEmpty(conRenderer.FollowingObject.Name))
          tsFollow.Text = @"Stop Following " + conRenderer.FollowingObject.Name;
				else
					tsFollow.Text = @"Stop Following";
			}
      
			if (conRenderer.CurrentlySelectedObjects.Count > 0)
				tsGroupSelected.Enabled = true;
			else
				tsGroupSelected.Enabled = false;

			if (conRenderer.CurrentSelectedObject != null)
			{
        if (!string.IsNullOrEmpty(conRenderer.CurrentSelectedObject.AttachedPathingNodeName))
					tsDetach.Enabled = true;
				else
					tsDetach.Enabled = false;

				tsLock.Enabled = true;

        if (conRenderer.CurrentSelectedObject.IsLocked)
				{
					tsLock.Text = @"Unlock Selected";
				}
				else
				{
					tsLock.Text = @"Lock Selected";
				}
			}
			else
			{
				tsDetach.Enabled = false;
				tsLock.Enabled = false;
			}
		}

		private void OnObjectSelected(VisualObject obj)
		{
			if (obj == null)
			{
				VisualObjectCollection col = new VisualObjectCollection();
				col.AddRange(conRenderer.RenderWindow.VisualObjects);
				col.AddRange(Globals.Paths.ToArray());

				foreach (VisualObject vo in col)
					vo.IsSelected = false;

        conRenderer.CurrentSelectedObject = null;

				// Select all the scene objects
				pgGrid.SelectedObject = col;
			}
			else
			{
				// Select just the object
				pgGrid.SelectedObject = obj;
			}
		}

		private void OnRemoveClicked(object sender, EventArgs e)
		{
			if (conRenderer.CurrentSelectedObject == null)
				return;

      VisualObject vo = conRenderer.CurrentSelectedObject;

			if (vo is LedgeBuilder)
			{
				var ledge = vo as LedgeBuilder;

        var objects = (from s in conRenderer.RenderWindow.VisualObjects
											 where s.AttachedPathingNodeName == ledge.Name
											 select s).ToList();

				if (objects.Count > 0)
				{
					if (MessageBox.Show(@"Would you also like to remove all objects attached to this pathing node as well?", @"Remove All Associated Objects Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						// Remove all scene objects as well
						foreach (var v in objects)
							Globals.ObjectRemoved.Invoke(v);

						// Remove path
						Globals.ObjectRemoved.Invoke(vo);
					}
					else
					{
						// Flush the pathing node off the objects
						foreach (var v in objects)
						{
							if (v is EditorSprite)
							{
								var sp = v as EditorSprite;
								sp.AttachedPathingNode = null;
								sp.AttachedPathingNodeName = string.Empty;
							}
							if (v is EditorActor)
							{
								var ac = v as EditorActor;
								ac.AttachedPathingNode = null;
								ac.AttachedPathingNodeName = string.Empty;
							}
							if (v is EditorEmitter)
							{
								var em = v as EditorEmitter;
								em.AttachedPathingNode = null;
								em.AttachedPathingNodeName = string.Empty;
							}
						}

						// Remove path
						Globals.ObjectRemoved.Invoke(vo);
					}
				}
				else
				{
					// Remove path
					Globals.ObjectRemoved.Invoke(vo);
				}
			}
			else
				Globals.ObjectRemoved.Invoke(vo);
		}

		private void OnDuplicateClicked(object sender, EventArgs e)
		{
      if (conRenderer.CurrentSelectedObject == null)
				return;

			#region Sprites

      if (conRenderer.CurrentSelectedObject is EditorSprite)
			{
        EditorSprite original = conRenderer.CurrentSelectedObject as EditorSprite;
				{
					EditorSprite sprite = new EditorSprite(Globals.Game)
																	{
																		Name = original.Name,
																		Texture = original.Texture,
																		AssetName = original.AssetName,
																		Position = new Vector2(original.Position.X - 25, original.Position.Y - 25),
																		Scale = original.Scale,
																		LayerDepth = original.LayerDepth,
																		Origin = original.Origin,
																		Rotation = original.Rotation,
																		StartPosition = original.StartPosition,
																		UpdateOrder = original.UpdateOrder,
																		CustomEffect = original.CustomEffect,
																		ShowBoundingBox = true,
																		BlendMode = original.BlendMode,
																		ScrollSpeed = original.ScrollSpeed,
																		DrawOrder = original.DrawOrder,
																		Visible = true,
																		Color = original.Color,
																		Effects = original.Effects,
																		Enabled = original.Enabled,
																		IsLocked = original.IsLocked,
																		IsCentered = original.IsCentered,
																		CurrentPathNodeIndex = original.CurrentPathNodeIndex,
																		CurrentPathLerpPosition = 0,
																		AttachedPathingNodeName = original.AttachedPathingNodeName,
																		AttachedPathingNode = original.AttachedPathingNode,
																		StartPathingLerpPosition = original.StartPathingLerpPosition,
																		StartPathNodeIndex = original.StartPathNodeIndex
																	};

					// Init this texture
					sprite.Initialize();

					// Add it to current our Key Frame
					Globals.ObjectAdded.Invoke(sprite);

					// Select it
					Globals.ObjectSelected.Invoke(sprite);
				}
			}

			#endregion

			#region Emitters

      if (conRenderer.CurrentSelectedObject is ParticleEmitter)
			{
        EditorEmitter original = conRenderer.CurrentSelectedObject as EditorEmitter;

				if (original != null)
				{
					EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer)
																		{
																			Name = original.Name,
																			AssetName = original.AssetName,
																			Camera = original.Camera,
																			ParticleColor = original.ParticleColor,
																			DrawOrder = original.DrawOrder,
																			EnableAlphaBlending = original.EnableAlphaBlending,
																			Enabled = original.Enabled,
																			ForceObjects = original.ForceObjects,
																			MinAcceleration = original.MinAcceleration,
																			MaxAcceleration = original.MaxAcceleration,
																			MinInitialSpeed = original.MinInitialSpeed,
																			MaxInitialSpeed = original.MaxInitialSpeed,
																			MinLifeSpan = original.MinLifeSpan,
																			MaxLifeSpan = original.MaxLifeSpan,
																			MinParticles = original.MinParticles,
																			MaxParticles = original.MaxParticles,
																			MinRotationSpeed = original.MinRotationSpeed,
																			MaxRotationSpeed = original.MaxRotationSpeed,
																			MinScale = original.MinScale,
																			MaxScale = original.MaxScale,
																			ShowBoundingBox = true,
																			Texture =
																				Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																																 Globals.GetDestinationTexturePath(original.AssetName)),
																			CurrentPathNodeIndex = original.CurrentPathNodeIndex,
																			CurrentPathLerpPosition = 0,
																			StartPathingLerpPosition = original.StartPathingLerpPosition,
																			StartPathNodeIndex = original.StartPathNodeIndex,
																			AttachedPathingNodeName = original.AttachedPathingNodeName,
																			AttachedPathingNode = original.AttachedPathingNode,
																			Position = new Vector2(original.Position.X - 25, original.Position.Y - 25),
																		};

					emitter.Initialize();

					// Add it to current our Key Frame
					Globals.ObjectAdded(emitter);

					// Select it
					Globals.ObjectSelected.Invoke(emitter);
				}
			}

			#endregion

			#region Groups

      if (conRenderer.CurrentSelectedObject is VisualObjectGroup)
			{
        EditorGroup original = conRenderer.CurrentSelectedObject as EditorGroup;

				if (original != null)
				{
					EditorGroup group = new EditorGroup(Globals.Game, conRenderer)
																{
																	AssetName = original.AssetName,
																	DrawOrder = original.DrawOrder,
																	UpdateOrder = original.UpdateOrder,
																	Enabled = original.Enabled,
																	Name = original.Name,
																	Scale = original.Scale,
																	Visible = original.Visible,
																	ShowBoundingBox = true,
																	IsLocked = original.IsLocked,
																	CurrentPathNodeIndex = original.CurrentPathNodeIndex,
																	CurrentPathLerpPosition = 0,
																	StartPathingLerpPosition = original.StartPathingLerpPosition,
																	StartPathNodeIndex = original.StartPathNodeIndex,
																	AttachedPathingNodeName = original.AttachedPathingNodeName,
																	AttachedPathingNode = original.AttachedPathingNode,
																	Position = new Vector2(original.Position.X - 25, original.Position.Y - 25)
																};

					foreach (VisualObject o in original.Objects)
					{
						if (o is EditorSprite)
						{
							EditorSprite sOriginal = o as EditorSprite;

							// Init our new Sprite
							EditorSprite sprite = new EditorSprite(Globals.Game)
																			{
																				Position = sOriginal.Position,
																				Name = sOriginal.Name,
																				Texture = sOriginal.Texture,
																				AssetName = sOriginal.AssetName,
																				Scale = sOriginal.Scale,
																				LayerDepth = sOriginal.LayerDepth,
																				Origin = sOriginal.Origin,
																				Rotation = sOriginal.Rotation,
																				StartPosition = sOriginal.StartPosition,
																				UpdateOrder = sOriginal.UpdateOrder,
																				CustomEffect = sOriginal.CustomEffect,
																				ShowBoundingBox = false,
																				BlendMode = sOriginal.BlendMode,
																				IsLocked = sOriginal.IsLocked,
																				Color = sOriginal.Color,
																				ScrollSpeed = sOriginal.ScrollSpeed
																			};

							// Init this texture
							sprite.Initialize();

							// Add it to our groups objects
							group.Objects.Add(sprite);
						}

						if (o is ParticleEmitter)
						{
							EditorEmitter eOriginal = o as EditorEmitter;

							if (eOriginal != null)
							{
								EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer)
																					{
																						Position = eOriginal.Position,
																						Name = eOriginal.Name,
																						AssetName = eOriginal.AssetName,
																						Camera = eOriginal.Camera,
																						ParticleColor = eOriginal.ParticleColor,
																						DrawOrder = eOriginal.DrawOrder,
																						EnableAlphaBlending = eOriginal.EnableAlphaBlending,
																						Enabled = eOriginal.Enabled,
																						ForceObjects = eOriginal.ForceObjects,
																						MinAcceleration = eOriginal.MinAcceleration,
																						MaxAcceleration = eOriginal.MaxAcceleration,
																						MinInitialSpeed = eOriginal.MinInitialSpeed,
																						MaxInitialSpeed = eOriginal.MaxInitialSpeed,
																						MinLifeSpan = eOriginal.MinLifeSpan,
																						MaxLifeSpan = eOriginal.MaxLifeSpan,
																						MinParticles = eOriginal.MinParticles,
																						MaxParticles = eOriginal.MaxParticles,
																						MinRotationSpeed = eOriginal.MinRotationSpeed,
																						MaxRotationSpeed = eOriginal.MaxRotationSpeed,
																						MinScale = eOriginal.MinScale,
																						MaxScale = eOriginal.MaxScale,
																						ShowBoundingBox = false,
																						Texture =
																							Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																																			 Globals.GetDestinationTexturePath(eOriginal.AssetName))
																					};

								emitter.Initialize();

								// Add it to our group
								group.Objects.Add(emitter);
							}
						}
					}

					// Initialize our group
					group.Initialize();

					// Add it to current our Key Frame
					Globals.ObjectAdded.Invoke(group);

					// Select it
					Globals.ObjectSelected.Invoke(group);
				}
			}

			#endregion

			#region Actors

      if (conRenderer.CurrentSelectedObject is EditorActor)
			{
        EditorActor proxy = conRenderer.CurrentSelectedObject as EditorActor;
				{
					EditorActor actor = new EditorActor(Globals.Game, string.Empty, proxy.AssetName, conRenderer);
					actor.Position = proxy.Position;
					actor.Scale = proxy.Scale;
					actor.DrawOrder = proxy.DrawOrder;
					actor.UpdateOrder = proxy.UpdateOrder;
					actor.Visible = proxy.Visible;
					actor.Role = proxy.Role;
					actor.SpriteSheetFileName = proxy.SpriteSheetFileName;
					actor.Name = proxy.Name;
					actor.Enabled = proxy.Enabled;
					actor.Direction = proxy.Direction;

					actor.ClipPlayer = new AnimationPlayer2D(Globals.Game);
					actor.ClipPlayer.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																															Globals.GetDestinationTexturePath(proxy.SpriteSheetFileName));

					actor.ClipPlayer.Sequences = proxy.ClipPlayer.Sequences;

					// Init our actor
					actor.Initialize();

					// Play our first sequence out the gate
					actor.Play(proxy.ClipPlayer.Sequences[0].Name, true);

					Globals.ObjectAdded.Invoke(actor);

					// Select it
					Globals.ObjectSelected.Invoke(actor);
				}
			}

			#endregion

			#region Buttons

      if (conRenderer.CurrentSelectedObject is EditorButton)
			{
				Catalyst3D.XNA.Engine.EntityClasses.UI.Button original =
          conRenderer.CurrentSelectedObject as Catalyst3D.XNA.Engine.EntityClasses.UI.Button;

			  EditorButton button = new EditorButton(Globals.Game, conRenderer);
				{
					button.Name = original.AssetName;
					button.AssetName = original.AssetName;
					button.BlendMode = original.BlendMode;
					button.Color = original.Color;
					button.DrawOrder = original.DrawOrder;
					button.UpdateOrder = original.UpdateOrder;
					button.Effects = original.Effects;
					button.Enabled = original.Enabled;
					button.LayerDepth = original.LayerDepth;
					button.Origin = original.Origin;
					button.Position = original.Position;
					button.Rotation = original.Rotation;
					button.Scale = original.Scale;
					button.Visible = original.Visible;
					button.ShowBoundingBox = original.ShowBoundingBox;
					button.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																										Globals.GetDestinationTexturePath(original.AssetName));
					button.IsLocked = original.IsLocked;
					button.ScrollSpeed = original.ScrollSpeed;
				}

				button.Initialize();

				Globals.ObjectAdded.Invoke(button);

				// Select it
				Globals.ObjectSelected.Invoke(button);
			}

			#endregion

			#region Pathing Nodes

      if (conRenderer.CurrentSelectedObject is LedgeBuilder)
			{
        var ledge = conRenderer.CurrentSelectedObject as LedgeBuilder;
				{
					var newLedge = new LedgeBuilder(Globals.Game, conRenderer)
													{
														Enabled = ledge.Enabled,
														CameraOffsetX = ledge.CameraOffsetX,
														CameraOffsetY = ledge.CameraOffsetY,
														Role = ledge.Role,
														Name = "Un-named Path"
													};

					foreach (var n in ledge.Nodes)
					{
						LedgeNodeDisplay node = new LedgeNodeDisplay(Globals.Game);
						node.Scale = n.Scale;
						node.TravelSpeed = n.TravelSpeed;
						node.Position = n.Position;
						node.Rotation = n.Rotation;

						node.Initialize();

						newLedge.Nodes.Add(node);
					}

					// Init the path
					newLedge.Initialize();

					List<VisualObject> dupedObjects = new List<VisualObject>();

					// Find all objects bound to this path and create new objects on this new path
          foreach (var vo in conRenderer.RenderWindow.VisualObjects)
					{
						#region Sprites

						if (vo is EditorSprite)
						{
							var sprite = vo as EditorSprite;
							if (sprite.AttachedPathingNode == ledge)
							{
								// Create a new Sprite
								var newSprite = new EditorSprite(Globals.Game)
																	{
																		AssetFolder = sprite.AssetFolder,
																		AssetName = sprite.AssetName,
																		AttachedPathingNode = newLedge,
																		AttachedPathingNodeName = newLedge.Name,
																		BlendMode = sprite.BlendMode,
																		BoundingBoxPadding = sprite.BoundingBoxPadding,
																		BoundingBoxScale = sprite.BoundingBoxScale,
																		CameraOffsetX = sprite.CameraOffsetX,
																		CameraOffsetY = sprite.CameraOffsetY,
																		Color = sprite.Color,
																		DrawOrder = sprite.DrawOrder,
																		UpdateOrder = sprite.UpdateOrder,
																		Name = sprite.Name,
																		Effects = sprite.Effects,
																		ObjectType = sprite.ObjectType,
																		ObjectTypeName = sprite.ObjectTypeName,
																		IsCentered = sprite.IsCentered,
																		CurrentPathLerpPosition = sprite.CurrentPathLerpPosition,
																		CurrentPathNodeIndex = sprite.CurrentPathNodeIndex,
																		StartPathingLerpPosition = sprite.StartPathingLerpPosition,
																		StartPathNodeIndex = sprite.StartPathNodeIndex,
																		StartPosition = sprite.StartPosition,
																		Enabled = sprite.Enabled,
																		Visible = sprite.Visible,
																		GameScreen = sprite.GameScreen,
																		Texture = sprite.Texture
																	};

								// Init it
								newSprite.Initialize();

								// Add it to our local collection
								dupedObjects.Add(newSprite);
							}
						}

						#endregion

						#region Groups

						if (vo is EditorGroup)
						{
							var original = vo as EditorGroup;
							if (original.AttachedPathingNode == ledge)
							{
								EditorGroup gp = new EditorGroup(Globals.Game, conRenderer);
								gp.AssetName = Path.GetFileName(original.AssetName);
								gp.DrawOrder = original.DrawOrder;
								gp.UpdateOrder = original.UpdateOrder;
								gp.Enabled = original.Enabled;
								gp.Name = original.Name;
								gp.Picker = new BoundingBoxRenderer(original.Game);
								gp.Scale = original.Scale;
								gp.Visible = original.Visible;
								gp.ShowBoundingBox = original.ShowBoundingBox;
								gp.Position = original.Position;
								gp.CameraOffsetX = Globals.CurrentCameraOffsetX;
								gp.CameraOffsetY = Globals.CurrentCameraOffsetY;
								gp.CameraZoomOffset = Globals.CurrentCameraZoom;

								gp.StartPathingLerpPosition = original.StartPathingLerpPosition;
								gp.StartPathNodeIndex = original.StartPathNodeIndex;
								gp.CurrentPathLerpPosition = original.StartPathingLerpPosition;
								gp.CurrentPathNodeIndex = original.CurrentPathNodeIndex;
								gp.ObjectTypeName = original.ObjectTypeName;

								gp.AttachedPathingNodeName = newLedge.Name;
								gp.AttachedPathingNode = newLedge;

								foreach (VisualObject o in original.Objects)
								{
									#region Sprites

									if (o is Sprite)
									{
										Sprite originalSprite = o as Sprite;

										EditorSprite sprite = new EditorSprite(Globals.Game);
										sprite.Name = originalSprite.Name;
										sprite.AssetName = Path.GetFileName(originalSprite.AssetName);
										sprite.BlendMode = originalSprite.BlendMode;
										sprite.Color = originalSprite.Color;
										sprite.DrawOrder = originalSprite.DrawOrder;
										sprite.UpdateOrder = originalSprite.UpdateOrder;
										sprite.Effects = originalSprite.Effects;
										sprite.Enabled = originalSprite.Enabled;
										sprite.LayerDepth = originalSprite.LayerDepth;
										sprite.Origin = originalSprite.Origin;
										sprite.Position = originalSprite.Position;
										sprite.Rotation = originalSprite.Rotation;
										sprite.Scale = originalSprite.Scale;
										sprite.Visible = originalSprite.Visible;
										sprite.ShowBoundingBox = originalSprite.ShowBoundingBox;
										sprite.Texture = originalSprite.Texture;
										sprite.ScrollSpeed = originalSprite.ScrollSpeed;

										sprite.CurrentPathNodeIndex = originalSprite.StartPathNodeIndex;
										sprite.CurrentPathLerpPosition = originalSprite.StartPathingLerpPosition;
										sprite.StartPathNodeIndex = originalSprite.StartPathNodeIndex;
										sprite.StartPathingLerpPosition = originalSprite.StartPathingLerpPosition;

										sprite.AttachedPathingNodeName = newLedge.Name;
										sprite.AttachedPathingNode = newLedge;

										// Load up our custom shader for our sprite objects
										sprite.Initialize();

										// Add the sprite to the frame's sprite collection
										gp.Objects.Add(sprite);
									}

									#endregion

									#region Emitters

									if (o is ParticleEmitter)
									{
										ParticleEmitter originalEmitter = o as ParticleEmitter;

										EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
										emitter.Name = originalEmitter.Name;
										emitter.AssetName = originalEmitter.AssetName;
										emitter.Position = originalEmitter.Position;
										emitter.ParticleColor = originalEmitter.ParticleColor;
										emitter.DrawOrder = originalEmitter.DrawOrder;
										emitter.EnableAlphaBlending = originalEmitter.EnableAlphaBlending;
										emitter.Enabled = originalEmitter.Enabled;
										emitter.ForceObjects = originalEmitter.ForceObjects;
										emitter.MinAcceleration = originalEmitter.MinAcceleration;
										emitter.MaxAcceleration = originalEmitter.MaxAcceleration;
										emitter.MinInitialSpeed = originalEmitter.MinInitialSpeed;
										emitter.MaxInitialSpeed = originalEmitter.MaxInitialSpeed;
										emitter.MinLifeSpan = originalEmitter.MinLifeSpan;
										emitter.MaxLifeSpan = originalEmitter.MaxLifeSpan;
										emitter.MinParticles = originalEmitter.MinParticles;
										emitter.MaxParticles = originalEmitter.MaxParticles;
										emitter.MinRotationSpeed = originalEmitter.MinRotationSpeed;
										emitter.MaxRotationSpeed = originalEmitter.MaxRotationSpeed;
										emitter.MinScale = originalEmitter.MinScale;
										emitter.MaxScale = originalEmitter.MaxScale;
										emitter.Origin = originalEmitter.Origin;
										emitter.UpdateOrder = originalEmitter.UpdateOrder;
										emitter.Visible = originalEmitter.Visible;
										emitter.ShowBoundingBox = originalEmitter.ShowBoundingBox;
										emitter.Texture = originalEmitter.Texture;

										emitter.StartPathingLerpPosition = originalEmitter.CurrentPathLerpPosition;
										emitter.StartPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

										emitter.CurrentPathLerpPosition = originalEmitter.CurrentPathLerpPosition;
										emitter.CurrentPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

										emitter.AttachedPathingNodeName = newLedge.Name;
										emitter.AttachedPathingNode = newLedge;

										emitter.RespawnParticles = originalEmitter.RespawnParticles;
										emitter.Initialize();

										// Add it to our group
										gp.Objects.Add(emitter);
									}

									#endregion

									if (o is Actor)
									{
										MessageBox.Show(@"Not Yet Implemented to group Actor Objects!");
									}
								}

								gp.Initialize();

								// Add the actor to our key frames visual object collection
								dupedObjects.Add(gp);
							}
						}

						#endregion

						#region Emitters

						if (vo is EditorEmitter)
						{
							EditorEmitter sceneEmitter = vo as EditorEmitter;
							if (sceneEmitter.AttachedPathingNode == ledge)
							{

								EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
								emitter.Name = sceneEmitter.Name;
								emitter.AssetName = sceneEmitter.AssetName;
								emitter.Position = sceneEmitter.Position;
								emitter.ParticleColor = sceneEmitter.ParticleColor;
								emitter.DrawOrder = sceneEmitter.DrawOrder;
								emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
								emitter.Enabled = sceneEmitter.Enabled;
								emitter.ForceObjects = sceneEmitter.ForceObjects;
								emitter.MinAcceleration = sceneEmitter.MinAcceleration;
								emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
								emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
								emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
								emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
								emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
								emitter.MinParticles = sceneEmitter.MinParticles;
								emitter.MaxParticles = sceneEmitter.MaxParticles;
								emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
								emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
								emitter.MinScale = sceneEmitter.MinScale;
								emitter.MaxScale = sceneEmitter.MaxScale;
								emitter.Origin = sceneEmitter.Origin;
								emitter.UpdateOrder = sceneEmitter.UpdateOrder;
								emitter.Visible = sceneEmitter.Visible;
								emitter.ShowBoundingBox = sceneEmitter.ShowBoundingBox;
								emitter.Texture = sceneEmitter.Texture;
								emitter.ObjectTypeName = sceneEmitter.ObjectTypeName;

								emitter.CurrentPathLerpPosition = sceneEmitter.StartPathingLerpPosition;
								emitter.StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition;
								emitter.StartPathNodeIndex = sceneEmitter.StartPathNodeIndex;
								emitter.CurrentPathNodeIndex = sceneEmitter.StartPathNodeIndex;

								emitter.AttachedPathingNodeName = newLedge.Name;
								emitter.AttachedPathingNode = newLedge;

								emitter.RespawnParticles = sceneEmitter.RespawnParticles;
								emitter.Initialize();

								dupedObjects.Add(emitter);
							}
						}

						#endregion

						#region Actors

						if (vo is EditorActor)
						{
							// Create a new Actor object
							EditorActor proxy = vo as EditorActor;
							if (proxy.AttachedPathingNode == ledge)
							{
								EditorActor actor = new EditorActor(Globals.Game, string.Empty, proxy.AssetName, conRenderer);
								actor.Position = proxy.Position;
								actor.Scale = proxy.Scale;
								actor.DrawOrder = proxy.DrawOrder;
								actor.UpdateOrder = proxy.UpdateOrder;
								actor.Visible = proxy.Visible;
								actor.Role = proxy.Role;
								actor.AssetName = proxy.AssetName;
								actor.SpriteSheetFileName = proxy.SpriteSheetFileName;
								actor.Name = proxy.Name;
								actor.Enabled = proxy.Enabled;
								actor.Direction = proxy.Direction;
								actor.IsCentered = proxy.IsCentered;
								actor.IsLooped = proxy.IsLooped;
								actor.ObjectTypeName = proxy.ObjectTypeName;

								actor.ClipPlayer = new AnimationPlayer2D(Globals.Game);
								actor.ClipPlayer.Texture = proxy.ClipPlayer.Texture;
								actor.ClipPlayer.Sequences = proxy.ClipPlayer.Sequences;
								actor.ClipPlayer.IsFlipped = proxy.ClipPlayer.IsFlipped;
								actor.ClipPlayer.IsLooped = proxy.ClipPlayer.IsLooped;

								actor.CurrentPathLerpPosition = proxy.StartPathingLerpPosition;
								actor.CurrentPathNodeIndex = proxy.StartPathNodeIndex;

								actor.StartPathingLerpPosition = proxy.StartPathingLerpPosition;
								actor.StartPathNodeIndex = proxy.StartPathNodeIndex;

								actor.AttachedPathingNodeName = newLedge.Name;
								actor.AttachedPathingNode = newLedge;

								actor.IsInitialized = true;

								// Init our actor
								actor.Initialize();

								// Play our first sequence out the gate
								actor.Play(proxy.ClipPlayer.Sequences[0].Name, true);

								dupedObjects.Add(actor);
							}
						}

						#endregion
					}

					// Add all these to the scene
					foreach (var v in dupedObjects)
						Globals.ObjectAdded.Invoke(v);

					// Add it to the pathing colleciton and the scene
					Globals.Paths.Add(newLedge);

					// Select this path out the gate
					Globals.ObjectSelected.Invoke(newLedge);
				}
			}

			#endregion

			#region Label

			// NEED TO DO THIS

			#endregion
		}

		private void OnObjectAdded(VisualObject sprite)
		{
			Globals.ObjectSelected.Invoke(sprite);
		}

		private void OnSaveCurrentPath()
		{
			VisualObjectCollection col = new VisualObjectCollection();
      col.AddRange(conRenderer.RenderWindow.VisualObjects);
			col.AddRange(Globals.Paths.ToArray());

			pgGrid.SelectedObject = col;
		}

		private void OnObjectRemoved(VisualObject sprite)
		{
			VisualObjectCollection col = new VisualObjectCollection();
			col.AddRange(conRenderer.RenderWindow.VisualObjects);
			col.AddRange(Globals.Paths.ToArray());

			pgGrid.SelectedObject = col;
		}

		private void OnObjectsGroupped(List<VisualObject> objects)
		{
			// Refresh our Property Grid
			if (objects != null)
			{
				VisualObjectCollection voc = new VisualObjectCollection();
				voc.AddRange(objects);

				// Deselect any objects
        conRenderer.CurrentSelectedObject = null;

				// Update the property grid with our objects
				pgGrid.SelectedObject = voc;
			}
		}

		private void LoadProject(bool showResult)
		{
			// Create our Open File Dialog
			OpenFileDialog dia = new OpenFileDialog();
			dia.Filter = @"Catalyst3D Project File|*.c2d";
			dia.InitialDirectory = Globals.AppSettings.ProjectPath;

			Globals.IsDialogWindowOpen = true;

			if (dia.ShowDialog() == DialogResult.OK)
			{
				Globals.Paths.Clear();
			  
        conRenderer.RenderWindow.VisualObjects.Clear();

				LoadProject(showResult, dia.FileName);
			}

			Globals.IsDialogWindowOpen = false;
		}

		private void LoadProject(bool result, string filename)
		{
			try
			{
				CatalystProject2D proj = Serializer.Deserialize<CatalystProject2D>(filename);

				Globals.IsProjectLoaded = true;

				// Store our current projects path and file name
				Globals.AppSettings.ProjectPath = Path.GetDirectoryName(filename);
				Globals.AppSettings.ProjectFilename = Path.GetFileName(filename);

				// Flush any paths
        Globals.Paths.Clear();

				// Clear our scene object collection
        conRenderer.RenderWindow.VisualObjects.Clear();

				// Flush all our containers
				conSpriteContainer.pnContainer.Controls.Clear();
				conSpriteContainer.SelectedSpriteIndex = 0;

				#region Load our Pathing Nodes

				foreach (Ledge l in proj.Paths)
				{
				  LedgeBuilder p = new LedgeBuilder(Globals.Game, conRenderer);
					p.Role = l.Role;
					p.Name = l.Name;
					p.DrawOrder = 100; // Draw always ontop of everything
					p.IsTraveling = l.IsTraveling;
					p.LedgeTavelAlgo = l.LedgeTavelAlgo;

					foreach (LedgeNode n in l.Nodes)
					{
						LedgeNodeDisplay node = new LedgeNodeDisplay(Globals.Game);
						node.Position = n.Position;
						node.Scale = n.Scale;
						node.TravelSpeed = n.TravelSpeed;
						node.DrawOrder = n.DrawOrder;
						node.Rotation = n.Rotation;
						node.AnimationSequence = n.AnimationSequence;
						node.IsLooped = n.IsLooped;
						node.SpriteEffect = n.SpriteEffect;
						node.Initialize();

						p.Nodes.Add(node);
					}

					// Init the Ledge
					p.Initialize();

					// Add our pathing nodes
          Globals.Paths.Add(p);
				}

				#endregion

				#region Load up our Scene Objects

				foreach (VisualObject obj in proj.SceneObjects)
				{
					#region Buttons

					if (obj is Catalyst3D.XNA.Engine.EntityClasses.UI.Button)
					{
						Catalyst3D.XNA.Engine.EntityClasses.UI.Button original = obj as Catalyst3D.XNA.Engine.EntityClasses.UI.Button;

						EditorButton button = new EditorButton(Globals.Game, conRenderer);
						button.Name = original.Name;
						button.AssetName = original.AssetName;
						button.BlendMode = original.BlendMode;
						button.Color = original.Color;
						button.DrawOrder = original.DrawOrder;
						button.UpdateOrder = original.UpdateOrder;
						button.Effects = original.Effects;
						button.Enabled = original.Enabled;
						button.LayerDepth = original.LayerDepth;
						button.Origin = original.Origin;
						button.Position = original.Position;
						button.Rotation = original.Rotation;
						button.Scale = original.Scale;
						button.IsCentered = original.IsCentered;
						button.Visible = original.Visible;
						button.ShowBoundingBox = original.ShowBoundingBox;
						button.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(original.AssetName));
						button.IsLocked = original.IsLocked;
						button.ScrollSpeed = original.ScrollSpeed;

						Globals.ObjectAdded.Invoke(button);

						// Due to this inheritting Sprite make sure it skips over the rest
						continue;
					}

					#endregion

					#region Sprites

					if (obj is Sprite && obj.GetType() != typeof(SpriteBox))
					{
						Sprite original = obj as Sprite;

						EditorSprite sprite = new EditorSprite(Globals.Game);
						sprite.Name = original.Name;
						sprite.AssetName = original.AssetName;
						sprite.AssetFolder = Globals.GetDestinationTexturePath(original.AssetName);
						sprite.BlendMode = original.BlendMode;
						sprite.Color = original.Color;
						sprite.DrawOrder = original.DrawOrder;
						sprite.UpdateOrder = original.UpdateOrder;
						sprite.Effects = original.Effects;
						sprite.Enabled = original.Enabled;
						sprite.LayerDepth = original.LayerDepth;
						sprite.Origin = original.Origin;
						sprite.Position = original.Position;
						sprite.Rotation = original.Rotation;
						sprite.Scale = original.Scale;
						sprite.Visible = original.Visible;
						sprite.ShowBoundingBox = original.ShowBoundingBox;
						sprite.IsLocked = original.IsLocked;
						sprite.ScrollSpeed = original.ScrollSpeed;
						sprite.IsCentered = original.IsCentered;
						sprite.ObjectTypeName = original.ObjectTypeName;

						sprite.CurrentPathLerpPosition = original.CurrentPathLerpPosition;
						sprite.CurrentPathNodeIndex = original.CurrentPathNodeIndex;

						sprite.StartPathingLerpPosition = original.StartPathingLerpPosition;
						sprite.StartPathNodeIndex = original.StartPathNodeIndex;

						sprite.AttachedPathingNodeName = original.AttachedPathingNodeName;

						if (!string.IsNullOrEmpty(original.AttachedPathingNodeName))
						{
							sprite.AttachedPathingNode = conRenderer.GetPath(original.AttachedPathingNodeName);
						}

						Globals.ObjectAdded.Invoke(sprite);
					}

					#endregion

					#region Emitters

					if (obj is ParticleEmitter)
					{
						ParticleEmitter sceneEmitter = obj as ParticleEmitter;

						EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
						emitter.Name = sceneEmitter.Name;
						emitter.AssetName = sceneEmitter.AssetName;
						emitter.AssetFolder = Globals.GetDestinationTexturePath(sceneEmitter.AssetName);
						emitter.Position = sceneEmitter.Position;
						emitter.ParticleColor = sceneEmitter.ParticleColor;
						emitter.DrawOrder = sceneEmitter.DrawOrder;
						emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
						emitter.Enabled = sceneEmitter.Enabled;
						emitter.ForceObjects = sceneEmitter.ForceObjects;
						emitter.MinAcceleration = sceneEmitter.MinAcceleration;
						emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
						emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
						emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
						emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
						emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
						emitter.MinParticles = sceneEmitter.MinParticles;
						emitter.MaxParticles = sceneEmitter.MaxParticles;
						emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
						emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
						emitter.MinScale = sceneEmitter.MinScale;
						emitter.MaxScale = sceneEmitter.MaxScale;
						emitter.Origin = sceneEmitter.Origin;
						emitter.UpdateOrder = sceneEmitter.UpdateOrder;
						emitter.Visible = sceneEmitter.Visible;
						emitter.ShowBoundingBox = sceneEmitter.ShowBoundingBox;
						emitter.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(sceneEmitter.AssetName));
						emitter.ObjectTypeName = sceneEmitter.ObjectTypeName;
						emitter.IsLocked = sceneEmitter.IsLocked;

						emitter.CurrentPathLerpPosition = sceneEmitter.StartPathingLerpPosition;
						emitter.StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition;

						emitter.StartPathNodeIndex = sceneEmitter.StartPathNodeIndex;
						emitter.CurrentPathNodeIndex = sceneEmitter.StartPathNodeIndex;
						emitter.AttachedPathingNodeName = sceneEmitter.AttachedPathingNodeName;

						if (!string.IsNullOrEmpty(sceneEmitter.AttachedPathingNodeName))
							emitter.AttachedPathingNode = conRenderer.GetPath(sceneEmitter.AttachedPathingNodeName);

						emitter.RespawnParticles = sceneEmitter.RespawnParticles;

						Globals.ObjectAdded.Invoke(emitter);
					}

					#endregion

					#region Groups

					if (obj is VisualObjectGroup)
					{
						VisualObjectGroup original = obj as VisualObjectGroup;

						EditorGroup group = new EditorGroup(Globals.Game, conRenderer);
						group.AssetName = Path.GetFileName(original.AssetName);
						group.DrawOrder = original.DrawOrder;
						group.UpdateOrder = original.UpdateOrder;
						group.Enabled = original.Enabled;
						group.Name = original.Name;
					  group.Picker = new BoundingBoxRenderer(Globals.Game);
						group.Scale = original.Scale;
						group.Visible = original.Visible;
						group.ShowBoundingBox = original.ShowBoundingBox;
						group.Position = original.Position;
						group.CameraOffsetX = Globals.CurrentCameraOffsetX;
						group.CameraOffsetY = Globals.CurrentCameraOffsetY;
						group.CameraZoomOffset = Globals.CurrentCameraZoom;
						group.AttachedPathingNodeName = original.AttachedPathingNodeName;
						group.StartPathingLerpPosition = original.StartPathingLerpPosition;
						group.StartPathNodeIndex = original.StartPathNodeIndex;
						group.CurrentPathLerpPosition = original.StartPathingLerpPosition;
						group.CurrentPathNodeIndex = original.CurrentPathNodeIndex;
						group.ObjectTypeName = original.ObjectTypeName;
						group.IsLocked = original.IsLocked;

						if (!string.IsNullOrEmpty(original.AttachedPathingNodeName))
							group.AttachedPathingNode = conRenderer.GetPath(original.AttachedPathingNodeName);

						foreach (VisualObject o in original.Objects)
						{
							#region Sprites

							if (o is Sprite)
							{
								Sprite originalSprite = o as Sprite;

								EditorSprite sprite = new EditorSprite(Globals.Game);
								sprite.Name = originalSprite.Name;
								sprite.AssetName = Path.GetFileName(originalSprite.AssetName);
								sprite.AssetFolder = Globals.GetDestinationTexturePath(originalSprite.AssetName);
								sprite.BlendMode = originalSprite.BlendMode;
								sprite.Color = originalSprite.Color;
								sprite.DrawOrder = originalSprite.DrawOrder;
								sprite.UpdateOrder = originalSprite.UpdateOrder;
								sprite.Effects = originalSprite.Effects;
								sprite.Enabled = originalSprite.Enabled;
								sprite.LayerDepth = originalSprite.LayerDepth;
								sprite.Origin = originalSprite.Origin;
								sprite.Position = originalSprite.Position;
								sprite.Rotation = originalSprite.Rotation;
								sprite.Scale = originalSprite.Scale;
								sprite.Visible = originalSprite.Visible;
								sprite.ShowBoundingBox = originalSprite.ShowBoundingBox;
								sprite.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																													Globals.GetDestinationTexturePath(
																														Path.GetFileName(originalSprite.AssetName)));
								sprite.ScrollSpeed = originalSprite.ScrollSpeed;
								sprite.AttachedPathingNodeName = group.AttachedPathingNodeName;

								sprite.CurrentPathNodeIndex = originalSprite.StartPathNodeIndex;
								sprite.CurrentPathLerpPosition = originalSprite.StartPathingLerpPosition;

								sprite.StartPathNodeIndex = originalSprite.StartPathNodeIndex;
								sprite.StartPathingLerpPosition = originalSprite.StartPathingLerpPosition;

								if (group.AttachedPathingNode != null)
									sprite.AttachedPathingNode = group.AttachedPathingNode;

								// Add the sprite to the frame's sprite collection
								group.Objects.Add(sprite);
							}

							#endregion

							#region Emitters

							if (o is ParticleEmitter)
							{
								ParticleEmitter originalEmitter = o as ParticleEmitter;

								EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
								emitter.Name = originalEmitter.Name;
								emitter.AssetName = originalEmitter.AssetName;
								emitter.AssetFolder = Globals.GetDestinationTexturePath(originalEmitter.AssetName);
								emitter.Position = originalEmitter.Position;
								emitter.ParticleColor = originalEmitter.ParticleColor;
								emitter.DrawOrder = originalEmitter.DrawOrder;
								emitter.EnableAlphaBlending = originalEmitter.EnableAlphaBlending;
								emitter.Enabled = originalEmitter.Enabled;
								emitter.ForceObjects = originalEmitter.ForceObjects;
								emitter.MinAcceleration = originalEmitter.MinAcceleration;
								emitter.MaxAcceleration = originalEmitter.MaxAcceleration;
								emitter.MinInitialSpeed = originalEmitter.MinInitialSpeed;
								emitter.MaxInitialSpeed = originalEmitter.MaxInitialSpeed;
								emitter.MinLifeSpan = originalEmitter.MinLifeSpan;
								emitter.MaxLifeSpan = originalEmitter.MaxLifeSpan;
								emitter.MinParticles = originalEmitter.MinParticles;
								emitter.MaxParticles = originalEmitter.MaxParticles;
								emitter.MinRotationSpeed = originalEmitter.MinRotationSpeed;
								emitter.MaxRotationSpeed = originalEmitter.MaxRotationSpeed;
								emitter.MinScale = originalEmitter.MinScale;
								emitter.MaxScale = originalEmitter.MaxScale;
								emitter.Origin = originalEmitter.Origin;
								emitter.UpdateOrder = originalEmitter.UpdateOrder;
								emitter.Visible = originalEmitter.Visible;
								emitter.ShowBoundingBox = originalEmitter.ShowBoundingBox;
								emitter.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(originalEmitter.AssetName));

								emitter.StartPathingLerpPosition = originalEmitter.CurrentPathLerpPosition;
								emitter.StartPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

								emitter.CurrentPathLerpPosition = originalEmitter.CurrentPathLerpPosition;
								emitter.CurrentPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

								if (!string.IsNullOrEmpty(originalEmitter.AttachedPathingNodeName))
									emitter.AttachedPathingNode = conRenderer.GetPath(originalEmitter.AttachedPathingNodeName);

								emitter.RespawnParticles = originalEmitter.RespawnParticles;

								// Add it to our group
								group.Objects.Add(emitter);
							}

							#endregion

							if (o is Actor)
							{
								MessageBox.Show(@"Not Yet Implemented to group Actor Objects!");
							}
						}

						// Add the actor to our key frames visual object collection
						Globals.ObjectAdded.Invoke(group);
					}

					#endregion

					#region Actors

					if (obj is Actor)
					{
						Actor proxy = obj as Actor;

						EditorActor actor = new EditorActor(Globals.Game, string.Empty, proxy.AssetName, conRenderer);
						actor.Position = proxy.Position;
						actor.Scale = proxy.Scale;
						actor.DrawOrder = proxy.DrawOrder;
						actor.UpdateOrder = proxy.UpdateOrder;
						actor.Visible = proxy.Visible;
						actor.Role = proxy.Role;
						actor.AssetName = proxy.AssetName;
						actor.SpriteSheetFileName = proxy.SpriteSheetFileName;
						actor.Name = proxy.Name;
						actor.Enabled = proxy.Enabled;
						actor.Direction = proxy.Direction;
						actor.IsCentered = proxy.IsCentered;
						actor.IsLooped = proxy.IsLooped;
						actor.ObjectTypeName = proxy.ObjectTypeName;
						actor.IsLocked = proxy.IsLocked;

						actor.ClipPlayer = new AnimationPlayer2D(Globals.Game);

						string ssFilename = Path.GetFileNameWithoutExtension(Globals.GetDestinationTexturePath(proxy.SpriteSheetFileName));
						
						actor.ClipPlayer.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(ssFilename) + ".png");

						actor.ClipPlayer.Sequences = proxy.ClipPlayer.Sequences;
						actor.ClipPlayer.IsFlipped = proxy.ClipPlayer.IsFlipped;
						actor.ClipPlayer.IsLooped = proxy.ClipPlayer.IsLooped;

						actor.CurrentPathLerpPosition = proxy.StartPathingLerpPosition;
						actor.CurrentPathNodeIndex = proxy.StartPathNodeIndex;

						actor.StartPathingLerpPosition = proxy.StartPathingLerpPosition;
						actor.StartPathNodeIndex = proxy.StartPathNodeIndex;

						actor.AttachedPathingNodeName = proxy.AttachedPathingNodeName;

						if (!string.IsNullOrEmpty(proxy.AttachedPathingNodeName))
						{
							actor.AttachedPathingNode = conRenderer.GetPath(proxy.AttachedPathingNodeName);

							if (actor.AttachedPathingNode.Nodes[actor.CurrentPathNodeIndex].AnimationSequence != null)
								actor.ClipPlayer.CurrentSequence = actor.AttachedPathingNode.Nodes[actor.CurrentPathNodeIndex].AnimationSequence;
						}

						Globals.ObjectAdded.Invoke(actor);

						// Play our first sequence out the gate
						actor.Play(proxy.ClipPlayer.Sequences[0].Name, true);
					}

					#endregion

					#region Labels

					if (obj is Label)
					{
						Label original = obj as Label;

						EditorLabel label = new EditorLabel(Globals.Game, obj.AssetName, conRenderer);
						label.Name = original.Name;
						label.AssetName = original.AssetName;
						label.FontColor = original.FontColor;
						label.DrawOrder = original.DrawOrder;
						label.UpdateOrder = original.UpdateOrder;
						label.SpriteEffects = original.SpriteEffects;
						label.Enabled = original.Enabled;
						label.LayerDepth = original.LayerDepth;
						label.Position = original.Position;
						label.Rotation = original.Rotation;
						label.Scale = original.Scale;
						label.Visible = original.Visible;
						label.ShowBoundingBox = original.ShowBoundingBox;
						label.IsLocked = original.IsLocked;
						label.IsCentered = original.IsCentered;
						label.ObjectTypeName = original.ObjectTypeName;
						label.ShadowColor = original.ShadowColor;
						label.ShadowOffset = original.ShadowOffset;
						label.IsShadowVisible = original.IsShadowVisible;
						label.Text = original.Text;

						Globals.ObjectAdded.Invoke(label);
					}

					#endregion

					#region Sprite Boxes

					if (obj is SpriteBox)
					{
						SpriteBox original = obj as SpriteBox;

						EditorSpriteBox sprite = new EditorSpriteBox(Globals.Game, conRenderer);
						sprite.Name = original.Name;
						sprite.Width = original.Width;
						sprite.Height = original.Height;
						sprite.AssetName = original.AssetName;
						sprite.BlendMode = original.BlendMode;
						sprite.Color = original.Color;
						sprite.DrawOrder = original.DrawOrder;
						sprite.UpdateOrder = original.UpdateOrder;
						sprite.Effects = original.Effects;
						sprite.Enabled = original.Enabled;
						sprite.LayerDepth = original.LayerDepth;
						sprite.Origin = original.Origin;
						sprite.Position = original.Position;
						sprite.Rotation = original.Rotation;
						sprite.Scale = original.Scale;
						sprite.Visible = original.Visible;
						sprite.ShowBoundingBox = original.ShowBoundingBox;
						sprite.IsLocked = original.IsLocked;
						sprite.ScrollSpeed = original.ScrollSpeed;
						sprite.IsCentered = original.IsCentered;
						sprite.ObjectTypeName = original.ObjectTypeName;

						sprite.CurrentPathLerpPosition = original.CurrentPathLerpPosition;
						sprite.CurrentPathNodeIndex = original.CurrentPathNodeIndex;

						sprite.StartPathingLerpPosition = original.StartPathingLerpPosition;
						sprite.StartPathNodeIndex = original.StartPathNodeIndex;

						sprite.AttachedPathingNodeName = original.AttachedPathingNodeName;

						if (!string.IsNullOrEmpty(original.AttachedPathingNodeName))
						{
							sprite.AttachedPathingNode = conRenderer.GetPath(original.AttachedPathingNodeName);
						}

						Globals.ObjectAdded.Invoke(sprite);
					}

					#endregion
				}

				#endregion

				#region Load our Sprite Container Objects

				if (proj.SpriteContainerFiles.Count > 0)
				{
					// Found sprites to load into our Sprite Container .. Load them up
					foreach (string t in proj.SpriteContainerFiles)
					{
						try
						{
							Image img = Image.FromFile(Globals.GetDestinationTexturePath(t));
							conSpriteContainer.AddSprite(img, t);
						}
						catch (FileNotFoundException)
						{
							if (
								MessageBox.Show(Path.GetFileName(t) + @" was not found! Would you like to Re-Locate this Resource?",
																@"File not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
							{
								OpenFileDialog newLocation = new OpenFileDialog();
								if (newLocation.ShowDialog() == DialogResult.OK)
								{
									Image img = Image.FromFile(newLocation.FileName);
									conSpriteContainer.AddSprite(img, newLocation.FileName);
								}
							}
						}
					}
				}

				#endregion

				// Restore our screen resolution
				if (proj.ScreenResolution == new Vector2(1920, 1080))
					Globals.AppSettings.Resolution = 0;

				if (proj.ScreenResolution == new Vector2(1280, 720))
					Globals.AppSettings.Resolution = 1;

				if (proj.ScreenResolution == new Vector2(800, 480))
					Globals.AppSettings.Resolution = 2;

				if (proj.ScreenResolution == new Vector2(480, 800))
					Globals.AppSettings.Resolution = 3;

				// Reload all our projects var's
				Globals.CurrentCameraOffsetX = proj.CameraOffsetX;
				Globals.CurrentCameraOffsetY = proj.CameraOffsetY;

				conRenderer.CameraFollowOffsetX = proj.CameraFollowOffsetX;
				tsCameraOffsetX.Text = proj.CameraFollowOffsetX.ToString();

				if (proj.CameraFollowing != null)
				{
					var vo = (from s in conRenderer.RenderWindow.VisualObjects
										where s.Name == proj.CameraFollowing
										select s).SingleOrDefault();

					if (vo != null)
					{
						Globals.ObjectSelected.Invoke(vo);
            conRenderer.FollowingObject = vo;
					}
				}
				else
				{
          conRenderer.CurrentSelectedObject = null;
					Globals.ObjectSelected.Invoke(null);
				}

				if (Globals.OnSceneEvent != null)
					Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);

				Globals.IsScenePaused = false;
				Globals.IsSceneStopped = true;

				if (result)
					MessageBox.Show(@"Successfully Loaded The C2D Project!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception er)
			{
				MessageBox.Show(er.Message);
			}
		}

		private void SaveProject(bool showResult)
		{
			Globals.IsDialogWindowOpen = true;

			// If no project is loaded prompt to create/save it
			if (!Globals.IsProjectLoaded)
			{
				SaveFileDialog dia = new SaveFileDialog();

				dia.Filter = @"Catalyst3D Project File|*.c2d";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					// Store the path\filename in our global settings
					Globals.AppSettings.ProjectPath = Path.GetDirectoryName(dia.FileName);
					Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.FileName);
				}
				else
				{
					return;
				}
			}

			if (Globals.OnSceneEvent != null)
				Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);

			if (Globals.ObjectSelected != null)
				Globals.ObjectSelected.Invoke(null);

			CatalystProject2D proj = new CatalystProject2D();

			#region Pathing Nodes

			foreach (LedgeBuilder original in Globals.Paths)
			{
				Ledge ledge = new Ledge();

				ledge.Name = original.Name;
				ledge.Role = original.Role;
				ledge.IsTraveling = original.IsTraveling;
				ledge.LedgeTavelAlgo = original.LedgeTavelAlgo;

				foreach (LedgeNodeDisplay n in original.Nodes)
				{
					LedgeNode node = new LedgeNode();
					node.Position = n.Position;
					node.Scale = n.Scale;
					node.TravelSpeed = n.TravelSpeed;
					node.DrawOrder = n.DrawOrder;
					node.Rotation = n.Rotation;
					node.AnimationSequence = n.AnimationSequence;
					node.IsLooped = n.IsLooped;
					node.SpriteEffect = n.SpriteEffect;
					ledge.Nodes.Add(node);
				}

				// Add it up
				proj.Paths.Add(ledge);
			}

			#endregion

			foreach (VisualObject t in conRenderer.RenderWindow.VisualObjects)
			{
				#region Buttons

				if (t is EditorButton)
				{
					EditorButton sceneButton = t as EditorButton;

					// Create a new Sprite Entity so we can serialize it
					Catalyst3D.XNA.Engine.EntityClasses.UI.Button button = new Catalyst3D.XNA.Engine.EntityClasses.UI.Button
					                                                       	{
					                                                       		BlendMode = sceneButton.BlendMode,
					                                                       		AssetName = Path.GetFileName(sceneButton.AssetName),
					                                                       		AssetFolder = string.Empty,
					                                                       		Color = sceneButton.Color,
					                                                       		Effects = sceneButton.Effects,
					                                                       		LayerDepth = sceneButton.LayerDepth,
					                                                       		Origin = sceneButton.Origin,
					                                                       		Position = sceneButton.Position,
					                                                       		Rotation = sceneButton.Rotation,
					                                                       		Scale = sceneButton.Scale,
					                                                       		DrawOrder = sceneButton.DrawOrder,
					                                                       		UpdateOrder = sceneButton.UpdateOrder,
					                                                       		Enabled = sceneButton.Enabled,
					                                                       		Visible = sceneButton.Visible,
					                                                       		ShowBoundingBox = false,
					                                                       		ScrollSpeed = sceneButton.ScrollSpeed,
					                                                       		IsLocked = sceneButton.IsLocked,
					                                                       		Name = sceneButton.Name,
					                                                       		IsCentered = sceneButton.IsCentered
					                                                       	};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(button);
				}

				#endregion

				#region Sprites

				if (t is EditorSprite)
				{
					EditorSprite sceneSprite = t as EditorSprite;

					// Create a new Sprite Entity so we can serialize it
					Sprite sprite = new Sprite
					                	{
					                		BlendMode = sceneSprite.BlendMode,
					                		AssetName = Path.GetFileName(sceneSprite.AssetName),
					                		AssetFolder = string.Empty,
					                		Color = sceneSprite.Color,
					                		Effects = sceneSprite.Effects,
					                		LayerDepth = sceneSprite.LayerDepth,
					                		Origin = sceneSprite.Origin,
					                		Position = sceneSprite.Position,
					                		Rotation = sceneSprite.Rotation,
					                		Scale = sceneSprite.Scale,
					                		DrawOrder = sceneSprite.DrawOrder,
					                		UpdateOrder = sceneSprite.UpdateOrder,
					                		Enabled = sceneSprite.Enabled,
					                		Visible = sceneSprite.Visible,
					                		ShowBoundingBox = false,
					                		ScrollSpeed = sceneSprite.ScrollSpeed,
					                		IsLocked = sceneSprite.IsLocked,
					                		Name = sceneSprite.Name,
					                		IsCentered = sceneSprite.IsCentered,
					                		CurrentPathNodeIndex = sceneSprite.CurrentPathNodeIndex,
					                		CurrentPathLerpPosition = sceneSprite.CurrentPathLerpPosition,
					                		AttachedPathingNodeName = sceneSprite.AttachedPathingNodeName,
					                		StartPathingLerpPosition = sceneSprite.StartPathingLerpPosition,
					                		StartPathNodeIndex = sceneSprite.StartPathNodeIndex,
					                		ObjectTypeName = sceneSprite.ObjectTypeName,
					                	};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(sprite);
				}

				#endregion

				#region Emitters

				if (t is EditorEmitter)
				{
					EditorEmitter sceneEmitter = t as EditorEmitter;

					ParticleEmitter emitter = new ParticleEmitter();
					emitter.AssetName = Path.GetFileName(sceneEmitter.AssetName);
					emitter.AssetFolder = string.Empty;
					emitter.Camera = sceneEmitter.Camera;
					emitter.DrawOrder = sceneEmitter.DrawOrder;
					emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
					emitter.Enabled = sceneEmitter.Enabled;
					emitter.ForceObjects = sceneEmitter.ForceObjects;
					emitter.MinAcceleration = sceneEmitter.MinAcceleration;
					emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
					emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
					emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
					emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
					emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
					emitter.MinParticles = sceneEmitter.MinParticles;
					emitter.MaxParticles = sceneEmitter.MaxParticles;
					emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
					emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
					emitter.MinScale = sceneEmitter.MinScale;
					emitter.MaxScale = sceneEmitter.MaxScale;
					emitter.Position = sceneEmitter.Position;
					emitter.ParticleColor = sceneEmitter.ParticleColor;
					emitter.Name = sceneEmitter.Name;
					emitter.Origin = sceneEmitter.Origin;
					emitter.UpdateOrder = sceneEmitter.UpdateOrder;
					emitter.Visible = sceneEmitter.Visible;
					emitter.ShowBoundingBox = false;
					emitter.RespawnParticles = sceneEmitter.RespawnParticles;
					emitter.AttachedPathingNodeName = sceneEmitter.AttachedPathingNodeName;
					emitter.StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition;
					emitter.StartPathNodeIndex = sceneEmitter.StartPathNodeIndex;
					emitter.ObjectTypeName = sceneEmitter.ObjectTypeName;
					emitter.IsLocked = sceneEmitter.IsLocked;

					proj.SceneObjects.Add(emitter);
				}

				#endregion

				#region Groups

				if (t is EditorGroup)
				{
					EditorGroup original = t as EditorGroup;

					VisualObjectGroup group = new VisualObjectGroup
					                          	{
					                          		AssetName = original.AssetName,
					                          		AssetFolder = string.Empty,
					                          		DrawOrder = original.DrawOrder,
					                          		UpdateOrder = original.UpdateOrder,
					                          		Enabled = original.Enabled,
					                          		Name = original.Name,
					                          		Position = original.Position,
					                          		Scale = original.Scale,
					                          		ShowBoundingBox = false,
					                          		Visible = original.Visible,
					                          		CurrentPathNodeIndex = original.CurrentPathNodeIndex,
					                          		CurrentPathLerpPosition = original.CurrentPathLerpPosition,
					                          		AttachedPathingNodeName = original.AttachedPathingNodeName,
					                          		StartPathingLerpPosition = original.StartPathingLerpPosition,
					                          		StartPathNodeIndex = original.StartPathNodeIndex,
					                          		ObjectTypeName = original.ObjectTypeName,
					                          		IsLocked = original.IsLocked
					                          	};

					foreach (VisualObject o in original.Objects)
					{
						if (o is EditorSprite)
						{
							EditorSprite sceneSprite = o as EditorSprite;

							// Init our new Sprite
							Sprite sprite = new Sprite
							                	{
							                		BlendMode = sceneSprite.BlendMode,
							                		AssetName = Path.GetFileName(sceneSprite.AssetName),
							                		AssetFolder = string.Empty,
							                		Color = sceneSprite.Color,
							                		Effects = sceneSprite.Effects,
							                		LayerDepth = sceneSprite.LayerDepth,
							                		Origin = sceneSprite.Origin,
							                		Position = sceneSprite.Position,
							                		Rotation = sceneSprite.Rotation,
							                		Scale = sceneSprite.Scale,
							                		DrawOrder = sceneSprite.DrawOrder,
							                		UpdateOrder = sceneSprite.UpdateOrder,
							                		Enabled = sceneSprite.Enabled,
							                		Visible = sceneSprite.Visible,
							                		ShowBoundingBox = false,
							                		ScrollSpeed = sceneSprite.ScrollSpeed,
							                		IsLocked = sceneSprite.IsLocked,
							                		Name = sceneSprite.Name,
							                		CurrentPathNodeIndex = original.CurrentPathNodeIndex,
							                		CurrentPathLerpPosition = original.CurrentPathLerpPosition,
							                		AttachedPathingNodeName = original.AttachedPathingNodeName,
							                		StartPathingLerpPosition = original.StartPathingLerpPosition,
							                		StartPathNodeIndex = original.StartPathNodeIndex
							                	};

							// Add it to our groups objects
							group.Objects.Add(sprite);
						}

						if (o is EditorEmitter)
						{
							EditorEmitter sceneEmitter = o as EditorEmitter;

							ParticleEmitter emitter = new ParticleEmitter
							                          	{
							                          		AssetName = Path.GetFileName(sceneEmitter.AssetName),
							                          		AssetFolder = string.Empty,
							                          		Camera = sceneEmitter.Camera,
							                          		DrawOrder = sceneEmitter.DrawOrder,
							                          		EnableAlphaBlending = sceneEmitter.EnableAlphaBlending,
							                          		Enabled = sceneEmitter.Enabled,
							                          		ForceObjects = sceneEmitter.ForceObjects,
							                          		MinAcceleration = sceneEmitter.MinAcceleration,
							                          		MaxAcceleration = sceneEmitter.MaxAcceleration,
							                          		MinInitialSpeed = sceneEmitter.MinInitialSpeed,
							                          		MaxInitialSpeed = sceneEmitter.MaxInitialSpeed,
							                          		MinLifeSpan = sceneEmitter.MinLifeSpan,
							                          		MaxLifeSpan = sceneEmitter.MaxLifeSpan,
							                          		MinParticles = sceneEmitter.MinParticles,
							                          		MaxParticles = sceneEmitter.MaxParticles,
							                          		MinRotationSpeed = sceneEmitter.MinRotationSpeed,
							                          		MaxRotationSpeed = sceneEmitter.MaxRotationSpeed,
							                          		MinScale = sceneEmitter.MinScale,
							                          		MaxScale = sceneEmitter.MaxScale,
							                          		Position = sceneEmitter.Position,
							                          		ParticleColor = sceneEmitter.ParticleColor,
							                          		Name = sceneEmitter.Name,
							                          		Origin = sceneEmitter.Origin,
							                          		UpdateOrder = sceneEmitter.UpdateOrder,
							                          		Visible = sceneEmitter.Visible,
							                          		ShowBoundingBox = false,
							                          		RespawnParticles = sceneEmitter.RespawnParticles,
							                          		StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition,
							                          		StartPathNodeIndex = sceneEmitter.StartPathNodeIndex
							                          	};

							// Add it to our group
							group.Objects.Add(emitter);
						}
					}

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(group);
				}


				#endregion

				#region Actors

				if (t is EditorActor)
				{
					EditorActor original = t as EditorActor;

					Actor actor = new Actor
					              	{
					              		AssetName = Path.GetFileName(original.AssetName),
					              		AssetFolder = string.Empty,
					              		SpriteSheetFileName = Path.GetFileName(original.SpriteSheetFileName),
					              		Direction = original.Direction,
					              		DrawOrder = original.DrawOrder,
					              		UpdateOrder = original.UpdateOrder,
					              		Enabled = original.Enabled,
					              		Name = original.Name,
					              		Visible = original.Visible,
					              		Position = original.Position,
					              		Role = original.Role,
					              		Scale = original.Scale,
					              		IsCentered = original.IsCentered,
					              		AttachedPathingNodeName = original.AttachedPathingNodeName,
					              		CurrentPathLerpPosition = original.CurrentPathLerpPosition,
					              		CurrentPathNodeIndex = original.CurrentPathNodeIndex,
					              		ObjectTypeName = original.ObjectTypeName,
					              		ShowBoundingBox = false,
					              		IsLocked = original.IsLocked
					              	};

					actor.ClipPlayer = new AnimationPlayer2D(Globals.Game)
					                   	{
					                   		Sequences = original.ClipPlayer.Sequences,
					                   		IsFlipped = original.ClipPlayer.IsFlipped,
					                   		IsLooped = original.ClipPlayer.IsLooped,
					                   		ShowBoundingBox = false
					                   	};

					proj.SceneObjects.Add(actor);
				}

				#endregion

				#region Labels

				if (t is EditorLabel)
				{
					EditorLabel original = t as EditorLabel;

					Label label = new Label
					              	{
					              		Name = original.Name,
					              		AssetName = original.AssetName,
					              		AssetFolder = string.Empty,
					              		FontColor = original.FontColor,
					              		DrawOrder = original.DrawOrder,
					              		UpdateOrder = original.UpdateOrder,
					              		SpriteEffects = original.SpriteEffects,
					              		Enabled = original.Enabled,
					              		LayerDepth = original.LayerDepth,
					              		Position = original.Position,
					              		Rotation = original.Rotation,
					              		Scale = original.Scale,
					              		Visible = original.Visible,
					              		ShowBoundingBox = original.ShowBoundingBox,
					              		IsLocked = original.IsLocked,
					              		IsCentered = original.IsCentered,
					              		ObjectTypeName = original.ObjectTypeName,
					              		ShadowColor = original.ShadowColor,
					              		ShadowOffset = original.ShadowOffset,
					              		IsShadowVisible = original.IsShadowVisible,
					              		Text = original.Text
					              	};

					proj.SceneObjects.Add(label);
				}

				#endregion

				#region Sprite Boxes

				if (t is EditorSpriteBox)
				{
					EditorSpriteBox sceneSprite = t as EditorSpriteBox;

					// Create a new Sprite Entity so we can serialize it
					SpriteBox sprite = new SpriteBox
					                   	{
					                   		BlendMode = sceneSprite.BlendMode,
					                   		AssetName = Path.GetFileName(sceneSprite.AssetName),
					                   		AssetFolder = string.Empty,
					                   		Color = sceneSprite.Color,
					                   		Width = sceneSprite.Width,
					                   		Height = sceneSprite.Height,
					                   		Effects = sceneSprite.Effects,
					                   		LayerDepth = sceneSprite.LayerDepth,
					                   		Origin = sceneSprite.Origin,
					                   		Position = sceneSprite.Position,
					                   		Rotation = sceneSprite.Rotation,
					                   		Scale = sceneSprite.Scale,
					                   		DrawOrder = sceneSprite.DrawOrder,
					                   		UpdateOrder = sceneSprite.UpdateOrder,
					                   		Enabled = sceneSprite.Enabled,
					                   		Visible = sceneSprite.Visible,
					                   		ShowBoundingBox = false,
					                   		ScrollSpeed = sceneSprite.ScrollSpeed,
					                   		IsLocked = sceneSprite.IsLocked,
					                   		Name = sceneSprite.Name,
					                   		IsCentered = sceneSprite.IsCentered,
					                   		CurrentPathNodeIndex = sceneSprite.CurrentPathNodeIndex,
					                   		CurrentPathLerpPosition = sceneSprite.CurrentPathLerpPosition,
					                   		AttachedPathingNodeName = sceneSprite.AttachedPathingNodeName,
					                   		StartPathingLerpPosition = sceneSprite.StartPathingLerpPosition,
					                   		StartPathNodeIndex = sceneSprite.StartPathNodeIndex,
					                   		ObjectTypeName = sceneSprite.ObjectTypeName,
					                   	};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(sprite);
				}

				#endregion
			}

			#region Sprite Container's Objects

			for (int i = 0; i < conSpriteContainer.pnContainer.Controls.Count; i++)
			{
				conSprite con = conSpriteContainer.pnContainer.Controls[i] as conSprite;
				if (con != null)
				{
					// Store the filename of the asset for loading later
					proj.SpriteContainerFiles.Add(Path.GetFileName(con.FileName));
				}
			}

			#endregion

			// Save the screen resolution in the project
			switch (Globals.AppSettings.Resolution)
			{
				case 0:
					proj.ScreenResolution = new Vector2(1920, 1080);
					break;
				case 1:
					proj.ScreenResolution = new Vector2(1280, 720);
					break;
				case 2:
					proj.ScreenResolution = new Vector2(800, 480);
					break;
				case 3:
					proj.ScreenResolution = new Vector2(480, 800);
					break;
			}

			float camFoX;
			if (float.TryParse(tsCameraOffsetX.Text, out camFoX))
				proj.CameraFollowOffsetX = camFoX;

			proj.CameraOffsetX = Globals.CurrentCameraOffsetX;
			proj.CameraOffsetY = Globals.CurrentCameraOffsetY;

      if (conRenderer.FollowingObject != null)
				proj.CameraFollowing = conRenderer.FollowingObjectName;

			// Save the project to disk
			Serializer.Serialize(Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename, proj);

			// Flag that a project is loaded as well
			Globals.IsProjectLoaded = true;

			Globals.IsDialogWindowOpen = false;

			if (showResult)
				MessageBox.Show(@"Successfully Saved the Project!", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void SaveProjectAs()
		{
			if (Globals.OnSceneEvent != null)
				Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);

			Globals.ObjectSelected.Invoke(null);

			Globals.IsDialogWindowOpen = true;

			string originalPath = Globals.AppSettings.ProjectPath;

			// Init our save file dialog
			SaveFileDialog dia = new SaveFileDialog();

			dia.Filter = @"Catalyst3D Project File|*.c2d";
			dia.InitialDirectory = Globals.AppSettings.ProjectPath;

			if (dia.ShowDialog() == DialogResult.OK)
			{
				// Store the path\filename in our global settings
				Globals.AppSettings.ProjectPath = Path.GetDirectoryName(dia.FileName);
				Globals.AppSettings.ProjectFilename = Path.GetFileName(dia.FileName);

				if (!Directory.Exists(Globals.AppSettings.ProjectPath + @"\Textures"))
					Directory.CreateDirectory(Globals.AppSettings.ProjectPath + @"\Textures");
			}
			else
			{
				return;
			}

			CatalystProject2D proj = new CatalystProject2D();

			#region Pathing Nodes

			foreach (LedgeBuilder original in Globals.Paths)
			{
				Ledge ledge = new Ledge();

				ledge.Name = original.Name;
				ledge.Role = original.Role;
				ledge.IsTraveling = original.IsTraveling;
				ledge.LedgeTavelAlgo = original.LedgeTavelAlgo;

				foreach (LedgeNodeDisplay n in original.Nodes)
				{
					LedgeNode node = new LedgeNode();
					node.Position = n.Position;
					node.Scale = n.Scale;
					node.TravelSpeed = n.TravelSpeed;
					node.DrawOrder = n.DrawOrder;
					node.Rotation = n.Rotation;
					node.AnimationSequence = n.AnimationSequence;
					node.IsLooped = n.IsLooped;

					ledge.Nodes.Add(node);
				}

				// Add it up
				proj.Paths.Add(ledge);
			}

			#endregion

      foreach (VisualObject t in conRenderer.RenderWindow.VisualObjects)
			{
				#region Buttons

				if (t is EditorButton)
				{
					EditorButton sceneButton = t as EditorButton;

					if (!File.Exists(Globals.GetDestinationTexturePath(sceneButton.AssetName)))
					{
						// Copy our other asset to this projects texture directory
						string texPath = originalPath + @"\Textures\" + sceneButton.AssetName;
						File.Copy(texPath, Globals.GetDestinationTexturePath(sceneButton.AssetName), true);
					}

					// Create a new Sprite Entity so we can serialize it
					Catalyst3D.XNA.Engine.EntityClasses.UI.Button button = new Catalyst3D.XNA.Engine.EntityClasses.UI.Button
																																	{
																																		BlendMode = sceneButton.BlendMode,
																																		AssetName = Path.GetFileName(sceneButton.AssetName),
																																		Color = sceneButton.Color,
																																		Effects = sceneButton.Effects,
																																		LayerDepth = sceneButton.LayerDepth,
																																		Origin = sceneButton.Origin,
																																		Position = sceneButton.Position,
																																		Rotation = sceneButton.Rotation,
																																		Scale = sceneButton.Scale,
																																		DrawOrder = sceneButton.DrawOrder,
																																		UpdateOrder = sceneButton.UpdateOrder,
																																		Enabled = sceneButton.Enabled,
																																		Visible = sceneButton.Visible,
																																		ShowBoundingBox = sceneButton.ShowBoundingBox,
																																		ScrollSpeed = sceneButton.ScrollSpeed,
																																		IsLocked = sceneButton.IsLocked,
																																		Name = sceneButton.Name,
																																		IsCentered = sceneButton.IsCentered
																																	};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(button);
				}

				#endregion

				#region Sprites

				if (t is EditorSprite)
				{
					EditorSprite sceneSprite = t as EditorSprite;

					if (!File.Exists(Globals.GetDestinationTexturePath(sceneSprite.AssetName)))
					{
						// Copy our other asset to this projects texture directory
						string texPath = originalPath + @"\Textures\" + sceneSprite.AssetName;
						File.Copy(texPath, Globals.GetDestinationTexturePath(sceneSprite.AssetName), true);
					}

					// Create a new Sprite Entity so we can serialize it
					Sprite sprite = new Sprite
														{
															BlendMode = sceneSprite.BlendMode,
															AssetName = Path.GetFileName(sceneSprite.AssetName),
															Color = sceneSprite.Color,
															Effects = sceneSprite.Effects,
															LayerDepth = sceneSprite.LayerDepth,
															Origin = sceneSprite.Origin,
															Position = sceneSprite.Position,
															Rotation = sceneSprite.Rotation,
															Scale = sceneSprite.Scale,
															DrawOrder = sceneSprite.DrawOrder,
															UpdateOrder = sceneSprite.UpdateOrder,
															Enabled = sceneSprite.Enabled,
															Visible = sceneSprite.Visible,
															ShowBoundingBox = sceneSprite.ShowBoundingBox,
															ScrollSpeed = sceneSprite.ScrollSpeed,
															IsLocked = sceneSprite.IsLocked,
															Name = sceneSprite.Name,
															IsCentered = sceneSprite.IsCentered,
															CurrentPathNodeIndex = sceneSprite.CurrentPathNodeIndex,
															CurrentPathLerpPosition = sceneSprite.CurrentPathLerpPosition,
															AttachedPathingNodeName = sceneSprite.AttachedPathingNodeName,
															StartPathingLerpPosition = sceneSprite.StartPathingLerpPosition,
															StartPathNodeIndex = sceneSprite.StartPathNodeIndex,
															ObjectTypeName = sceneSprite.ObjectTypeName
														};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(sprite);
				}


				#endregion

				#region Emitters

				if (t is EditorEmitter)
				{
					EditorEmitter sceneEmitter = t as EditorEmitter;

					if (!File.Exists(Globals.GetDestinationTexturePath(sceneEmitter.AssetName)))
					{
						// Copy our other asset to this projects texture directory
						string texPath = originalPath + @"\Textures\" + sceneEmitter.AssetName;
						File.Copy(texPath, Globals.GetDestinationTexturePath(sceneEmitter.AssetName), true);
					}

					ParticleEmitter emitter = new ParticleEmitter();
					emitter.AssetName = Path.GetFileName(sceneEmitter.AssetName);
					emitter.Camera = sceneEmitter.Camera;
					emitter.DrawOrder = sceneEmitter.DrawOrder;
					emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
					emitter.Enabled = sceneEmitter.Enabled;
					emitter.ForceObjects = sceneEmitter.ForceObjects;
					emitter.MinAcceleration = sceneEmitter.MinAcceleration;
					emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
					emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
					emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
					emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
					emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
					emitter.MinParticles = sceneEmitter.MinParticles;
					emitter.MaxParticles = sceneEmitter.MaxParticles;
					emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
					emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
					emitter.MinScale = sceneEmitter.MinScale;
					emitter.MaxScale = sceneEmitter.MaxScale;
					emitter.Position = sceneEmitter.Position;
					emitter.ParticleColor = sceneEmitter.ParticleColor;
					emitter.Name = sceneEmitter.Name;
					emitter.Origin = sceneEmitter.Origin;
					emitter.UpdateOrder = sceneEmitter.UpdateOrder;
					emitter.Visible = sceneEmitter.Visible;
					emitter.ShowBoundingBox = sceneEmitter.ShowBoundingBox;
					emitter.RespawnParticles = sceneEmitter.RespawnParticles;
					emitter.AttachedPathingNodeName = sceneEmitter.AttachedPathingNodeName;
					emitter.StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition;
					emitter.StartPathNodeIndex = sceneEmitter.StartPathNodeIndex;
					emitter.ObjectTypeName = sceneEmitter.ObjectTypeName;

					proj.SceneObjects.Add(emitter);
				}

				#endregion

				#region Groups

				if (t is EditorGroup)
				{
					EditorGroup original = t as EditorGroup;

					VisualObjectGroup group = new VisualObjectGroup
																			{
																				AssetName = original.AssetName,
																				DrawOrder = original.DrawOrder,
																				UpdateOrder = original.UpdateOrder,
																				Enabled = original.Enabled,
																				Name = original.Name,
																				Position = original.Position,
																				Scale = original.Scale,
																				ShowBoundingBox = original.ShowBoundingBox,
																				Visible = original.Visible,
																				CurrentPathNodeIndex = original.CurrentPathNodeIndex,
																				CurrentPathLerpPosition = original.CurrentPathLerpPosition,
																				AttachedPathingNodeName = original.AttachedPathingNodeName,
																				StartPathingLerpPosition = original.StartPathingLerpPosition,
																				StartPathNodeIndex = original.StartPathNodeIndex,
																				ObjectTypeName = original.ObjectTypeName
																			};

					foreach (VisualObject o in original.Objects)
					{
						#region Sprites

						if (o is EditorSprite)
						{
							EditorSprite sceneSprite = o as EditorSprite;

							if (!File.Exists(Globals.GetDestinationTexturePath(sceneSprite.AssetName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = originalPath + @"\Textures\" + sceneSprite.AssetName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(sceneSprite.AssetName), true);
							}

							// Init our new Sprite
							Sprite sprite = new Sprite
																{
																	BlendMode = sceneSprite.BlendMode,
																	AssetName = Path.GetFileName(sceneSprite.AssetName),
																	Color = sceneSprite.Color,
																	Effects = sceneSprite.Effects,
																	LayerDepth = sceneSprite.LayerDepth,
																	Origin = sceneSprite.Origin,
																	Position = sceneSprite.Position,
																	Rotation = sceneSprite.Rotation,
																	Scale = sceneSprite.Scale,
																	DrawOrder = sceneSprite.DrawOrder,
																	UpdateOrder = sceneSprite.UpdateOrder,
																	Enabled = sceneSprite.Enabled,
																	Visible = sceneSprite.Visible,
																	ShowBoundingBox = sceneSprite.ShowBoundingBox,
																	ScrollSpeed = sceneSprite.ScrollSpeed,
																	IsLocked = sceneSprite.IsLocked,
																	Name = sceneSprite.Name,
																	CurrentPathNodeIndex = original.CurrentPathNodeIndex,
																	CurrentPathLerpPosition = original.CurrentPathLerpPosition,
																	AttachedPathingNodeName = original.AttachedPathingNodeName,
																	StartPathingLerpPosition = original.StartPathingLerpPosition,
																	StartPathNodeIndex = original.StartPathNodeIndex
																};

							// Add it to our groups objects
							group.Objects.Add(sprite);
						}

						#endregion

						#region Emitters

						if (o is EditorEmitter)
						{
							EditorEmitter sceneEmitter = o as EditorEmitter;

							if (!File.Exists(Globals.GetDestinationTexturePath(sceneEmitter.AssetName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = originalPath + @"\Textures\" + sceneEmitter.AssetName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(sceneEmitter.AssetName), true);
							}

							ParticleEmitter emitter = new ParticleEmitter
																					{
																						AssetName = Path.GetFileName(sceneEmitter.AssetName),
																						Camera = sceneEmitter.Camera,
																						DrawOrder = sceneEmitter.DrawOrder,
																						EnableAlphaBlending = sceneEmitter.EnableAlphaBlending,
																						Enabled = sceneEmitter.Enabled,
																						ForceObjects = sceneEmitter.ForceObjects,
																						MinAcceleration = sceneEmitter.MinAcceleration,
																						MaxAcceleration = sceneEmitter.MaxAcceleration,
																						MinInitialSpeed = sceneEmitter.MinInitialSpeed,
																						MaxInitialSpeed = sceneEmitter.MaxInitialSpeed,
																						MinLifeSpan = sceneEmitter.MinLifeSpan,
																						MaxLifeSpan = sceneEmitter.MaxLifeSpan,
																						MinParticles = sceneEmitter.MinParticles,
																						MaxParticles = sceneEmitter.MaxParticles,
																						MinRotationSpeed = sceneEmitter.MinRotationSpeed,
																						MaxRotationSpeed = sceneEmitter.MaxRotationSpeed,
																						MinScale = sceneEmitter.MinScale,
																						MaxScale = sceneEmitter.MaxScale,
																						Position = sceneEmitter.Position,
																						ParticleColor = sceneEmitter.ParticleColor,
																						Name = sceneEmitter.Name,
																						Origin = sceneEmitter.Origin,
																						UpdateOrder = sceneEmitter.UpdateOrder,
																						Visible = sceneEmitter.Visible,
																						ShowBoundingBox = sceneEmitter.ShowBoundingBox,
																						RespawnParticles = sceneEmitter.RespawnParticles,
																						StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition,
																						StartPathNodeIndex = sceneEmitter.StartPathNodeIndex
																					};

							// Add it to our group
							group.Objects.Add(emitter);
						}

						#endregion
					}

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(group);
				}

				#endregion

				#region Actors

				if (t is EditorActor)
				{
					EditorActor original = t as EditorActor;

					if (!File.Exists(Globals.GetDestinationTexturePath(original.SpriteSheetFileName)))
					{
						// Copy our other asset to this projects texture directory
						string texPath = originalPath + @"\Textures\" + original.SpriteSheetFileName;
						File.Copy(texPath, Globals.GetDestinationTexturePath(original.SpriteSheetFileName), true);
					}

					Actor actor = new Actor
													{
														AssetName = Path.GetFileName(original.AssetName),
														SpriteSheetFileName = original.SpriteSheetFileName,
														Direction = original.Direction,
														DrawOrder = original.DrawOrder,
														UpdateOrder = original.UpdateOrder,
														Enabled = original.Enabled,
														Name = original.Name,
														Visible = original.Visible,
														Position = original.Position,
														Role = original.Role,
														Scale = original.Scale,
														IsCentered = original.IsCentered,
														AttachedPathingNodeName = original.AttachedPathingNodeName,
														CurrentPathLerpPosition = original.CurrentPathLerpPosition,
														CurrentPathNodeIndex = original.CurrentPathNodeIndex,
														ObjectTypeName = original.ObjectTypeName
													};

					actor.ClipPlayer = new AnimationPlayer2D(Globals.Game)
															{
																Sequences = original.ClipPlayer.Sequences,
																IsFlipped = original.ClipPlayer.IsFlipped,
																IsLooped = original.ClipPlayer.IsLooped
															};

					proj.SceneObjects.Add(actor);
				}

				#endregion

				#region Labels

				if (t is EditorLabel)
				{
					EditorLabel original = t as EditorLabel;

					Label label = new Label
					{
						Name = original.Name,
						AssetName = original.AssetName,
						FontColor = original.FontColor,
						DrawOrder = original.DrawOrder,
						UpdateOrder = original.UpdateOrder,
						SpriteEffects = original.SpriteEffects,
						Enabled = original.Enabled,
						LayerDepth = original.LayerDepth,
						Position = original.Position,
						Rotation = original.Rotation,
						Scale = original.Scale,
						Visible = original.Visible,
						ShowBoundingBox = original.ShowBoundingBox,
						IsLocked = original.IsLocked,
						IsCentered = original.IsCentered,
						ObjectTypeName = original.ObjectTypeName,
						ShadowColor = original.ShadowColor,
						ShadowOffset = original.ShadowOffset,
						IsShadowVisible = original.IsShadowVisible,
						Text = original.Text
					};

					proj.SceneObjects.Add(label);
				}

				#endregion

				#region Sprite Boxes

				if (t is EditorSpriteBox)
				{
					EditorSpriteBox sceneSprite = t as EditorSpriteBox;

					// Create a new Sprite Entity so we can serialize it
					SpriteBox sprite = new SpriteBox
					{
						BlendMode = sceneSprite.BlendMode,
						AssetName = Path.GetFileName(sceneSprite.AssetName),
						Color = sceneSprite.Color,
						Width = sceneSprite.Width,
						Height = sceneSprite.Height,
						Effects = sceneSprite.Effects,
						LayerDepth = sceneSprite.LayerDepth,
						Origin = sceneSprite.Origin,
						Position = sceneSprite.Position,
						Rotation = sceneSprite.Rotation,
						Scale = sceneSprite.Scale,
						DrawOrder = sceneSprite.DrawOrder,
						UpdateOrder = sceneSprite.UpdateOrder,
						Enabled = sceneSprite.Enabled,
						Visible = sceneSprite.Visible,
						ShowBoundingBox = false,
						ScrollSpeed = sceneSprite.ScrollSpeed,
						IsLocked = sceneSprite.IsLocked,
						Name = sceneSprite.Name,
						IsCentered = sceneSprite.IsCentered,
						CurrentPathNodeIndex = sceneSprite.CurrentPathNodeIndex,
						CurrentPathLerpPosition = sceneSprite.CurrentPathLerpPosition,
						AttachedPathingNodeName = sceneSprite.AttachedPathingNodeName,
						StartPathingLerpPosition = sceneSprite.StartPathingLerpPosition,
						StartPathNodeIndex = sceneSprite.StartPathNodeIndex,
						ObjectTypeName = sceneSprite.ObjectTypeName,
					};

					// Add it to our new Serializable Key Frame's Sprite Collection
					proj.SceneObjects.Add(sprite);
				}

				#endregion
			}

			#region Sprite Container's Objects

			for (int i = 0; i < conSpriteContainer.pnContainer.Controls.Count; i++)
			{
				conSprite con = conSpriteContainer.pnContainer.Controls[i] as conSprite;
				if (con != null)
				{
					if (!File.Exists(Globals.GetDestinationTexturePath(con.FileName)))
					{
						// Copy our other asset to this projects texture directory
						string texPath = originalPath + @"\Textures\" + con.FileName;
						File.Copy(texPath, Globals.GetDestinationTexturePath(con.FileName), true);
					}

					// Store the filename of the asset for loading later
					proj.SpriteContainerFiles.Add(Path.GetFileName(con.FileName));
				}
			}

			#endregion

			// Save the screen resolution in the project
			switch (Globals.AppSettings.Resolution)
			{
				case 0:
					proj.ScreenResolution = new Vector2(1920, 1080);
					break;
				case 1:
					proj.ScreenResolution = new Vector2(1280, 720);
					break;
				case 2:
					proj.ScreenResolution = new Vector2(800, 480);
					break;
				case 3:
					proj.ScreenResolution = new Vector2(480, 800);
					break;
			}

			float camFoX;
			if (float.TryParse(tsCameraOffsetX.Text, out camFoX))
				proj.CameraFollowOffsetX = camFoX;

			proj.CameraOffsetX = Globals.CurrentCameraOffsetX;
			proj.CameraOffsetY = Globals.CurrentCameraOffsetY;

      if (conRenderer.FollowingObject != null)
				proj.CameraFollowing = conRenderer.FollowingObjectName;

			// Save the project to disk
			Serializer.Serialize(Globals.AppSettings.ProjectPath + @"\" + Globals.AppSettings.ProjectFilename, proj);

			// Flag that a project is loaded as well
			Globals.IsProjectLoaded = true;

			Globals.IsDialogWindowOpen = false;

			MessageBox.Show(@"Successfully Saved the Project!", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void pgGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
		{
			if (e.NewSelection.Value != null && e.OldSelection != null)
			{
				VisualObject v = e.NewSelection.Value as VisualObject;

				if (v != null)
          conRenderer.CurrentSelectedObject = v;
				else
				{
					if(e.NewSelection.Parent != null)
					{
						VisualObject p = e.NewSelection.Parent.Value as VisualObject;

						if (p != null)
							conRenderer.CurrentSelectedObject = p;

					}
				}
			}
		}

		private void conContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Globals.WindowSizeChanged.Invoke(sender, e);
		}

		private void tsExit_Click(object sender, EventArgs e)
		{
			Globals.Game.Exit();
		}

		private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
				return;

			if (MessageBox.Show(@"Are you sure you want to save changes?", @"Save Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				if (!string.IsNullOrEmpty(Globals.AppSettings.ProjectPath))
				{
					Globals.SaveProject.Invoke(true);
				}
				else
				{
					MessageBox.Show(@"Could not save project due to no Project Path Set!");
				}
			}
		}

		private void tsLoadProject_Click(object sender, EventArgs e)
		{
			//Globals.LoadProject.Invoke(false);
			LoadProject(true);
		}

		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
      if (conRenderer.RenderWindow.VisualObjects.Count > 0)
			{
				if (MessageBox.Show(@"Save Current Project?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					SaveProject(true);
				}
			}

			Globals.Paths.Clear();

			conSpriteContainer.pnContainer.Controls.Clear();
			conSpriteContainer.SelectedSpriteIndex = 0;

			// Create our Folder Browse
			FolderBrowserDialog dia = new FolderBrowserDialog();
			dia.SelectedPath = Globals.AppSettings.ProjectPath;
			dia.ShowNewFolderButton = true;
			dia.RootFolder = Environment.SpecialFolder.DesktopDirectory;

			if (dia.ShowDialog() == DialogResult.OK)
			{
				// Create our fresh collection for holding scene objects
        conRenderer.RenderWindow.VisualObjects = new List<VisualObject>();

				Globals.AppSettings.ProjectPath = dia.SelectedPath;
				Directory.CreateDirectory(Globals.AppSettings.ProjectPath + @"\Textures");

				// Call Save Project right out the gave so it creates our .c3d file and textures directory
				SaveProject(false);
			}

			Globals.IsProjectLoaded = true;

			// Flush the Scene Objects Colleciton
      conRenderer.RenderWindow.VisualObjects = new List<VisualObject>();

			// Flush the Property Grid
			pgGrid.SelectedObject = null;

		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmOptions options = new frmOptions();
			options.Show();
		}

		private void tsAddEmitter_Click(object sender, EventArgs e)
		{
			AddEmitter();
		}
		private void addEmitterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddEmitter();
		}

		private void tsGroupObjects_Click(object sender, EventArgs e)
		{
      frmGroupObjects group = new frmGroupObjects(conRenderer.RenderWindow.VisualObjects, conRenderer);
			group.Tag = Globals.ObjectsGroupped;
			group.Show();
		}
		private void groupObjectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
      frmGroupObjects group = new frmGroupObjects(conRenderer.RenderWindow.VisualObjects, conRenderer);
			group.Tag = Globals.ObjectsGroupped;
			group.Show();
		}

		private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
				return;

			if (
				MessageBox.Show(@"Are you sure you want to close this project?", @"Close Project Confirmation",
				                MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				Globals.IsProjectLoaded = false;

				if (Globals.OnSceneEvent != null)
					Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);

				Globals.CurrentCameraOffsetX = 50;
				Globals.CurrentCameraOffsetY = 50;

				conRenderer.CurrentlySelectedObjects = new List<VisualObject>();

				conRenderer.FollowingObjectName = string.Empty;
				conRenderer.FollowingObject = null;

				// Rebuild our rendering window
				foreach (var p in Globals.Paths)
					p.UnloadContent();

				Globals.Paths.Clear();

				// Flush all our containers
				conSpriteContainer.pnContainer.Controls.Clear();
				conSpriteContainer.SelectedSpriteIndex = 0;

				Globals.AppSettings.ProjectPath = string.Empty;
				Globals.AppSettings.ProjectFilename = string.Empty;

				// Flush the scene objects
				foreach (var v in conRenderer.RenderWindow.VisualObjects)
					v.Dispose();

				conRenderer.RenderWindow.VisualObjects.Clear();

				Globals.ObjectSelected.Invoke(null);

				Globals.WindowSizeChanged.Invoke(null, null);
			}
		}

		private void btnDuplicateSpecial_Click(object sender, EventArgs e)
		{
      conRenderer.CurrentSelectedObject = null;
			Globals.ObjectSelected.Invoke(null);

      frmDuplicateSpecial frm = new frmDuplicateSpecial(conRenderer.RenderWindow.VisualObjects, conRenderer);
			frm.Show();
		}
		private void duplicateSpecialToolStripMenuItem_Click(object sender, EventArgs e)
		{
      conRenderer.CurrentSelectedObject = null;
			Globals.ObjectSelected.Invoke(null);

		  frmDuplicateSpecial frm = new frmDuplicateSpecial(conRenderer.RenderWindow.VisualObjects, conRenderer);
			frm.Show();
		}

		private void tsDeselectAll_Click(object sender, EventArgs e)
		{
      conRenderer.CurrentSelectedObject = null;
			Globals.ObjectSelected.Invoke(null);
		}

		private void tsCreatePath_Click(object sender, EventArgs e)
		{
			Globals.ObjectSelected.Invoke(null);

			if (Globals.IsPathingToolVisible)
			{
				tsCreatePath.Text = @"Show Pathing Tool";
				Globals.IsPathingToolVisible = false;
			}
			else
			{
				tsCreatePath.Text = @"Hide Pathing Tool";
				Globals.IsPathingToolVisible = true;
			}
		}
		private void tsSavePath_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
				return;

			tsCreatePath.Text = @"Show Pathing Tool";
			Globals.IsPathingToolVisible = false;

			Globals.OnSaveCurrentPath.Invoke();
		}

		private void tsCancelPath_Click(object sender, EventArgs e)
		{
			Globals.ObjectSelected.Invoke(null);
			tsCreatePath.Text = @"Show Pathing Tool";

			Globals.OnCanceledCurrentPath.Invoke();

			Globals.IsPathingToolVisible = false;
		}

		private void tsResetCamera_Click(object sender, EventArgs e)
		{
			Globals.CurrentCameraOffsetX = 0;
			Globals.CurrentCameraOffsetY = 0;
		}

		private void AddEmitter()
		{
			// Create our Open File Dialog
			OpenFileDialog dia = new OpenFileDialog();
			dia.Filter = @"PNG|*.png|DDS|*.dds|JPG|*.jpg";
			dia.InitialDirectory = Globals.AppSettings.ProjectPath;

			if (dia.ShowDialog() == DialogResult.OK)
			{
				try
				{
					// Move the texture to our project -> textures directory
					if (!File.Exists(Globals.GetDestinationTexturePath(dia.FileName)))
						File.Copy(dia.FileName, Globals.GetDestinationTexturePath(dia.FileName), true);

					// Particle Emitter
					EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
					emitter.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice, Globals.GetDestinationTexturePath(dia.FileName));
					emitter.AssetName = Path.GetFileName(dia.FileName);
					emitter.Name = Path.GetFileName(dia.FileName);
					emitter.ShowBoundingBox = true;

					emitter.Initialize();

					// Add it to our scene
					Globals.ObjectAdded.Invoke(emitter);

					Globals.ObjectSelected.Invoke(emitter);
				}
				catch (Exception er)
				{
					MessageBox.Show(er.Message);
				}
			}
		}

		private void tsAddModel_Click(object sender, EventArgs e)
		{
			if (Globals.IsProjectLoaded)
			{
				// Create our Open File Dialog
				OpenFileDialog dia = new OpenFileDialog();

				dia.Filter = @"FBX|*.fbx";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					try
					{
						string output = Globals.AppSettings.ProjectPath;

            EditorModel model = new EditorModel(Globals.Game, conRenderer.Camera);
						model.Initialize();

						if (File.Exists(output))
							File.Delete(output);

						// Create our Builder
						ContentBuilder builder = new ContentBuilder(output);

						// Add our Build Files
						builder.Add(dia.FileName, Path.GetFileNameWithoutExtension(dia.FileName), null, "ModelProcessor");

						// Build our content
						string result = builder.Build();

						// Display Results
						if (string.IsNullOrEmpty(result))
						{
							// Create a new content manager just for loading this baby up
							ContentManager content = new ContentManager(Globals.Game.Services, Globals.AppSettings.ProjectPath);

							// Load it up on our 3d model
							model.Content = content.Load<Model>(Path.GetFileNameWithoutExtension(dia.FileName));

							// Add it to our scene objects collection
							Globals.ObjectAdded.Invoke(model);
						}
						else
						{
							MessageBox.Show(@"Error: Failed to Build Model from FBX! (" + result + @")");
						}
					}
					catch (Exception er)
					{
						throw new Exception(er.Message);
					}
				}
			}
		}

		private void tsAddActor_Click(object sender, EventArgs e)
		{
			if (Globals.IsProjectLoaded)
			{
				// Create our Open File Dialog
				OpenFileDialog dia = new OpenFileDialog();

				dia.Filter = @"Catalyst3D Actor File|*.a2d";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					try
					{
						Actor original = Serializer.Deserialize<Actor>(dia.FileName);

						EditorActor actor = new EditorActor(Globals.Game, string.Empty, original.AssetName, conRenderer)
																	{
																		AssetName = original.AssetName,
																		SpriteSheetFileName = original.SpriteSheetFileName,
																		Direction = original.Direction,
																		DrawOrder = original.DrawOrder,
																		UpdateOrder = original.UpdateOrder,
																		Name = original.Name,
																		PlayerIndex = original.PlayerIndex,
																		Position = Vector2.Zero,
																		Role = original.Role,
																		Scale = original.Scale,
																		Enabled = original.Enabled,
																		Visible = original.Visible
																	};

						actor.ClipPlayer = new AnimationPlayer2D(Globals.Game)
																 {
																	 AssetName = original.ClipPlayer.AssetName,
																	 Sequences = original.ClipPlayer.Sequences,
																	 IsLooped = original.ClipPlayer.IsLooped,
																	 IsFlipped = original.ClipPlayer.IsFlipped
																 };

						// Move the sprite sheet to our project -> textures directory
						if (!File.Exists(Globals.GetDestinationTexturePath(original.SpriteSheetFileName)))
							File.Copy(Path.GetDirectoryName(dia.FileName) + @"\" + original.SpriteSheetFileName,
												Globals.GetDestinationTexturePath(original.SpriteSheetFileName), true);

						actor.ClipPlayer.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																																Globals.AppSettings.ProjectPath + @"\Textures\" +
																																original.SpriteSheetFileName);

						actor.Initialize();

						actor.Play("Idle", true);

						// Add it to our scene objects collection
						Globals.ObjectAdded.Invoke(actor);
					}
					catch
					{
						throw new Exception();
					}
				}
			}
		}

		private void tsAddButton_Click(object sender, EventArgs e)
		{
			if (Globals.IsProjectLoaded)
			{
				// Create our Open File Dialog
				OpenFileDialog dia = new OpenFileDialog();
				dia.Filter = @"PNG|*.png|DDS|*.dds|JPG|*.jpg";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					try
					{
						// Move the texture to our project -> textures directory
						if (!File.Exists(Globals.GetDestinationTexturePath(dia.FileName)))
							File.Copy(dia.FileName, Globals.GetDestinationTexturePath(dia.FileName), true);

						// Button
						EditorButton button = new EditorButton(Globals.Game, conRenderer);
						button.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																											Globals.GetDestinationTexturePath(dia.FileName));

						button.AssetName = Path.GetFileName(dia.FileName);
						button.Name = Path.GetFileName(dia.FileName);
						button.ShowBoundingBox = true;

						button.Initialize();

						// Add it to our scene
						Globals.ObjectAdded.Invoke(button);

						Globals.ObjectSelected.Invoke(button);
					}
					catch (Exception er)
					{
						MessageBox.Show(er.Message);
					}
				}
			}
		}

		private void tsPlay_Click(object sender, EventArgs e)
		{
			if (Globals.OnSceneEvent != null)
			{
				Globals.OnSceneEvent.Invoke(Enums.SceneState.Playing);
				Globals.IsScenePaused = false;
				Globals.IsSceneStopped = false;
			}
		}

		private void tsPause_Click(object sender, EventArgs e)
		{
			if (Globals.OnSceneEvent != null)
			{
				Globals.OnSceneEvent.Invoke(Enums.SceneState.Paused);
				Globals.IsScenePaused = true;
				Globals.IsSceneStopped = false;
			}
		}

		private void tsStop_Click(object sender, EventArgs e)
		{
			if (Globals.OnSceneEvent != null)
			{
				Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);
				Globals.IsSceneStopped = true;
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
				return;

			SaveProjectAs();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Globals.ObjectUndo != null)
			{
				// Undo the previous action on our object
        Globals.ObjectUndo.Invoke(conRenderer.CurrentSelectedObject);
			}
		}

		private void tsAddLabel_Click(object sender, EventArgs e)
		{
			frmAddLabel frm = new frmAddLabel(conRenderer);
			frm.Show();
		}

		private void tsAttachNodes_Click(object sender, EventArgs e)
		{
			frmAttachPathingNode frm = new frmAttachPathingNode();
      frm.SceneObjects = conRenderer.RenderWindow.VisualObjects;
      frm.PathingNodes = Globals.Paths;
			frm.Show();
		}

		private void importProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
			{
				MessageBox.Show(@"No project is currently loaded! Please load a project before attempting to import.", "",
												MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Create our Open File Dialog
			OpenFileDialog dia = new OpenFileDialog();
			dia.Filter = @"Catalyst3D Project File|*.c2d";
			dia.InitialDirectory = Globals.AppSettings.ProjectPath;

			if (dia.ShowDialog() == DialogResult.OK)
			{
				try
				{
					CatalystProject2D proj = Serializer.Deserialize<CatalystProject2D>(dia.FileName);

					Globals.IsProjectLoaded = true;

					#region Load our Scenes Ledges

					foreach (Ledge l in proj.Paths)
					{
						LedgeBuilder p = new LedgeBuilder(Globals.Game, conRenderer);
						p.Role = l.Role;
						p.Name = l.Name;
						p.DrawOrder = 100; // Draw always ontop of everything
						p.IsTraveling = l.IsTraveling;

						foreach (LedgeNode n in l.Nodes)
						{
							LedgeNodeDisplay node = new LedgeNodeDisplay(Globals.Game);
							node.Position = n.Position;
							node.Scale = n.Scale;
							node.Initialize();
							node.TravelSpeed = n.TravelSpeed;
							p.Nodes.Add(node);
						}

						// Init the Ledge
						p.Initialize();

						// Add our pathing nodes
            Globals.Paths.Add(p);
					}

					#endregion

					#region Load up our Scene Objects

					foreach (VisualObject obj in proj.SceneObjects)
					{
						#region Buttons

						if (obj is Catalyst3D.XNA.Engine.EntityClasses.UI.Button)
						{
							Catalyst3D.XNA.Engine.EntityClasses.UI.Button original = obj as Catalyst3D.XNA.Engine.EntityClasses.UI.Button;

							if (!File.Exists(Globals.GetDestinationTexturePath(original.AssetName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + original.AssetName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(original.AssetName), true);
							}

							EditorButton button = new EditorButton(Globals.Game, conRenderer);
							button.Name = original.Name;
							button.AssetName = original.AssetName;
							button.BlendMode = original.BlendMode;
							button.Color = original.Color;
							button.DrawOrder = original.DrawOrder;
							button.UpdateOrder = original.UpdateOrder;
							button.Effects = original.Effects;
							button.Enabled = original.Enabled;
							button.LayerDepth = original.LayerDepth;
							button.Origin = original.Origin;
							button.Position = original.Position;
							button.Rotation = original.Rotation;
							button.Scale = original.Scale;
							button.Visible = original.Visible;
							button.ShowBoundingBox = original.ShowBoundingBox;
							button.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																												Globals.GetDestinationTexturePath(original.AssetName));
							button.IsLocked = original.IsLocked;
							button.ScrollSpeed = original.ScrollSpeed;
							button.Initialize();

							Globals.ObjectAdded.Invoke(button);

							// Due to this inheritting Sprite make sure it skips over the rest
							continue;
						}

						#endregion

						#region Sprites

						if (obj is Sprite)
						{
							Sprite original = obj as Sprite;

							if (!File.Exists(Globals.GetDestinationTexturePath(original.AssetName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + original.AssetName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(original.AssetName), true);
							}

							EditorSprite sprite = new EditorSprite(Globals.Game);
							sprite.Name = original.Name;
							sprite.AssetName = original.AssetName;
							sprite.BlendMode = original.BlendMode;
							sprite.Color = original.Color;
							sprite.DrawOrder = original.DrawOrder;
							sprite.UpdateOrder = original.UpdateOrder;
							sprite.Effects = original.Effects;
							sprite.Enabled = original.Enabled;
							sprite.LayerDepth = original.LayerDepth;
							sprite.Origin = original.Origin;
							sprite.Position = original.Position;
							sprite.Rotation = original.Rotation;
							sprite.Scale = original.Scale;
							sprite.Visible = original.Visible;
							sprite.ShowBoundingBox = original.ShowBoundingBox;
							sprite.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																												Globals.GetDestinationTexturePath(original.AssetName));
							sprite.IsLocked = original.IsLocked;
							sprite.ScrollSpeed = original.ScrollSpeed;
							sprite.IsCentered = original.IsCentered;

							sprite.CurrentPathLerpPosition = original.CurrentPathLerpPosition;
							sprite.CurrentPathNodeIndex = original.CurrentPathNodeIndex;

							sprite.StartPathingLerpPosition = original.StartPathingLerpPosition;
							sprite.StartPathNodeIndex = original.StartPathNodeIndex;

							sprite.AttachedPathingNodeName = original.AttachedPathingNodeName;

							if (!string.IsNullOrEmpty(original.AttachedPathingNodeName))
							{
								sprite.AttachedPathingNode = conRenderer.GetPath(original.AttachedPathingNodeName);
							}

							sprite.Initialize();

							Globals.ObjectAdded.Invoke(sprite);
						}

						#endregion

						#region Emitters

						if (obj is ParticleEmitter)
						{
							ParticleEmitter sceneEmitter = obj as ParticleEmitter;

							if (!File.Exists(Globals.GetDestinationTexturePath(sceneEmitter.AssetName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + sceneEmitter.AssetName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(sceneEmitter.AssetName), true);
							}

							EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
							emitter.Name = sceneEmitter.Name;
							emitter.AssetName = sceneEmitter.AssetName;
							emitter.Position = sceneEmitter.Position;
							emitter.ParticleColor = sceneEmitter.ParticleColor;
							emitter.DrawOrder = sceneEmitter.DrawOrder;
							emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
							emitter.Enabled = sceneEmitter.Enabled;
							emitter.ForceObjects = sceneEmitter.ForceObjects;
							emitter.MinAcceleration = sceneEmitter.MinAcceleration;
							emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
							emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
							emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
							emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
							emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
							emitter.MinParticles = sceneEmitter.MinParticles;
							emitter.MaxParticles = sceneEmitter.MaxParticles;
							emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
							emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
							emitter.MinScale = sceneEmitter.MinScale;
							emitter.MaxScale = sceneEmitter.MaxScale;
							emitter.Origin = sceneEmitter.Origin;
							emitter.UpdateOrder = sceneEmitter.UpdateOrder;
							emitter.Visible = sceneEmitter.Visible;
							emitter.ShowBoundingBox = sceneEmitter.ShowBoundingBox;
							emitter.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																												 Globals.GetDestinationTexturePath(sceneEmitter.AssetName));

							emitter.CurrentPathLerpPosition = sceneEmitter.StartPathingLerpPosition;
							emitter.StartPathingLerpPosition = sceneEmitter.StartPathingLerpPosition;

							emitter.StartPathNodeIndex = sceneEmitter.StartPathNodeIndex;
							emitter.CurrentPathNodeIndex = sceneEmitter.StartPathNodeIndex;
							emitter.AttachedPathingNodeName = sceneEmitter.AttachedPathingNodeName;

							if (!string.IsNullOrEmpty(sceneEmitter.AttachedPathingNodeName))
								emitter.AttachedPathingNode = conRenderer.GetPath(sceneEmitter.AttachedPathingNodeName);

							emitter.RespawnParticles = sceneEmitter.RespawnParticles;
							emitter.Initialize();

							Globals.ObjectAdded.Invoke(emitter);
						}

						#endregion

						#region Groups

						if (obj is VisualObjectGroup)
						{
							VisualObjectGroup original = obj as VisualObjectGroup;

							EditorGroup group = new EditorGroup(Globals.Game, conRenderer);
							group.AssetName = original.AssetName;
							group.DrawOrder = original.DrawOrder;
							group.UpdateOrder = original.UpdateOrder;
							group.Enabled = original.Enabled;
							group.Name = original.Name;
							group.Picker = new BoundingBoxRenderer(original.Game);
							group.Scale = original.Scale;
							group.Visible = original.Visible;
							group.ShowBoundingBox = original.ShowBoundingBox;
							group.Position = original.Position;
							group.CameraOffsetX = Globals.CurrentCameraOffsetX;
							group.CameraOffsetY = Globals.CurrentCameraOffsetY;
							group.CameraZoomOffset = Globals.CurrentCameraZoom;
							group.AttachedPathingNodeName = original.AttachedPathingNodeName;
							group.StartPathingLerpPosition = original.StartPathingLerpPosition;
							group.StartPathNodeIndex = original.StartPathNodeIndex;
							group.CurrentPathLerpPosition = original.StartPathingLerpPosition;
							group.CurrentPathNodeIndex = original.CurrentPathNodeIndex;

							if (!string.IsNullOrEmpty(original.AttachedPathingNodeName))
								group.AttachedPathingNode = conRenderer.GetPath(original.AttachedPathingNodeName);

							foreach (VisualObject o in original.Objects)
							{
								#region Sprites

								if (o is Sprite)
								{
									Sprite originalSprite = o as Sprite;

									if (!File.Exists(Globals.GetDestinationTexturePath(originalSprite.AssetName)))
									{
										// Copy our other asset to this projects texture directory
										string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + originalSprite.AssetName;
										File.Copy(texPath, Globals.GetDestinationTexturePath(originalSprite.AssetName), true);
									}

									EditorSprite sprite = new EditorSprite(Globals.Game);
									sprite.Name = originalSprite.Name;
									sprite.AssetName = Path.GetFileName(originalSprite.AssetName);
									sprite.BlendMode = originalSprite.BlendMode;
									sprite.Color = originalSprite.Color;
									sprite.DrawOrder = originalSprite.DrawOrder;
									sprite.UpdateOrder = originalSprite.UpdateOrder;
									sprite.Effects = originalSprite.Effects;
									sprite.Enabled = originalSprite.Enabled;
									sprite.LayerDepth = originalSprite.LayerDepth;
									sprite.Origin = originalSprite.Origin;
									sprite.Position = originalSprite.Position;
									sprite.Rotation = originalSprite.Rotation;
									sprite.Scale = originalSprite.Scale;
									sprite.Visible = originalSprite.Visible;
									sprite.ShowBoundingBox = originalSprite.ShowBoundingBox;
									sprite.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																														Globals.GetDestinationTexturePath(originalSprite.AssetName));
									sprite.ScrollSpeed = originalSprite.ScrollSpeed;
									sprite.AttachedPathingNodeName = group.AttachedPathingNodeName;

									sprite.CurrentPathNodeIndex = originalSprite.StartPathNodeIndex;
									sprite.CurrentPathLerpPosition = originalSprite.StartPathingLerpPosition;

									sprite.StartPathNodeIndex = originalSprite.StartPathNodeIndex;
									sprite.StartPathingLerpPosition = originalSprite.StartPathingLerpPosition;

									if (group.AttachedPathingNode != null)
										sprite.AttachedPathingNode = group.AttachedPathingNode;

									// Load up our custom shader for our sprite objects
									sprite.Initialize();

									// Add the sprite to the frame's sprite collection
									group.Objects.Add(sprite);
								}

								#endregion

								#region Emitters

								if (o is ParticleEmitter)
								{
									ParticleEmitter originalEmitter = o as ParticleEmitter;

									if (!File.Exists(Globals.GetDestinationTexturePath(originalEmitter.AssetName)))
									{
										// Copy our other asset to this projects texture directory
										string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + originalEmitter.AssetName;
										File.Copy(texPath, Globals.GetDestinationTexturePath(originalEmitter.AssetName), true);
									}

									EditorEmitter emitter = new EditorEmitter(Globals.Game, conRenderer);
									emitter.Name = originalEmitter.Name;
									emitter.AssetName = originalEmitter.AssetName;
									emitter.Position = originalEmitter.Position;
									emitter.ParticleColor = originalEmitter.ParticleColor;
									emitter.DrawOrder = originalEmitter.DrawOrder;
									emitter.EnableAlphaBlending = originalEmitter.EnableAlphaBlending;
									emitter.Enabled = originalEmitter.Enabled;
									emitter.ForceObjects = originalEmitter.ForceObjects;
									emitter.MinAcceleration = originalEmitter.MinAcceleration;
									emitter.MaxAcceleration = originalEmitter.MaxAcceleration;
									emitter.MinInitialSpeed = originalEmitter.MinInitialSpeed;
									emitter.MaxInitialSpeed = originalEmitter.MaxInitialSpeed;
									emitter.MinLifeSpan = originalEmitter.MinLifeSpan;
									emitter.MaxLifeSpan = originalEmitter.MaxLifeSpan;
									emitter.MinParticles = originalEmitter.MinParticles;
									emitter.MaxParticles = originalEmitter.MaxParticles;
									emitter.MinRotationSpeed = originalEmitter.MinRotationSpeed;
									emitter.MaxRotationSpeed = originalEmitter.MaxRotationSpeed;
									emitter.MinScale = originalEmitter.MinScale;
									emitter.MaxScale = originalEmitter.MaxScale;
									emitter.Origin = originalEmitter.Origin;
									emitter.UpdateOrder = originalEmitter.UpdateOrder;
									emitter.Visible = originalEmitter.Visible;
									emitter.ShowBoundingBox = originalEmitter.ShowBoundingBox;
									emitter.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																														 Globals.GetDestinationTexturePath(originalEmitter.AssetName));

									emitter.StartPathingLerpPosition = originalEmitter.CurrentPathLerpPosition;
									emitter.StartPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

									emitter.CurrentPathLerpPosition = originalEmitter.CurrentPathLerpPosition;
									emitter.CurrentPathNodeIndex = originalEmitter.CurrentPathNodeIndex;

									if (!string.IsNullOrEmpty(originalEmitter.AttachedPathingNodeName))
										emitter.AttachedPathingNode = conRenderer.GetPath(originalEmitter.AttachedPathingNodeName);

									emitter.RespawnParticles = originalEmitter.RespawnParticles;
									emitter.Initialize();

									// Add it to our group
									group.Objects.Add(emitter);
								}

								#endregion

								if (o is Actor)
								{
									throw new Exception("Not Yet Implemented to group Actor Objects!");
								}
							}

							group.Initialize();

							// Add the actor to our key frames visual object collection
							Globals.ObjectAdded.Invoke(group);
						}

						#endregion

						#region Actors

						if (obj is Actor)
						{
							Actor proxy = obj as Actor;

							EditorActor actor = new EditorActor(Globals.Game, string.Empty, proxy.AssetName, conRenderer);
							actor.Position = proxy.Position;
							actor.Scale = proxy.Scale;
							actor.DrawOrder = proxy.DrawOrder;
							actor.UpdateOrder = proxy.UpdateOrder;
							actor.Visible = proxy.Visible;
							actor.Role = proxy.Role;
							actor.SpriteSheetFileName = proxy.SpriteSheetFileName;
							actor.Name = proxy.Name;
							actor.Enabled = proxy.Enabled;
							actor.Direction = proxy.Direction;
							actor.IsCentered = proxy.IsCentered;
							actor.IsLooped = proxy.IsLooped;

							if (!File.Exists(Globals.GetDestinationTexturePath(proxy.SpriteSheetFileName)))
							{
								// Copy our other asset to this projects texture directory
								string texPath = Path.GetDirectoryName(dia.FileName) + @"/Textures/" + proxy.SpriteSheetFileName;
								File.Copy(texPath, Globals.GetDestinationTexturePath(proxy.SpriteSheetFileName), true);
							}

							actor.ClipPlayer = new AnimationPlayer2D(Globals.Game);
							actor.ClipPlayer.Texture = Utilitys.TextureFromFile(Globals.Game.GraphicsDevice,
																																	Globals.GetDestinationTexturePath(proxy.SpriteSheetFileName) + ".png");

							actor.ClipPlayer.Sequences = proxy.ClipPlayer.Sequences;
							actor.ClipPlayer.IsFlipped = proxy.ClipPlayer.IsFlipped;
							actor.ClipPlayer.IsLooped = proxy.ClipPlayer.IsLooped;

							// Pass the game screen to the object
              actor.GameScreen = conRenderer.RenderWindow;

							actor.CurrentPathLerpPosition = proxy.StartPathingLerpPosition;
							actor.CurrentPathNodeIndex = proxy.StartPathNodeIndex;

							actor.StartPathingLerpPosition = proxy.StartPathingLerpPosition;
							actor.StartPathNodeIndex = proxy.StartPathNodeIndex;

							actor.AttachedPathingNodeName = proxy.AttachedPathingNodeName;

							if (!string.IsNullOrEmpty(proxy.AttachedPathingNodeName))
							{
								actor.AttachedPathingNode = conRenderer.GetPath(proxy.AttachedPathingNodeName);
							}

							// Init our actor
							actor.Initialize();

							// Play our first sequence out the gate
							actor.Play(proxy.ClipPlayer.Sequences[0].Name, true);

							Globals.ObjectAdded.Invoke(actor);
						}

						#endregion
					}

					#endregion

					#region Load our Sprite Container Objects

					if (proj.SpriteContainerFiles.Count > 0)
					{
						// Found sprites to load into our Sprite Container .. Load them up
						foreach (string texture in proj.SpriteContainerFiles)
						{
							bool import = true;
							foreach (conSprite sc in conSpriteContainer.pnContainer.Controls)
							{
								if (sc.FileName != Path.GetFileName(texture))
									import = false;
							}

							if (import)
							{
								Image img = Image.FromFile(Globals.GetDestinationTexturePath(texture));
								conSpriteContainer.AddSprite(img, texture);
							}
						}
					}

					#endregion

          conRenderer.CurrentSelectedObject = null;
					Globals.ObjectSelected.Invoke(null);

					MessageBox.Show(@"Successfully Imported The C2D Project!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception er)
				{
					MessageBox.Show(er.Message);
				}
			}
		}

		private void exportVisibleObjectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Globals.IsProjectLoaded)
				return;

			if (MessageBox.Show(@"Are you sure you want to export only visible scene objects to a new .c2d file?", @"Export Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SaveFileDialog dia = new SaveFileDialog();

				dia.Filter = @"Catalyst3D Project File|*.c2d";
				dia.InitialDirectory = Globals.AppSettings.ProjectPath;

				if (dia.ShowDialog() == DialogResult.OK)
				{
					string intermediatePath = Path.GetDirectoryName(dia.FileName);
					string intermediateFileName = Path.GetFileName(dia.FileName);

					// Create our Textures Directory
					if (!Directory.Exists(Path.Combine(intermediatePath, @"Textures")))
						Directory.CreateDirectory(Path.Combine(intermediatePath, @"Textures"));

					// Create our new Catalyst 2D Project
					CatalystProject2D proj = new CatalystProject2D();

					// Loop thru our scene objects
          foreach (VisualObject t in conRenderer.RenderWindow.VisualObjects)
					{
						if (!t.Visible)
							continue;

						#region Buttons

						if (t is EditorButton)
						{
							EditorButton sceneButton = t as EditorButton;

							// Create a new Sprite Entity so we can serialize it
							Catalyst3D.XNA.Engine.EntityClasses.UI.Button button = new Catalyst3D.XNA.Engine.EntityClasses.UI.Button
							{
								BlendMode = sceneButton.BlendMode,
								AssetName = Path.GetFileNameWithoutExtension(sceneButton.AssetName),
								Color = sceneButton.Color,
								Effects = sceneButton.Effects,
								LayerDepth = sceneButton.LayerDepth,
								Origin = sceneButton.Origin,
								Position = sceneButton.Position,
								Rotation = sceneButton.Rotation,
								Scale = sceneButton.Scale,
								DrawOrder = sceneButton.DrawOrder,
								UpdateOrder = sceneButton.UpdateOrder,
								Enabled = sceneButton.Enabled,
								Visible = sceneButton.Visible,
								ShowBoundingBox = sceneButton.ShowBoundingBox,
								ScrollSpeed = sceneButton.ScrollSpeed,
								IsLocked = sceneButton.IsLocked,
								Name = sceneButton.Name
							};



							// Add it to our new Serializable Key Frame's Sprite Collection
							proj.SceneObjects.Add(button);
						}

						#endregion

						#region Sprites

						if (t is EditorSprite)
						{
							EditorSprite sceneSprite = t as EditorSprite;

							// Create a new Sprite Entity so we can serialize it
							Sprite sprite = new Sprite
							{
								BlendMode = sceneSprite.BlendMode,
								AssetName = Path.GetFileNameWithoutExtension(sceneSprite.AssetName),
								Color = sceneSprite.Color,
								Effects = sceneSprite.Effects,
								LayerDepth = sceneSprite.LayerDepth,
								Origin = sceneSprite.Origin,
								Position = sceneSprite.Position,
								Rotation = sceneSprite.Rotation,
								Scale = sceneSprite.Scale,
								DrawOrder = sceneSprite.DrawOrder,
								UpdateOrder = sceneSprite.UpdateOrder,
								Enabled = sceneSprite.Enabled,
								Visible = sceneSprite.Visible,
								ShowBoundingBox = sceneSprite.ShowBoundingBox,
								ScrollSpeed = sceneSprite.ScrollSpeed,
								IsLocked = sceneSprite.IsLocked,
								Name = sceneSprite.Name,
								IsCentered = sceneSprite.IsCentered
							};

							// Add it to our new Serializable Key Frame's Sprite Collection
							proj.SceneObjects.Add(sprite);
						}


						#endregion

						#region Emitters

						if (t is EditorEmitter)
						{
							EditorEmitter sceneEmitter = t as EditorEmitter;

							ParticleEmitter emitter = new ParticleEmitter();
							emitter.AssetName = sceneEmitter.AssetName;
							emitter.Camera = sceneEmitter.Camera;
							emitter.DrawOrder = sceneEmitter.DrawOrder;
							emitter.EnableAlphaBlending = sceneEmitter.EnableAlphaBlending;
							emitter.Enabled = sceneEmitter.Enabled;
							emitter.ForceObjects = sceneEmitter.ForceObjects;
							emitter.MinAcceleration = sceneEmitter.MinAcceleration;
							emitter.MaxAcceleration = sceneEmitter.MaxAcceleration;
							emitter.MinInitialSpeed = sceneEmitter.MinInitialSpeed;
							emitter.MaxInitialSpeed = sceneEmitter.MaxInitialSpeed;
							emitter.MinLifeSpan = sceneEmitter.MinLifeSpan;
							emitter.MaxLifeSpan = sceneEmitter.MaxLifeSpan;
							emitter.MinParticles = sceneEmitter.MinParticles;
							emitter.MaxParticles = sceneEmitter.MaxParticles;
							emitter.MinRotationSpeed = sceneEmitter.MinRotationSpeed;
							emitter.MaxRotationSpeed = sceneEmitter.MaxRotationSpeed;
							emitter.MinScale = sceneEmitter.MinScale;
							emitter.MaxScale = sceneEmitter.MaxScale;
							emitter.Position = sceneEmitter.Position;
							emitter.ParticleColor = sceneEmitter.ParticleColor;
							emitter.Name = sceneEmitter.Name;
							emitter.Origin = sceneEmitter.Origin;
							emitter.UpdateOrder = sceneEmitter.UpdateOrder;
							emitter.Visible = sceneEmitter.Visible;
							emitter.ShowBoundingBox = sceneEmitter.ShowBoundingBox;
							emitter.RespawnParticles = sceneEmitter.RespawnParticles;

							proj.SceneObjects.Add(emitter);
						}

						#endregion

						#region Groups

						if (t is EditorGroup)
						{
							EditorGroup original = t as EditorGroup;

							VisualObjectGroup group = new VisualObjectGroup
							{
								AssetName = original.AssetName,
								DrawOrder = original.DrawOrder,
								UpdateOrder = original.UpdateOrder,
								Enabled = original.Enabled,
								Name = original.Name,
								Position = original.Position,
								Scale = original.Scale,
								ShowBoundingBox = original.ShowBoundingBox,
								Visible = original.Visible,
							};

							foreach (VisualObject o in original.Objects)
							{
								if (o is EditorSprite)
								{
									EditorSprite sceneSprite = o as EditorSprite;

									// Init our new Sprite
									Sprite sprite = new Sprite
									{
										BlendMode = sceneSprite.BlendMode,
										AssetName = Path.GetFileNameWithoutExtension(sceneSprite.AssetName),
										Color = sceneSprite.Color,
										Effects = sceneSprite.Effects,
										LayerDepth = sceneSprite.LayerDepth,
										Origin = sceneSprite.Origin,
										Position = sceneSprite.Position,
										Rotation = sceneSprite.Rotation,
										Scale = sceneSprite.Scale,
										DrawOrder = sceneSprite.DrawOrder,
										UpdateOrder = sceneSprite.UpdateOrder,
										Enabled = sceneSprite.Enabled,
										Visible = sceneSprite.Visible,
										ShowBoundingBox = sceneSprite.ShowBoundingBox,
										ScrollSpeed = sceneSprite.ScrollSpeed,
										IsLocked = sceneSprite.IsLocked,
										Name = sceneSprite.Name,
									};

									// Add it to our groups objects
									group.Objects.Add(sprite);
								}

								if (o is EditorEmitter)
								{
									EditorEmitter sceneEmitter = o as EditorEmitter;

									ParticleEmitter emitter = new ParticleEmitter
									{
										AssetName = sceneEmitter.AssetName,
										Camera = sceneEmitter.Camera,
										DrawOrder = sceneEmitter.DrawOrder,
										EnableAlphaBlending = sceneEmitter.EnableAlphaBlending,
										Enabled = sceneEmitter.Enabled,
										ForceObjects = sceneEmitter.ForceObjects,
										MinAcceleration = sceneEmitter.MinAcceleration,
										MaxAcceleration = sceneEmitter.MaxAcceleration,
										MinInitialSpeed = sceneEmitter.MinInitialSpeed,
										MaxInitialSpeed = sceneEmitter.MaxInitialSpeed,
										MinLifeSpan = sceneEmitter.MinLifeSpan,
										MaxLifeSpan = sceneEmitter.MaxLifeSpan,
										MinParticles = sceneEmitter.MinParticles,
										MaxParticles = sceneEmitter.MaxParticles,
										MinRotationSpeed = sceneEmitter.MinRotationSpeed,
										MaxRotationSpeed = sceneEmitter.MaxRotationSpeed,
										MinScale = sceneEmitter.MinScale,
										MaxScale = sceneEmitter.MaxScale,
										Position = sceneEmitter.Position,
										ParticleColor = sceneEmitter.ParticleColor,
										Name = sceneEmitter.Name,
										Origin = sceneEmitter.Origin,
										UpdateOrder = sceneEmitter.UpdateOrder,
										Visible = sceneEmitter.Visible,
										ShowBoundingBox = sceneEmitter.ShowBoundingBox,
										RespawnParticles = sceneEmitter.RespawnParticles
									};

									// Add it to our group
									group.Objects.Add(emitter);
								}
							}

							// Add it to our new Serializable Key Frame's Sprite Collection
							proj.SceneObjects.Add(group);
						}


						#endregion

						#region Pathing Nodes

						if (t is LedgeBuilder)
						{
							LedgeBuilder original = t as LedgeBuilder;

							Ledge ledge = new Ledge();


							ledge.Name = original.Name;
							ledge.Role = original.Role;

							foreach (LedgeNodeDisplay n in original.Nodes)
							{
								LedgeNode node = new LedgeNode();
								node.Position = n.Position;
								ledge.Nodes.Add(node);
							}

							// Add it up
							proj.Paths.Add(ledge);
						}

						#endregion

						#region Actors

						if (t is EditorActor)
						{
							EditorActor original = t as EditorActor;

							Actor actor = new Actor
							{
								AssetName = original.AssetName,
								SpriteSheetFileName = original.SpriteSheetFileName,
								Direction = original.Direction,
								DrawOrder = original.DrawOrder,
								UpdateOrder = original.UpdateOrder,
								Enabled = original.Enabled,
								Name = original.Name,
								Visible = original.Visible,
								Position = original.Position,
								Role = original.Role,
								Scale = original.Scale
							};

							actor.ClipPlayer = new AnimationPlayer2D(Globals.Game)
							{
								Sequences = original.ClipPlayer.Sequences,
								IsFlipped = original.ClipPlayer.IsFlipped,
								IsLooped = original.ClipPlayer.IsLooped
							};

							proj.SceneObjects.Add(actor);
						}

						#endregion
					}

					#region Sprite Container's Objects

					for (int i = 0; i < conSpriteContainer.pnContainer.Controls.Count; i++)
					{
						conSprite con = conSpriteContainer.pnContainer.Controls[i] as conSprite;
						if (con != null)
						{
							// Loop thru our scene objects
              foreach (VisualObject o in conRenderer.RenderWindow.VisualObjects)
							{
								if (o.Visible)
								{
									string newPath = Path.Combine(intermediatePath, @"Textures", con.FileName);
									string originalPath = Path.Combine(Globals.AppSettings.ProjectPath, @"Textures", Path.GetFileName(con.FileName));

									// If the object is visible and the file does not exist in our new textures folder copy it there.
									if (!File.Exists(newPath))
									{
										File.Copy(originalPath, newPath);
									}

									// Store the filename of the asset for loading later
									proj.SpriteContainerFiles.Add(Path.GetFileName(con.FileName));
								}
							}
						}
					}

					#endregion

					// Save the screen resolution in the project
					switch (Globals.AppSettings.Resolution)
					{
						case 0:
							proj.ScreenResolution = new Vector2(1920, 1080);
							break;
						case 1:
							proj.ScreenResolution = new Vector2(1280, 720);
							break;
						case 2:
							proj.ScreenResolution = new Vector2(800, 480);
							break;
						case 3:
							proj.ScreenResolution = new Vector2(480, 800);
							break;
					}

					// Save the project to disk
					Serializer.Serialize(Path.Combine(intermediatePath, intermediateFileName), proj);

					MessageBox.Show(@"Successfully Exported the Project!", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}
			}
		}

		private void loadGameTypesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dia = new OpenFileDialog();

			if (dia.ShowDialog() == DialogResult.OK)
				Globals.LoadAssembly(dia.FileName);
		}

		private void tsAddRectangle_Click(object sender, EventArgs e)
		{
			EditorSpriteBox rec = new EditorSpriteBox(Globals.Game, conRenderer);
			rec.Name = "Rectangle";
			rec.Initialize();

			Globals.ObjectAdded.Invoke(rec);
		}

		private void tsFollow_Click(object sender, EventArgs e)
		{
      if (conRenderer.FollowingObject != null)
			{
        conRenderer.FollowingObjectName = string.Empty;
        conRenderer.FollowingObject = null;
			}
			else
			{
        conRenderer.FollowingObject = conRenderer.CurrentSelectedObject;
			}
		}

		private void tsResetCamera2_Click(object sender, EventArgs e)
		{
			Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);
			Globals.CurrentCameraOffsetX = 50;
			Globals.CurrentCameraOffsetY = 50;
		}

		private void tsCameraOffsetX_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(tsCameraOffsetX.Text))
				return;

			float value;
			if (float.TryParse(tsCameraOffsetX.Text, out value))
				conRenderer.CameraFollowOffsetX = value;
		}

		private void tsLock_Click(object sender, EventArgs e)
		{
      if (conRenderer.CurrentSelectedObject != null)
			{
        if (conRenderer.CurrentSelectedObject.IsLocked)
          conRenderer.CurrentSelectedObject.IsLocked = false;
				else
          conRenderer.CurrentSelectedObject.IsLocked = true;
			}
		}

		private void tsGroupSelected_Click(object sender, EventArgs e)
		{
			// Create our new Group
			EditorGroup vg = new EditorGroup(Globals.Game, conRenderer);
			vg.Name = "unnamed group";
			vg.ShowBoundingBox = true;

			var removals = new List<VisualObject>();
      var selected = conRenderer.RenderWindow.VisualObjects.Where(v => v.IsSelected).ToList();

			for (int i = 0; i < selected.Count(); i++)
			{
				if (selected[i] is EditorGroup)
				{
					var group = selected[i] as EditorGroup;

					if (group != null)
					{
						foreach (var v in group.Objects)
						{
							vg.Objects.Add(v);
							removals.Remove(v);
						}
					}
				}
				else
				{
					if (selected[i] != null)
					{
						if (i == 0)
						{
							vg.Scale = selected[i].Scale;
							vg.Position = selected[i].Position;
						}

						vg.Objects.Add(selected[i]);
						removals.Remove(selected[i]);
					}
				}

			}

			vg.Initialize();

			Globals.ObjectAdded.Invoke(vg);
		}

		private void tsDetach_Click(object sender, EventArgs e)
		{
      if (conRenderer.CurrentSelectedObject != null)
			{
        if (conRenderer.CurrentSelectedObject is EditorActor)
        {
          var actor = conRenderer.CurrentSelectedObject as EditorActor;
          {
            actor.AttachedPathingNode = null;
            actor.AttachedPathingNodeName = string.Empty;
          }
        }
        else
				{
          conRenderer.CurrentSelectedObject.AttachedPathingNode = null;
          conRenderer.CurrentSelectedObject.AttachedPathingNodeName = string.Empty;
				}
			}
		}
	}
}