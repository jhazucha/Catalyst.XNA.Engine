using System.Collections.Generic;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Catalyst3D.XNA.Engine
{
  public class AudioManager : UpdatableObject
  {
    private readonly ContentManager ContentManager;
    private readonly Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
    private readonly Dictionary<string, Song> Music = new Dictionary<string, Song>();
    private readonly Dictionary<string, SoundEffectInstance> PlayingSounds;

    public string AudioFolder;
    public string CurrentSong;

    public float MusicVolume = 1;
    public float SoundEffectVolume = 1;

    public AudioManager(Game game, string audioFolder, float mVolume, float sVolume)
      : base(game)
    {
      MusicVolume = mVolume;
      SoundEffectVolume = sVolume;
      AudioFolder = audioFolder;

      // WP7 = 16 / XBOX 360 = 300?
      PlayingSounds = new Dictionary<string, SoundEffectInstance>(16);

      // Local Content Manager
      ContentManager = new ContentManager(Game.Services, AudioFolder);
    }

    public void Add<T>(string name)
    {
			if (typeof(T) == typeof(SoundEffect))
			{
				if (SoundEffects.ContainsKey(name))
					return;

				// sound effect
				if (!IsInitialized)
					Initialize();

				// Create the sound
				SoundEffect sound = ContentManager.Load<SoundEffect>(name);
				SoundEffects.Add(name, sound);
			}
			else
			{
				Song song = ContentManager.Load<Song>(name);
				Music.Add(name, song);
			}
    }

    public void Play<T>(string name, float vol, int pitch, int pan, bool looped)
    {
      if (string.IsNullOrEmpty(name))
        return;

      if (typeof(T) == typeof(SoundEffect))
      {
        if (SoundEffects.ContainsKey(name))
        {
          SoundEffect sound;

          if (SoundEffects.TryGetValue(name, out sound))
          {
            // Re-load it if its been disposed (unloaded)
            if (sound.IsDisposed)
              sound = ContentManager.Load<SoundEffect>(name);

            PlayingSounds.Add(name + PlayingSounds.Count, sound.CreateInstance());
            PlayingSounds[name] = sound.CreateInstance();
            PlayingSounds[name].Volume = vol;
            PlayingSounds[name].Pitch = pitch;
            PlayingSounds[name].Pan = pan;
            PlayingSounds[name].IsLooped = looped;
            PlayingSounds[name].Play();

            if (!Enabled)
              PlayingSounds[name].Pause();
          }
        }
      }
      else if (typeof(T) == typeof(Song))
      {
        if (Music.ContainsKey(name))
        {
          if (CurrentSong == name)
            return;

          if (MediaPlayer.State == MediaState.Playing)
            MediaPlayer.Stop();

          if (MediaPlayer.State == MediaState.Paused)
            MediaPlayer.Resume();
          else
          {
            MediaPlayer.Volume = MusicVolume;
            MediaPlayer.Play(Music[name]);
            MediaPlayer.IsRepeating = true;
          }

          CurrentSong = name;
        }
      }
    }

    public SoundEffectInstance Get<T>(string name)
    {
      if (typeof(T) == typeof(SoundEffect))
      {
        SoundEffectInstance effect;

        if (PlayingSounds.TryGetValue(name, out effect))
          return effect;
      }
      return null;
    }

    public void PauseMusic()
    {
      if (MediaPlayer.State == MediaState.Playing)
        MediaPlayer.Pause();
    }
    public void ResumeMusic()
    {
      if (MediaPlayer.State == MediaState.Paused)
      {
        MediaPlayer.Volume = MusicVolume;
        MediaPlayer.Resume();
      }
    }
    public void StopMusic()
    {
      if (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused)
      {
        CurrentSong = string.Empty;

        MediaPlayer.Stop();
      }
    }

    public void ChangePitch(string name, float val)
    {
      if (PlayingSounds.ContainsKey(name))
      {
        if (PlayingSounds[name].IsDisposed)
          return;

        PlayingSounds[name].Stop(true);
        PlayingSounds[name].Pitch = val;
        PlayingSounds[name].Play();
      }
    }

    public void StopSound(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;

      if (PlayingSounds.ContainsKey(name))
      {
        if (PlayingSounds[name].IsDisposed)
          return;

        PlayingSounds[name].Stop(true);
        PlayingSounds[name].Dispose();
        PlayingSounds[name] = null;
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      var soundRemovals = new List<string>();

      foreach (var p in PlayingSounds)
      {
        if (p.Value.State == SoundState.Stopped)
        {
          soundRemovals.Add(p.Key);
        }
      }

      // Flush them out
      foreach (var r in soundRemovals)
        PlayingSounds.Remove(r);

      if (CurrentSong != null && MediaPlayer.State == MediaState.Stopped)
        CurrentSong = null;
    }

    protected override void Dispose(bool disposing)
    {
      CurrentSong = null;
      
      ContentManager.Unload();

      PlayingSounds.Clear();
      SoundEffects.Clear();
      Music.Clear();

      base.Dispose(disposing);
    }

    public void UnloadContent()
    {
      ContentManager.Unload();
    }
  }
}