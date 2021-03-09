using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
	public class QuadTreeNode : VisualObject
	{
		public QuadTreeNode Parent;

		public QuadTreeNode(Game game)
			: base(game)
		{
		}

		~QuadTreeNode()
		{
			Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			lock(this)
			{
				Parent = null;
			}

			GC.SuppressFinalize(this);
			base.Dispose(disposing);
		}
	}
}