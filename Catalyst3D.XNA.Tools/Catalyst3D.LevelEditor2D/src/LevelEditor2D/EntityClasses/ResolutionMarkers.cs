using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.EntityClasses
{
  public class ResolutionMarkers : VisualObject
  {
    private Texture2D RedLine;
    private Texture2D GrayLine;
    private Texture2D BlueLine;

    private const int ScreenCount = 20;

    public ResolutionMarkers(Game game) : base(game)
    {
    }

		public override void LoadContent()
		{
			// Init our Sprite Batch
			if (SpriteBatch == null)
				SpriteBatch = new SpriteBatch(Globals.Game.GraphicsDevice);

			// Set the data on our textures

			if (RedLine == null)
				RedLine = Utilitys.GenerateTexture(Globals.Game.GraphicsDevice, Color.Red, 1, 1);

			if (GrayLine == null)
				GrayLine = Utilitys.GenerateTexture(Globals.Game.GraphicsDevice, Color.Gray, 1, 1);

			if (BlueLine == null)
				BlueLine = Utilitys.GenerateTexture(Globals.Game.GraphicsDevice, Color.LightBlue, 1, 1);

			base.LoadContent();
		}

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

      if (SpriteBatch != null)
      {
      	SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null,
      	                  Matrix.CreateTranslation(Globals.CurrentCameraOffsetX, Globals.CurrentCameraOffsetY, 0));

        int height = 0;
        int width = 0;

        int titleSafeX;
        int titleSafeY;

				switch(Globals.AppSettings.Resolution)
				{
					case 0:
						height = 1080;
						width = 1920;
						titleSafeX = 432; // 864
						titleSafeY = 768; // 1536

						// First Pane Left Side 1080p
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2, 1, 50), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2, 50, 1), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2 + 1486, 1, 50), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2 + 1536, 50, 1), Color.White);

						break;

					case 1:
						height = 720;
						width = 1280;
						titleSafeX = 256; // 1024
						titleSafeY = 144; // 576

						// First Pane Left Side 720p
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2, 1, 50), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2, 50, 1), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2 + 526, 1, 50), Color.White);
						SpriteBatch.Draw(BlueLine, new Rectangle(titleSafeX/2, titleSafeY/2 + 576, 50, 1), Color.White);
						
						break;

					case 2:
						height = 480;
						width = 800;

						// TODO: Title Safe Areas

						break;

					case 3:
						height = 800;
						width = 480;

						// TODO: Title Safe Areas

						break;
				}

      	// Draw Top Line
        SpriteBatch.Draw(RedLine, new Rectangle(0, -1, width * ScreenCount, 1), Color.White);

        // Draw Left Line
        SpriteBatch.Draw(RedLine, new Rectangle(0, 0, 1, height), Color.White);

        // Draw Bottom Line
        SpriteBatch.Draw(RedLine, new Rectangle(0, height, width * ScreenCount, 1), Color.White);

        // Draw screen break markers
        for (int i = 1; i < ScreenCount; i++)
          SpriteBatch.Draw(GrayLine, new Rectangle(width*i, 0, 1, height), Color.White);

        // Far Right Line
        SpriteBatch.Draw(RedLine, new Rectangle(width*ScreenCount, 0, 1, height), Color.White);

        SpriteBatch.End();
      }
    }

    public override void UnloadContent()
    {
      base.UnloadContent();

    	SpriteBatch = null;
    	BlueLine = null;
    	RedLine = null;
    	GrayLine = null;
    }
  }
}