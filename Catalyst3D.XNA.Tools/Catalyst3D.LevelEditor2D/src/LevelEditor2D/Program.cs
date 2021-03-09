using System;
using System.IO;
using System.IO.IsolatedStorage;
using Catalyst3D.XNA.Engine.UtilityClasses;
using LevelEditor2D.EntityClasses;

namespace LevelEditor2D
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			using (Game1 game = new Game1())
			{
				IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User |
				                                                            IsolatedStorageScope.Assembly |
				                                                            IsolatedStorageScope.Domain, null, null);

				if (isoStore.FileExists("settings.xml"))
				{
					Globals.AppSettings = Serializer.IsoDeserialize<AppSettings>(isoStore, "settings.xml");

					if (!string.IsNullOrEmpty(Globals.AppSettings.ObjectTypesPath))
					{
						Globals.LoadAssembly(Globals.AppSettings.ObjectTypesPath);
					}
				}
				else
				{

					Globals.AppSettings = new AppSettings();
					Globals.AppSettings.ContentPath = AppDomain.CurrentDomain.BaseDirectory;
					Globals.AppSettings.ProjectPath = AppDomain.CurrentDomain.BaseDirectory;
					Globals.AppSettings.RenderSurfaceColor = "Black";

					// Save this default config to disk
					Serializer.IsoSerialize(isoStore, "settings.xml", Globals.AppSettings);
				}

				if (args.Length > 0)
				{
					Globals.AppSettings.ProjectPath = Path.GetDirectoryName(args[0]);
					Globals.AppSettings.ProjectFilename = Path.GetFileName(args[0]);
				}
				else
					Globals.AppSettings.ProjectFilename = string.Empty;

				game.Run();
			}
		}
	}
}