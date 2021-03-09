using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses
{
	public class QuadTree : QuadTreeNode
	{
		public QuadTreeNode UpperLeft;
		public QuadTreeNode UpperRight;
		public QuadTreeNode LowerLeft;
		public QuadTreeNode LowerRight;

		public QuadTree(Game game) : base(game)
		{
		}

		protected override void Dispose(bool disposing)
		{
			lock(this)
			{
				if(disposing)
				{
					if(UpperLeft != null)
						UpperLeft.Dispose();

					if(UpperRight != null)
						UpperRight.Dispose();

					if(LowerLeft != null)
						LowerLeft.Dispose();

					if(LowerRight != null)
						LowerRight.Dispose();
				}

				UpperLeft = null;
				UpperRight = null;
				LowerLeft = null;
				LowerRight = null;
			}

			base.Dispose(disposing);
		}
	}
}