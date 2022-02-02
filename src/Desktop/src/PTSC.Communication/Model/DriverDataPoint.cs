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
            Position.X = 0;
            Position.Y = 0;
            Position.Z = 0;
            Rotation.X = 0;
            Rotation.Y = 0;
            Rotation.Z = 0;
            Rotation.W = 0;
        }
        public DriverDataPoint(String name,float x, float y, float z, float qx, float qy, float qz, float qw)
        {
            this.name = name;
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            Rotation.X = qx;
            Rotation.Y = qy;
            Rotation.Z = qz;
            Rotation.W = qw;
        }

        void IDriverDataPoint.setPosition(List<double> data)
        {
            if (data.Count != 0)
            {
                this.Position.X = (float)data[0];
                this.Position.Y = (float)data[1];
                this.Position.Z = (float)data[2];
            }
        }

        string IDriverDataPoint.Serialize(string seperator)
        {
            string serializedProperty = string.Empty;

            serializedProperty += $"{Name}{seperator}";
            // serializing the values by x, z, y, because the coordinate system in switched between the y and z-axis
            serializedProperty += $"{X}{seperator}";
            serializedProperty += $"{Y}{seperator}";
            serializedProperty += $"{Z}{seperator}";
            // serialize point rotation
            serializedProperty += $"{qW}{seperator}";
            serializedProperty += $"{qX}{seperator}";
            serializedProperty += $"{qY}{seperator}";
            serializedProperty += $"{qZ}{seperator}";

            return serializedProperty;
        }

        public void setRotation(Quaternion q)
        {
            this.Rotation = q;
        }

        public Quaternion getRotation()
        {
            return this.Rotation;
        }


        string name;
        public Vector3 Position;
        public Quaternion Rotation;

        public string Name { get => name; }
        public double X { get => Position.X; set => Position.X = (float)value; }
        public double Y { get => Position.Y; set => Position.Y = (float)value; }
        public double Z { get => Position.Z; set => Position.Z = (float)value; }
        public double qW { get => Rotation.W; set => Rotation.W = (float)value; }
        public double qX { get => Rotation.X; set => Rotation.X = (float)value; }
        public double qY { get => Rotation.Y; set => Rotation.Y = (float)value; }
        public double qZ { get => Rotation.Z; set => Rotation.Z = (float)value; }
    }
}
