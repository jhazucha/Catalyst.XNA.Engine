using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine
{
  public class GameComponent : IGameComponent, IUpdateable, IDisposable
  {
    private bool enabled = true;
    private int updateOrder;

		public GameComponent(Game game)
		{
			Game = game;
		}

#if !XBOX360 && !WINDOWS_PHONE
		[Browsable(false)]
#endif
    [ContentSerializerIgnore, XmlIgnore]
    public Game Game { get; set; }

    #region IDisposable Members

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    #region IGameComponent Members

    public virtual void Initialize()
    {
    }

    #endregion

    #region IUpdateable Members

    public virtual void Update(GameTime gameTime)
    {
    }

    // Properties
    public bool Enabled
    {
      get { return enabled; }
      set
      {
        if (enabled != value)
        {
          enabled = value;
          OnEnabledChanged(this, EventArgs.Empty);
        }
      }
    }

    public int UpdateOrder
    {
      get { return updateOrder; }
      set
      {
        if (updateOrder != value)
        {
          updateOrder = value;
          OnUpdateOrderChanged(this, EventArgs.Empty);
        }
      }
    }

    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    #endregion

    public event EventHandler Disposed;

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        lock (this)
        {
          if (Game != null)
          {
          	Game.Components.Remove(this);
          }
          if (Disposed != null)
          {
            Disposed(this, EventArgs.Empty);
          }
        }
      }
    }

    ~GameComponent()
    {
    	Dispose(false);
    }

    protected virtual void OnEnabledChanged(object sender, EventArgs args)
    {
      if (EnabledChanged != null)
      {
        EnabledChanged(this, args);
      }
    }

    protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
    {
      if (UpdateOrderChanged != null)
      {
        UpdateOrderChanged(this, args);
      }
    }
  }
}