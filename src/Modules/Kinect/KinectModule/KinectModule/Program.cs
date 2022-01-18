using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace KinectModule
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var kinectv2 = new Kinectv2();
            var kinectModuleClient = new KinectModuleClient(kinectv2);
            kinectModuleClient.Start();

            while (true)
            {
                Task.Delay(100).Wait();
                Console.WriteLine("Hello from Kinect!");
            }
        }
    }
}
