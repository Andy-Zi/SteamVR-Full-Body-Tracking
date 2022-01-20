using KinectModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectV1
{
    internal class Program
    {
        static KinectV1 kinect;
        static void Main(string[] args)
        {
            kinect = new KinectV1();
            var kinectModuleClient = new KinectModuleClient(kinect);
            kinectModuleClient.Start();
            Console.WriteLine("Started Kinect V1!");
            Console.ReadLine();
            Console.WriteLine("Stopping Kinect V1!");
            kinect?.Stop();

        }
    }
}
