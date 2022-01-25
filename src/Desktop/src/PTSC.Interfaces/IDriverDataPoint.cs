namespace PTSC.Interfaces
{
    public interface IDriverDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double rotationW { get; set; }
        public double rotationX { get; set; }
        public double rotationY { get; set; }
        public double rotationZ { get; set; }
    }
}
