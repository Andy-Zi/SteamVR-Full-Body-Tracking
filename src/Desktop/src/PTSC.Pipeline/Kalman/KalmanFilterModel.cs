using PTSC.Interfaces;
using PTSC.Nameservice;
using System.Collections.Concurrent;

namespace PTSC.Pipeline.Kalman
{
    public class KalmanFilterModel : IKalmanFilterModel
    {

        protected object lockObject = new object();
        public bool IsInitialized { get; protected set; }

        private ConcurrentDictionary<string, KalmanFilter> Filters = new();

        public KalmanFilterModel()
        {
           
        }

        public void Initialize(double dt = 1.0 / 60, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1)
        {
                IsInitialized = false;
                Filters.Clear();
                foreach (var key in ModulePipeConstants.SkeletonParts)
                {
                    Filters.TryAdd(key, new KalmanFilter(dt, std_X, std_Y, std_Z, std_V));
                }
                IsInitialized = true; 
        }
        public async Task<IModuleData> Update(IModuleData moduledata)
        {
            if (!IsInitialized)
                return moduledata;


            foreach(var (key,value) in moduledata)
            {
                if(Filters.TryGetValue(key, out var filter))
                {
                    await filter.Update(value);
                }
            }
            
            return moduledata;
        }

        public void Initialize(IApplicationSettings settings)
        {
            Initialize(1.0 / settings.KalmanFPS, settings.KalmanXError, settings.KalmanYError, settings.KalmanZError, settings.KalmanVelocityError);
        }
    }
}
