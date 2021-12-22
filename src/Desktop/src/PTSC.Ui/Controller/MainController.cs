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



        List<SubscriptionToken> SubscriptionTokens = new();
        public IEventAggregator EventAggregator { get; set; }

        bool shouldPlot3DGraph = false;

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
            SubscriptionTokens.Add(EventAggregator.GetEvent<ImageProcessedEvent>().Subscribe(DisplayImage, ThreadOption.UIThread, false));
            SubscriptionTokens.Add(EventAggregator.GetEvent<ModuleDataProcessedEvent>().Subscribe(Plot3DGraph, ThreadOption.UIThread, false));
            ModuleWrapper.Value.OnError += ModuleWrapper_OnMessage;
            ModuleWrapper.Value.OnMessage += ModuleWrapper_OnMessage;
            this.View.tabControlModuleView.Selected += TabControlModuleView_TabIndexChanged;
        }

        private void TabControlModuleView_TabIndexChanged(object sender, EventArgs e)
        {
            if(this.View.tabControlModuleView.SelectedTab.Text == "Module Output")
            {
                shouldPlot3DGraph = false;
                ModulePipeServer.Value.RetrieveImage = ModuleWrapper.Value.CurrentDetectionModule?.SupportsImage ?? true;
            }
            else
            {
                //Skeleton Page
                shouldPlot3DGraph = true;
                ModulePipeServer.Value.RetrieveImage = false;
            }
        }

        private void Plot3DGraph(ModuleDataProcessedPayload obj)
        {
            if (shouldPlot3DGraph)
            {
                this.View.Graph3D.Plot(obj.ModuleDataModel);
            }
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
            foreach(var token in SubscriptionTokens)
                token.Dispose();
            ModulePipeServer.Value.Stop();
            ProcessingPipeline.Value.Stop();
            DriverPipeServer.Value.Stop();

            Task.Delay(50).Wait();
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
