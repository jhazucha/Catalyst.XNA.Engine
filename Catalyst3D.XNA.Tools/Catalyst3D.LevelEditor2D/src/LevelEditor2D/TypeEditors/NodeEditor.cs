using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using LevelEditor2D.Controls;

namespace LevelEditor2D.TypeEditors
{
	public class NodeEditor : UITypeEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if (editorService != null)
			{
			  conNodeEditor con = new conNodeEditor(editorService);
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
