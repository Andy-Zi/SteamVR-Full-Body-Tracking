using PTSC.Interfaces;
using System.Reflection;

namespace PTSC.Communication.Model
{

    public class ModuleDataModel
    {
        public List<double> NOSE { get; set; }
        public List<double> LEFT_EYE { get; set; }
        public List<double> RIGHT_EYE { get; set; }
        public List<double> LEFT_EAR { get; set; }
        public List<double> RIGHT_EAR { get; set; }
        public List<double> LEFT_SHOULDER { get; set; }
        public List<double> RIGHT_SHOULDER { get; set; }
        public List<double> LEFT_ELBOW { get; set; }
        public List<double> RIGHT_ELBOW { get; set; }
        public List<double> LEFT_WRIST { get; set; }
        public List<double> RIGHT_WRIST { get; set; }
        public List<double> LEFT_HIP { get; set; }
        public List<double> RIGHT_HIP { get; set; }
        public List<double> LEFT_KNEE { get; set; }
        public List<double> RIGHT_KNEE { get; set; }
        public List<double> LEFT_ANKLE { get; set; }
        public List<double> RIGHT_ANKLE { get; set; }
    }
}
