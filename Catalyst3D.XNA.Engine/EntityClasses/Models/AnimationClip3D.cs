using System;
using System.Collections.Generic;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models
{
  public class CTAnimationClip3D
  {
    public TimeSpan Duration { get; set; }
    public IList<CTKeyframe3D> Keyframes { get; set; }

    public CTAnimationClip3D(TimeSpan duration, IList<CTKeyframe3D> keyframes)
    {
      Duration = duration;
      Keyframes = keyframes;
    }
  }
}