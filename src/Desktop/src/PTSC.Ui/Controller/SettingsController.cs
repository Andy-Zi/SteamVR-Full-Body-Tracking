﻿using PTSC.Communication.Controller;
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

        protected string GetControlBindingField(Control control)
        {
            switch (control)
            {
                case NumericUpDown _:
                    return "Value";
                case CheckBox _:
                    return "Checked";

                default:
                    return "Value";
            }
        }

  

        protected override void BindData()
        {
            Model = ((ApplicationSettingsModel)ApplicationEnvironment.Settings).Clone();
            Model.ResetState();
            void BindControlls(Control control)
            {
                if(control is GroupBox groupBox)
                {
                    foreach(var obj in groupBox.Controls)
                    {
                        BindControlls(obj as Control);
                    }
                }


                var propertyName = control.Tag as string;

                if (propertyName == null)
                    return;

                control.DataBindings.Add(GetControlBindingField(control), Model, propertyName);
            }

            foreach(var obj in this.View.Controls)
            {
                BindControlls(obj as Control);
            }
        }


        private void UpdateSettings()
        {
            UpdateModuleServer();
            UpdateKalman();
            UpdatePipeline();
            UpdateDataController();

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
        }

        private void UpdateDataController()
        {
            DataController.ScalingOffset = Model.Scaling;
            DataController.RotationOffset = Model.Rotation;

        }
    }
}
