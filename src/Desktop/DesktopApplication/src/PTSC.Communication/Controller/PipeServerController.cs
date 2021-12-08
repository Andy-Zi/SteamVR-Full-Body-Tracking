using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using Unity;

//public delegate void DelegateMessage(string Reply);

namespace PTSC.Communication.Controller
{
    public class PipeServerController
    {
        //public event DelegateMessage PipeMessage;
        [Dependency] public ILogger Logger { get; set; }
        protected NamedPipeServerStream server;
        public void Start()
        {
            // Create Pipe Server
           server = new NamedPipeServerStream(ModulePipeDataModel.PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte);
           Task.Run(ServerFunction);
        }


        protected void ServerFunction()
        {
            server.WaitForConnection();
            while (true)
            {
                var stopwatch = Stopwatch.StartNew();
                var message = new Span<byte>(new byte[ModulePipeDataModel.BufferSize]);
                var lenght = server.Read(message);
                if (lenght > 0)
                {
                    if (lenght == ModulePipeDataModel.JsonBufferSize)
                    {

                        //Only JSON no Image
                        var jsondata = message[..ModulePipeDataModel.JsonBufferSize];
                        string receivedData = Encoding.UTF8.GetString(jsondata.ToArray(), 0, jsondata.Length);

                        Logger.Log($"Received data: {receivedData}");
                        stopwatch.Stop();
                        Logger.Log($"Time: {stopwatch.ElapsedMilliseconds}");

                    }
                    else
                    {
                        var jsondata = message[..ModulePipeDataModel.JsonBufferSize];
                        var imagedata = message[ModulePipeDataModel.JsonBufferSize..lenght];
                        var img = Cv2.ImDecode(imagedata, ImreadModes.Color);
                        Cv2.Flip(img, img, FlipMode.Y);
                        var test = BitmapConverter.ToBitmap(img);
                        stopwatch.Stop();
                        Logger.Log($"Time: {stopwatch.ElapsedMilliseconds}");
                    }
                }
                message.Clear();
            }
        }

        public void Stop()
        {
            server.Close();
        }
    }

         
}
