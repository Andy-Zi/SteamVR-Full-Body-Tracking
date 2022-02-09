namespace PTSC.Interfaces
{
    public interface IApplicationSettings : IModel
    {

        bool LogPositionData { get; set; }
        double Scaling { get; set; }
        double Rotation { get; set; }
        bool UseKalmanFilter { get; set; }
        int KalmanFPS { get; set; }
        double KalmanXError { get; set; }
        double KalmanYError { get; set; }
        double KalmanZError { get; set; }
        double KalmanVelocityError { get; set; }
        bool UseHipAsFootRotation { get; set; }
        int FPSLimit { get; set; }
        bool SupportModuleImage { get; set; }

        bool RotationSmoothing { get; set; }
    }
}
