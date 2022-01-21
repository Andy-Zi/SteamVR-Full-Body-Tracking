using System.IO.Pipes;
using System.Text;
using PTSC.Interfaces;
using Unity;
using Prism.Events;
using PTSC.PubSub;
using PTSC.Nameservice;

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
        private DriverConnectionEvent driverConnectionEvent;
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
                subscriptionToken = dataProcessedEvent.Subscribe(SendData, ThreadOption.BackgroundThread);
                driverConnectionEvent = EventAggregator.GetEvent<DriverConnectionEvent>();
                server = new NamedPipeServerStream(DriverPipeConstants.PipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte);
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
                driverConnectionEvent.Publish(new ConnectionPayload(true));
            });
        }

        private void SendData(DataProcessedPayload obj)
        {
            Logger.Log($"Serialized DriverData: {DataController.SerializeDriverData(obj.DriverDataModel)}");
            // send data if a client is connected to the pipe
            if (isClientConnected)
            {
                string serializedDataString = DataController.SerializeDriverData(obj.DriverDataModel);
                Logger.Log($"Serialized DriverData: {serializedDataString}");
                byte[] buffer = Encoding.UTF8.GetBytes(serializedDataString);
                try
                {
                    server?.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    //Restart the Server on Error 
                    server?.Close();
                    driverConnectionEvent.Publish(new ConnectionPayload(true));
                    server = new NamedPipeServerStream(DriverPipeConstants.PipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte);
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
