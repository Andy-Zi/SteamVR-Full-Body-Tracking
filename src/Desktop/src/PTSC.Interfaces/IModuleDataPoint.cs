namespace PTSC.Interfaces
{
    public interface IModuleDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Visibility { get; }
        List<double> GetValues();
        void Update(IEnumerable<double> newData);
    }
}
