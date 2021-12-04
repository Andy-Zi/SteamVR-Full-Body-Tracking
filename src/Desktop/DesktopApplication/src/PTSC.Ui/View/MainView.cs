using PTSC.Ui.Controller;

namespace PTSC.Ui.View
{
    public partial class MainView : BaseView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //((MainController)Controller).SaySomething();
            ((MainController)Controller).PipeTest();
        }
    }
}