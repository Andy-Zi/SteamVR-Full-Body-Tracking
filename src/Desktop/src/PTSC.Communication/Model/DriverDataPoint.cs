using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PTSC.Interfaces;

namespace PTSC.Communication.Model
{
    public class DriverDataPoint : IDriverDataPoint
    {
        public DriverDataPoint(String name)
        {
            this.name = name;
            position.X = 0;
            position.Y = 0;
            position.Z = 0;
            rotation.X = 0;
            rotation.Y = 0;
            rotation.Z = 0;
            rotation.W = 0;
        }
        public DriverDataPoint(String name,float x, float y, float z, float qx, float qy, float qz, float qw)
        {
            this.name = name;
            position.X = x;
            position.Y = y;
            position.Z = z;
            rotation.X = qx;
            rotation.Y = qy;
            rotation.Z = qz;
            rotation.W = qw;
        }

        void IDriverDataPoint.setPosition(List<double> data)
        {
            if (data.Count != 0)
            {
                this.position.X = (float)data[0];
                this.position.Y = (float)data[1];
                this.position.Z = (float)data[2];
            }
        }

        string IDriverDataPoint.Serialize(string seperator)
        {
            string serializedProperty = string.Empty;

            serializedProperty += $"{Name}{seperator}";
            // serializing the values by x, z, y, because the coordinate system in switched between the y and z-axis
            serializedProperty += $"{position.X}{seperator}";
            serializedProperty += $"{position.Y}{seperator}";
            serializedProperty += $"{position.Z}{seperator}";
            // serialize point rotation
            serializedProperty += $"{rotation.W}{seperator}";
            serializedProperty += $"{rotation.X}{seperator}";
            serializedProperty += $"{rotation.Y}{seperator}";
            serializedProperty += $"{rotation.Z}{seperator}";

            return serializedProperty;
        }

        public void setRotation(Quaternion q)
        {
            this.rotation = q;
        }

        string name;
        Vector3 position;
        Quaternion rotation;

        public string Name { get => name; }
        public double X { get => position.X; set => position.X = (float)value; }
        public double Y { get => position.Y; set => position.Y = (float)value; }
        public double Z { get => position.Z; set => position.Z = (float)value; }
        public double qW { get => rotation.W; set => rotation.W = (float)value; }
        public double qX { get => rotation.X; set => rotation.X = (float)value; }
        public double qY { get => rotation.Y; set => rotation.Y = (float)value; }
        public double qZ { get => rotation.Z; set => rotation.Z = (float)value; }
    }
}
