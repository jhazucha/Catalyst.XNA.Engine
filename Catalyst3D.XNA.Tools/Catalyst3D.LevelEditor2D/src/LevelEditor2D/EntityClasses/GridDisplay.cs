using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
  public class GridDisplay : VisualObject
  {
    private VertexPositionColor[] vVerts;
    private VertexPositionColor[] hVerts;
    private Effect gridShader;

    public Color GridColor = Color.White;

    public const int xLineCount = 62;
    public const int yLineCount = 58;
    public const float spacing = 0.1f;

    public GridDisplay(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();

      // Create our vertex declaration
    	//vDeclaration = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());

      vVerts = new VertexPositionColor[xLineCount];
      hVerts = new VertexPositionColor[yLineCount];

      Vector2 pos = new Vector2(-3, -3);

      // Vertical Lines
      for (int i = 0; i < xLineCount; i += 2)
      {
        vVerts[i].Position = new Vector3(pos.X + (i * spacing), pos.Y, 0);
        vVerts[i].Color = GridColor;

        vVerts[i + 1].Position = new Vector3(pos.X + (i * spacing), -pos.Y, 0);
        vVerts[i + 1].Color = GridColor;
      }

      // Horizontal Lines
      for (int i = 0; i < yLineCount; i += 2)
      {
        hVerts[i].Position = new Vector3(pos.X, pos.Y + (i * spacing), 0);
        hVerts[i].Color = GridColor;

        hVerts[i + 1].Position = new Vector3(-pos.X, pos.Y + (i * spacing), 0);
        hVerts[i + 1].Color = GridColor;
      }
    }

    public override void LoadContent()
    {
      base.LoadContent();

      // Load our Grid Shader
    	gridShader = GameScreen.Content.Load<Effect>("GridShader");
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

      Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 1))*Matrix.CreateScale(1, 1, 1);

      gridShader.Parameters["World"].SetValue(world);
      gridShader.Parameters["GridColor"].SetValue(GridColor.ToVector4());

			foreach(EffectPass pass in gridShader.CurrentTechnique.Passes)
			{
				pass.Apply();

				// Draw our Vertical Grid Lines
				GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vVerts, 0, vVerts.Length/2);

				// Draw our Horizontal Grid Lines
				GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, hVerts, 0, hVerts.Length/2);
			}
    }
  }
}