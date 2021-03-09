namespace Catalyst3D.XNA.Engine.EnumClasses
{
  public enum EmitterType
  {
    Point,
    Volume
  }

  public enum SelectedDisplayMode
  {
    None,
    Rotation,
    Translation,
    Scale
  }

  public enum SelectedAxis
  {
    None,
    X,
    Y,
    Z
  }

  public enum BoundingType
  {
    Box,
    Sphere,
    Plane
  }

  public enum Direction
  {
    Up,
    Down,
    Left,
    Right
  }

  public enum ButtonState
  {
    None,
    Selected
  }

  public enum Fading
  {
    In,
    Out
  }

  public enum ScreenState
  {
    TransitionOn,
    Active,
    TransitionOff,
    Hidden,
    Behind,
    Exiting
  }

  public enum TimerType
  {
    CountUp,
    CountDown
  }

  public enum ActorRole
  {
    Player = 0,
    Enemy = 1,
    NPC = 2,
    Prop = 3
  }

  public enum ActorState
  {
    Idle,
    Walking,
    Running,
    Falling,
    Dieing,
    Dead,
    Punching,
    Kicking,
    TakingDamage,
    Jumping
  }

  public enum LedgeState
  {
    Attached,
    Seeking
  }

  public enum LedgeTravelAlgo
  {
    RespawnAtFirstNode,
    LerpToFirstNode,
    StopAtLastNode,
    RandomlyPickNextNode,
    HideAfterLastNode
  }

  public enum LedgeRole
  {
    Path = 0,
    Ground = 1,
    Ledge = 2,
    Stairs = 3,
    Boundry = 4,
    HitBox = 5
  }

  public enum LandscapeBlockEdge
  {
    Top,
    Bottom,
    Left,
    Right
  }

  public enum ObjectState
  {
    Alive,
    Dieing,
    Dead,
    TransitioningOff
  }
  public enum SliderType
  {
    Horizontal,
    Vertical
  }

  public enum SlidingDirection
  {
    Left,
    Right,
    Up,
    Down
  }

  public enum GameState
  {
    InMenus,
    CountDown,
    Running,
    Paused,
    GameOver,
    NavigatedAway,
    NavigatedTo,
    TrainingCompleted
  }
  public enum AudioType
  {
    Music,
    SoundFX
  }
}