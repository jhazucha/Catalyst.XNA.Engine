using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Testing.EntityClasses
{
  public class Jude : Actor
  {
    public int MoveSpeed = 90;
    public int LeanSpeed = 40;

    public int Health;
    public int Lives = 5;

    public float MaxBikeLean = 25.0f;

    private InputHandler Input;

    public Jude(Game game) : base(game, string.Empty, string.Empty) { }

  	public Jude()
  	{
  	}

  	public override void Initialize()
    {
      // Grab an Instance of the Input Handler Service in the main Game Class
      Input = (InputHandler)Game.Services.GetService(typeof(InputHandler));

      base.Initialize();
    }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Position += new Vector2(5f, 0);
		}
  }
}
