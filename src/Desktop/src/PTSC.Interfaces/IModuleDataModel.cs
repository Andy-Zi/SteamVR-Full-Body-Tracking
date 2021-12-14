namespace PTSC.Interfaces
{
    public interface IModuleDataModel
    {
        List<double> left_shoulder { get; set; }
        List<double> right_shoulder { get; set; }
        List<double> left_elbow { get; set; }
        List<double> right_elbow { get; set; }
        List<double> left_wrist { get; set; }
        List<double> right_wrist { get; set; }
        List<double> left_hip { get; set; }
        List<double> right_hip { get; set; }
        List<double> left_knee { get; set; }
        List<double> right_knee { get; set; }
        List<double> left_ankle { get; set; }
        List<double> right_ankle { get; set; }
        List<double> left_heel { get; set; }
        List<double> right_heel { get; set; }
        List<double> left_foot_index { get; set; }
        List<double> right_foot_index { get; set; }
    }
}
