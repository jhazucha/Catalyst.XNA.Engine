using System;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models
{
	public class CTKeyframe3D
	{
		private readonly int boneValue;
		private readonly TimeSpan timeValue;
		private readonly Matrix transformValue;
    
    public int Bone
    {
      get { return boneValue; }
    }

    public TimeSpan Time
    {
      get { return timeValue; }
    }

    public Matrix Transform
    {
      get { return transformValue; }
    }

		public CTKeyframe3D(int bone, TimeSpan time, Matrix transform)
		{
			boneValue = bone;
			timeValue = time;
			transformValue = transform;
		}
	}
}