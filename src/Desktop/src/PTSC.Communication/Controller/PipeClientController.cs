using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTSC.Interfaces;
using Unity;
using PTSC.Communication.Model;
using Prism.Events;
using PTSC.PubSub;

namespace PTSC.Communication.Controller
{

    public class PipeClientController
    {
        public PipeClientController()
        {

        }

        [Dependency] public ILogger Logger { get; set; }

        [Dependency] public DataController DataController { get; set; }

        [Dependency] public IEventAggregator EventAggregator { get; set; }

        SubscriptionToken subscriptionToken;
        NamedPipeClientStream ClientStream;
        public void Start()
        {
            try
            {
                ClientStream = new NamedPipeClientStream(".", DriverPipeDataModel.PipeName, PipeDirection.Out);
                ClientStream.Connect(DriverPipeDataModel.ClientTimeout);
                subscriptionToken = EventAggregator.GetEvent<DataProcessedEvent>().Subscribe(SendData,ThreadOption.BackgroundThread);
                Logger.Log("Pipe connection established.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        private void SendData(DataProcessedPayload obj)
        {
            if (ClientStream?.IsConnected ?? false)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(DataController.SerializeDriverData(obj.DriverDataModel));
                ClientStream?.Write(buffer, 0, buffer.Length);
            }
        }

        public void Stop()
        {
            ClientStream.Dispose();
        }
    }

         
}
