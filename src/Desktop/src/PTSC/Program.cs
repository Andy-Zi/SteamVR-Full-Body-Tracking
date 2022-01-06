using PTSC.Communication.Controller;
using PTSC.Interfaces;
using PTSC.Modules;
using PTSC.Pipeline;
using PTSC.Pipeline.Kalman;
using PTSC.Ui.Controller;
using PTSC.Ui.View;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

namespace PTSC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var executablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string moduleDirectory = executablePath;


#if DEBUG
            //Get 'src'-Directory of Repo
            for (int i = 0; i < 4; i++)
                moduleDirectory = Directory.GetParent(moduleDirectory).FullName;

            moduleDirectory = Path.Combine(moduleDirectory, "Modules");

#else
            // In release Mode we expect a "Modules" Directory next to the *.exe
            moduleDirectory = Path.Combine(moduleDirectory, "Modules");
#endif


            var container = new UnityContainer();
            container.RegisterInstance<ILogger>(new Logger(executablePath));
            var repo = container.Resolve<ModuleRepository>(new ResolverOverride[] { new ParameterOverride("moduleDirectory", moduleDirectory) });
            repo.Load();
            container.RegisterInstance(repo);

            container.RegisterType<MainView, MainView>();
            container.RegisterType<MainController, MainController>();


            container.RegisterType<DriverPipeServerController, DriverPipeServerController>(new ContainerControlledLifetimeManager());
            container.RegisterType<DriverPipeServerController, DriverPipeServerController>(new ContainerControlledLifetimeManager());
            container.RegisterType<ProcessingPipeline, ProcessingPipeline>(new ContainerControlledLifetimeManager());
            container.RegisterType<ModuleWrapper, ModuleWrapper>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<IKalmanFilterModel>(new KalmanFilterModel());

            ApplicationConfiguration.Initialize();
            var controller = container.Resolve<MainController>();
            Application.Run(controller.RegisterEventAggregator(container).Initialize().View);
        }
    }
}