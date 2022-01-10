using PTSC.Ui.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Ui.Controller
{
    public static class ViewBindings
    {

        private static string GetControlBindingField(Control control)
        {
            switch (control)
            {
                case NumericUpDown _:
                    return "Value";
                case CheckBox _:
                    return "Checked";
                case TextBox _:
                    return "Text";
                case RichTextBox _:
                    return "Text";
                default:
                    return "Value";
            }
        }

        public static void BindView(BaseView baseView, object model)
        {
            void BindControlls(Control control)
            {
                if (control is GroupBox groupBox)
                {
                    foreach (var obj in groupBox.Controls)
                    {
                        BindControlls(obj as Control);
                    }
                }


                var propertyName = control.Tag as string;

                if (propertyName == null)
                    return;

                control.DataBindings.Add(ViewBindings.GetControlBindingField(control), model, propertyName);
            }

            foreach (var obj in baseView.Controls)
            {
                BindControlls(obj as Control);
            }
        }
    }
}
