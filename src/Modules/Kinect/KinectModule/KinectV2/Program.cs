using KinectModule;
using System;
using System.Threading.Tasks;

namespace KinectV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var kinect = new KinectV2();
            var kinectModuleClient = new KinectModuleClient(kinect);
            kinectModuleClient.Start();
            Console.WriteLine("Started Kinect V2!");
            Console.ReadLine();
            Console.WriteLine("Stopping Kinect V2!");
            kinect.Stop();

        }

    }
}
