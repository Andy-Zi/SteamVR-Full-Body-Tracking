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
        static void Main(string[] args)
        {
            var kinectv1 = new KinectV1();
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
