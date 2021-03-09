using System;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
#if !WINDOWS_PHONE
  [Serializable]
#endif
  public class LedgeNode
  {
    [ContentSerializer]
    public float TravelSpeed { get; set; }

    [ContentSerializer]
    public float Rotation { get; set; }

    [ContentSerializer]
    public Vector2 Position { get; set; }

    [ContentSerializer]
    public Vector2 Scale = Vector2.One;

    [ContentSerializer]
    public int DrawOrder;

    [ContentSerializer]
    public Sequence2D AnimationSequence { get; set; }

    [ContentSerializer]
    public bool IsLooped { get; set; }

    [ContentSerializer]
    public SpriteEffects SpriteEffect { get; set; }

    [ContentSerializer]
    public TimeSpan RespawnDelay { get; set; }

    [ContentSerializer]
    public TimeSpan NodeElapsedTime { get; set; }
  }
}