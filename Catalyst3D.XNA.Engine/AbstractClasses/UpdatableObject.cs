using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.AbstractClasses
{
	public abstract class UpdatableObject : GameComponent
	{
		public int Index;
		public bool IsInitialized;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public GameScreen GameScreen;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public float CameraOffsetX;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public float CameraOffsetY;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
		public float CameraZoomOffset;

		public string Name { get; set; }

		public Vector2 Position { get; set; }

		protected UpdatableObject(Game game) : base(game)
		{
		}

		public override void Initialize()
		{
			IsInitialized = true;

			base.Initialize();
		}
	}
}