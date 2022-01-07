using System.Collections;
using System.Reflection;
using System.Text.Json;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using Unity;
using System;

namespace PTSC.Communication.Controller
{
    public class DataController
    {
        [Dependency] public ILogger Logger { get; set; }
        [Dependency] public IApplicationEnvironment ApplicationEnvironment { get; set; }

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
                // scale coordinate before serializing
                coordinates = ScaleCoordinates(coordinates);
                // rotate coordinate before serializing
                coordinates = RotateCoordinates(coordinates);
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

        private List<double> ScaleCoordinates(List<double> coordinates)
        {
            List<double> resultingCoordinate = new List<double>();
            double Scaling = ApplicationEnvironment.Settings.Scaling;
            foreach (var coord in coordinates.Take(3))
            {
                double newCoordinate = coord * Scaling;
                resultingCoordinate.Add(newCoordinate);
            }
            return resultingCoordinate;
        }

        private List<double> RotateCoordinates(List<double> coordinates)
        {
            double Rotation = ToRadians(ApplicationEnvironment.Settings.Rotation);
            double x = coordinates[0];
            double y = coordinates[1];
            double newX = Math.Cos(Rotation) * x - Math.Sin(Rotation) * y;
            double newY = Math.Sin(Rotation) * x + Math.Cos(Rotation) * y;
            double newZ = coordinates[2];
            return new List<double> { newX, newY, newZ };
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
