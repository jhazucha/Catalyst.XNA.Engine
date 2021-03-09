using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using LevelEditor2D.Controls;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.TypeEditors
{
  public class EmitterForceEditor : UITypeEditor
  {
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			var editorService = provider.GetService(typeof (IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if (editorService != null)
			{
			  conEmitterForces con = new conEmitterForces((List<Vector2>) value, editorService);
				editorService.DropDownControl(con);

				return value;
			}
			return null;
		}

  	public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.DropDown;
    }
  }
}