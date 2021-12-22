using PTSC.Interfaces;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using PTSC.Communication.Controller;
using Unity;
using PTSC.Pipeline;
using Prism.Events;
using PTSC.PubSub;
using PTSC.Modules;
using System.ComponentModel;
using ChartWin;

namespace PTSC.Ui.Controller
{
    public class MainController : BaseController<MainModel, MainView>
    {
        [Dependency] public ILogger Logger { get; set; }
        //Lazy Dependency Injection
        [Dependency] public Lazy<DriverPipeServerController> DriverPipeServer { get; set; }
        [Dependency] public Lazy<ModulePipeServerController> ModulePipeServer { get; set; }
        [Dependency] public Lazy<ProcessingPipeline> ProcessingPipeline { get; set; }
        [Dependency] public Lazy<ModuleWrapper> ModuleWrapper { get; set; }
        [Dependency] public ModuleRepository ModuleRepository { get; set; }

        Graph3D chart;
        public IEventAggregator EventAggregator { get; set; }
        public MainController(MainView view) : base(view)
        {

        }

        public BaseController<MainModel, MainView> RegisterEventAggregator(IUnityContainer container)
        {
            //This need to be consturcted on the UI-Thread to enable the Publish on UI-Thread option!
            EventAggregator = new EventAggregator();
            container.RegisterInstance<IEventAggregator>(EventAggregator);
            return this;
        }
        public override BaseController<MainModel, MainView> Initialize()
        {
            chart = new();
            this.View.tabControl.TabPages[1].Controls.Add(chart);
            chart.Dock = DockStyle.Fill;
            BindData();
            ModulePipeServer.Value.FPSLimit = 30;
            ModulePipeServer.Value.RetrieveImage = true;
            ModulePipeServer.Value.Start();
            ProcessingPipeline.Value.Start();
            DriverPipeServer.Value.Start();
            Subscribe();
            return base.Initialize();
        }

        private void Subscribe()
        {
            EventAggregator.GetEvent<ImageProcessedEvent>().Subscribe(DisplayImage, ThreadOption.UIThread, false);
            EventAggregator.GetEvent<ModuleDataProcessedEvent>().Subscribe(Plot3DGraph, ThreadOption.UIThread, false);
            ModuleWrapper.Value.OnError += ModuleWrapper_OnMessage;
            ModuleWrapper.Value.OnMessage += ModuleWrapper_OnMessage;
        }

        private void Plot3DGraph(ModuleDataProcessedPayload obj)
        {
            chart.Chard3DSeries.Points?.Clear();
            var data = obj.ModuleDataModel;
            
            foreach(var row in data.GetData().Values)
            {
                if (row == null)
                    continue;
                chart.AddXY3d(-row[0], -row[1], -row[2]);
            }
            chart.Refresh();
        }

        private void ModuleWrapper_OnMessage(string message)
        {
            this.View.Invoke(() =>
            {
                this.View.richTextBoxModule.Text += message;
            });
        }

        private void BindData()
        {
            var availableModules = new BindingList<IDetectionModule>(ModuleRepository.Values.ToList());
            this.View.comboBoxModule.ValueMember = null;
            this.View.comboBoxModule.DisplayMember = "Name";
            this.View.comboBoxModule.DataSource = availableModules;
        }

        private void DisplayImage(ImageProcessedPayload obj)
        {
            this.View.pictureBoxImage.SuspendLayout();
            try
            {
                 var oldImage = this.View.pictureBoxImage.Image;
                 this.View.pictureBoxImage.Image = null;
                 this.View.pictureBoxImage.Image = (Bitmap)obj.Image?.Clone();
                 this.View.pictureBoxImage.Refresh();
                 oldImage?.Dispose();
            }
            catch {}
            finally
            {
                this.View.pictureBoxImage.ResumeLayout();
                obj.Image?.Dispose();
            }
        }

        public override void Dispose()
        {
            ModulePipeServer.Value.Stop();
            ProcessingPipeline.Value.Stop();
            DriverPipeServer.Value.Stop();
            base.Dispose();
        }

        internal void StartModule()
        {
            var selectedModule  = this.View.comboBoxModule.SelectedItem as IDetectionModule;
            if (selectedModule != null)
            {
                ModuleWrapper.Value.Start(selectedModule);
            }

            //Disable Image-Processing if the Module doesnt support it 
            if(ModuleWrapper.Value.CurrentDetectionModule != null)
            {
                ModulePipeServer.Value.RetrieveImage = ModuleWrapper.Value.CurrentDetectionModule.SupportsImage;
            }
        }

        internal void StopModule()
        {
            this.View.richTextBoxModule.Clear();
            ModuleWrapper.Value.Stop();
        }
    }
}
