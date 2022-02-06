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

        object LockObject = new object();
        [Dependency] public ILogger Logger { get; set; }
        [Dependency("DriverDataLogger")] public ILogger DriverDataLogger { get; set; }

        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }

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
                CreateServer(true);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void CreateServer(bool force=false)
        {
            lock (LockObject)
            {
                if (!isClientConnected && !force)
                    return;

                isClientConnected = false;

                server?.Close();
                server?.Dispose();

                server = new NamedPipeServerStream(DriverPipeConstants.PipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte);
                
                Task.Run(() =>
                {
                    server.WaitForConnection();
                    Logger.Log("Driver pipe server: client connected.");
                    isClientConnected = true;
                    driverConnectionEvent.Publish(new ConnectionPayload(true));
                });
            }
        }

        private void SendData(DataProcessedPayload obj)
        {
            string serializedDataString = DataController.SerializeDriverData(obj.DriverData);
            if (ApplicationEnvironment.Settings.LogPositionData)
                DriverDataLogger.Log($"{DataController.SerializeDriverData(obj.DriverData)}");
            // send data if a client is connected to the pipe
            if (isClientConnected)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(serializedDataString);
                try
                {
                    server?.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    //Restart the Server on Error 
                    
                    driverConnectionEvent.Publish(new ConnectionPayload(false));
                    CreateServer();
                }
                
            }
        }

        public void Stop()
        {
            server.Dispose();
        }
    }

         
}
