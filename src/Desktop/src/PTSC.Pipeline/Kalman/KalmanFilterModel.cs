using PTSC.Interfaces;

namespace PTSC.Pipeline.Kalman
{
    public class KalmanFilterModel : IKalmanFilterModel
    {

        protected object lockObject = new object();
        public bool IsInitialized { get; protected set; }

        KalmanFilter NOSE;
        KalmanFilter LEFT_EYE;
        KalmanFilter RIGHT_EYE;
        KalmanFilter LEFT_EAR;
        KalmanFilter RIGHT_EAR;
        KalmanFilter LEFT_SHOULDER;
        KalmanFilter RIGHT_SHOULDER;
        KalmanFilter LEFT_ELBOW;
        KalmanFilter RIGHT_ELBOW;
        KalmanFilter LEFT_WRIST;
        KalmanFilter RIGHT_WRIST;
        KalmanFilter LEFT_HIP;
        KalmanFilter RIGHT_HIP;
        KalmanFilter LEFT_KNEE;
        KalmanFilter RIGHT_KNEE;
        KalmanFilter LEFT_ANKLE;
        KalmanFilter RIGHT_ANKLE;
        public KalmanFilterModel()
        {
           
        }

        public void Initialize(double dt = 1.0 / 60, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1)
        {
            lock (lockObject)
            {
                IsInitialized = false;
                NOSE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_EYE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_EYE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_EAR = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_EAR = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_SHOULDER = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_SHOULDER = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_ELBOW = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_ELBOW = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_WRIST = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_WRIST = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_HIP = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_HIP = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_KNEE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_KNEE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                LEFT_ANKLE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                RIGHT_ANKLE = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
                IsInitialized = true;
            }
            
        }
        public IModuleDataModel Update(IModuleDataModel moduledata)
        {
            if (!IsInitialized)
                return moduledata;

            lock (lockObject)
            {
                moduledata.NOSE = NOSE.Update(moduledata.NOSE);
                moduledata.LEFT_EYE = LEFT_EYE.Update(moduledata.LEFT_EYE);
                moduledata.RIGHT_EYE = RIGHT_EYE.Update(moduledata.RIGHT_EYE);
                moduledata.LEFT_EAR = LEFT_EAR.Update(moduledata.LEFT_EAR);
                moduledata.RIGHT_EAR = RIGHT_EAR.Update(moduledata.RIGHT_EAR);
                moduledata.LEFT_SHOULDER = LEFT_SHOULDER.Update(moduledata.LEFT_SHOULDER);
                moduledata.RIGHT_SHOULDER = RIGHT_SHOULDER.Update(moduledata.RIGHT_SHOULDER);
                moduledata.LEFT_ELBOW = LEFT_ELBOW.Update(moduledata.LEFT_ELBOW);
                moduledata.RIGHT_ELBOW = RIGHT_ELBOW.Update(moduledata.RIGHT_ELBOW);
                moduledata.LEFT_WRIST = LEFT_WRIST.Update(moduledata.LEFT_WRIST);
                moduledata.RIGHT_WRIST = RIGHT_WRIST.Update(moduledata.RIGHT_WRIST);
                moduledata.LEFT_HIP = LEFT_HIP.Update(moduledata.LEFT_HIP);
                moduledata.RIGHT_HIP = RIGHT_HIP.Update(moduledata.RIGHT_HIP);
                moduledata.LEFT_KNEE = LEFT_KNEE.Update(moduledata.LEFT_KNEE);
                moduledata.RIGHT_KNEE = RIGHT_KNEE.Update(moduledata.RIGHT_KNEE);
                moduledata.LEFT_ANKLE = LEFT_ANKLE.Update(moduledata.LEFT_ANKLE);
                moduledata.RIGHT_ANKLE = RIGHT_ANKLE.Update(moduledata.RIGHT_ANKLE);
            }
            return moduledata;
        }

        public void Initialize(IApplicationSettings settings)
        {
            Initialize(1.0 / settings.KalmanFPS, settings.KalmanXError, settings.KalmanYError, settings.KalmanZError, settings.KalmanVelocityError);
        }
    }
}
