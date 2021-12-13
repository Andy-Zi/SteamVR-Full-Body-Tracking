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

        public string SerializeDriverData(object obj)
        {
            string serialOutput = string.Empty;
            IList<PropertyInfo> props = new List<PropertyInfo>(obj.GetType().GetProperties()); // list of object properties

            foreach (var prop in props)
            {
                // check if property is of enumerable type
                if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    IList values = prop.GetValue(obj, null) as IList;
                    serialOutput += prop.Name + ";";
                    foreach (var val in values)
                    {
                        serialOutput += val.ToString() + ";";
                    }
                }
                else
                {
                    serialOutput += prop.Name + ";";
                    serialOutput += prop.GetValue(obj, null).ToString() + ";";
                }
            }
            Logger.Log("Serialized data: " + serialOutput);
            return serialOutput;
        }

        public ModuleDataModel DeserializeModuleData(string jsonString)
        {
            return JsonSerializer.Deserialize<ModuleDataModel>(jsonString);
        }
    }
}
