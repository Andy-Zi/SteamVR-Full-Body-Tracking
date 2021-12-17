namespace PTSC.Interfaces
{
    public interface IModuleDataModel
    {
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
        List<double> LEFT_HEEL { get; set; }
        List<double> RIGHT_HEEL { get; set; }
        List<double> LEFT_FOOT_INDEX { get; set; }
        List<double> RIGHT_FOOT_INDEX { get; set; }
        List<double> NOSE { get; set; }
    }
}
