using System.Collections.Generic;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.ContentTypeReader
{
	public class CT2DProjectReader : ContentTypeReader<CT2DProject>
	{
		protected override CT2DProject Read(ContentReader input, CT2DProject existingInstance)
		{
			existingInstance = new CT2DProject();
			existingInstance.Ledges = input.ReadObject<List<CTLedge>>();
			existingInstance.SceneObjects = input.ReadObject<List<CTVisualObject>>();

			return existingInstance;
		}
	}
}