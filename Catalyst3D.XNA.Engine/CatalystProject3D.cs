using System.Collections.Generic;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine
{
	public class CatalystProject3D
	{
		[ContentSerializer(SharedResource = true)]
		public List<VisualObject> SceneObjects = new List<VisualObject>();
	}
}
