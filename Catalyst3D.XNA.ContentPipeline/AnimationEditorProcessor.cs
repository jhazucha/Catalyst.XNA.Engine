using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = Catalyst3D.XNA.Engine.EntityClasses.Actor;
using TOutput = Catalyst3D.XNA.Engine.EntityClasses.Actor;

namespace Catalyst3D.XNA.ContentPipeline
{
  [ContentProcessor(DisplayName = "Catalyst Animation Editor Processor")]
  public class AnimationEditorProcessor : ContentProcessor<TInput, TOutput>
  {
    public override TOutput Process(TOutput input, ContentProcessorContext context)
    {
      return input;
    }
  }
}