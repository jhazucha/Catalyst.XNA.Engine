using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using LevelEditor2D.Controls;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor2D
{
  public static class Globals
  {
    public delegate void ProjectEvent(bool showResult);
    public delegate void SpriteContainerEvent(int frameNumber);

    public delegate void ObjectEvent(VisualObject sprite);
    public delegate void ObjectGroupEvent(List<VisualObject> objects);

    public delegate void CameraPannedEvent(float distanceX, float distanceY);

    public delegate void FrameEvent(int frameNumber);
    public delegate void FlushFramesEvent();

    public delegate List<Sequence2D> SequenceAddEvent(Sequence2D seq);
    public delegate List<Sequence2D> SequenceRemoveEvent(int index);

    public delegate void DrawEvent(GameTime gameTime);
    public delegate void UpdateEvent(GameTime gameTime);

    public delegate void WindowSize(object sender, EventArgs e);

  	public delegate void PathingEvent();

		public delegate void AnimationEvent(Enums.SceneState state);

    public static ProjectEvent SaveProject;
    public static ProjectEvent LoadProject;
		
    public static CameraPannedEvent CameraPanned;

    public static Game Game;

    public static float CameraSensitivity = 2f;
    public static float CurrentCameraZoom = 1f;
    public static float CurrentCameraOffsetX;
    public static float CurrentCameraOffsetY;

    public static FrameEvent FrameSelectedEvent;
    public static FrameEvent FrameRemovedEvent;
    public static FlushFramesEvent RemoveAllFramesEvent;

    public static ObjectEvent ObjectAdded;
    public static ObjectEvent ObjectRemoved;
    public static ObjectEvent ObjectSelected;
    public static ObjectEvent ObjectUndo;

    public static ObjectGroupEvent ObjectsGroupped;

    public static SpriteContainerEvent ContainerEvent;

  	public static conRenderer RenderWindow;
    
    public static AppSettings AppSettings;

    public static DrawEvent Draw;
    public static UpdateEvent Update;

    public static WindowSize WindowSizeChanged;

    public static PathingEvent OnPathingNodePlaced;
    public static PathingEvent OnSaveCurrentPath;
  	public static PathingEvent OnCanceledCurrentPath;
  	public static PathingEvent OnPathingNodeSelected;

    public static List<LedgeBuilder> Paths = new List<LedgeBuilder>();
    
  	public static AnimationEvent OnSceneEvent;

  	public static bool IsProjectLoaded;
    public static bool IsPathingToolVisible;
  	public static bool IsPathingNodeShown = true;
  	public static bool IsScenePaused;
  	public static bool IsSceneStopped;
  	public static bool IsDialogWindowOpen;

    public static int CurrentMouseX;
    public static int CurrentMouseY;

  	public static int CurrentRenderWindowWidth;
  	public static int CurrentRenderWindowHeight;

  	public static GraphicsDeviceManager GraphicsDeviceManager;

		public static string GetDestinationTexturePath(string filename)
		{
			return AppSettings.ProjectPath + @"\Textures\" + Path.GetFileName(filename);
		}

  	public static List<Type> CurrentGameObjectTypes = new List<Type>();

		public static void LoadAssembly(string filename)
		{
			Assembly asm = Assembly.LoadFrom(filename);

			if (asm != null)
			{
				// get the types
				Type[] types = asm.GetExportedTypes();

				foreach (Type t in types)
				{
					Type currentType = t.BaseType;

					while (currentType != typeof (object))
					{
						if (currentType != null)
							currentType = currentType.BaseType;

						if (currentType == typeof (VisualObject))
						{
							CurrentGameObjectTypes.Add(t);
							break;
						}
					}
				}
			}
		}
  }
}