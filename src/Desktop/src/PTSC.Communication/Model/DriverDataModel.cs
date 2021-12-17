using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    [Serializable]
    public class DriverDataModel : IDriverDataModel
    {
        public List<double> head { get; set; }
        public List<double> waist { get; set; }
        public List<double> left_foot { get; set; }
        public List<double> right_foot { get; set; }
    }
}
