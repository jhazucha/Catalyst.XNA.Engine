using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Audio
{
	public class SoundEffects : CatalystTestFixture
	{
		private SoundEffect sound;
		private SoundEffectInstance soundInstance;
		
		private Button btnPlay;
		private Button btnStop;
		private Button btnPitchUp;
		private Button btnPitchDown;

		private float CurrentPitch;

		[Test]
		public void Test()
		{
			Run();
		}

		protected override void Initialize()
		{
			IsMouseVisible = true;

			btnPlay = new Button(this);
			btnPlay.OnClick += OnPlayClicked;
			btnPlay.Position = new Vector2(100, 100);
			btnPlay.Scale = new Vector2(3, 3);
			Components.Add(btnPlay);

			btnStop = new Button(this);
			btnStop.OnClick += OnStopClicked;
			btnStop.Position = new Vector2(200, 100);
			btnStop.Scale = new Vector2(3, 3);
			Components.Add(btnStop);

			btnPitchUp = new Button(this);
			btnPitchUp.OnClick += OnPitchUpClicked;
			btnPitchUp.Position = new Vector2(300, 100);
			btnPitchUp.Scale = new Vector2(3, 3);
			Components.Add(btnPitchUp);

			btnPitchDown = new Button(this);
			btnPitchDown.OnClick += OnPitchDownClicked;
			btnPitchDown.Position = new Vector2(400, 100);
			btnPitchDown.Scale = new Vector2(3, 3);
			Components.Add(btnPitchDown);

			base.Initialize();
		}

		private void OnPitchDownClicked()
		{

			CurrentPitch -= 0.05f;

			if(CurrentPitch < -1)
				CurrentPitch = -1;
		}

		private void OnPitchUpClicked()
		{
			CurrentPitch += 0.05f;

			if(CurrentPitch > 1)
				CurrentPitch = 1;
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Load our content
			sound = Content.Load<SoundEffect>("Motorbike-Idle");
			
			// Setup our Sound Effect Instance
			soundInstance = sound.CreateInstance();
			soundInstance.IsLooped = true;

			// Load our button textures
			btnPlay.Texture = Content.Load<Texture2D>("control_play");
			btnStop.Texture = Content.Load<Texture2D>("control_stop");

			btnPitchUp.Texture = Content.Load<Texture2D>("arrow_up");
			btnPitchDown.Texture = Content.Load<Texture2D>("arrow_down");

		}

		private void OnStopClicked()
		{
			if (soundInstance.State == SoundState.Playing)
				soundInstance.Stop();
		}
		private void OnPlayClicked()
		{
			if (soundInstance.State == SoundState.Playing)
				return;

			soundInstance.Play();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (soundInstance != null)
				soundInstance.Pitch = CurrentPitch;
		}


	}
}
