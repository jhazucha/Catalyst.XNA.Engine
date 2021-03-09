#if !WINDOWS_PHONE

using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Catalyst3D.XNA.Engine.EntityClasses.Audio
{
	public class AudioPlayer
	{
		private AudioEngine engine;
		private WaveBank wavebank;
		private SoundBank soundbank;

		public List<string> CueNames = new List<string>();

		/// <summary>
		/// Plays a sound
		/// </summary>
		/// <param name="cueName">Which sound to play</param>
		/// <returns>XACT cue to be used if you want to stop this particular looped 
		/// sound. Can be ignored for one shot sounds</returns>
		public Cue Play(string cueName)
		{
			Cue returnValue = soundbank.GetCue(cueName);
			returnValue.Play();
			return returnValue;
		}
		
		/// <summary>
		/// Stops a previously playing cue
		/// </summary>
		/// <param name="cue">The cue to stop that you got returned from Play(sound)
		/// </param>
		public void Stop(Cue cue)
		{
			if (cue != null)
			{
				cue.Stop(AudioStopOptions.Immediate);
			}
		}
		
		/// <summary>
		/// Init the Sound Banks
		/// </summary>
		/// <param name="xgs">XACT Global Settings Filename</param>
		/// <param name="xwb">Wave Bank Audio Filename</param>
		/// <param name="xsb">Sound Bank Filename</param>
		public void Initialize(string xgs, string xwb, string xsb)
		{
			engine = new AudioEngine(xgs);
			wavebank = new WaveBank(engine, xwb);
			soundbank = new SoundBank(engine, xsb);
		}

		/// <summary>
		/// Shuts down the sound code tidily
		/// </summary>
		public void Shutdown()
		{
			if (soundbank != null) soundbank.Dispose();
			if (wavebank != null) wavebank.Dispose();
			if (engine != null) engine.Dispose();
		}
	}
}

#endif