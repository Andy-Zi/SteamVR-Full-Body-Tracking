using PTSC.Interfaces;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using PTSC.Communication.Controller;
using Unity;
using PTSC.Pipeline;
using Prism.Events;
using PTSC.PubSub;

namespace PTSC.Ui.Controller
{
    public class MainController : BaseController<MainModel, MainView>
    {
        [Dependency] public ILogger Logger { get; set; }
        //Lazy Dependency Injection
        [Dependency] public Lazy<PipeClientController> PipeClient { get; set; }
        [Dependency] public Lazy<PipeServerController> PipeServer { get; set; }
        [Dependency] public Lazy<ProcessingPipeline> ProcessingPipeline { get; set; }
        [Dependency] public Lazy<PipeClientController> PipeClientController { get; set; }
        
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
 
            PipeServer.Value.FPSLimit = 30;
            PipeServer.Value.RetrieveImage = true;
            PipeServer.Value.Start();
            ProcessingPipeline.Value.Start();
            PipeClientController.Value.Start();
            EventAggregator.GetEvent<ImageProcessedEvent>().Subscribe(DisplayImage, ThreadOption.UIThread, false);
            return base.Initialize();
        }

        private void DisplayImage(ImageProcessedPayload obj)
        {
            this.View.pictureBoxImage.SuspendLayout();
            try
            {
                 this.View.pictureBoxImage.Image = obj.Image;
                 this.View.pictureBoxImage.Refresh();
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
            PipeServer.Value.Stop();
            ProcessingPipeline.Value.Stop();
            PipeClientController.Value.Stop();
            base.Dispose();
        }


    }
}
