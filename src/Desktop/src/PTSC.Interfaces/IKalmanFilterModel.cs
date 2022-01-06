namespace PTSC.Interfaces
{
    public interface IKalmanFilterModel
    {
        public bool IsInitialized { get; }
        IModuleDataModel Update(IModuleDataModel moduledata);

        void Initialize(IApplicationSettings settings);
        void Initialize(double dt = 1.0 / 60, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1);
    }
}
