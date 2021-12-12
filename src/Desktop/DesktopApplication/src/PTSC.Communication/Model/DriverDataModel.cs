using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    [Serializable]
    public class DriverDataModel : IDriverDataModel
    {
        public List<double> LEFT_HIP { get; set; }
        public List<double> RIGHT_HIP { get; set; }
        public List<double> LEFT_ANKLE { get; set; }
        public List<double> RIGHT_ANKLE { get; set; }
        public List<double> LEFT_WRIST { get; set; }
        public List<double> RIGHT_WRIST { get; set; }
        public List<double> NOSE { get; set; }
        public List<double> LEFT_FOOT_INDEX { get; set; }
        public List<double> RIGHT_FOOT_INDEX { get; set; }
    }
}
