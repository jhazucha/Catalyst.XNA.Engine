using System.Collections.Generic;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine
{
	public class CatalystProject2D : GameScreen
	{
		[ContentSerializer(SharedResource = true)]
		public List<VisualObject> SceneObjects = new List<VisualObject>();

		[ContentSerializer(SharedResource = true)]
		public List<string> SpriteContainerFiles = new List<string>();

		[ContentSerializerIgnore]
		private readonly string assetName;

		[ContentSerializer]
		public float CameraFollowOffsetX { get; set; }

		[ContentSerializer]
		public float CameraFollowOffsetY { get; set; }

		[ContentSerializer]
		public string CameraFollowing { get; set; }

		public Vector2 ScreenResolution = new Vector2(800, 480);

		public CatalystProject2D() : base(null, string.Empty) { }

		public CatalystProject2D(Game game, string asset)
			: base(game, string.Empty)
		{
			assetName = asset;
		}
	}
}