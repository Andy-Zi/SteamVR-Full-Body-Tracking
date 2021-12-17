using OpenCvSharp;
using Prism.Events;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using PTSC.OpenCV;
using PTSC.PubSub;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using Unity;

namespace PTSC.Communication.Controller
{
    /// <summary>
    /// Hosts the pipe-server that accepts the connections from the Detection-Modules
    /// </summary>
    public class ModulePipeServerController
    {
        [Dependency] public ILogger Logger { get; set; }
        [Dependency] public DataController DataController { get; set; }

        [Dependency] public IEventAggregator EventAggregator { get; set; }

        protected NamedPipeServerStream server;
        protected CancellationTokenSource cancellationTokenSource;
        protected DataRecievedEvent dataRecievedEvent;
        public CancellationToken CancellationToken { get; protected set; }

        /// <summary>
        /// Not retrieving an image saves a lot of CPU-Resources.
        /// </summary>
        public bool RetrieveImage { get; set; } = true;

        private int fpsLimit = -1;

        /// <summary>
        /// Set a FPS Limit to save CPU-Resources
        /// </summary>
        public int FPSLimit
        {
            get => fpsLimit;
            set {
                fpsLimit = value;
                shouldLimit = fpsLimit > 0;
                if (shouldLimit)
                    targetExecutionTime = (int)1000 /fpsLimit;
            }
        }
        protected int targetExecutionTime = 0;
        protected bool shouldLimit = false;

        public void Start()
        {
            // Create Pipe Server
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken = cancellationTokenSource.Token;
            dataRecievedEvent = EventAggregator.GetEvent<DataRecievedEvent>();
            StartServer();

        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        protected void StartServer()
        {
            server = new NamedPipeServerStream(ModulePipeDataModel.PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte);
            Task.Run(ServerFunction, CancellationToken);
        }

        protected void ServerFunction()
        {
            //Wait for a Client
            server.WaitForConnection();
            var message = new Span<byte>(new byte[ModulePipeDataModel.BufferSize]);
            Stopwatch stopwatch = new Stopwatch();
            IModuleDataModel moduleDataModel = null;
            Mat image = null;
            while (server.IsConnected)
            {
                if (shouldLimit)
                {
                    stopwatch.Start();
                }
                var lenght = server.Read(message);
                if (lenght > 0)
                {
                    //This has to be executed in Series because Span<T> can't be split between Tasks
                    if (RetrieveImage && lenght > ModulePipeDataModel.JsonBufferSize )
                    {
                        var imagedata = message[ModulePipeDataModel.JsonBufferSize..lenght];
                        image = HandleImageData(imagedata);
                    }

                    var jsondata = message[..ModulePipeDataModel.JsonBufferSize];

                    moduleDataModel = HandleJsonData(jsondata);
                    //Logger.Log(JsonSerializer.Serialize(moduleDataModel.LEFT_ANKLE));
                    //Logger.Log(JsonSerializer.Serialize(moduleDataModel.RIGHT_ANKLE));
                    //Dispatch Processing to different Thread
                    dataRecievedEvent.Publish(new DataRecievedPayload(moduleDataModel, image));
                }
                message.Clear();

                if(shouldLimit)
                {
                    //Delay Loop if necessary
                    stopwatch.Stop();
                    var delta = (int)(targetExecutionTime - stopwatch.ElapsedMilliseconds);
                    if (delta > 0)
                        Task.Delay(delta).Wait();
                    stopwatch.Reset();
                }
            }
            server.Close();

            //Restart the Server if it wasnt Cancelled!
            if (!CancellationToken.IsCancellationRequested)
                StartServer();
        }
        protected IModuleDataModel HandleJsonData(Span<byte> jsonData)
        {

            string receivedData = Encoding.UTF8.GetString(jsonData);
            return DataController.DeserializeModuleData(receivedData);
        }
        protected Mat HandleImageData(Span<byte> imageData)
        {
            return OpenCVService.DecodeImage(imageData);
        }
    }
}
