namespace PTSC.Interfaces
{
    public interface IModuleDataPoint
    {
        IModuleDataPoint Clone();
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Visibility { get; }
        List<double> GetValues();
        bool IsVisible(double threshold = 0.2);
    }
}
