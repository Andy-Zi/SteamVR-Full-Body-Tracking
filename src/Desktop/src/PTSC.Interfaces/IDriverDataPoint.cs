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
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
        float qW { get; set; }
        float qX { get; set; }
        float qY { get; set; }
        float qZ { get; set; }
    }
}
