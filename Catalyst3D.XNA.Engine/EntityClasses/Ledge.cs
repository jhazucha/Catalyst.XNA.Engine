using System;
using System.Collections.Generic;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.EnumClasses;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
#if !WINDOWS_PHONE && !XBOX360
	[Serializable]
#endif
	public class Ledge
	{
		private float _travelSpeed;
		private bool _useNodesDrawOrder;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public string Name { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public List<LedgeNode> Nodes = new List<LedgeNode>();

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public LedgeRole Role = LedgeRole.Ground;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public LedgeTravelAlgo LedgeTavelAlgo = LedgeTravelAlgo.RespawnAtFirstNode;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public bool IsTraveling;

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public float TravelSpeed { get { return _travelSpeed; } set { _travelSpeed = value; } }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif
		[ContentSerializer]
		public int DrawOrder { get; set; }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(true)]
#endif

		[ContentSerializer]
		public bool UseNodesDrawOrder { get { return _useNodesDrawOrder; } set { _useNodesDrawOrder = value;  } }
	}
}