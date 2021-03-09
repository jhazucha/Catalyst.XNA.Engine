#if !XBOX360 && !WINDOWS_PHONE

using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model = Microsoft.Xna.Framework.Graphics.Model;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class SelectedObjectDisplay3D : VisualObject
	{
		private Matrix[] _coneBoneTransforms;
		private Matrix[] _coneOriginalBoneTransforms;

		private Matrix _coneXWorld;
		private Matrix _coneYWorld;
		private Matrix _coneZWorld;

		private Matrix[] _cubeBoneTransforms;
		private Matrix[] _cubeOriginalBoneTransforms;

		private Matrix _cubeXWorld;
		private Matrix _cubeYWorld;
		private Matrix _cubeZWorld;

		private VertexPositionColor[] _vertX;
		private VertexPositionColor[] _vertY;
		private VertexPositionColor[] _vertZ;

		private Color _xColor = Color.Red;
		private Color _yColor = Color.Green;
		private Color _zColor = Color.Blue;
		
		private VertexDeclaration _vDeclaration;
		private BasicEffect _simpleEffect;

		public float LineLength { get; set; }
		
		public new Vector3 Position { get; set; }
		public new Vector3 Scale { get; set; }

		public SelectedDisplayMode SelectMode { get; set; }

		public Model TranslationPointer { get; set; }
		public Model ScalePointer { get; set; }

		public SelectedAxis SelectedAxis { get; set; }
		public BoundingType BoundingType { get; set; }

		public SelectedObjectDisplay3D(Game game)
			: base(game)
		{
			LineLength = 0.2f;
			SelectMode = SelectedDisplayMode.Translation;
			Scale = new Vector3(0.08f, 0.08f, 0.08f);
		}

		public override void Initialize()
		{
			// Get an instance of our Scene Manager controlling these objects
			if (GameScreen != null)
				GameScreen.SceneManager = (SceneManager) Game.Services.GetService(typeof (SceneManager));

			Camera.Initialize();

			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();

			// Setup vertex declaration
			_vDeclaration = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());
			_simpleEffect = new BasicEffect(GraphicsDevice);
			_simpleEffect.VertexColorEnabled = true;

			// Setup our Cone Pointer
			TranslationPointer = Game.Content.Load<Model>("Cone");

			_coneBoneTransforms = new Matrix[TranslationPointer.Bones.Count];
			_coneOriginalBoneTransforms = new Matrix[TranslationPointer.Bones.Count];

			TranslationPointer.CopyAbsoluteBoneTransformsTo(_coneBoneTransforms);
			TranslationPointer.CopyBoneTransformsTo(_coneOriginalBoneTransforms);

			// Setup our Cube Pointer
			ScalePointer = Game.Content.Load<Model>("Cube");
			_cubeBoneTransforms = new Matrix[ScalePointer.Bones.Count];
			_cubeOriginalBoneTransforms = new Matrix[ScalePointer.Bones.Count];
			ScalePointer.CopyAbsoluteBoneTransformsTo(_cubeBoneTransforms);
			ScalePointer.CopyBoneTransformsTo(_cubeOriginalBoneTransforms);
		}

		internal void SetupVertices()
		{
			// X Axis
			_vertX = new VertexPositionColor[2];
			_vertX[0].Position = Vector3.Zero;
			_vertX[0].Color = _xColor;

			_vertX[1].Position = new Vector3(LineLength, 0, 0);
			_vertX[1].Color = _xColor;

			// Y Axis
			_vertY = new VertexPositionColor[2];
			_vertY[0].Position = Vector3.Zero;
			_vertY[0].Color = _yColor;

			_vertY[1].Position = new Vector3(0, LineLength, 0);
			_vertY[1].Color = _yColor;

			// Z Axis
			_vertZ = new VertexPositionColor[2];
			_vertZ[0].Position = Vector3.Zero;
			_vertZ[0].Color = _zColor;

			_vertZ[1].Position = new Vector3(0, 0, LineLength);
			_vertZ[1].Color = _zColor;

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Camera.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Setup our vertices incase line length has changed
			SetupVertices();

			if (Camera != null)
			{
				_simpleEffect.World = Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);
				_simpleEffect.View = Camera.View;
				_simpleEffect.Projection = Camera.Projection;
			}
			
			#region X Axis Drawing

			if(SelectedAxis == SelectedAxis.X)
			{
				// Draw our X Axis Line
				_simpleEffect.AmbientLightColor = Color.Yellow.ToVector3();
				_simpleEffect.DiffuseColor = Color.Yellow.ToVector3();
			}
			else
			{
				// Draw our X Axis Line
				_simpleEffect.AmbientLightColor = _xColor.ToVector3();
				_simpleEffect.DiffuseColor = _xColor.ToVector3();
			}

			// Begin this effect pass
			_simpleEffect.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertX, 0, 1);

			#endregion

			#region Y Axis Drawing

		
			if (SelectedAxis == SelectedAxis.Y)
			{
				// Draw our X Axis Line
				_simpleEffect.AmbientLightColor = Color.Yellow.ToVector3();
				_simpleEffect.DiffuseColor = Color.Yellow.ToVector3();
			}
			else
			{
				_simpleEffect.AmbientLightColor = _yColor.ToVector3();
				_simpleEffect.DiffuseColor = _yColor.ToVector3();
			}

			// Begin this effect pass
			_simpleEffect.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertY, 0, 1);

			#endregion

			#region Z Axis Drawing
			
			if (SelectedAxis == SelectedAxis.Z)
			{
				// Draw our X Axis Line
				_simpleEffect.AmbientLightColor = Color.Yellow.ToVector3();
				_simpleEffect.DiffuseColor = Color.Yellow.ToVector3();
			}
			else
			{
				_simpleEffect.AmbientLightColor = _zColor.ToVector3();
				_simpleEffect.DiffuseColor = _zColor.ToVector3();
			}

			// Begin this effect pass
			_simpleEffect.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertZ, 0, 1);

			#endregion

			#region Translation Pointer

			if (SelectMode == SelectedDisplayMode.Translation)
			{
				#region Draw Cone Pointer for X Axis

				foreach (ModelMesh mesh in TranslationPointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						_coneXWorld = _cubeBoneTransforms[mesh.ParentBone.Index]*
						              Matrix.CreateScale(Scale)*
						              Matrix.CreateRotationZ(MathHelper.ToRadians(-90))*
						              Matrix.CreateTranslation(new Vector3(Position.X + LineLength, Position.Y, Position.Z));

						if (Camera != null)
						{
							effect.World = _coneXWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						if (SelectedAxis == SelectedAxis.X)
						{
							effect.DiffuseColor = Color.Yellow.ToVector3();
							effect.AmbientLightColor = Color.Yellow.ToVector3();
						}
						else
						{
							effect.DiffuseColor = _xColor.ToVector3();
							effect.AmbientLightColor = _xColor.ToVector3();
						}
					}
					mesh.Draw();
				}

				TranslationPointer.CopyBoneTransformsFrom(_coneOriginalBoneTransforms);

				#endregion

				#region Draw Cone Pointer for Y Axis

				foreach (ModelMesh mesh in TranslationPointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						_coneYWorld = _cubeBoneTransforms[mesh.ParentBone.Index] *
						              Matrix.CreateScale(Scale) *
						              Matrix.CreateTranslation(new Vector3(Position.X, Position.Y + LineLength, Position.Z));

						if (Camera != null)
						{
							effect.World = _coneYWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						if (SelectedAxis == SelectedAxis.Y)
						{
							effect.DiffuseColor = Color.Yellow.ToVector3();
							effect.AmbientLightColor = Color.Yellow.ToVector3();
						}
						else
						{
							effect.DiffuseColor = _yColor.ToVector3();
							effect.AmbientLightColor = _yColor.ToVector3();
						}

					}
					mesh.Draw();
				}

				TranslationPointer.CopyBoneTransformsFrom(_coneOriginalBoneTransforms);

				#endregion

				#region Draw Cone Pointer for Z Axis

				foreach (ModelMesh mesh in TranslationPointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						_coneZWorld = _cubeBoneTransforms[mesh.ParentBone.Index]*
						              Matrix.CreateScale(Scale)*
						              Matrix.CreateRotationX(MathHelper.ToRadians(90))*
						              Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z + LineLength));

						if (Camera != null)
						{
							effect.World = _coneZWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						if (SelectedAxis == SelectedAxis.Z)
						{
							effect.DiffuseColor = Color.Yellow.ToVector3();
							effect.AmbientLightColor = Color.Yellow.ToVector3();
						}
						else
						{
							effect.DiffuseColor = _zColor.ToVector3();
							effect.AmbientLightColor = _zColor.ToVector3();
						}
					}
					mesh.Draw();
				}

				TranslationPointer.CopyBoneTransformsFrom(_coneOriginalBoneTransforms);

				#endregion
			}

			#endregion

			#region Scale Pointer

			if (SelectMode == SelectedDisplayMode.Scale)
			{
				#region Draw Cube Pointer for X Axis

				foreach (ModelMesh mesh in ScalePointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{

						_cubeXWorld = _cubeBoneTransforms[mesh.ParentBone.Index] *
						              Matrix.CreateScale(Scale) *
						              Matrix.CreateTranslation(new Vector3(Position.X + LineLength, Position.Y, Position.Z));


						if (Camera != null)
						{
							effect.World = _cubeXWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						effect.AmbientLightColor = _xColor.ToVector3();
						effect.DiffuseColor = _xColor.ToVector3();

					}
					mesh.Draw();

				}

				// Copy our bone transforms from our original
				ScalePointer.CopyBoneTransformsFrom(_cubeOriginalBoneTransforms);

				#endregion

				#region Draw Cube Pointer for Y Axis

				foreach (ModelMesh mesh in ScalePointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{

						_cubeYWorld = _cubeBoneTransforms[mesh.ParentBone.Index] *
						              Matrix.CreateScale(Scale) *
						              Matrix.CreateTranslation(new Vector3(Position.X, Position.Y + LineLength, Position.Z));

						if (Camera != null)
						{
							effect.World = _cubeYWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						effect.DiffuseColor = _yColor.ToVector3();
						effect.AmbientLightColor = _yColor.ToVector3();

					}
					mesh.Draw();
				}

				// Copy our bone transforms from our original
				ScalePointer.CopyBoneTransformsFrom(_cubeOriginalBoneTransforms);

				#endregion

				#region Draw Cube Pointer for Z Axis

				foreach (ModelMesh mesh in ScalePointer.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{

						_cubeZWorld = _cubeBoneTransforms[mesh.ParentBone.Index] *
						              Matrix.CreateScale(Scale) *
						              Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z + LineLength));

						if (Camera != null)
						{
							effect.World = _cubeZWorld;
							effect.View = Camera.View;
							effect.Projection = Camera.Projection;
						}

						effect.EnableDefaultLighting();
						effect.PreferPerPixelLighting = true;

						effect.DiffuseColor = _zColor.ToVector3();
						effect.AmbientLightColor = _zColor.ToVector3();

					}
					mesh.Draw();
				}

				// Copy our bone transforms from our original
				ScalePointer.CopyBoneTransformsFrom(_cubeOriginalBoneTransforms);

				#endregion
			}

			#endregion

			base.Draw(gameTime);
		}

		public SelectedAxis RayIntersectsPointer(Ray ray)
		{
			if (SelectMode == SelectedDisplayMode.None)
				return SelectedAxis.None;

			// Translation Mode
			if (SelectMode == SelectedDisplayMode.Translation)
			{
				// check the translation pointers
				foreach (ModelMesh mesh in TranslationPointer.Meshes)
				{
					// Calculate our bounding sphere's on the 3 axis pointers
					BoundingSphere xSphere = TransformBoundingSphere(mesh.BoundingSphere, _coneXWorld);
					BoundingSphere ySphere = TransformBoundingSphere(mesh.BoundingSphere, _coneYWorld);
					BoundingSphere zSphere = TransformBoundingSphere(mesh.BoundingSphere, _coneZWorld);

					if (xSphere.Intersects(ray) != null)
						return SelectedAxis.X;

					if (ySphere.Intersects(ray) != null)
						return SelectedAxis.Y;

					if (zSphere.Intersects(ray) != null)
						return SelectedAxis.Z;

				}
			}

			// Scale Mode
			if (SelectMode == SelectedDisplayMode.Scale)
			{
				// check the scale pointers
				foreach (ModelMesh mesh in TranslationPointer.Meshes)
				{
					// Calculate our bounding sphere's on the 3 axis pointers
					BoundingSphere xSphere = TransformBoundingSphere(mesh.BoundingSphere, _cubeXWorld);
					BoundingSphere ySphere = TransformBoundingSphere(mesh.BoundingSphere, _cubeYWorld);
					BoundingSphere zSphere = TransformBoundingSphere(mesh.BoundingSphere, _cubeZWorld);

					if (xSphere.Intersects(ray) != null)
						return SelectedAxis.X;

					if (ySphere.Intersects(ray) != null)
						return SelectedAxis.Y;

					if (zSphere.Intersects(ray) != null)
						return SelectedAxis.Z;
				}
			}
			return SelectedAxis.None;
		}

		public BoundingSphere TransformBoundingSphere(BoundingSphere sphere, Matrix transform)
		{
			BoundingSphere transformedSphere;

			// Be sure to use the radius incase the sphere has been scaled at all!
			Vector3 scale = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);

			// Now transform it
			scale = Vector3.TransformNormal(scale, transform);

			transformedSphere.Radius = Math.Max(scale.X, Math.Max(scale.Y, scale.Z));
			transformedSphere.Center = Vector3.Transform(sphere.Center, transform);

			return transformedSphere;
		}
	}
}

#endif