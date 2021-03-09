using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = Catalyst3D.XNA.Engine.EntityClasses.Actor;

namespace Catalyst3D.XNA.ContentPipeline
{
  [ContentImporter(".a2d", DisplayName = "Catalyst Animation Editor Importer", DefaultProcessor = "")]
  public class AnimationEditorImporter : ContentImporter<TImport>
  {
    public override TImport Import(string filename, ContentImporterContext context)
    {
      return Serializer.Deserialize<TImport>(filename);
    }
  }
}
