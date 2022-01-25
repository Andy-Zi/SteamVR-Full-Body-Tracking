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
            serialOutput += SerializeProperty(driverData["head"], "head");
            serialOutput += SerializeProperty(driverData["head_rotation"]);
            serialOutput += SerializeProperty(driverData["waist"], "waist");
            serialOutput += SerializeProperty(driverData["waist_rotation"]);
            serialOutput += SerializeProperty(driverData["left_foot"], "left_foot");
            serialOutput += SerializeProperty(driverData["left_foot_rotation"]);
            serialOutput += SerializeProperty(driverData["right_foot"], "right_foot");
            serialOutput += SerializeProperty(driverData["right_foot_rotation"]);
            return serialOutput.Replace(",", ".");
        }

        public string SerializeProperty(IDriverDataPoint driverDataPoint, string keyWord = "")
        {
            string serializedProperty = string.Empty;
            // add coordinate values to serialized string if they are not null
            if (driverDataPoint != null)
            {
                if (keyWord == "") // serialization of a point's rotation values
                {
                    serializedProperty += $"{driverDataPoint.rotationW}{seperator}{driverDataPoint.rotationX}{seperator}{driverDataPoint.rotationY}{seperator}{driverDataPoint.rotationZ}{seperator}";
                }
                else // serialization of a point's space coordinates values
                {
                    // serializing the values by x, z, y, because the coordinate system in switched between the y and z-axis
                    serializedProperty += $"{keyWord}{seperator}{driverDataPoint.X}{seperator}{driverDataPoint.Z}{seperator}{driverDataPoint.Y}{seperator}";
                }
            }
            return serializedProperty;
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
