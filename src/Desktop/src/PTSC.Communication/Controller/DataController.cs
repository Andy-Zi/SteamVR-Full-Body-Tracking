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



        public const string seperator = ";";
        public double RotationOffset = 0;
        public double ScalingOffset = 1;

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
                // scale coordinate before serializing
                coordinates = ScaleCoordinates(coordinates);
                // rotate coordinate before serializing
                coordinates = RotateCoordinates(coordinates);

                serializedProperty += $"{keyWord}{seperator}{coordinates[0]}{seperator}{coordinates[2]}{seperator}{coordinates[1]}{seperator}";
                
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
            foreach (var coord in coordinates.Take(3))
            {
                double newCoordinate = coord * ScalingOffset;
                resultingCoordinate.Add(newCoordinate);
            }
            return resultingCoordinate;
        }

        private List<double> RotateCoordinates(List<double> coordinates)
        {
            double Rotation = ToRadians(RotationOffset);
            double x = coordinates[0];
            double y = coordinates[2];
            double newX = Math.Cos(Rotation) * x - Math.Sin(Rotation) * y;
            double newY = Math.Sin(Rotation) * x + Math.Cos(Rotation) * y;
            double newZ = coordinates[1];
            return new List<double> { newX, newY, newZ };
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
