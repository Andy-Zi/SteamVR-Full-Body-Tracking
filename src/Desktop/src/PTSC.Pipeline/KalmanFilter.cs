
using NumSharp;
using PTSC.Interfaces;

namespace PTSC.Pipeline
{
    public class KalmanFilterModel
    {
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
        public KalmanFilterModel(double dt = 1.0/60, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1)
        {
            this.NOSE           = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_EYE       = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_EYE      = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_EAR       = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_EAR      = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_SHOULDER  = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_SHOULDER = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_ELBOW     = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_ELBOW    = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_WRIST     = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_WRIST    = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_HIP       = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_HIP      = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_KNEE      = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_KNEE     = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.LEFT_ANKLE     = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
            this.RIGHT_ANKLE    = new KalmanFilter(dt, std_X, std_Y, std_Z, std_V);
        }
        public IModuleDataModel update(IModuleDataModel moduledata)
        {
            moduledata.NOSE             = this.NOSE.update(moduledata.NOSE);
            moduledata.LEFT_EYE         = this.LEFT_EYE.update(moduledata.LEFT_EYE);
            moduledata.RIGHT_EYE        = this.RIGHT_EYE.update(moduledata.RIGHT_EYE);
            moduledata.LEFT_EAR         = this.LEFT_EAR.update(moduledata.LEFT_EAR);
            //moduledata.NOSE = moduledata.NOSE;
            //moduledata.LEFT_EYE = moduledata.LEFT_EYE;
            //moduledata.RIGHT_EYE = moduledata.RIGHT_EYE;
            //moduledata.LEFT_EAR = moduledata.LEFT_EAR;
            
            moduledata.RIGHT_EAR        = this.RIGHT_EAR.update(moduledata.RIGHT_EAR);
            moduledata.LEFT_SHOULDER    = this.LEFT_SHOULDER.update(moduledata.LEFT_SHOULDER);
            moduledata.RIGHT_SHOULDER   = this.RIGHT_SHOULDER.update(moduledata.RIGHT_SHOULDER);
            moduledata.LEFT_ELBOW       = this.LEFT_ELBOW.update(moduledata.LEFT_ELBOW);
            moduledata.RIGHT_ELBOW      = this.RIGHT_ELBOW.update(moduledata.RIGHT_ELBOW);
            moduledata.LEFT_WRIST       = this.LEFT_WRIST.update(moduledata.LEFT_WRIST);
            moduledata.RIGHT_WRIST      = this.RIGHT_WRIST.update(moduledata.RIGHT_WRIST);
            moduledata.LEFT_HIP         = this.LEFT_HIP.update(moduledata.LEFT_HIP);
            moduledata.RIGHT_HIP        = this.RIGHT_HIP.update(moduledata.RIGHT_HIP);
            moduledata.LEFT_KNEE        = this.LEFT_KNEE.update(moduledata.LEFT_KNEE);
            moduledata.RIGHT_KNEE       = this.RIGHT_KNEE.update(moduledata.RIGHT_KNEE);
            moduledata.LEFT_ANKLE       = this.LEFT_ANKLE.update(moduledata.LEFT_ANKLE);
            moduledata.RIGHT_ANKLE      = this.RIGHT_ANKLE.update(moduledata.RIGHT_ANKLE);
            //moduledata.RIGHT_HIP = moduledata.RIGHT_HIP;
            //moduledata.RIGHT_EAR = moduledata.RIGHT_EAR;
            //moduledata.LEFT_SHOULDER = moduledata.LEFT_SHOULDER;
            //moduledata.LEFT_ELBOW = moduledata.LEFT_ELBOW;
            //moduledata.RIGHT_ELBOW = moduledata.RIGHT_ELBOW;
            //moduledata.LEFT_WRIST = moduledata.LEFT_WRIST;
            //moduledata.LEFT_KNEE = moduledata.LEFT_KNEE;
            //moduledata.RIGHT_KNEE = moduledata.RIGHT_KNEE;
            //moduledata.LEFT_ANKLE = moduledata.LEFT_ANKLE;
            //moduledata.RIGHT_ANKLE = moduledata.RIGHT_ANKLE;


            return moduledata;
        }
    }

    internal class KalmanFilter
    {
        protected double dt;                    // delta t (Zeit zwischen den Messungen (FPS)
        protected double std_X;                 // Messfehler X - Achse
        protected double std_Y;                 // Messfehler Y - Achse
        protected double std_Z;                 // Messfehler Z - Achse
        protected double std_V;                 // Standardabweichung Geschwindigkeit (Rauschen / Messfehler)

        protected NDArray F;                    // state Transition Matrix
        protected NDArray Q;                    // process Noise Matrix
        protected NDArray R;                    // Measurement Covariance Matrix
        protected NDArray x;                    // Start Initial Position (unkown at start)
        protected NDArray P;                    // estimate unvertainty (covariance) matrix of the current state (predicted at the previous state)
        protected NDArray H;                    // Measurement mapping (Observation Matrix)
        protected NDArray K;                    // Kalman Gain

        public KalmanFilter(double dt, double std_X = 0.005, double std_Y = 0.005, double std_Z = 0.005, double std_V = 1)
        {
            this.dt = dt;
            this.std_X = std_X;  
            this.std_Y = std_Y;
            this.std_Z = std_Z;
            this.std_V = std_V;

            // State Transition Matrix
            //                           x,  x', y, y', z, z'
            this.F = np.array(new double[,] {    
                                        { 1, dt, 0, 0,  0,  0 },             // x
                                        { 0,  1, 0, 0,  0,  0 },             // x'
                                        { 0,  0, 1, dt, 0,  0 },             // y
                                        { 0,  0, 0,  1, 0,  0 },             // y'
                                        { 0,  0, 0,  0, 1, dt },             // z
                                        { 0,  0, 0,  0, 0,  1 } } );         // z'

          

            // Process Noise Matrix
            this.Q = np.array( new double[,] {    
                                        { Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2, 0, 0, 0, 0 },
                                        { Math.Pow(dt, 3)/2, Math.Pow(dt, 2),   0, 0, 0, 0 },
                                        { 0, 0, Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2, 0, 0 },
                                        { 0, 0, Math.Pow(dt, 3)/2, Math.Pow(dt, 2),   0, 0 },
                                        { 0, 0, 0, 0, Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2 },
                                        { 0, 0, 0, 0, Math.Pow(dt, 3)/2, Math.Pow(dt, 2)   } } );

            this.Q = this.Q * Math.Pow(this.std_V, 2);

            // Measurement Covariance Matrix
            this.R = np.array( new double[,] {
                                        { Math.Pow(this.std_X, 2), 0, 0 },
                                        { 0, Math.Pow(this.std_Y, 2), 0 },
                                        { 0, 0, Math.Pow(this.std_Z, 2) } });

            // Start Initial Position (Unknown at start)
            this.x = np.array( new double[] { 0, 0, 0, 0, 0, 0 } ).reshape(new int[] { 6,1 });

            // estimate uncertainty (covariance) matrix of the current state (predicted at the previous state)
            this.P = np.array( new double[,] {
                                        { 500, 0, 0, 0, 0, 0 },
                                        { 0, 500 ,0, 0, 0, 0 },
                                        { 0, 0, 500, 0, 0, 0 },
                                        { 0, 0, 0, 500, 0, 0 },
                                        { 0, 0, 0, 0, 500, 0 },
                                        { 0, 0, 0, 0, 0, 500 }, } );

            this.H = np.array( new double[,] {
                                        { 1, 0, 0, 0, 0, 0 },
                                        { 0, 0, 1, 0, 0, 0 },
                                        { 0, 0, 0, 0, 1, 0 } });

            this.predict();

        }

        protected void predict()
        {
            this.P = np.dot(this.F, np.dot(this.P, this.F.T)) + this.Q;
        }

        public List<double> update(List<double> measure)
        {
            NDArray measureND = np.array(measure);
            NDArray temp_measureND = np.copy(measureND);
            measureND = measureND["0:3"];
            measureND = measureND.reshape(new int[]{ 3,1 });

            // calculate Kalman Gain
            var tttt = np.dot(this.P, this.H.T);
            
            var inverse = (np.dot(this.H, np.dot(this.P, this.H.T)) + this.R);          // np.linalg.inv buggy = return null ; known problem
            inverse = np.eye(inverse.shape[1]) / inverse;

            var iter = inverse.AsIterator<double>();
            var iter2 = inverse.AsIterator<double>();
            while(iter.HasNext())
            {
                var val = iter.MoveNext();
                if (val.Equals(np.nan)) 
                {
                    iter2.MoveNextReference() = 0;
                } else
                {
                    iter2.MoveNext();
                }
            }
            this.K = np.dot(this.P, np.dot(this.H.T, inverse));

            // estimate new Value
            /*NDArray temp;
            try
            {
                temp = np.matmul(this.H, this.x);
            } catch (System.NotSupportedException)
            {
                temp = np.array(new double[] { 0, 0, 0 }).T;
            }*/

            this.x = this.x + np.matmul(this.K, measureND - np.dot(this.H, this.x));

            // I identity matrix
            NDArray I = np.eye(this.H.shape[1]);

            var xx = I - np.dot(this.K, this.H);
            var xxx = np.transpose(xx);
            var tt = np.dot(this.K, np.dot(this.R, this.K.T));
            var ttt = np.dot(xx, np.dot(this.P, xxx));

            this.P = np.dot(I - np.dot(this.K, this.H), np.dot(this.P, (I - np.dot(this.K, this.H)).T + np.dot(this.K, np.dot(this.R, this.K.T))));

            this.predict();


            return new List<double> () { this.x[0][0], this.x[2][0], this.x[4][0], temp_measureND[3] } ;
        }
    }
}
