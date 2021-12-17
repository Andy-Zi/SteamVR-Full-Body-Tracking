namespace PTSC.Interfaces
{
    public interface IDriverDataModel
    {
        List<double> head { get; set; }
        List<double> waist { get; set; }
        List<double> left_foot { get; set; }
        List<double> right_foot { get; set; }
    }
}
