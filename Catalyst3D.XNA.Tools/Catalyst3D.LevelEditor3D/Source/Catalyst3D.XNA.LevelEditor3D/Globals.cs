using System;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor3D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor3D
{
	public static class Globals
	{
		public static Game Game;

		public delegate void DrawEvent(GameTime gameTime);
		public delegate void UpdateEvent(GameTime gameTime);

		public delegate void InitializeEvent();

	  public delegate void ProjectLoadEvent(string filename);

		public delegate void ObjectEvent(VisualObject sprite);

		public delegate void WindowSize(object sender, EventArgs e);

		public static VisualObject CurrentSelectedObject;

		public static AppSettings AppSettings;

		public static DrawEvent Draw;
		public static UpdateEvent Update;

		public static InitializeEvent Initialize;

	  public static ProjectLoadEvent OnLoadProject;

		public static ObjectEvent ObjectAdded;
		public static ObjectEvent ObjectRemoved;
		public static ObjectEvent ObjectSelected;

		public static WindowSize WindowSizeChanged;

		public static int MouseX;
		public static int MouseY;

		public static int LandscapeBrushSize;

		public static Enums.BrushMode LandscapeBrushMode;
	}
}