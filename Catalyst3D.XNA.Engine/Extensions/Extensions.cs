using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Catalyst3D.XNA.Engine.AbstractClasses;

namespace Catalyst3D.XNA.Engine.Extensions
{
	public static class Extensions
	{
		public static T CastSpriteTo<T>(this VisualObject vo) where T : Sprite, new()
		{
			return vo as T;
		}
	}
}