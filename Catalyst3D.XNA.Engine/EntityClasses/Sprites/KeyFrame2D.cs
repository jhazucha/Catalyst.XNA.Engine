using System;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Sprites
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	public class KeyFrame2D
	{
		public int FrameNumber;
		public string AssetFilename { get; set; }
		public Rectangle SourceRectangle;
		public float Duration;
	}
}