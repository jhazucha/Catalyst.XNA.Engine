using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.EntityClasses.Audio;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Audio
{
	public class AudioManagerTest : CatalystTestFixture
	{
		private AudioManager AudioManager;

		private Button btnPlay;
		private Button btnStop;
		private Button btnPitchUp;
		private Button btnPitchDown;

		[Test]
		public void Test()
		{
			IsMouseVisible = true;

			GraphicsManager.PreferredBackBufferWidth = 600;
			GraphicsManager.PreferredBackBufferHeight = 200;

			AudioManager = new AudioManager(this, 1, 1);

			// Add a wav sound effect
			AudioManager.Add<Sound>("Motorbike-Idle", true);

      // Add a song to play too
		  AudioManager.Add<Music>("jungleloop", true);
		  AudioManager.MusicVolume = 0.3f;

			// Create a Play Button
			btnPlay = new Button(this);
			btnPlay.OnClick += OnPlayClicked;
			btnPlay.Position = new Vector2(100, 100);
			btnPlay.Scale = new Vector2(3, 3);
			Components.Add(btnPlay);

			// Create a Stop Button
			btnStop = new Button(this);
			btnStop.OnClick += OnStopClicked;
			btnStop.Position = new Vector2(200, 100);
			btnStop.Scale = new Vector2(3, 3);
			Components.Add(btnStop);

			// Create Pitch Up Button
			btnPitchUp = new Button(this);
			btnPitchUp.OnClick += OnPitchUpClicked;
			btnPitchUp.Position = new Vector2(300, 100);
			btnPitchUp.Scale = new Vector2(3, 3);
			Components.Add(btnPitchUp);

			// Create Pitch Down Button
			btnPitchDown = new Button(this);
			btnPitchDown.OnClick += OnPitchDownClicked;
			btnPitchDown.Position = new Vector2(400, 100);
			btnPitchDown.Scale = new Vector2(3, 3);
			Components.Add(btnPitchDown);

			Run();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our button textures
			btnPlay.Texture = Content.Load<Texture2D>("control_play");
			btnStop.Texture = Content.Load<Texture2D>("control_stop");

			btnPitchUp.Texture = Content.Load<Texture2D>("arrow_up");
			btnPitchDown.Texture = Content.Load<Texture2D>("arrow_down");

		}

		private void OnPitchDownClicked()
		{
			float pitch = AudioManager.Get<Sound>("Motorbike-Idle").Pitch;

			pitch -= 0.1f;

			AudioManager.ChangePitch<Sound>("Motorbike-Idle", pitch);
		}

		private void OnPitchUpClicked()
		{
			float pitch = AudioManager.Get<Sound>("Motorbike-Idle").Pitch;


			pitch += 0.1f;

			AudioManager.ChangePitch<Sound>("Motorbike-Idle", pitch);
		}

		private void OnStopClicked()
		{
			AudioManager.Stop<Sound>("Motorbike-Idle");
		}
		private void OnPlayClicked()
		{
			AudioManager.Play<Sound>("Motorbike-Idle");
		}
	}
}
