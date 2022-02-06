using Prism.Events;
using PTSC.Interfaces;
using PTSC.Nameservice;
using PTSC.PubSub;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using Unity;

namespace PTSC.Ui.Controller
{
    public class DebugController : BaseController<MainModel, DebugView>
    {

        [Dependency] public IEventAggregator EventAggregator { get; set; }

        protected readonly List<string> modes = new() { "Driver","Module"};
        protected readonly List<string> driver_parts = new (){"None","Hip", "LeftFoot", "RightFoot" };
        protected readonly List<string> moduels_parts = ModulePipeConstants.SkeletonParts.Prepend("None").ToList();
        protected DebugView debugView => (DebugView)this.View;

        protected string partToObserve = null;
        protected string mode = "Driver";
        public DebugController(DebugView view) : base(view)
        {
           
        }
        


        protected override void BindData()
        {
            base.BindData();
            debugView.comboBox_Mode.DataSource = modes;
            BindPartSelection();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            debugView.comboBox_Mode.SelectedIndexChanged += ComboBox_Mode_SelectedIndexChanged;
            debugView.comboBox_Part.SelectedIndexChanged += ComboBox_Part_SelectedIndexChanged;

            EventAggregator.GetEvent<DataRecievedEvent>().Subscribe(OnDataRecieved,ThreadOption.UIThread, false);
            EventAggregator.GetEvent<DataProcessedEvent>().Subscribe(OnDataProcessed, ThreadOption.UIThread, false);
        }

        private void OnDataProcessed(DataProcessedPayload obj)
        {
            if (mode != "Driver")
                return;

            if (partToObserve == null)
                return;

            string DisplayData(IDriverDataPoint driverDataPoint)
            {
                string message = string.Empty;
                message += "Coordinates:\r\n";
                message += $"X: {driverDataPoint.X}\r\n";
                message += $"Y: {driverDataPoint.Y}\r\n";
                message += $"Z: {driverDataPoint.Z}\r\n";
                message += "Rotation:\r\n";
                message += $"X: {driverDataPoint.qX}\r\n";
                message += $"Y: {driverDataPoint.qY}\r\n";
                message += $"Z: {driverDataPoint.qZ}\r\n";
                message += $"W: {driverDataPoint.qW}\r\n";
                return message;
            }

            string toDisplay = null;

            switch (partToObserve)
            {
                case "Hip":
                    toDisplay = DisplayData(obj.DriverData.waist);
                    break;
                case "LeftFoot":
                    toDisplay = DisplayData(obj.DriverData.left_foot);
                    break;
                case "RightFoot":
                    toDisplay = DisplayData(obj.DriverData.right_foot);
                    break;
            }
            debugView.textBox.Text = toDisplay;
        }

        private void OnDataRecieved(DataRecievedPayload obj)
        {
            if (mode != "Module")
                return;

            if (partToObserve == null)
                return;


            string DisplayData(IModuleDataPoint moduleDataPoint)
            {
                string message = string.Empty;
                message += "Coordinates:\r\n";
                message += $"X: {moduleDataPoint.X}\r\n";
                message += $"Y: {moduleDataPoint.Y}\r\n";
                message += $"Z: {moduleDataPoint.Z}\r\n";
                message += $"Visibility: {moduleDataPoint.Visibility}\r\n";
                return message;
            }
            debugView.textBox.Text = DisplayData(obj.ModuleData[partToObserve]);
        }

        private void ComboBox_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (string)debugView.comboBox_Part.SelectedItem;
            if(item != "None")
            {
                partToObserve = item;
            }
            else
            {
                partToObserve = null;
            }
        }

        private void ComboBox_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (string)debugView.comboBox_Mode.SelectedItem;
            mode = item;
            BindPartSelection();
        }

        private void BindPartSelection()
        {
            if (mode == "Module")
                debugView.comboBox_Part.DataSource = moduels_parts;
            else
                debugView.comboBox_Part.DataSource =  driver_parts;
        }
    }
}
