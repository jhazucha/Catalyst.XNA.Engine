using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework.Audio;

namespace Catalyst3D.XNA.Engine.EntityClasses.Audio
{
	public class Sound
	{
		public SoundEffect Content;
		public SoundEffectInstance Instance;

		public float Pitch;
		public readonly bool IsLooped;
		public float Volume = 1;

	  public AudioType Type;

		public SoundState State
		{
			get
			{
				if(Instance != null)
					return Instance.State;

				return SoundState.Stopped;
			}
		}

		public Sound(bool isLooped)
		{
			IsLooped = isLooped;
		}

		public void Initialize()
		{
			if(Content != null)
			{
				Instance = Content.CreateInstance();
				Instance.IsLooped = IsLooped;
			}
		}
	}
}
