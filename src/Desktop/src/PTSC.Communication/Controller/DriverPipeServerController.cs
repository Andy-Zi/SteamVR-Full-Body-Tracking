using System.IO.Pipes;
using System.Text;
using PTSC.Interfaces;
using Unity;
using PTSC.Communication.Model;
using Prism.Events;
using PTSC.PubSub;

namespace PTSC.Communication.Controller
{

    public class DriverPipeServerController
    {
        public DriverPipeServerController()
        {

        }

        [Dependency] public ILogger Logger { get; set; }

        [Dependency] public DataController DataController { get; set; }

        [Dependency] public IEventAggregator EventAggregator { get; set; }

        protected SubscriptionToken subscriptionToken;
        protected NamedPipeServerStream server;
        protected CancellationTokenSource cancellationTokenSource;
        protected DataProcessedEvent dataProcessedEvent;
        public CancellationToken CancellationToken { get; protected set; }
        bool isClientConnected;
        public void Start()
        {
            try
            {
                dataProcessedEvent = EventAggregator.GetEvent<DataProcessedEvent>();
                subscriptionToken = EventAggregator.GetEvent<DataProcessedEvent>().Subscribe(SendData, ThreadOption.BackgroundThread);
                server = new NamedPipeServerStream(DriverPipeDataModel.PipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte);
                WaitForConnection();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }


        private void WaitForConnection()
        {
            isClientConnected = false;
            Task.Run(() =>
            {
                server.WaitForConnection();
                Logger.Log("Driver pipe server: client connected.");
                isClientConnected = true;
            });
        }

        private void SendData(DataProcessedPayload obj)
        {
            // send data if a client is connected to the pipe
            if (isClientConnected)
            {
                Logger.Log("Sending data to driver");
                string serializedDataString = DataController.SerializeDriverData(obj.DriverDataModel);
                Logger.Log($"Serializing driver data: {serializedDataString}");
                byte[] buffer = Encoding.UTF8.GetBytes(serializedDataString);
                try
                {
                    server?.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    //Restart the Server on Error 
                    server?.Close();
                    server = new NamedPipeServerStream(DriverPipeDataModel.PipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte);
                    WaitForConnection();
                }
                
            }
        }

        public void Stop()
        {
            server.Dispose();
        }
    }

         
}
