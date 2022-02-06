using PTSC.Interfaces;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using PTSC.Communication.Controller;
using Unity;
using PTSC.Pipeline;
using Prism.Events;
using PTSC.PubSub;
using System.ComponentModel;
using PTSC.Ui.Modules;

namespace PTSC.Ui.Controller
{
    public class MainController : BaseController<MainModel, MainView>
    {
        public IUnityContainer UnityContainer { get; private set; }
        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }
        [Dependency] public ILogger Logger { get; set; }
        //Lazy Dependency Injection
        [Dependency] public Lazy<DriverPipeServerController> DriverPipeServer { get; set; }
        [Dependency] public Lazy<ModulePipeServerController> ModulePipeServer { get; set; }
        [Dependency] public Lazy<ProcessingPipeline> ProcessingPipeline { get; set; }
        [Dependency] public Lazy<ModuleWrapper> ModuleWrapper { get; set; }
        [Dependency] public ModuleRepository ModuleRepository { get; set; }

        [Dependency] public IKalmanFilterModel KalmanFilterModel { get; set; }


        List<SubscriptionToken> SubscriptionTokens = new();
        public IEventAggregator EventAggregator { get; set; }

        bool shouldPlot3DGraph = false;

        public MainController(MainView view) : base(view)
        {

        }

        public BaseController<MainModel, MainView> RegisterEventAggregator(IUnityContainer container)
        {
            //This need to be consturcted on the UI-Thread to enable the Publish on UI-Thread option!
            UnityContainer = container;
            EventAggregator = new EventAggregator();
            container.RegisterInstance<IEventAggregator>(EventAggregator);
            return this;
        }
        public override BaseController<MainModel, MainView> Initialize()
        {

#if DEBUG
            this.View.ShowDebugButton(true);
#else
            this.View.ShowDebugButton(false);
#endif
            ModulePipeServer.Value.FPSLimit = ApplicationEnvironment.Settings.FPSLimit;
            ModulePipeServer.Value.RetrieveImage = ApplicationEnvironment.Settings.SupportModuleImage;

            KalmanFilterModel.Initialize(ApplicationEnvironment.Settings);

            ProcessingPipeline.Value.UseKalmanFilter = ApplicationEnvironment.Settings.UseKalmanFilter;

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
            SubscriptionTokens.Add(EventAggregator.GetEvent<PipelineLatencyEvent>().Subscribe(PlotPipelineLatency, ThreadOption.UIThread, false));
            SubscriptionTokens.Add(EventAggregator.GetEvent<ModuleLatencyEvent>().Subscribe(PlotModuleLatency, ThreadOption.UIThread, false));
            SubscriptionTokens.Add(EventAggregator.GetEvent<ModuleConnectionEvent>().Subscribe(OnModuleConnected, ThreadOption.UIThread, false));
            SubscriptionTokens.Add(EventAggregator.GetEvent<DriverConnectionEvent>().Subscribe(OnDriverConnected, ThreadOption.UIThread, false));
            ModuleWrapper.Value.OnError += ModuleWrapper_OnMessage;
            ModuleWrapper.Value.OnMessage += ModuleWrapper_OnMessage;
            this.View.tabControlModuleView.Selected += TabControlModuleView_TabIndexChanged;
        }

        private void OnDriverConnected(ConnectionPayload obj)
        {
            this.View.labelDriverStateValue.Text = obj.IsConnected ? "Connected" : "Disconnected";
        }

        internal void ShowDebugView()
        {
            UnityContainer.Resolve<DebugController>()
              .Initialize()
              .View
              .Show();
        }

        internal void ShowModuleOptions()
        {
            var selectedModule = this.View.comboBoxModule.SelectedItem as ModuleModel;
            if(selectedModule != null)
            {
                var result = UnityContainer.Resolve<ModuleController>()
                .WithModule(selectedModule)
                .Initialize()
                .View
                .ShowDialog();

                if(result == DialogResult.OK)
                {
                    //Rebind the Combobox
                    var availableModules = new BindingList<IDetectionModule>(ModuleRepository.Values.ToList());
                    this.View.comboBoxModule.ValueMember = null;
                    this.View.comboBoxModule.DisplayMember = "Name";
                    this.View.comboBoxModule.DataSource = availableModules;
                }
            }

        }

        internal void ShowOptions()
        {
            UnityContainer.Resolve<SettingsController>()
                .Initialize()
                .View
                .ShowDialog();
        }

        private void OnModuleConnected(ConnectionPayload obj)
        {
            this.View.labelModuleStateValue.Text = obj.IsConnected ? "Connected" : "Disconnected";

            if (!obj.IsConnected)
            {
                this.View.richTextBoxModule.Clear();
                this.View.ModuleLatencyGraph.Clear();
                this.View.PiplineLatencyGraph.Clear();
                this.View.Graph3D.Clear();
                this.View.pictureBoxImage.SuspendLayout();
                this.View.pictureBoxImage.Image = null;
                this.View.pictureBoxImage.Refresh();
                this.View.pictureBoxImage.ResumeLayout();
            }
        }

        private void PlotModuleLatency(LatencyPayload obj)
        {
            this.View.ModuleLatencyGraph.Update(obj.Latency);
        }

        private void PlotPipelineLatency(LatencyPayload obj)
        {
            this.View.PiplineLatencyGraph.Update(obj.Latency);
        }

        private void TabControlModuleView_TabIndexChanged(object sender, EventArgs e)
        {
            if(this.View.tabControlModuleView.SelectedTab.Text == "Module Output")
            {
                shouldPlot3DGraph = false;
                ModulePipeServer.Value.RetrieveImage = ModuleWrapper.Value.CurrentDetectionModule?.SupportsImage ?? true && ApplicationEnvironment.Settings.SupportModuleImage;
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
                this.View.Graph3D.Plot(obj.ModuleData);
            }
        }

        private void ModuleWrapper_OnMessage(string message)
        {
            this.View.Invoke(() =>
            {
                this.View.richTextBoxModule.Text += message + "\n";
            });
        }

        protected override void BindData()
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

                if (ModuleWrapper.Value.CurrentDetectionModule != null)
                {
                    if (!ModuleWrapper.Value.CurrentDetectionModule.Name.Equals(selectedModule.Name))
                        StopModule();
                }
                ModuleWrapper.Value.Start(selectedModule);
            }

            //Disable Image-Processing if the Module doesnt support it 
            if(ModuleWrapper.Value.CurrentDetectionModule != null)
            {
                ModulePipeServer.Value.RetrieveImage = ModuleWrapper.Value.CurrentDetectionModule.SupportsImage && ApplicationEnvironment.Settings.SupportModuleImage;
            }
        }

        internal void StopModule()
        {
            ModuleWrapper.Value.Stop();
        }
    }
}
