using System.Collections.Generic;

namespace Interfaces
{
    public interface IModuleDataModel
    {
        void NormalizeToHead();
        List<double> NOSE { get; set; }
        List<double> LEFT_EYE { get; set; }
        List<double> RIGHT_EYE { get; set; }
        List<double> LEFT_EAR { get; set; }
        List<double> RIGHT_EAR { get; set; }
        List<double> LEFT_SHOULDER { get; set; }
        List<double> RIGHT_SHOULDER { get; set; }
        List<double> LEFT_ELBOW { get; set; }
        List<double> RIGHT_ELBOW { get; set; }
        List<double> LEFT_WRIST { get; set; }
        List<double> RIGHT_WRIST { get; set; }
        List<double> LEFT_HIP { get; set; }
        List<double> RIGHT_HIP { get; set; }
        List<double> LEFT_KNEE { get; set; }
        List<double> RIGHT_KNEE { get; set; }
        List<double> LEFT_ANKLE { get; set; }
        List<double> RIGHT_ANKLE { get; set; }
    }
}
