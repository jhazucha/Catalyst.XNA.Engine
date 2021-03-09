using System.Drawing;
using System.Windows.Forms;

namespace Catalyst3D.PluginSDK.Controls
{
  public partial class conTool : UserControl
  {
    public Image ButtonImage;

    public bool IsSelected;

    public conTool()
    {
      InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      if (ButtonImage != null)
      {
        e.Graphics.DrawImage(ButtonImage, new Rectangle(0, 0, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel);
      }
    }
  }
}