using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.EntityClasses.Sprites
{
	public class Sequence2D
	{
		public string Name { get; set; }
		public int StartFrame;
    public int EndFrame;

    [ContentSerializer(SharedResource = true)]
    public List<KeyFrame2D> Frames = new List<KeyFrame2D>();

		public Sequence2D() { }

		public Sequence2D(string name)
		{
		  Name = name;
		}
	}
}