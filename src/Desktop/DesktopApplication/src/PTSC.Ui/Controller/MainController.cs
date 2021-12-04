using PTSC.Interfaces;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using PTSC.Communication.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;


namespace PTSC.Ui.Controller
{
    public class MainController : BaseController<MainModel, MainView>
    {
        [Dependency] public ILogger Logger { get; set; }
        [Dependency] public PipeClientController PipeClient { get; set; }
        [Dependency] public PipeServerController PipeServer { get; set; }

        public MainController(MainView view) : base(view)
        {

        }

        public void SaySomething()
        {
            Logger.Log("foobar");
        }
        public override BaseController<MainModel, MainView> Initialize()
        {
            PipeServer.Start();
            return base.Initialize();
        }

        public override void Dispose()
        {
            PipeServer.Stop();
            base.Dispose();
        }

        public void PipeTest()
        {
            PipeClient.SendData("Hello World!");
            Logger.Log("Sent data");
        }
    }
}
