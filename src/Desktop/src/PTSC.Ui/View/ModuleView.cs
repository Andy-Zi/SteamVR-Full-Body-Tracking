using PTSC.Ui.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Ui.View
{
    public partial class ModuleView : BaseView
    {

        public ModuleView()
        {
            InitializeComponent();
        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            (this.Controller as ModuleController).OnOk();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            (this.Controller as ModuleController).OnCancel();
        }
    }
}
