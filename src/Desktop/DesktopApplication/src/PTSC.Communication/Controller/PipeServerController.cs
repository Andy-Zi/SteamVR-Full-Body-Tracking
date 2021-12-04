using System.IO.Pipes;
using System.Text;
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

        public void Start()
        {
            // Create Async Pipe Server
            NamedPipeServerStream serverStream = new NamedPipeServerStream(ModulePipeDataModel.PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            // Wait for Client Connection
            serverStream.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), serverStream);
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeServerStream streamServer = (NamedPipeServerStream)iar.AsyncState;
                // End waiting for the connection
                streamServer.EndWaitForConnection(iar);

                byte[] buffer = new byte[ModulePipeDataModel.BufferSize];

                // Read the incoming message
                streamServer.Read(buffer, 0, ModulePipeDataModel.BufferSize);

                // Convert byte buffer to string
                string receivedData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                // Remove null Payload from string
                receivedData = receivedData.Replace("\0", string.Empty);
                Logger.Log("Received data: " + receivedData);         

                // Kill original server and create new wait server
                streamServer.Close();
                streamServer = null;
                streamServer = new NamedPipeServerStream(ModulePipeDataModel.PipeName, PipeDirection.In,
                   1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                // Recursively wait for the connection again and again....
                streamServer.BeginWaitForConnection(
                   new AsyncCallback(WaitForConnectionCallBack), streamServer);
            }
            catch
            {
                return;
            }
        }

        public void Stop()
        {

        }
    }

         
}
