using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace AnimationEditor
{
	public class Utilitys
	{
		public static string GetDestinationTexturePath(string filename)
		{
			return Globals.AppSettings.ProjectPath + @"\Temp\" + Path.GetFileName(filename);
		}
	}
}