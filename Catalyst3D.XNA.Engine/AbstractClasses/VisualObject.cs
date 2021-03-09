using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.EntityClasses.Text;
using Catalyst3D.XNA.Engine.EntityClasses.UI;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.AbstractClasses
{
#if !XBOX360 && !WINDOWS_PHONE
	[TypeConverter(typeof(ExpandableObjectConverter))]
#endif
	[XmlInclude(typeof(Actor))]
	[XmlInclude(typeof(GameScreen))]
	[XmlInclude(typeof(SpriteBox))]
	[XmlInclude(typeof(Sprite))]
	[XmlInclude(typeof(ParticleEmitter))]
	[XmlInclude(typeof(VisualObjectGroup))]
	[XmlInclude(typeof(Button))]
	[XmlInclude(typeof(Label))]
	public abstract class VisualObject : DrawableGameComponent
	{
		public delegate void CustomShader(GameTime gameTime);

		public string ObjectTypeName { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public Type ObjectType { get; set; }

		public string Name { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public GameScreen GameScreen;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializer]
		public float CameraOffsetX;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializer]
		public float CameraOffsetY;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializer]
		public float CameraZoomOffset;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public bool IsSelected;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BoundingBox BoundingBox { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public bool ShowBoundingBox { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public BasicCamera Camera;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public int Index;

		[ContentSerializerIgnore, XmlIgnore]
		public bool EnableAlphaBlending = true;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public int PauseAlpha = 200;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public string AssetName { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public string AssetFolder;

		public Vector2 Position { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		public Vector2 Velocity { get; set; }

		public Vector2 Scale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Category("Orientation")]
		[TypeConverter(typeof(float))]
#endif
		public float Rotation { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public bool IsInitialized { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public Matrix World { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public Ledge AttachedPathingNode { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializer]
		public string AttachedPathingNodeName { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public bool IsLocked { get; set; }

		public Vector2 BoundingBoxScale { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public int CurrentPathNodeIndex { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializer]
		public ObjectState CurrentObjectState { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public SpriteBatch SpriteBatch { get; set; }

		[ContentSerializerIgnore, XmlIgnore]
		public ContentManager ContentManager { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
    [Browsable(false)]
#endif
    [ContentSerializer]
    public SpriteSortMode SpriteSortMode { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public bool IsCentered { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public bool IsBlurrEnabled { get; set; }

		protected VisualObject() : base(null) { }

		protected VisualObject(Game game)
			: base(game)
		{
			ShowBoundingBox = false;
			Scale = Vector2.One;
		}

		public override void Initialize()
		{
			base.Initialize();
			IsInitialized = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			IsLoaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Note: If we are utilizing velocity on the object and not just updating the Position use this instead of the position property
			if (Velocity.X != 0 || Velocity.Y != 0)
			{
				// Apply velocity.
				Position += Velocity * elapsedTime;
				Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			IsLoaded = false;
		}

	}
}