using System.Numerics;

namespace PTSC.Interfaces
{
    public interface IDriverDataPoint
    {
        void setPosition(List<double> data);
        void setRotation(Quaternion q);
        Quaternion getRotation();
        string Serialize(string seperator);
        string Name { get; }
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }
        double qW { get; set; }
        double qX { get; set; }
        double qY { get; set; }
        double qZ { get; set; }
    }
}
