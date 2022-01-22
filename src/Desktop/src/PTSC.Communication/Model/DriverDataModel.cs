using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    [Serializable]
    public class DriverDataModel : IDriverDataModel
    {
        public List<double> head { get; set; }
        public List<double> waist { get; set; }
        public List<double> left_hip { get; set; }
        public List<double> right_hip { get; set; }
        public List<double> left_knee { get; set; }
        public List<double> right_knee { get; set; }
        public List<double> left_foot { get; set; }
        public List<double> right_foot { get; set; }
        public List<double> left_foot_toes { get; set; }
        public List<double> right_foot_toes { get; set; }
        public List<double> head_rotation { get; set; }
        public List<double> waist_rotation { get; set; }
        public List<double> left_foot_rotation { get; set; }
        public List<double> right_foot_rotation { get; set; }
    }
}
