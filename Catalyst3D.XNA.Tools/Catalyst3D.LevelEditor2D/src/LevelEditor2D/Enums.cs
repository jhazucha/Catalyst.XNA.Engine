namespace LevelEditor2D
{
  public class Enums
  {
    public enum PickingState
    {
      PickingObject,
			PickingNode,
      Idle
    }
    public enum CameraState
    {
      Panning,
      Idle
    }
    public enum ActorType
    {
      Player1,
      Player2,
      Player3,
      Player4,
      BasicEnemy,
      AdvancedEnemy,
      Boss
    }

		public enum SceneState
		{
			Playing,
			Stopped,
			Paused
		}
  }
}