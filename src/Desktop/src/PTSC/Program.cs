using PTSC.Communication.Controller;
using PTSC.Interfaces;
using PTSC.Modules;
using PTSC.Pipeline;
using PTSC.Pipeline.Kalman;
using PTSC.Ui.Controller;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using System.Text.Json;
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
            var applicationEnvironment =  new ApplicationEnvironment();

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


            applicationEnvironment.ModulesDirectory = moduleDirectory;
            applicationEnvironment.SettingsPath = Path.Combine(executablePath, "ApplicationSettings.json");

            //Try to Load the Settings
            if (File.Exists(applicationEnvironment.SettingsPath))
            {
                var settings = JsonSerializer.Deserialize<ApplicationSettingsModel>(File.ReadAllText(applicationEnvironment.SettingsPath));
                settings.ResetState();
                applicationEnvironment.Settings = settings;
            }
            else{
                //Create New
                applicationEnvironment.Settings = new ApplicationSettingsModel().Default();
                File.WriteAllText(applicationEnvironment.SettingsPath, JsonSerializer.Serialize(applicationEnvironment.Settings,new JsonSerializerOptions() { WriteIndented = true}));
            }

            var container = new UnityContainer();

            var logger = new Logger(executablePath);
            applicationEnvironment.LoggingPath = logger.LoggingPath;

            container.RegisterInstance<ILogger>(logger);

            container.RegisterInstance<IApplicationEnvironment>(applicationEnvironment);

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