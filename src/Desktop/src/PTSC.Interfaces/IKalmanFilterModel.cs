namespace PTSC.Interfaces
{
    public interface IKalmanFilterModel
    {
        public bool IsInitialized { get; }
        Task<IModuleData> Update(IModuleData data);
        void Initialize(IApplicationSettings settings);
        void Initialize(double dt = 1.0 / 60, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1);
    }
}
