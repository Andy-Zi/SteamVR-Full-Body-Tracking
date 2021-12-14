namespace PTSC.Interfaces
{
    public interface IDriverDataModel
    {
        List<double> LEFT_HIP { get; set; }
        List<double> RIGHT_HIP { get; set; }
        List<double> LEFT_ANKLE { get; set; }
        List<double> RIGHT_ANKLE { get; set; }
        List<double> LEFT_WRIST { get; set; }
        List<double> RIGHT_WRIST { get; set; }
        List<double> NOSE { get; set; }
        List<double> LEFT_FOOT_INDEX { get; set; }
        List<double> RIGHT_FOOT_INDEX { get; set; }
    }
}
