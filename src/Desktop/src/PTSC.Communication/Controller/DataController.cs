using System.Text.Json;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using Unity;

namespace PTSC.Communication.Controller
{
    public class DataController
    {
        [Dependency] public ILogger Logger { get; set; }
        [Dependency("ModuleDataLogger")] public ILogger ModuleDataLogger { get; set; }
        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }

        public const string seperator = ";";

        public string SerializeDriverData(IDriverData driverData)
        {
            string serialOutput = string.Empty;

            // add x, y, z coordinates after keyword
            serialOutput += driverData.waist.Serialize(seperator);
            serialOutput += driverData.left_foot.Serialize(seperator);
            serialOutput += driverData.right_foot.Serialize(seperator);
            return serialOutput.Replace(",", ".");
        }

        public ModuleDataModel DeserializeModuleData(string jsonString)
        {
            if (jsonString != string.Empty)
            {
                string receivedData = jsonString.Replace("\0", string.Empty);
                ModuleDataLogger.Log($"Received ModuleData: {receivedData}");
                return JsonSerializer.Deserialize<ModuleDataModel>(receivedData);
            }
            return null;
        }
    }
}
