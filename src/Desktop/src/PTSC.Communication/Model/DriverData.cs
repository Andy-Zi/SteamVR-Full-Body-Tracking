using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    [Serializable]
    public class DriverData : IDriverData
    {
        public IDriverDataPoint waist { get; set; }
        public IDriverDataPoint left_foot { get; set; }
        public IDriverDataPoint right_foot { get; set; }
    }
}
