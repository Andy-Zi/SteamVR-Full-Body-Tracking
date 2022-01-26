

using Interfaces;
using Newtonsoft.Json;
using System.Drawing;
using System.IO.Pipes;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace KinectModule
{
    public class KinectModuleClient
    {
        object LockObject = new object();

        NamedPipeClientStream pipeclient;
        IKinectAdapter kinect;
        Bitmap image = null;
        public KinectModuleClient(IKinectAdapter kinect)
        {
            pipeclient = new NamedPipeClientStream(".", "PTSCModulePipe", PipeDirection.Out);
            this.kinect = kinect;
            this.kinect.OnDataProcessed += Kinect_OnDataProcessed;
            this.kinect.OnImageProcessed += this.Kinect_OnImageProcessed;
        }

        private void Kinect_OnImageProcessed(Bitmap image)
        {
                this.image?.Dispose();
                this.image = null;
                this.image = image;    
        }

        public void Start()
        {
            pipeclient.Connect();
        }

        private void Kinect_OnDataProcessed(IModuleDataModel data)
        {

            if (pipeclient.IsConnected)
            { 
                    data.NormalizeToHead();
                    var text = JsonConvert.SerializeObject(data);
                    byte[] text_buffer = Encoding.UTF8.GetBytes(text);

                    if(image != null)
                    {
                        using (Mat mat = BitmapConverter.ToMat(image)) {
                            Cv2.ImEncode(".jpg", mat, out var img_buffer);
                            var combined_buffer = new byte[8192 + img_buffer.Length];
                            text_buffer.CopyTo(combined_buffer, 0);
                        img_buffer.CopyTo(combined_buffer, 8192);
                        pipeclient.Write(combined_buffer, 0, combined_buffer.Length);
                    } 
                        

                    }
                    else{
                        pipeclient.Write(text_buffer, 0, text_buffer.Length);
                    }
            }
        }
    }
}