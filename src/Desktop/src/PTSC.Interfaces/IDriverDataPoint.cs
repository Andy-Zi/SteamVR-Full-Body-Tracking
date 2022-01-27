using System.Numerics;

namespace PTSC.Interfaces
{
    public interface IDriverDataPoint
    {
        public abstract void setPosition(List<double> data);
        public abstract void setRotation(Quaternion q);
        public abstract string Serialize(string seperator);
        public string Name { get; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double qW { get; set; }
        public double qX { get; set; }
        public double qY { get; set; }
        public double qZ { get; set; }
    }
}
