using System.Text.Json;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using Unity;

namespace PTSC.Communication.Controller
{
    public class DataController
    {
        [Dependency] public ILogger Logger { get; set; }
        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }

        public const string seperator = ";";

        public string SerializeDriverData(IDriverDataModel driverData)
        {
            string serialOutput = string.Empty;

            // add x, y, z coordinates after keyword
            serialOutput += SerializeProperty(driverData.head, "head");
            serialOutput += SerializeProperty(driverData.head_rotation);
            serialOutput += SerializeProperty(driverData.waist, "waist");
            serialOutput += SerializeProperty(driverData.waist_rotation);
            serialOutput += SerializeProperty(driverData.left_foot, "left_foot");
            serialOutput += SerializeProperty(driverData.left_foot_rotation);
            serialOutput += SerializeProperty(driverData.right_foot, "right_foot");
            serialOutput += SerializeProperty(driverData.right_foot_rotation);
            return serialOutput.Replace(",", ".");
        }

        public string SerializeProperty(List<double> coordinates, string keyWord = "")
        {
            string serializedProperty = string.Empty;
            // add coordinate values to serialized string if they are not null
            if (coordinates != null)
            {
                if (keyWord == "") // serialization for point direction
                {
                    serializedProperty += $"{coordinates[0]}{seperator}{coordinates[1]}{seperator}{coordinates[2]}{seperator}{coordinates[3]}{seperator}";
                }
                else // serialization for point coordinates
                {
                    serializedProperty += $"{keyWord}{seperator}{coordinates[0]}{seperator}{coordinates[2]}{seperator}{coordinates[1]}{seperator}";
                }
            }
            return serializedProperty;
        }

        public ModuleDataModel DeserializeModuleData(string jsonString)
        {
            if (jsonString != string.Empty)
            {
                string receivedData = jsonString.Replace("\0", string.Empty);
                Logger.Log($"Received ModuleData: {receivedData}");
                return JsonSerializer.Deserialize<ModuleDataModel>(receivedData);
            }
            return null;
        }
    }
}
