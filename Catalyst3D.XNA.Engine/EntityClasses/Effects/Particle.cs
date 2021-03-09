using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Effects
{
  public class Particle
  {
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    
    public Color Color = Color.Black;
    public Texture2D Texture;

    public float Scale;
    public float TimeSinceStart;
    public float LifeSpan;
    public float AlphaValue;
    public float Rotation;
    public float RotationSpeed;

    public bool Active
    {
      get { return TimeSinceStart < LifeSpan; }
    }

    public void Update(float time)
    {
      Velocity += Acceleration * time;
      Position += Velocity*time;
      Rotation += RotationSpeed * time;

      TimeSinceStart += time;   
    }

    public void Initialize(Vector2 position, Vector2 velocity, Vector2 accel, float lifeSpan, float scale, float speed)
    {
      // set the values to the requested values
      Position = position;
      Velocity = velocity;
      Acceleration = accel;
      LifeSpan = lifeSpan;
      Scale = scale;
      RotationSpeed = speed;
      TimeSinceStart = 0.0f;
      //Rotation = Utilitys.RandomBetween(0, MathHelper.TwoPi);
    }
  }
}