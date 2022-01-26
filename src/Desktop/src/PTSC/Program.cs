using PTSC.Communication.Controller;
using PTSC.Interfaces;
using PTSC.Pipeline;
using PTSC.Pipeline.Kalman;
using PTSC.Ui.Controller;
using PTSC.Ui.Model;
using PTSC.Ui.Modules;
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

            var executableDirecory = System.AppContext.BaseDirectory;

            var logger = new Logger(executableDirecory);
            logger.Log($"Base Path: {executableDirecory}");
            string moduleDirectory = executableDirecory;

            try
            {
#if DEBUG
                logger.Log("Running in Debug Mode");
                //Get 'src'-Directory of Repo
                for (int i = 0; i < 6; i++)
                    moduleDirectory = Directory.GetParent(moduleDirectory).FullName;

                moduleDirectory = Path.Combine(moduleDirectory, "Modules");

#else
                logger.Log("Running in Release Mode");
                // In release Mode we expect a "Modules" Directory next to the *.exe
                moduleDirectory = Path.Combine(moduleDirectory, "Modules");
#endif
            }
            catch (Exception e)
            {
                logger.Log(e.Message);
            }



            applicationEnvironment.ModulesDirectory = moduleDirectory;
            applicationEnvironment.SettingsPath = Path.Combine(executableDirecory, "ApplicationSettings.json");
            
            try
            {
                //Try to Load the Settings
                if (File.Exists(applicationEnvironment.SettingsPath))
                {
                    var settings = JsonSerializer.Deserialize<ApplicationSettingsModel>(File.ReadAllText(applicationEnvironment.SettingsPath));
                    settings.ResetState();
                    applicationEnvironment.Settings = settings;
                }
                else
                {
                    //Create New
                    applicationEnvironment.Settings = new ApplicationSettingsModel().Default();
                    File.WriteAllText(applicationEnvironment.SettingsPath, JsonSerializer.Serialize(applicationEnvironment.Settings, new JsonSerializerOptions() { WriteIndented = true }));
                }
            }catch (Exception e)
            {
                logger?.Log(e.Message);
            }

            logger?.Log("Building Container");

            try
            {
                var container = new UnityContainer();


                applicationEnvironment.LoggingPath = logger.LoggingPath;

                container.RegisterInstance<ILogger>(logger);
                container.RegisterInstance<ILogger>("ModuleDataLogger", new Logger(executableDirecory, "moduleData.txt"));
                container.RegisterInstance<ILogger>("DriverDataLogger", new Logger(executableDirecory, "driverData.txt"));

                container.RegisterInstance<IApplicationEnvironment>(applicationEnvironment);

                var repo = container.Resolve<ModuleRepository>(new ResolverOverride[] { new ParameterOverride("moduleDirectory", moduleDirectory) });
                repo.Load();
                container.RegisterInstance(repo);

                container.RegisterType<MainView, MainView>();
                container.RegisterType<MainController, MainController>();


                container.RegisterType<DriverPipeServerController, DriverPipeServerController>(new ContainerControlledLifetimeManager());
                container.RegisterType<DriverPipeServerController, DriverPipeServerController>(new ContainerControlledLifetimeManager());
                container.RegisterType<DataController, DataController>(new ContainerControlledLifetimeManager());
                container.RegisterType<ProcessingPipeline, ProcessingPipeline>(new ContainerControlledLifetimeManager());
                container.RegisterType<ModuleWrapper, ModuleWrapper>(new ContainerControlledLifetimeManager());
                container.RegisterInstance<IKalmanFilterModel>(new KalmanFilterModel());

                ApplicationConfiguration.Initialize();
                var controller = container.Resolve<MainController>();
                Application.Run(controller.RegisterEventAggregator(container).Initialize().View);
            }catch(Exception e)
            {
                logger.Log("Fatal Error!");
                logger.Log(e.Message);
            }
           
        }
    }
}