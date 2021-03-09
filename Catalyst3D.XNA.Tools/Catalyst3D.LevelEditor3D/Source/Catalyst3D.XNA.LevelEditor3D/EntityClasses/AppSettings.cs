using System;
using Microsoft.Xna.Framework;

namespace LevelEditor3D.EntityClasses
{
	[Serializable]
	public class AppSettings
	{
		public string ContentPath;
		public Color RenderSurfaceColor = Color.Black;
		
		public int Resolution;
	}
}