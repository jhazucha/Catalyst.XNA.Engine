using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Environment;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.StructClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System.Collections.Generic;

namespace Catalyst3D.XNA.Testing.Graphics.Landscapes
{
	[TestFixture]
	public class LODLandscapePushPullTest : CatalystTestFixture
	{
		private Landscape Landscape;
		private BasicCamera Camera;
		
		private const int BrushSize = 2;

		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			GraphicsManager.PreferredBackBufferWidth = 800;
			GraphicsManager.PreferredBackBufferHeight = 600;

			// Create our camera
			Camera = new BasicCamera(this);
			Camera.Position = new Vector3(1024, 100, 1024);
			Camera.Target = new Vector3(0, -100, 0);
			Camera.FarPlane = 2000;
			Components.Add(Camera);

			// Create some input actions to move around the ground with
			var inputActions = new List<InputAction>
			                   	{
			                   		new InputAction("Exit", Buttons.Back, Keys.Escape),
			                   		new InputAction("Forward", Buttons.LeftThumbstickLeft, Keys.W),
			                   		new InputAction("Backward", Buttons.LeftThumbstickLeft, Keys.S),
			                   		new InputAction("Left", Buttons.LeftThumbstickLeft, Keys.A),
			                   		new InputAction("Right", Buttons.LeftThumbstickLeft, Keys.D),
			                   		new InputAction("Up", Buttons.LeftTrigger, Keys.PageUp),
			                   		new InputAction("Down", Buttons.RightTrigger, Keys.PageDown),
			                   	};

			// Add these input actions to our Input Handler
			InputHandler.Commands.AddRange(inputActions);

			// Create our Landscape
			Landscape = new Landscape(this, 128, 1024, 1024, Camera);
			Landscape.Smoothness = 1.5f;
			Landscape.SetShaderParameters += OnShaderParams;
			Components.Add(Landscape);

			Run();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			Landscape.CustomEffect = Content.Load<Effect>("EditorLandscapeShader");
			//ground = Content.Load<Texture2D>("checker");
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(InputHandler.IsActionPressed("Exit"))
				Exit();

			MouseState state = Mouse.GetState();
			Ray ray = Utilitys.CalculateRayFromCursor(GraphicsDevice, Camera, state.X, state.Y);

			for(int x = 0; x < Landscape.Blocks.Length; x++)
			{
				for(int y = 0; y < Landscape.Blocks[x].Length; y++)
				{
					if(Landscape.Blocks[x][y].Intersects(ray))
					{
						for(int v = 0; v < Landscape.Blocks[x][y].Verts.Length; v++)
						{
							// Get the position of the vertice
							Vector3 pos = Landscape.Blocks[x][y].Verts[v].Position;

							// Create a bounding box around the vertice padded with 1 unit in each direction
							BoundingBox box = new BoundingBox(new Vector3(pos.X - 1, pos.Y - 1, pos.Z - 1),
																								new Vector3(pos.X + 1, pos.Y + 1, pos.Z + 1));

							// Check to see if the 
							if(ray.Intersects(box) != null)
							{
								if(state.LeftButton == ButtonState.Pressed)
								{
									RaiseVertices(box, Landscape.Blocks[x][y].Verts, BrushSize, BrushSize);

									// Dispose the VB or memory will leak all over the place!
									Landscape.Blocks[x][y].VertexBuffer.Dispose();

									// Create the new VB
									Landscape.Blocks[x][y].VertexBuffer = new VertexBuffer(GraphicsDevice, Landscape.Blocks[x][y].Declaration,
																																				 Landscape.Blocks[x][y].Verts.Length, BufferUsage.None);
									
									Landscape.Blocks[x][y].VertexBuffer.SetData(Landscape.Blocks[x][y].Verts);
								}
							}
						}
					}
				}
			}
		}

		private void OnShaderParams(GameTime gametime)
		{
			Landscape.CustomEffect.CurrentTechnique = Landscape.CustomEffect.Techniques["WireFrame"];
			Landscape.CustomEffect.Parameters["World"].SetValue(Landscape.World);
			Landscape.CustomEffect.Parameters["View"].SetValue(Camera.View);
			Landscape.CustomEffect.Parameters["Projection"].SetValue(Camera.Projection);

			Landscape.CustomEffect.Parameters["AmbientLightColor"].SetValue(Color.White.ToVector4());
			Landscape.CustomEffect.Parameters["AmbientLightIntensity"].SetValue(0.6f);

			Landscape.CustomEffect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(-1, 1, 0));
			Landscape.CustomEffect.Parameters["DiffuseLightColor"].SetValue(Color.Blue.ToVector4());
			Landscape.CustomEffect.Parameters["DiffuseLightIntensity"].SetValue(50.0f);

		

		}

		private void RaiseVertices(BoundingBox box, CustomLandscapeVertex[] Verts, int brushsize, float weight)
		{
			for(int i = 0; i < Verts.Length; i++)
			{
				Vector3 min = new Vector3(Verts[i].Position.X - brushsize,
																	Verts[i].Position.Y - brushsize,
																	Verts[i].Position.Z - brushsize);

				Vector3 max = new Vector3(Verts[i].Position.X + brushsize,
																	Verts[i].Position.Y + brushsize,
																	Verts[i].Position.Z + brushsize);

				BoundingBox vertBox = new BoundingBox(min, max);

				if(box.Intersects(vertBox))
				{
					Verts[i].Position += new Vector3(0, weight, 0);
				}
			}
		}

	}
}