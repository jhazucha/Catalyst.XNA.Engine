using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine;
using Catalyst3D.XNA.Engine.AbstractClasses;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Catalyst3D.XNA.Engine.EntityClasses.Effects;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.Controls
{
  public partial class conRenderer : UserControl
  {
    public bool IsRendering = true;
    public Color SurfaceColor = Color.DarkGray;
  	public float CameraFollowOffsetX;
    public BasicCamera Camera;
    public ResolutionMarkers SceneMarkers;
    public SceneManager SceneManager;
    public RendererWindow RenderWindow;
    public string FollowingObjectName;
    public VisualObject FollowingObject;
    public VisualObject CurrentSelectedObject;
    public List<VisualObject> CurrentlySelectedObjects = new List<VisualObject>();

  	public conRenderer()
    {
      InitializeComponent();
    }

  	protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Globals.ObjectAdded += OnObjectAdded;
			Globals.ObjectSelected += OnObjectSelected;
			Globals.ObjectRemoved += OnObjectRemoved;
			Globals.Draw += OnDraw;
			Globals.Update += OnUpdate;

			// Store this for quick access to our drop down pgrid editors
			Globals.RenderWindow = this;

			Camera = new BasicCamera(Globals.Game);
			Camera.Position = new Vector3(0, 1, 50);

			// Create our Resolution Ruler Marks
      SceneMarkers = new ResolutionMarkers(Globals.Game);
      SceneMarkers.Initialize();

			// Create our Scene Manager
			SceneManager = new SceneManager(Globals.Game);
			SceneManager.Initialize();

			// Create our Game Screen for Rendering
  		RenderWindow = new RendererWindow(Globals.Game);

			// Add our Game Screen for rendering to it
      SceneManager.Load(RenderWindow);

		}

  	private void OnUpdate(GameTime gametime)
		{
			// Update the camera
			Camera.Update(gametime);

      // Update our render windows height and width vars
			Globals.CurrentRenderWindowWidth = Width;
			Globals.CurrentRenderWindowHeight = Height;

      // Update our scene's viewport color
      SceneManager.ViewportColor = Globals.AppSettings.GetRenderSurfaceColor();

      // Update are scene markers
      SceneMarkers.Update(gametime);

			// Only follow if we are selected on them!
			if (FollowingObject != null)
			{
        if (FollowingObject is LedgeBuilder)
				{
          var path =FollowingObject as LedgeBuilder;
				  {
				    var vo = (from s in RenderWindow.VisualObjects
				              where s.AttachedPathingNodeName == path.Name
				              select s).FirstOrDefault();

				    if (vo != null)
				      Globals.CurrentCameraOffsetX = -(vo.Position.X - CameraFollowOffsetX);
				  }
				}
				else
				{
          Globals.CurrentCameraOffsetX = -(FollowingObject.Position.X - CameraFollowOffsetX);
				}

        FollowingObject.IsSelected = true;
        FollowingObject.ShowBoundingBox = true;
        FollowingObjectName = FollowingObject.Name;
			}
			else
			{
				FollowingObjectName = string.Empty;
			}

			

			foreach (LedgeBuilder b in Globals.Paths)
			{
				b.CameraOffsetX = Globals.CurrentCameraOffsetX;
				b.CameraOffsetY = Globals.CurrentCameraOffsetY;
				b.CameraZoomOffset = Globals.CurrentCameraZoom;
				b.Update(gametime);
			}

			var sceneObjects = (from db in RenderWindow.VisualObjects orderby db.UpdateOrder select db).ToList();
			foreach (VisualObject v in sceneObjects)
			{
				v.CameraOffsetX = Globals.CurrentCameraOffsetX;
				v.CameraOffsetY = Globals.CurrentCameraOffsetY;
				v.CameraZoomOffset = Globals.CurrentCameraZoom;

				if (!Globals.IsDialogWindowOpen)
				{
					if (CurrentlySelectedObjects.Count > 0)
					{
						foreach (var vo in CurrentlySelectedObjects)
							vo.IsSelected = true;
					}
					else if (CurrentSelectedObject == v)
						v.IsSelected = true;
					else
						v.IsSelected = false;
				}
				
				v.Update(gametime);
			}
		}

  	public void OnDraw(GameTime gametime)
    {
      // Draw our scene objects
			var sceneObjects = (from db in RenderWindow.VisualObjects orderby db.DrawOrder orderby db.DrawOrder ascending select db).ToList();
      
			foreach (VisualObject v in sceneObjects)
        v.Draw(gametime);

      SceneMarkers.Draw(gametime);


			foreach (LedgeBuilder b in Globals.Paths)
				b.Draw(gametime);
    }

		public void OnObjectRemoved(VisualObject obj)
		{
			CurrentSelectedObject = null;

      RenderWindow.RemoveVisualObject(obj);

      if (Globals.Paths.Contains(obj))
			{
				LedgeBuilder b = obj as LedgeBuilder;

				if (b != null)
          Globals.Paths.Remove(b);
			}
		}

  	public void OnObjectSelected(VisualObject obj)
    {
			foreach (VisualObject vo in RenderWindow.VisualObjects)
			{
				vo.IsSelected = false;
				vo.ShowBoundingBox = false;
			}

  		foreach (LedgeBuilder b in Globals.Paths)
  		{
  			b.IsSelected = false;
  			b.Visible = false;
  			b.ShowBoundingBox = false;
  		}

    	if (obj != null)
      {
				if(obj is LedgeBuilder)
					obj.Visible = true;

				if (obj is Sprite)
				{
					var s = obj as Sprite;
					var ledge = GetPath(s.AttachedPathingNodeName);
					
					if (ledge != null)
					{
						ledge.IsSelected = true;
						ledge.Visible = true;
					}
				}

				if (obj is Actor)
				{
					var a = obj as Actor;
					var ledge = GetPath(a.AttachedPathingNodeName);

					if (ledge != null)
					{
						ledge.IsSelected = true;
						ledge.Visible = true;
					}
				}

				if (obj is ParticleEmitter)
				{
					var e = obj as ParticleEmitter;
					var ledge = GetPath(e.AttachedPathingNodeName);

					if (ledge != null)
					{
						ledge.IsSelected = true;
						ledge.Visible = true;
					}
				}

      	obj.Visible = true;
      	obj.IsSelected = true;
      	obj.ShowBoundingBox = true;
        
        CurrentSelectedObject = obj;
      }
    }

		private void OnObjectAdded(VisualObject obj)
		{
			if (obj.GameScreen == null)
				obj.GameScreen = RenderWindow;

			// Add it to our scene objects collection
			RenderWindow.AddVisualObject(obj);
		}

  	public LedgeBuilder GetPath(string pathName)
		{
      return (from p in Globals.Paths where p.Name == pathName select p).FirstOrDefault();
		}
		
  }
}