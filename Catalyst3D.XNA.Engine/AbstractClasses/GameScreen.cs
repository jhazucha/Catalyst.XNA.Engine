using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EntityClasses.Input;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.AbstractClasses
{
  public class GameScreen : VisualObject
	{
		private readonly List<VisualObject> QueuedAdditions = new List<VisualObject>();
		private readonly List<VisualObject> QueuedRemovals = new List<VisualObject>();
		
		private Texture2D black;
		private GestureType enabledGestures = GestureType.None;
		private bool isUpdating;
    private float screenAlpha = 1;

		public delegate void TransitionEffect();

		public event TransitionEffect OnScreenTransition;

		[ContentSerializerIgnore, XmlIgnore] public AudioManager AudioManager;
		[ContentSerializerIgnore, XmlIgnore] public InputHandler InputHandler;

    [ContentSerializerIgnore, XmlIgnore] public ScreenState State = ScreenState.TransitionOn;

		[ContentSerializerIgnore, XmlIgnore] public TimeSpan ElapsedTransition = TimeSpan.Zero;
		[ContentSerializerIgnore, XmlIgnore] public bool IsDialogWindow;

    [ContentSerializerIgnore, XmlIgnore] public TimeSpan FadeSpeed = TimeSpan.FromSeconds(1.5f);
	  [ContentSerializerIgnore, XmlIgnore] public float TotalFadeAlpha = 0.5f;

		[ContentSerializerIgnore, XmlIgnore]
		public GestureType EnabledGestures
		{
			get { return enabledGestures; }
			set
			{
				enabledGestures = value;
				TouchPanel.EnabledGestures = value;
			}
		}

		[ContentSerializerIgnore, XmlIgnore] public SceneManager SceneManager;
		[ContentSerializerIgnore, XmlIgnore] public ContentManager Content;
		[ContentSerializerIgnore, XmlIgnore] public ContentManager AudioContent;

		[ContentSerializer] public string AudioFolder = string.Empty;
		[ContentSerializer] public string ProjectFolder = string.Empty;
		[ContentSerializer] public string ProjectFileName = string.Empty;

		[ContentSerializer] public List<VisualObject> VisualObjects = new List<VisualObject>();
		[ContentSerializer] public List<Ledge> Paths = new List<Ledge>();
		[ContentSerializer] public List<UpdatableObject> UpdatableObjects = new List<UpdatableObject>();
	  [ContentSerializer] public Color BackgroundColor = Color.Black;

#if WINDOWS
		public GameScreen()
			: base(null)
		{
		}
#endif

		public GameScreen(Game game, string assetFolder)
			: base(game)
		{
			AssetFolder = assetFolder;
		  EnabledGestures = GestureType.Tap;
		}

		public GameScreen(Game game, bool isDialog, string assetFolder)
			: base(game)
		{
			IsDialogWindow = isDialog;

			AssetFolder = assetFolder;
      EnabledGestures = GestureType.Tap;
		}

		/*
    public virtual void Activate(bool withInstance)
    {
      if (SceneManager.SpriteBatch == null)
        throw new Exception("Error GS: Scene Managers Sprite Batch is NULL!");

      // Store a reference of sprite batch for rendering sprites on our game screen
      SpriteBatch = SceneManager.SpriteBatch;

      // Init a new content manager
      if (Content == null)
      {
        if (!string.IsNullOrEmpty(AssetFolder))
          Content = new ContentManager(Game.Services, Game.Content.RootDirectory + "/" + AssetFolder);
        else
          Content = new ContentManager(Game.Services, Game.Content.RootDirectory);
      }

      if (AudioManager == null)
      {
        // Init our Audio Manager
        AudioManager = new AudioManager(Game, AudioFolder, 1, 1);
        AddUpdatableObject(AudioManager);
      }

      // Create a black texture for doing menu transitions
      if (black == null || black.IsDisposed)
        black = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.Black, Game.GraphicsDevice.Viewport.Width,
                                         Game.GraphicsDevice.Viewport.Height);

      if (!IsLoaded)
      {
        foreach (var v in VisualObjects.Where(v => v != null))
        {
          v.SpriteBatch = SceneManager.SpriteBatch;
          v.GameScreen = this;

          // Load its content
          v.LoadContent();
        }
      }

      IsInitialized = true;
    }

    public virtual void Deactivate() { }
		*/

		public override void Initialize()
		{
			if (SceneManager.SpriteBatch == null)
				throw new Exception("Error GS: Scene Managers Sprite Batch is NULL!");

			// Store a reference of sprite batch for rendering sprites on our game screen
			SpriteBatch = SceneManager.SpriteBatch;

			// Init a new content manager
			if (Content == null)
			{
				if (!string.IsNullOrEmpty(AssetFolder))
					Content = new ContentManager(Game.Services, Game.Content.RootDirectory + "/" + AssetFolder);
				else
					Content = new ContentManager(Game.Services, Game.Content.RootDirectory);
			}

			if (AudioManager == null)
			{
				// Init our Audio Manager
				AudioManager = new AudioManager(Game, AudioFolder, 1, 1);
				AddUpdatableObject(AudioManager);
			}

			base.Initialize();

			IsInitialized = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			// Create a black texture for doing menu transitions
			if (black == null || black.IsDisposed)
				black = Utilitys.GenerateTexture(Game.GraphicsDevice, Color.Black, Game.GraphicsDevice.Viewport.Width,
				                                 Game.GraphicsDevice.Viewport.Height);

			foreach (var v in VisualObjects)
			{
				if (v != null && !v.IsLoaded)
				{
					v.SpriteBatch = SceneManager.SpriteBatch;
					v.GameScreen = this;

					// Load its content
					v.LoadContent();
				}
			}
		}

  	public override void Update(GameTime gameTime)
    {
      if (State == ScreenState.Hidden)
        return;

      base.Update(gameTime);

      isUpdating = true;

      if (State == ScreenState.Behind && OnScreenTransition == null)
      {
        screenAlpha += (float) gameTime.ElapsedGameTime.TotalMilliseconds/(float) FadeSpeed.TotalMilliseconds;
        screenAlpha = MathHelper.Clamp(screenAlpha, 0, TotalFadeAlpha);
      }
      else
      {
        screenAlpha -= (float) gameTime.ElapsedGameTime.TotalMilliseconds/(float) FadeSpeed.TotalMilliseconds;
        screenAlpha = MathHelper.Clamp(screenAlpha, 0, TotalFadeAlpha);
      }

      if (State == ScreenState.TransitionOn)
      {
        if (OnScreenTransition == null)
        {
          screenAlpha -= (float) gameTime.ElapsedGameTime.TotalMilliseconds/(float) FadeSpeed.TotalMilliseconds;
          screenAlpha = MathHelper.Clamp(screenAlpha, 0, TotalFadeAlpha);

          if (screenAlpha == 0)
            State = ScreenState.Active;
        }
      }

      if (State == ScreenState.TransitionOff)
      {
        if (OnScreenTransition == null)
        {
          screenAlpha += (float) gameTime.ElapsedGameTime.TotalMilliseconds/(float) FadeSpeed.TotalMilliseconds;
          screenAlpha = MathHelper.Clamp(screenAlpha, 0, TotalFadeAlpha);

          if (screenAlpha == TotalFadeAlpha)
            SceneManager.Remove(this);
        }
      }

      if (State == ScreenState.Active)
      {
        // Add any that were added while the rendering engine was rendering frames
        foreach (var vo in QueuedAdditions)
        {
          VisualObjects.Add(vo);

          // Flag to the Scene Manager that we added an object to the scene
          if (SceneManager.ObjectAdded != null)
            SceneManager.ObjectAdded.Invoke(vo);
        }

        foreach (var vo in QueuedRemovals)
        {
          VisualObjects.Remove(vo);

          // Flag to the Scene Manager that we added an object to the scene
          if (SceneManager.ObjectAdded != null)
            SceneManager.ObjectRemoved.Invoke(vo);
        }

        if (QueuedAdditions.Count > 0)
          QueuedAdditions.Clear();

        if (QueuedRemovals.Count > 0)
          QueuedRemovals.Clear();

        foreach (UpdatableObject vo in UpdatableObjects)
          vo.Update(gameTime);
      }

      foreach (VisualObject vo in VisualObjects)
      {
        if (!vo.Enabled)
          continue;

        if (SceneManager != null)
          vo.SpriteSortMode = SceneManager.SpriteSortMode;

        vo.CameraOffsetX = CameraOffsetX;
        vo.CameraOffsetY = CameraOffsetY;
        vo.CameraZoomOffset = CameraZoomOffset;
        vo.Update(gameTime);
      }

      isUpdating = false;
    }

    public override void Draw(GameTime gameTime)
		{
      base.Draw(gameTime);

			if (State != ScreenState.Hidden)
			{
				var sceneObjects = (from s in VisualObjects where s.Visible orderby s.DrawOrder ascending select s).ToList();

				foreach (VisualObject vo in sceneObjects)
				{
					vo.Draw(gameTime);
				}

				if (State == ScreenState.TransitionOn || State == ScreenState.TransitionOff || State == ScreenState.Behind)
				{
					// Use our Custom one unless they have their own hooked
          if (OnScreenTransition != null)
            OnScreenTransition.Invoke();
          else
          {
            SpriteBatch.Begin(SpriteSortMode, BlendState.AlphaBlend);
            SpriteBatch.Draw(black,
                             new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width,
                                           Game.GraphicsDevice.Viewport.Height),
                             new Color(255, 255, 255, screenAlpha));

            SpriteBatch.End();
          }
				}
			}
		}

		public void AddVisualObject(VisualObject vo)
		{
			if (SceneManager.SpriteBatch == null)
        throw new Exception("Game Screens Sprite Batch Reference is null!");

			if (vo.SpriteBatch == null)
				vo.SpriteBatch = SceneManager.SpriteBatch;

			vo.GameScreen = this;

			if (!vo.IsInitialized)
				vo.Initialize();

			if (!isUpdating)
			{
				VisualObjects.Add(vo);

				// Flag to the Scene Manager that we added an object to the scene
				if (SceneManager.ObjectAdded != null)
					SceneManager.ObjectAdded.Invoke(vo);
			}
			else
				QueuedAdditions.Add(vo);
		}

		public void AddUpdatableObject(UpdatableObject vo)
		{
			vo.GameScreen = this;

			if (!vo.IsInitialized)
				vo.Initialize();

			UpdatableObjects.Add(vo);
		}

		public void RemoveVisualObject(VisualObject vo)
		{
			vo.Visible = false;
			vo.Enabled = false;

			vo.UnloadContent();

			if (!isUpdating)
				VisualObjects.Remove(vo);
			else
				 QueuedRemovals.Add(vo);
		}

		public void Load2DProject(GameScreen screen, string filename)
		{
			CatalystProject2D project = Content.Load<CatalystProject2D>(filename);

			if (project == null)
				throw new Exception("Error: Failed to load the Catalyst3D 2D Project!");

			LoadProject(screen, project);

			// Call this to try to load additional content they may need
			//LoadContent();
		}
		private void LoadProject(GameScreen screen, CatalystProject2D project)
		{
			// First load any pathing ledges
			if (project.Paths.Count > 0)
				Paths.AddRange(project.Paths);

			// Loop thru our objects and reload them
			foreach (VisualObject vo in project.SceneObjects)
			{
				if (vo is VisualObjectGroup)
				{
					VisualObjectGroup group = vo as VisualObjectGroup;

					foreach (VisualObject o in group.Objects)
					{
						if (o is Sprite && o.GetType() != typeof (SpriteBox))
						{
							Sprite sprite = o as Sprite;
							sprite.AssetFolder = AssetFolder;
							sprite.Game = Game;

							sprite.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(o.AssetName));

							sprite.GameScreen = this;

							// Re-attach any nodes
							if (!string.IsNullOrEmpty(sprite.AttachedPathingNodeName))
								sprite.AttachedPathingNode = GetPath(sprite.AttachedPathingNodeName);
						}

						if (o is SpriteBox)
						{
							SpriteBox sprite = o as SpriteBox;
							sprite.AssetFolder = AssetFolder;
							sprite.Game = Game;
							sprite.GameScreen = this;

							// Re-attach any nodes
							if (!string.IsNullOrEmpty(sprite.AttachedPathingNodeName))
								sprite.AttachedPathingNode = GetPath(sprite.AttachedPathingNodeName);
						}

						if (o is ParticleEmitter)
						{
							ParticleEmitter emitter = o as ParticleEmitter;
							emitter.AssetFolder = AssetFolder;
							emitter.Game = Game;

							emitter.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(o.AssetName));

							emitter.GameScreen = this;
						}
					}

					group.Game = Game;

					AddVisualObject(group);
				}

				if (vo is Actor)
				{
					Actor a = vo as Actor;

					a.Game = Game;
					a.GameScreen = this;
					a.ClipPlayer.Game = Game;
					a.AssetFolder = AssetFolder;

					a.ClipPlayer.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(a.SpriteSheetFileName));

					AddVisualObject(a);

					continue;
				}

				if (vo is Button)
				{
					Button button = vo as Button;
					button.Game = Game;
					button.GameScreen = this;
					button.AssetFolder = AssetFolder;

					button.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(button.AssetName));

					AddVisualObject(button);

					continue;
				}

				// Make sure its a sprite and not the spritebox
				if (vo is Sprite && vo.GetType() != typeof (SpriteBox))
				{
					Sprite sprite = vo as Sprite;
					sprite.Game = Game;
					sprite.GameScreen = this;
					sprite.AssetFolder = AssetFolder;

					sprite.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(sprite.AssetName));

					// Re-attach any nodes
					if (!string.IsNullOrEmpty(sprite.AttachedPathingNodeName))
						sprite.AttachedPathingNode = GetPath(sprite.AttachedPathingNodeName);

					AddVisualObject(sprite);
				}

				if (vo is SpriteBox)
				{
					SpriteBox sprite = vo as SpriteBox;
					sprite.AssetFolder = AssetFolder;
					sprite.Game = Game;
					sprite.GameScreen = this;

					// Re-attach any nodes
					if (!string.IsNullOrEmpty(sprite.AttachedPathingNodeName))
						sprite.AttachedPathingNode = GetPath(sprite.AttachedPathingNodeName);

					AddVisualObject(sprite);
				}

				if (vo is ParticleEmitter)
				{
					ParticleEmitter emitter = vo as ParticleEmitter;
					emitter.Game = Game;
					emitter.GameScreen = this;
					emitter.AssetFolder = AssetFolder;

					emitter.Texture = screen.Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(emitter.AssetName));

					AddVisualObject(emitter);
				}
			}
		}

		public void Load2DProject(GameScreen screen, string filename, Assembly assembly)
		{
			CatalystProject2D project = screen.Content.Load<CatalystProject2D>(filename);

			if (project == null)
				throw new Exception("Error: Failed to load the Catalyst3D 2D Project!");

			if (project.Paths != null)
				Paths.AddRange(project.Paths);

			foreach (var v in project.SceneObjects)
			{
				if (!string.IsNullOrEmpty(v.ObjectTypeName))
				{
					// load as this type
					var type = assembly.GetType(v.ObjectTypeName);

					var obj = (VisualObject) Activator.CreateInstance(type, new object[] {Game, ContentManager});

					if (obj is Actor)
					{
						Actor n = obj as Actor;
						Actor o = v as Actor;

						if (o != null)
						{
							// Dig out the sprite sheet on these
							n.SpriteSheetFileName = o.SpriteSheetFileName;
							n.ClipPlayer.Sequences = o.ClipPlayer.Sequences;
							n.IsCentered = o.IsCentered;
							n.StartPathingLerpPosition = o.StartPathingLerpPosition;
							n.StartPathNodeIndex = o.StartPathNodeIndex;
							n.Direction = o.Direction;
							n.StartPathingLerpPosition = o.StartPathingLerpPosition;
							n.StartPathNodeIndex = o.StartPathNodeIndex;
							n.SpriteEffects = o.SpriteEffects;
							n.CurrentPathLerpPosition = o.CurrentPathLerpPosition;
							n.CurrentPathNodeIndex = o.CurrentPathNodeIndex;
						}
					}

					if (obj is Sprite)
					{
						Sprite n = obj as Sprite;
						Sprite o = v as Sprite;

						if (o != null)
						{
							n.SpriteSortMode = o.SpriteSortMode;
							n.StartPathNodeIndex = o.StartPathNodeIndex;
							n.StartPosition = o.StartPosition;
							n.Effects = o.Effects;
							n.IsCentered = o.IsCentered;
							n.IsLerpingBackToFirstNode = o.IsLerpingBackToFirstNode;
							n.LayerDepth = o.LayerDepth;
							n.BlendMode = o.BlendMode;
							n.Color = o.Color;
							n.ScrollSpeed = o.ScrollSpeed;
							n.CurrentPathLerpPosition = o.CurrentPathLerpPosition;
							n.CurrentPathNodeIndex = o.CurrentPathNodeIndex;
						}
					}

					if (obj is ParticleEmitter)
					{
						ParticleEmitter n = obj as ParticleEmitter;
						ParticleEmitter o = v as ParticleEmitter;

						if (o != null)
						{
							n.StartPathNodeIndex = o.StartPathNodeIndex;
							n.StartPathingLerpPosition = o.StartPathingLerpPosition;
							n.RespawnParticles = o.RespawnParticles;
							n.ParticleColor = o.ParticleColor;
							n.MaxAcceleration = o.MaxAcceleration;
							n.MinAcceleration = o.MinAcceleration;
							n.MaxInitialSpeed = o.MaxInitialSpeed;
							n.MinInitialSpeed = o.MinInitialSpeed;
							n.MaxLifeSpan = o.MaxLifeSpan;
							n.MinLifeSpan = o.MinLifeSpan;
							n.MinParticles = o.MinParticles;
							n.MaxParticles = o.MaxParticles;
							n.MaxRotationSpeed = o.MaxRotationSpeed;
							n.MinRotationSpeed = o.MinRotationSpeed;
							n.MinScale = o.MinScale;
							n.MaxScale = o.MaxScale;
							n.DelayedStartTime = o.DelayedStartTime;

							n.StartPathNodeIndex = o.StartPathNodeIndex;
							n.StartPathingLerpPosition = o.StartPathingLerpPosition;
							n.CurrentPathLerpPosition = o.CurrentPathLerpPosition;
							n.CurrentPathNodeIndex = o.CurrentPathNodeIndex;
						}
					}

					obj.GameScreen = this;
					obj.AssetName = v.AssetName;
					obj.ObjectType = type;
					obj.ObjectTypeName = v.ObjectTypeName;
					obj.Position = v.Position;
					obj.Scale = v.Scale;
					obj.AttachedPathingNode = (project.Paths.Where(p => p.Name == v.AttachedPathingNodeName)).FirstOrDefault();
					obj.AttachedPathingNodeName = v.AttachedPathingNodeName;
					obj.Enabled = v.Enabled;
					obj.Visible = v.Visible;
					obj.Rotation = v.Rotation;
					obj.Name = v.Name;
					obj.SpriteBatch = SpriteBatch;
					obj.DrawOrder = v.DrawOrder;
					obj.SpriteSortMode = v.SpriteSortMode;
					obj.IsLocked = v.IsLocked;
					obj.CurrentPathNodeIndex = v.CurrentPathNodeIndex;
					obj.IsCentered = v.IsCentered;
					AddVisualObject(obj);
				}
			}
		}

  	public Ledge GetPath(string name)
		{
			return (from l in Paths where l.Name == name select l).SingleOrDefault();
		}

		public Actor LoadActor(string assetName)
		{
			if (!IsInitialized)
				throw new Exception("Error: You cannot call LoadActor before the Scene Manager has been initialized!");

			// Unpack the Project
			var actor = Content.Load<Actor>(assetName);

			if (actor == null)
				throw new Exception("Error: Could not load the Actor!");

			actor.Game = Game;
			actor.GameScreen = this;

			actor.ClipPlayer.Game = Game;
			actor.ClipPlayer.GameScreen = this;

			actor.Initialize();

			// Out the gate play our first sequence
			if (actor.ClipPlayer.Sequences.Count > 0)
				actor.ClipPlayer.Play(actor.ClipPlayer.Sequences[0].Name, true);

			VisualObjects.Add(actor);

			return actor;
		}

		public T LoadActor<T>(string folderName, string assetName) where T : Actor, new()
		{
			Actor original = Content.Load<Actor>(folderName + "/" + assetName);

			if (original == null)
				throw new Exception("Error: Could not load actor please check the asset name!");

			// Create our new Player Object
			T actor = new T
									{
										Game = Game,
										Name = original.Name,
										Direction = original.Direction,
										Position = original.Position,
										Scale = original.Scale,
										DrawOrder = original.DrawOrder,
										PlayerIndex = original.PlayerIndex,
										Enabled = original.Enabled,
										AssetName = assetName,
										AssetFolder = folderName,
										Role = original.Role,
										Visible = original.Visible,
										GameScreen = this
									};

			// Init our animation clip player
			actor.ClipPlayer = new AnimationPlayer2D(Game)
													{
														AssetName = original.ClipPlayer.AssetName,
														Enabled = original.ClipPlayer.Enabled,
														Sequences = original.ClipPlayer.Sequences,
														Texture = original.ClipPlayer.Texture,
														GameScreen = this
													};

			// init the new player object
			actor.Initialize();

			// play the first sequence out the gate
			if (actor.ClipPlayer.Sequences.Count > 0)
				actor.Play(actor.ClipPlayer.Sequences[0].Name, true);

			return actor;
		}

		public T Get<T>() where T : class
		{
			return (from vo in VisualObjects where vo.GetType() is T select vo as T).FirstOrDefault();
		}

		public T Get<T>(GameScreen screen, string name) where T : class
		{
			return (from vo in screen.VisualObjects where vo.Name == name select vo as T).FirstOrDefault();
		}

		public T Get<T>(string name) where T : class
		{
			var v = (from vo in VisualObjects where vo.Name == name select vo as T).FirstOrDefault();

			if (v != null)
				return v;

			return (from up in UpdatableObjects where up.Name == name select up as T).FirstOrDefault();
		}

		public List<T> GetAll<T>() where T : class
		{
			return (from vo in VisualObjects where vo.GetType() is T select vo as T).ToList();
		}

		public T ChangeActor<T>(string name) where T : Actor, new()
		{
			VisualObject newObject = null;
			VisualObject oldObject = null;

			// loop thru the scene objects
			foreach (VisualObject o in VisualObjects)
			{
				if (o is Actor && o.Name == name)
				{
					Actor original = o as Actor;

					// Create our new Player Object
					T actor = new T
					{
						Game = Game,
						Name = original.Name,
						Direction = original.Direction,
						Position = original.Position,
						Scale = original.Scale,
						DrawOrder = original.DrawOrder,
						PlayerIndex = original.PlayerIndex,
						Enabled = original.Enabled,
						AssetName = original.AssetName,
						Role = original.Role,
						Visible = original.Visible,
						GameScreen = this
					};

					// Init our animation clip player
					actor.ClipPlayer = new AnimationPlayer2D(Game)
					{
						AssetName = original.ClipPlayer.AssetName,
						Enabled = original.ClipPlayer.Enabled,
						Sequences = original.ClipPlayer.Sequences,
						Texture = original.ClipPlayer.Texture,
						GameScreen = this
					};

					// init the new player object
					actor.Initialize();

					// play the first sequence out the gate
					actor.Play(actor.ClipPlayer.Sequences[0].Name, true);

					// New Objects
					newObject = actor;

					// Remove it from scene manager
					oldObject = original;
				}
			}

			// Remove the old
			VisualObjects.Remove(oldObject);

			// Insert the new
			VisualObjects.Add(newObject);

			return newObject as T;
		}

		public override void UnloadContent()
		{
			black = null;

			if (AudioManager != null)
				AudioManager.UnloadContent();

			if (ContentManager != null)
				ContentManager.Unload();

			// Make sure everything is unloaded
			foreach (var v in VisualObjects)
				v.UnloadContent();

			base.UnloadContent();
		}

  	public List<VisualObject> Contains(string name)
		{
			var objects = (from v in VisualObjects where v.Name == name select v).ToList();
			return objects.Count > 0 ? objects : null;
		}
	}
}