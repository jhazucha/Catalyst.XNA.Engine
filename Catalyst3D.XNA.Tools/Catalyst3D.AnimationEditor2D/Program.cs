using System;
using System.IO;
using Catalyst3D.XNA.Engine.UtilityClasses;

namespace AnimationEditor
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      using (Main game = new Main())
      {
        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml"))
        {
          // Load our app settings from disk
        	Globals.AppSettings = Serializer.Deserialize<AppSettings>(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml");
        }
        else
        {

          Globals.AppSettings = new AppSettings();
          Globals.AppSettings.ContentPath = AppDomain.CurrentDomain.BaseDirectory;
          Globals.AppSettings.ProjectPath = AppDomain.CurrentDomain.BaseDirectory;
          Globals.AppSettings.RenderSurfaceColor = "Black";

          // Save this default config to disk
          Serializer.Serialize(AppDomain.CurrentDomain.BaseDirectory + @"/settings.xml", Globals.AppSettings);
        }

        game.Run();
      }
    }
  }
}
