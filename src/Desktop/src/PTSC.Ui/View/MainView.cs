using ChartWin;
using PTSC.Ui.Controller;

namespace PTSC.Ui.View
{
    public partial class MainView : BaseView
    {

        public Graph3D Graph3D;
        public MainView()
        {
            InitializeComponent();
            Graph3D = new();
            this.tabPageSkeleton.Controls.Add(Graph3D);
            Graph3D.Dock = DockStyle.Fill;

        }

        private void buttonStartModule_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).StartModule();
        }

        private void buttonStopModule_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).StopModule();
        }
    }
}