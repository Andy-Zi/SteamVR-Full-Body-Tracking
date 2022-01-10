using ChartWin;
using PTSC.Ui.Controller;

namespace PTSC.Ui.View
{
    public partial class MainView : BaseView
    {

        public Graph3D Graph3D;
        public LatencyGraph PiplineLatencyGraph;
        public LatencyGraph ModuleLatencyGraph;
        public MainView()
        {
            InitializeComponent();
            Graph3D = new();
            this.tabPageSkeleton.Controls.Add(Graph3D);
            Graph3D.Dock = DockStyle.Fill;

            PiplineLatencyGraph = new();
            this.panelPipelineContainer.Controls.Add(PiplineLatencyGraph);
            PiplineLatencyGraph.Dock = DockStyle.Fill;

            ModuleLatencyGraph = new();
            this.panelModuleContainer.Controls.Add(ModuleLatencyGraph);
            this.ModuleLatencyGraph.Dock = DockStyle.Fill;

            //this.groupBoxDriver.Paint += GroupBox_Paint;
            //this.groupBoxModule.Paint += GroupBox_Paint;
            //this.groupBoxVisualization.Paint += GroupBox_Paint;
        }

        private void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            var textColor = Color.Black;
            var borderColor = Color.Gray;
            var box = (GroupBox)sender;
            var graphics = e.Graphics;

            if (box != null)
            {
                using Brush textBrush = new SolidBrush(textColor);
                using Brush borderBrush = new SolidBrush(borderColor);
                using Pen borderPen = new Pen(borderBrush,2f);
                SizeF strSize = graphics.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                graphics.Clear(this.BackColor);

                // Draw text
                graphics.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                graphics.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                graphics.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                graphics.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                graphics.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                graphics.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

   

        private void buttonStartModule_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).StartModule();
        }

        private void buttonStopModule_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).StopModule();
        }

        private void buttonApplicationSettings_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).ShowOptions();
        }

        private void button_moduleSettings_Click(object sender, EventArgs e)
        {
            (this.Controller as MainController).ShowModuleOptions();
        }
    }
}