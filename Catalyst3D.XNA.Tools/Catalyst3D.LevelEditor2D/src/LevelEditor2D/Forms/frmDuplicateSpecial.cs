using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor2D.Controls;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.Forms
{
	public partial class frmDuplicateSpecial : Form
	{
		private readonly List<VisualObject> SceneObjects;
	  private readonly conRenderer RenderWindow;

		public frmDuplicateSpecial(List<VisualObject> obj, conRenderer renderer)
		{
			InitializeComponent();

		  RenderWindow = renderer;

			SceneObjects = obj;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Globals.IsDialogWindowOpen = true;

			rlSceneObjects.DisplayMember = "Name";
			rlSceneObjects.DataSource = SceneObjects;
		}

		private void btnDuplicate_Click(object sender, EventArgs e)
		{
			int counts = Convert.ToInt16(tbQTY.Text);
			int offsetX = Convert.ToInt16(tbXOffset.Text);
			int offsetY = Convert.ToInt16(tbYOffset.Text);

			float totalWidth = 0;
			float totalHeight = 0;

			#region Groups

			if(SceneObjects[rlSceneObjects.SelectedIndex] is EditorGroup)
			{
				EditorGroup original = SceneObjects[rlSceneObjects.SelectedIndex] as EditorGroup;

				if(original != null)
				{
					for(int i = 0; i < counts; i++)
					{
            EditorGroup group = new EditorGroup(Globals.Game, RenderWindow)
														{
															AssetName = original.AssetName,
															DrawOrder = original.DrawOrder,
															UpdateOrder = original.UpdateOrder,
															Enabled = original.Enabled,
															Name = original.Name,
															Scale = original.Scale,
															Visible = original.Visible,
															ShowBoundingBox = true,
															Position = original.Position
														};

						foreach(VisualObject o in original.Objects)
						{
							if(o is EditorSprite)
							{
								EditorSprite sOriginal = o as EditorSprite;

								// Init our new Sprite
                EditorSprite sprite = new EditorSprite(sOriginal.Game)
																	{
																		Name = sOriginal.Name,
																		Texture = sOriginal.Texture,
																		AssetName = sOriginal.AssetName,
																		Position = sOriginal.Position,
																		Scale = sOriginal.Scale,
																		LayerDepth = sOriginal.LayerDepth,
																		Origin = sOriginal.Origin,
																		Rotation = sOriginal.Rotation,
																		StartPosition = sOriginal.StartPosition,
																		UpdateOrder = sOriginal.UpdateOrder,
																		CustomEffect = sOriginal.CustomEffect,
																		ShowBoundingBox = false,
																		BlendMode = sOriginal.BlendMode,
																		DrawOrder = sOriginal.DrawOrder,
																		ScrollSpeed = sOriginal.ScrollSpeed,
																	};

								// Init this texture
								sprite.Initialize();

								// Add it to our groups objects
								group.Objects.Add(sprite);
							}

							if(o is EditorEmitter)
							{
								EditorEmitter eOriginal = o as EditorEmitter;

                EditorEmitter emitter = new EditorEmitter(eOriginal.Game, RenderWindow)
																		{
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
																			Texture = eOriginal.Texture,
																			Position = eOriginal.Position
																		};

								emitter.Initialize();

								// Add it to our group
								group.Objects.Add(emitter);
							}
						}

						// Initialize our group
						group.Initialize();

						float width = 0;
						float height = 0;

						foreach(VisualObject o in group.Objects)
						{
							if(o is EditorSprite)
							{
								EditorSprite s = o as EditorSprite;
								if((s.Texture.Width * s.Scale.X) > width)
								{
									width = s.Texture.Width * s.Scale.X;
								}

								if((s.Texture.Height * s.Scale.Y) > height)
								{
									height = s.Texture.Height * s.Scale.Y;
								}

							}
							if(o is EditorEmitter)
							{
								EditorEmitter emitter = o as EditorEmitter;
								if((emitter.Texture.Width * emitter.Scale.X) > width)
								{
									width = emitter.Texture.Width * emitter.Scale.X;
								}

								if((emitter.Texture.Height * emitter.Scale.Y) > height)
								{
									height = emitter.Texture.Height * emitter.Scale.Y;
								}
							}
						}

						totalWidth += width + offsetX * (i + 1);
						totalHeight += height + offsetY * (i + 1);

						// Move the individual objects in the group
						// TODO: Make this later automatically done in the CTBaseGroup class!
						foreach(VisualObject vo in group.Objects)
						{
							switch(cbDirection.Text)
							{
								case "X":
									vo.Position += new Vector2(totalWidth, 0);
									break;
								case "Y":
									vo.Position += new Vector2(0, totalHeight);
									break;
								case "Both":
									vo.Position += new Vector2(totalWidth, totalHeight);
									break;
							}
						}

						// Move the Complete groups position too
						switch(cbDirection.Text)
						{
							case "X":
								group.Position += new Vector2(totalWidth, 0);
								break;
							case "Y":
								group.Position += new Vector2(0, totalHeight);
								break;
							case "Both":
								group.Position += new Vector2(totalWidth, totalHeight);
								break;
						}

						group.Name = original.Name + (i + 1);

						// Add it to current our Key Frame
						Globals.ObjectAdded.Invoke(group);
					}
				}
			}

			#endregion

			#region Sprites

			if(SceneObjects[rlSceneObjects.SelectedIndex] is EditorSprite)
			{
				EditorSprite sprite = SceneObjects[rlSceneObjects.SelectedIndex] as EditorSprite;

				if(sprite != null)
				{
					for(int i = 0; i < counts; i++)
					{
						totalWidth += (sprite.Texture.Width * sprite.Scale.X) + offsetX * (i + 1);
						totalHeight += (sprite.Texture.Height * sprite.Scale.Y) + offsetY * (i + 1);

            EditorSprite s = new EditorSprite(sprite.Game)
						                 	{
						                 		Name = sprite.Name,
						                 		AssetName = sprite.AssetName,
						                 		BlendMode = sprite.BlendMode,
						                 		Color = sprite.Color,
						                 		CameraOffsetX = sprite.CameraOffsetX,
						                 		CameraOffsetY = sprite.CameraOffsetY,
						                 		CameraZoomOffset = sprite.CameraZoomOffset,
						                 		CustomEffect = sprite.CustomEffect,
						                 		DrawOrder = sprite.DrawOrder,
						                 		Effects = sprite.Effects,
						                 		Enabled = sprite.Enabled,
						                 		IsLocked = sprite.IsLocked,
						                 		ShowBoundingBox = sprite.ShowBoundingBox,
						                 		LayerDepth = sprite.LayerDepth,
						                 		Origin = sprite.Origin,
						                 		Position = sprite.Position,
						                 		Rotation = sprite.Rotation,
						                 		Scale = sprite.Scale,
						                 		UpdateOrder = sprite.UpdateOrder,
						                 		Visible = sprite.Visible,
						                 		Texture = sprite.Texture,
						                 		ScrollSpeed = sprite.ScrollSpeed,
						                 		AttachedPathingNode = sprite.AttachedPathingNode,
						                 		AttachedPathingNodeName = sprite.AttachedPathingNodeName,
						                 		CurrentPathLerpPosition = sprite.CurrentPathLerpPosition,
						                 		CurrentPathNodeIndex = sprite.CurrentPathNodeIndex,
						                 		StartPathingLerpPosition = sprite.StartPathingLerpPosition,
						                 		StartPathNodeIndex = sprite.StartPathNodeIndex,
						                 	};

						s.Initialize();

						switch(cbDirection.Text)
						{
							case "X":
								s.Position += new Vector2(totalWidth, 0);
								break;
							case "Y":
								s.Position += new Vector2(0, totalHeight);
								break;
							case "Both":
								s.Position += new Vector2(totalWidth, totalHeight);
								break;

						}

						// Add it to our scene
						Globals.ObjectAdded.Invoke(s);
					}
				}
			}

			#endregion

			#region Emitters

			if(SceneObjects[rlSceneObjects.SelectedIndex] is EditorEmitter)
			{
				EditorEmitter original = SceneObjects[rlSceneObjects.SelectedIndex] as EditorEmitter;

				if(original != null)
				{
					for(int i = 1; i < counts; i++)
					{
						totalWidth += (original.Texture.Width * original.Scale.X) + offsetX * (i + 1);
						totalHeight += (original.Texture.Height * original.Scale.Y) + offsetY * (i + 1);

            EditorEmitter emitter = new EditorEmitter(original.Game, RenderWindow)
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
						                        		Texture = original.Texture,
						                        		Position = original.Position,
						                        		AttachedPathingNode = original.AttachedPathingNode,
						                        		AttachedPathingNodeName = original.AttachedPathingNodeName,
						                        		CurrentPathLerpPosition = original.CurrentPathLerpPosition,
						                        		CurrentPathNodeIndex = original.CurrentPathNodeIndex,
						                        		StartPathingLerpPosition = original.StartPathingLerpPosition,
						                        		StartPathNodeIndex = original.StartPathNodeIndex
						                        	};

						emitter.Initialize();

						switch(cbDirection.Text)
						{
							case "X":
								emitter.Position += new Vector2(totalWidth, 0);
								break;
							case "Y":
								emitter.Position += new Vector2(0, totalHeight);
								break;
							case "Both":
								emitter.Position += new Vector2(totalWidth, totalHeight);
								break;
						}

						// Add it to current our Key Frame
						Globals.ObjectAdded.Invoke(emitter);
					}
				}
			}

			#endregion

			#region Custom Sprite Boxes

			if (SceneObjects[rlSceneObjects.SelectedIndex] is EditorSpriteBox)
			{
				EditorSpriteBox sprite = SceneObjects[rlSceneObjects.SelectedIndex] as EditorSpriteBox;

				if (sprite != null)
				{
					for (int i = 0; i < counts; i++)
					{
						totalWidth += (sprite.Texture.Width*sprite.Scale.X) + offsetX*(i + 1);
						totalHeight += (sprite.Texture.Height*sprite.Scale.Y) + offsetY*(i + 1);

            EditorSpriteBox s = new EditorSpriteBox(sprite.Game, RenderWindow)
						                    	{
						                    		Name = sprite.Name,
						                    		AssetName = sprite.AssetName,
						                    		BlendMode = sprite.BlendMode,
						                    		Width = sprite.Width,
						                    		Height = sprite.Height,
						                    		Color = sprite.Color,
						                    		CameraOffsetX = sprite.CameraOffsetX,
						                    		CameraOffsetY = sprite.CameraOffsetY,
						                    		CameraZoomOffset = sprite.CameraZoomOffset,
						                    		CustomEffect = sprite.CustomEffect,
						                    		DrawOrder = sprite.DrawOrder,
						                    		Effects = sprite.Effects,
						                    		Enabled = sprite.Enabled,
						                    		IsLocked = sprite.IsLocked,
						                    		ShowBoundingBox = sprite.ShowBoundingBox,
						                    		LayerDepth = sprite.LayerDepth,
						                    		Origin = sprite.Origin,
						                    		Position = sprite.Position,
						                    		Rotation = sprite.Rotation,
						                    		Scale = sprite.Scale,
						                    		UpdateOrder = sprite.UpdateOrder,
						                    		Visible = sprite.Visible,
						                    		Texture = sprite.Texture,
						                    		ScrollSpeed = sprite.ScrollSpeed,
						                    		AttachedPathingNode = sprite.AttachedPathingNode,
						                    		AttachedPathingNodeName = sprite.AttachedPathingNodeName,
						                    		CurrentPathLerpPosition = sprite.CurrentPathLerpPosition,
						                    		CurrentPathNodeIndex = sprite.CurrentPathNodeIndex,
						                    		StartPathingLerpPosition = sprite.StartPathingLerpPosition,
						                    		StartPathNodeIndex = sprite.StartPathNodeIndex
						                    	};

						s.Initialize();

						switch (cbDirection.Text)
						{
							case "X":
								s.Position += new Vector2(totalWidth, 0);
								break;
							case "Y":
								s.Position += new Vector2(0, totalHeight);
								break;
							case "Both":
								s.Position += new Vector2(totalWidth, totalHeight);
								break;
						}

						// Add it to our scene
						Globals.ObjectAdded.Invoke(s);
					}
				}
			}
			
			#endregion

			Globals.IsDialogWindowOpen = false;

			Dispose();
		}

		private void rlSceneObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(rlSceneObjects.SelectedItem != null)
			{
				if(rlSceneObjects.SelectedValue is EditorGroup)
				{
					EditorGroup g = rlSceneObjects.SelectedValue as EditorGroup;
					foreach(VisualObject vo in g.Objects)
						vo.ShowBoundingBox = true;
				}

				Globals.ObjectSelected((VisualObject)rlSceneObjects.SelectedValue);
			}
		}

		protected override void DestroyHandle()
		{
			Globals.IsDialogWindowOpen = false;

			base.DestroyHandle();
		}
	}
}