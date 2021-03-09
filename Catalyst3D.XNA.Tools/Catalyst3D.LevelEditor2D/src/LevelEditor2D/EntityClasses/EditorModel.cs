using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Models;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
  public class EditorModel : Model
  {
    public EditorModel(Game game, BasicCamera cam) : base(game, cam)
    {
    }

    public EditorModel(Game game, string asset, BasicCamera cam) : base(game, asset, cam)
    {
    }
  }
}
