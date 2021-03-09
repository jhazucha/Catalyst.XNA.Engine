using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = Catalyst3D.XNA.Engine.CatalystProject2D;

namespace Catalyst3D.XNA.ContentPipeline
{
	[ContentImporter(".c2d", DisplayName = "Catalyst 2D Level Editor Importer", DefaultProcessor = "")]
	public class LevelEditor2DImporter : ContentImporter<TImport>
	{
		public override TImport Import(string filename, ContentImporterContext context)
		{
			return Serializer.Deserialize<TImport>(filename);
		}
	}
}