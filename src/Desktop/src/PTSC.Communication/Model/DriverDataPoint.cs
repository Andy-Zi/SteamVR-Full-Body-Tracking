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
        public DriverDataPoint(string name)
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

        protected float Round(float value)
        {
            return MathF.Round(value, 3);
        }
        public DriverDataPoint(string name,float x, float y, float z, float qx, float qy, float qz, float qw)
        {
            this.name = name;
            Position.X = Round(x);
            Position.Y = Round(y);
            Position.Z = Round(z);
            Rotation.X = Round(qx);
            Rotation.Y = Round(qy);
            Rotation.Z = Round(qz);
            Rotation.W = Round(qw);
        }

        void IDriverDataPoint.setPosition(List<double> data)
        {
            if (data.Count != 0)
            {
                this.X = (float)data[0];
                this.Y = (float)data[1];
                this.Z = (float)data[2];
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
            this.qW = q.W;
            this.qX = q.X;
            this.qY = q.Y;
            this.qZ = q.Z;
        }

        public Quaternion getRotation()
        {
            return this.Rotation;
        }


        string name;
        public Vector3 Position;
        public Quaternion Rotation;

        public string Name { get => name; }
        public float X { get => Position.X; set => Position.X = Round(value); }
        public float Y { get => Position.Y; set => Position.Y = Round(value); }
        public float Z { get => Position.Z; set => Position.Z = Round(value); }
        public float qW { get => Rotation.W; set => Rotation.W = Round(value); }
        public float qX { get => Rotation.X; set => Rotation.X = Round(value); }
        public float qY { get => Rotation.Y; set => Rotation.Y = Round(value); }
        public float qZ { get => Rotation.Z; set => Rotation.Z = Round(value); }
    }
}
