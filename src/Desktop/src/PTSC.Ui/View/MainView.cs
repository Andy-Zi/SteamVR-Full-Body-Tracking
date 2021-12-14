using PTSC.Ui.Controller;

namespace PTSC.Ui.View
{
    public partial class MainView : BaseView
    {
        public MainView()
        {
            InitializeComponent();
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