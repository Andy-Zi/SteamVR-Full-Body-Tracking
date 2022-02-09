using PTSC.Communication.Controller;
using PTSC.Interfaces;
using PTSC.Pipeline;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using System.Text.Json;
using Unity;

namespace PTSC.Ui.Controller
{
    public class SettingsController : BaseController<ApplicationSettingsModel, SettingsView>
    {

        [Dependency] public ModulePipeServerController ModulePipeServer { get; set; }
        [Dependency] public ProcessingPipeline ProcessingPipeline { get; set; }
        [Dependency] public DataController DataController { get; set; }
        [Dependency] public IKalmanFilterModel KalmanFilterModel { get; set; }

        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }
        ApplicationSettingsModel Model;

        public SettingsController(SettingsView view) : base(view)
        {

        }

        internal void OnOk()
        {
            ApplicationEnvironment.Settings = Model;
            File.WriteAllText(ApplicationEnvironment.SettingsPath, JsonSerializer.Serialize(ApplicationEnvironment.Settings, new JsonSerializerOptions() { WriteIndented = true }));
            UpdateSettings();
            this.View.Close();
        }

        internal void OnApply()
        {
            UpdateSettings();
        }

        internal void OnCancel()
        {
            Model = ((ApplicationSettingsModel)ApplicationEnvironment.Settings).Clone();
            UpdateSettings();
            this.View.Close();
        }

        protected override void BindData()
        {
            Model = ((ApplicationSettingsModel)ApplicationEnvironment.Settings).Clone();
            Model.ResetState();
            ViewBindings.BindView(this.View, Model);
        }

        private void UpdateSettings()
        {
            UpdateModuleServer();
            UpdateKalman();
            UpdatePipeline();

        }

        private void UpdateModuleServer()
        {
            ModulePipeServer.FPSLimit = Model.FPSLimit;
            ModulePipeServer.RetrieveImage = ModulePipeServer.RetrieveImage && Model.SupportModuleImage;
        }

        private void UpdateKalman()
        {
            KalmanFilterModel.Initialize(Model);
        }

        private void UpdatePipeline()
        {
            ProcessingPipeline.UseKalmanFilter = Model.UseKalmanFilter;
            ProcessingPipeline.ScalingOffset = Model.Scaling;
            ProcessingPipeline.RotationOffset = Model.Rotation;
            ProcessingPipeline.UseHipAsFootRotation = Model.UseHipAsFootRotation;
        }
    }
}
