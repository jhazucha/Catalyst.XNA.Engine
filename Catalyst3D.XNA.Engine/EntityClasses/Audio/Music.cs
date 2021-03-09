using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework.Media;

namespace Catalyst3D.XNA.Engine.EntityClasses.Audio
{
	public class Music
	{
		public int Index;
    
    public bool IsLooped;

    public Song Content;
	  public AudioType Type;

	  public Music(bool isLooped)
		{
			IsLooped = isLooped;
		}
	}
}
