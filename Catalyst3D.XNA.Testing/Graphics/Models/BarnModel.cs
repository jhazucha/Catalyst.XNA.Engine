using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Models;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
  public class BarnModel : CatalystTestFixture
  {
    private Model barnModel;
    private BasicCamera camera;

    [Test]
    public void Test()
    {
      camera = new BasicCamera(this);
      camera.Position = new Vector3(-50, 25, 50);
      Components.Add(camera);

      barnModel = new Model(this, "barn_rev3", camera);
      Components.Add(barnModel);

      Run();
    }
  }
}
