using System;
using System.Collections.Generic;
using AnimationEditor.EntityClasses;
using AnimationEditor.Screens;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;

namespace AnimationEditor
{
  public static class Globals
  {
    public static Game Game;
    public static AppSettings AppSettings;
    public delegate void ProjectEvent(bool showResult);

    public delegate void FormEvent(string name);

    public delegate void WindowSize(object sender, EventArgs e);

    public static ProjectEvent SaveProject;
    public static ProjectEvent LoadProject;
    public static ProjectEvent CleanupTextureFolder;
  	public static ProjectEvent RefreshSequenceDropDown;
  	public static ProjectEvent ReBakeSpriteSheet;

    public delegate void DrawEvent(GameTime gameTime);
    public delegate void UpdateEvent(GameTime gameTime);
    public delegate void ObjectEvent(EditorActor visualObject);

    public delegate void SequencerEvent();

    public delegate void ActorEvent(List<Sequence2D> sequences);

    public static ObjectEvent ActorAdded;
    public static ObjectEvent ObjectRemoved;
    public static ObjectEvent ObjectSelected;
    public static ActorEvent UpdateActorSequences;

    public static SequencerEvent UpdateSequences;

    public static DrawEvent Draw;
    public static UpdateEvent Update;

    public static WindowSize WindowSizeChanged;

    public static bool IsProjectLoaded;

    public static float CurrentCameraOffsetX;
    public static float CurrentCameraOffsetY;

  	public static Renderer Renderer;

    public static FormEvent AddSequence;
  }
}
