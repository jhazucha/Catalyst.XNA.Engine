using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using AnimationEditor.Controls;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace AnimationEditor.TypeEditors
{
	public class SequenceEditor : UITypeEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			var editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if(editorService != null)
			{
				conSequenceEditor con = new conSequenceEditor((List<Sequence2D>) value, editorService);
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
