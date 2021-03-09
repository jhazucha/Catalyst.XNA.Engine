using System;
using System.IO;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor3D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor3D
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
      using (global::LevelEditor3D.LevelEditor3D game = new global::LevelEditor3D.LevelEditor3D(args))
      {
        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml"))
        {
          // Load our app settings from disk
          Globals.AppSettings =
            Serializer.Deserialize<AppSettings>(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml");
        }
        else
        {
          Globals.AppSettings = new AppSettings();
          Globals.AppSettings.ContentPath = AppDomain.CurrentDomain.BaseDirectory;
          Globals.AppSettings.RenderSurfaceColor = Color.Black;

          // Save this default config to disk
          Serializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml", Globals.AppSettings);
        }

        game.Run();
      }
		}
	}
}

