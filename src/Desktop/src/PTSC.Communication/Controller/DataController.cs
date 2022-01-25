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
            serialOutput += SerializeProperty("head", driverData["head"], driverData["head_rotation"]);
            serialOutput += SerializeProperty("waist", driverData["waist"], driverData["waist_rotation"]);
            serialOutput += SerializeProperty("left_foot", driverData["left_foot"], driverData["left_foot_rotation"]);
            serialOutput += SerializeProperty("right_foot", driverData["right_foot"], driverData["right_foot_rotation"]);
            return serialOutput.Replace(",", ".");
        }

        public string SerializeProperty(string keyWord, IDriverDataPoint driverDataPoint, IDriverDataPoint driverDataPointRotation)
        {
            string serializedProperty = string.Empty;
            // add coordinate values to serialized string if they are not null
            if (driverDataPoint != null && driverDataPointRotation != null)
            {
                serializedProperty += $"{keyWord}{seperator}";
                // serializing the values by x, z, y, because the coordinate system in switched between the y and z-axis
                serializedProperty += $"{driverDataPoint.X}{seperator}";
                serializedProperty += $"{driverDataPoint.Z}{seperator}";
                serializedProperty += $"{driverDataPoint.Y}{seperator}";
                // serialize point rotation
                serializedProperty += $"{driverDataPointRotation.rotationW}{seperator}";
                serializedProperty += $"{driverDataPointRotation.rotationX}{seperator}";
                serializedProperty += $"{driverDataPointRotation.rotationY}{seperator}";
                serializedProperty += $"{driverDataPointRotation.rotationZ}{seperator}";
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
