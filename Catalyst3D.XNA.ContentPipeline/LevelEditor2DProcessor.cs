using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = Catalyst3D.XNA.Engine.CatalystProject2D;
using TOutput = Catalyst3D.XNA.Engine.CatalystProject2D;

namespace Catalyst3D.XNA.ContentPipeline
{
	[ContentProcessor(DisplayName = "Catalyst 2D Level Editor Processor")]
	public class LevelEditor2DProcessor : ContentProcessor<TInput, TOutput>
	{
		public override TOutput Process(TInput input, ContentProcessorContext context)
		{
			return input;
		}
	}
}