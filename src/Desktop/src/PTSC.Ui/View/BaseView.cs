using PTSC.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTSC.Ui.View
{
    public partial class BaseView : Form, IView
    {
        public IController Controller { get; protected set; }
        public BaseView()
        {
            InitializeComponent();
        }

        public IView Initialize(IController controller)
        {
            Controller = controller;
            return this;
        }
    }
}
