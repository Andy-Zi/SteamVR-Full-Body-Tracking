using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTSC.Interfaces;
using Unity;
using PTSC.Communication.Model;


namespace PTSC.Communication.Controller
{

    public class PipeClientController
    {
        public PipeClientController()
        {

        }

        [Dependency] public ILogger Logger { get; set; }
        public void SendData(string data)
        {
            try
            {
                NamedPipeClientStream clientStream = new NamedPipeClientStream(".", DriverPipeDataModel.PipeName, PipeDirection.Out, PipeOptions.Asynchronous);
                clientStream.Connect(DriverPipeDataModel.ClientTimeout);
                Logger.Log("Pipe connection established.");
                byte[] buffer = Encoding.UTF8.GetBytes(data);

                clientStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(AsyncSend), clientStream);
            }
            catch (TimeoutException ex)
            {
                Logger.Log(ex.Message);
            }
        }

        private void AsyncSend(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeClientStream pipeStream = (NamedPipeClientStream)iar.AsyncState;

                // End the write
                pipeStream.EndWrite(iar);
                pipeStream.Flush();
                pipeStream.Close();
                pipeStream.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }
    }

         
}
