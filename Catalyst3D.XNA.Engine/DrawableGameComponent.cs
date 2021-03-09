using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine
{
	public class DrawableGameComponent : GameComponent, IDrawable
  {
    private IGraphicsDeviceService deviceService;
		private int drawOrder;
		private bool initialized;
		private bool visible;

    public bool IsLoaded { get; set; }

    public DrawableGameComponent(Game game)
      : base(game)
    {
      visible = true;
    }

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
		[ContentSerializerIgnore, XmlIgnore]
    public GraphicsDevice GraphicsDevice
    {
      get
      {
        if (deviceService == null)
        {
          return null;
        }
        return deviceService.GraphicsDevice;
      }
    }

    #region IDrawable Members

	  public event EventHandler<EventArgs> DrawOrderChanged;
	  public event EventHandler<EventArgs> VisibleChanged;

    public virtual void Draw(GameTime gameTime)
    {
    }

    public int DrawOrder
    {
      get { return drawOrder; }
      set
      {
        if (drawOrder != value)
        {
          drawOrder = value;
          OnDrawOrderChanged(this, EventArgs.Empty);
        }
      }
    }

    public bool Visible
    {
      get { return visible; }
      set
      {
        if (visible != value)
        {
          visible = value;
          OnVisibleChanged(this, EventArgs.Empty);
        }
      }
    }

    #endregion

    private void DeviceCreated(object sender, EventArgs e)
    {
      LoadContent();
    }

    private void DeviceDisposing(object sender, EventArgs e)
    {
      UnloadContent();
    }

    private void DeviceReset(object sender, EventArgs e)
    {
    	LoadContent();
    }

    private void DeviceResetting(object sender, EventArgs e)
    {
    	UnloadContent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
      	UnloadContent();

        if (deviceService != null)
        {
          deviceService.DeviceCreated -= DeviceCreated;
          deviceService.DeviceResetting -= DeviceResetting;
          deviceService.DeviceReset -= DeviceReset;
          deviceService.DeviceDisposing -= DeviceDisposing;
        }
      }
      base.Dispose(disposing);
    }

    public override void Initialize()
    {
      base.Initialize();

			if (!initialized)
      {
        deviceService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
        
        if (deviceService == null)
        {
          throw new InvalidOperationException("Missing Graphics Device Service!");
        }

        deviceService.DeviceCreated += DeviceCreated;
        deviceService.DeviceResetting += DeviceResetting;
        deviceService.DeviceReset += DeviceReset;
        deviceService.DeviceDisposing += DeviceDisposing;

        if (deviceService.GraphicsDevice != null)
        {
        	LoadContent();
        }
      }
      initialized = true;
    }

		public virtual void LoadContent()
		{
		  IsLoaded = true;
		}

    protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
    {
      if (DrawOrderChanged != null)
      {
        DrawOrderChanged(this, args);
      }
    }

    protected virtual void OnVisibleChanged(object sender, EventArgs args)
    {
      if (VisibleChanged != null)
      {
        VisibleChanged(this, args);
      }
    }

    public virtual void UnloadContent()
    {
      
    }
  }
}