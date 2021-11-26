using PTSC.Interfaces;
using PTSC.Ui.Controller;
using PTSC.Ui.View;
using Unity;

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
            var container = new UnityContainer();
            container.RegisterInstance<ILogger>(new Logger(System.Reflection.Assembly.GetExecutingAssembly().Location));

            container.RegisterType<MainView, MainView>();
            container.RegisterType<MainController, MainController>();

            ApplicationConfiguration.Initialize();
            var controller = container.Resolve<MainController>();
            Application.Run(controller.Initialize().View);
        }
    }
}