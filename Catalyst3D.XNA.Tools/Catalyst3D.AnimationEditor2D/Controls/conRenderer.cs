using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AnimationEditor.EntityClasses;
using AnimationEditor.Screens;
using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;

namespace AnimationEditor.Controls
{
  public partial class conRenderer : UserControl
  {
    public Game Game;
    public bool IsRendering = true;
    public Color SurfaceColor = Color.DarkGray;

    public SceneManager Manager;
    public EditorActor Actor;

    public conRenderer()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      Globals.ObjectRemoved += OnObjectRemoved;
      Globals.Update += OnUpdate;
      Globals.UpdateActorSequences += OnUpdateSequences;
      Globals.ActorAdded += OnActorAdded;

      // Init our Screen Manager
      Manager = new SceneManager(Globals.Game);

      // Create our Rendering Window
      Globals.Renderer = new Renderer(Globals.Game);
    	Manager.Add(Globals.Renderer);
    }

    private void OnActorAdded(EditorActor actor)
    {
      Actor = actor;

    	Globals.Renderer.AddVisualObject(actor);
    }

    private void OnUpdateSequences(List<Sequence2D> sequences)
    {
      Actor.ClipPlayer.Sequences = sequences;
    }

    private void OnUpdate(GameTime gametime)
    {
      // Incase we changed it in the options window update it
      Manager.ViewportColor = Globals.AppSettings.GetRenderSurfaceColor();
    }
    
		public void OnObjectRemoved(VisualObject obj)
		{
			Actor = null;
			Manager.CurrentScreen.VisualObjects.Remove(obj);
    }
  }
}