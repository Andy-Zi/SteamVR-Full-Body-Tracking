using Fclp;
using KinectModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectV1
{

    public class ApplicationArguments
    {
        public bool UseCamera { get; set; }
    }

    internal class Program
    {
        static KinectV1 kinect;
        static void Main(string[] args)
        {

            var p = new FluentCommandLineParser<ApplicationArguments>();
            p.Setup(arg => arg.UseCamera)
                .As('c', "camera")
                .SetDefault(false);

            var result = p.Parse(args);
            if (result.HasErrors)
                throw new ArgumentException(result.ErrorText);




            kinect = new KinectV1(p.Object.UseCamera);
            var kinectModuleClient = new KinectModuleClient(kinect);
            kinectModuleClient.Start();
            Console.WriteLine("Started Kinect V1!");
            Console.ReadLine();
            Console.WriteLine("Stopping Kinect V1!");
            kinect?.Stop();

        }
    }
}
