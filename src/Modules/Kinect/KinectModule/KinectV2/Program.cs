using KinectModule;
using System;
using System.Threading.Tasks;

namespace KinectV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var kinectv1 = new KinectV2();
            var kinectModuleClient = new KinectModuleClient(kinectv1);
            kinectModuleClient.Start();

            while (true)
            {
                Task.Delay(100).Wait();
                Console.WriteLine("Hello from Kinect!");
            }
        }

    }
}
