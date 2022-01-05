using System.Collections;
using System.Reflection;
using System.Text.Json;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using Unity;

namespace PTSC.Communication.Controller
{
    public class DataController
    {
        [Dependency] public ILogger Logger { get; set; }

        public string seperator = ";";

        public string SerializeDriverData(IDriverDataModel driverData)
        {
            string serialOutput = string.Empty;

            // invert z axis
            //driverData.head[2] = -driverData.head[2];
            driverData.waist[2] = -driverData.waist[2];
            driverData.left_foot[2] = -driverData.left_foot[2];
            driverData.right_foot[2] = -driverData.right_foot[2];

            // add x, y, z coordinates after keyword
            serialOutput += SerializeProperty("head", driverData.head);
            serialOutput += SerializeProperty("waist", driverData.waist);
            serialOutput += SerializeProperty("left_foot", driverData.left_foot);
            serialOutput += SerializeProperty("right_foot", driverData.right_foot);
            return serialOutput.Replace(",", ".");
        }

        public string SerializeProperty(string keyWord, List<double> coordinates)
        {
            string serializedProperty = string.Empty;
            // add coordinate values to serialized string if they are not null
            if (coordinates != null)
            {
                serializedProperty = keyWord + seperator;
                foreach (var coord in coordinates.Take(3))
                {
                    serializedProperty += coord.ToString() + seperator;

                }
            }
            return serializedProperty;
        }

        public ModuleDataModel DeserializeModuleData(string jsonString)
        {
            if (jsonString != string.Empty)
            {
                return JsonSerializer.Deserialize<ModuleDataModel>(jsonString.Replace("\0", string.Empty));
            }
            return null;
        }
    }
}
