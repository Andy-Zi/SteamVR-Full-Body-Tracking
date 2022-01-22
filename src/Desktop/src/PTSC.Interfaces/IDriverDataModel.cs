namespace PTSC.Interfaces
{
    public interface IDriverDataModel
    {
        List<double> head { get; set; }
        List<double> waist { get; set; }
        List<double> left_hip { get; set; }
        List<double> right_hip { get; set; }
        List<double> left_knee { get; set; }
        List<double> right_knee { get; set; }
        List<double> left_foot { get; set; }
        List<double> right_foot { get; set; }
        List<double> left_foot_toes { get; set; }
        List<double> right_foot_toes { get; set; }
        List<double> head_rotation { get; set; }
        List<double> waist_rotation { get; set; }
        List<double> left_foot_rotation { get; set; }
        List<double> right_foot_rotation { get; set; }

    }
}
