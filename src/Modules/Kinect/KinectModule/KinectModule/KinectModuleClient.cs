

using Newtonsoft.Json;
using System.IO.Pipes;
using System.Text;

namespace KinectModule
{
    internal class KinectModuleClient
    {
        NamedPipeClientStream pipeclient;
        Kinectv2 kinect;
        public KinectModuleClient(Kinectv2 kinect)
        {
            pipeclient = new NamedPipeClientStream(".", "PTSCModulePipe", PipeDirection.Out);
            this.kinect = kinect;
            this.kinect.OnDataProcessed += Kinect_OnDataProcessed;
        }

        public void Start()
        {
            pipeclient.Connect();
        }

        private void Kinect_OnDataProcessed(ModuleDataModel data)
        {

            var text = JsonConvert.SerializeObject(data);
            if (pipeclient.IsConnected)
            {
                
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                pipeclient.Write(buffer, 0, buffer.Length);
            }
        }
    }
}