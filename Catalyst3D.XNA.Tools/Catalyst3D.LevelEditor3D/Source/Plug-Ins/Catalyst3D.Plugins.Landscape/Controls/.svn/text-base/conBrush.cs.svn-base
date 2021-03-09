using System.Drawing;
using System.Windows.Forms;

namespace Catalyst3D.Plugins.Landscape.Controls
{
	public partial class conBrush : UserControl
	{
		public Image BrushImage;
		public bool IsSelected;

		public conBrush()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (BrushImage != null)
			{
				e.Graphics.DrawImage(BrushImage, new Rectangle(1, 1, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel);
			}

			if (IsSelected)
				BackColor = Color.LightGreen;
			else
				BackColor = Color.Transparent;
		}
	}
}
