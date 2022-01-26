using Fclp;
using KinectModule;
using System;
using System.Threading.Tasks;

namespace KinectV2
{
    internal class Program
    {

        public class ApplicationArguments
        {
            public bool UseCamera { get; set; }
        }


        static void Main(string[] args)
        {

            var p = new FluentCommandLineParser<ApplicationArguments>();
            p.Setup(arg => arg.UseCamera)
                .As('c', "camera")
                .SetDefault(true);

            var result = p.Parse(args);
            if (result.HasErrors)
                throw new ArgumentException(result.ErrorText);

            var kinect = new KinectV2(p.Object.UseCamera);
            var kinectModuleClient = new KinectModuleClient(kinect);
            kinectModuleClient.Start();
            Console.WriteLine("Started Kinect V2!");
            Console.ReadLine();
            Console.WriteLine("Stopping Kinect V2!");
            kinect.Stop();

        }

    }
}
