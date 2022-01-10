
using NumSharp;
using PTSC.Interfaces;

namespace PTSC.Pipeline.Kalman
{

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
            F = np.array(new double[,] {
                                        { 1, dt, 0, 0,  0,  0 },             // x
                                        { 0,  1, 0, 0,  0,  0 },             // x'
                                        { 0,  0, 1, dt, 0,  0 },             // y
                                        { 0,  0, 0,  1, 0,  0 },             // y'
                                        { 0,  0, 0,  0, 1, dt },             // z
                                        { 0,  0, 0,  0, 0,  1 } });         // z'



            // Process Noise Matrix
            Q = np.array(new double[,] {
                                        { Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2, 0, 0, 0, 0 },
                                        { Math.Pow(dt, 3)/2, Math.Pow(dt, 2),   0, 0, 0, 0 },
                                        { 0, 0, Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2, 0, 0 },
                                        { 0, 0, Math.Pow(dt, 3)/2, Math.Pow(dt, 2),   0, 0 },
                                        { 0, 0, 0, 0, Math.Pow(dt, 4)/4, Math.Pow(dt, 3)/2 },
                                        { 0, 0, 0, 0, Math.Pow(dt, 3)/2, Math.Pow(dt, 2)   } });

            Q = Q * Math.Pow(this.std_V, 2);

            // Measurement Covariance Matrix
            R = np.array(new double[,] {
                                        { Math.Pow(this.std_X, 2), 0, 0 },
                                        { 0, Math.Pow(this.std_Y, 2), 0 },
                                        { 0, 0, Math.Pow(this.std_Z, 2) } });

            // Start Initial Position (Unknown at start)
            x = np.array(new double[] { 0, 0, 0, 0, 0, 0 }).reshape(new int[] { 6, 1 });

            // estimate uncertainty (covariance) matrix of the current state (predicted at the previous state)
            P = np.array(new double[,] {
                                        { 500, 0, 0, 0, 0, 0 },
                                        { 0, 500 ,0, 0, 0, 0 },
                                        { 0, 0, 500, 0, 0, 0 },
                                        { 0, 0, 0, 500, 0, 0 },
                                        { 0, 0, 0, 0, 500, 0 },
                                        { 0, 0, 0, 0, 0, 500 }, });

            H = np.array(new double[,] {
                                        { 1, 0, 0, 0, 0, 0 },
                                        { 0, 0, 1, 0, 0, 0 },
                                        { 0, 0, 0, 0, 1, 0 } });

            predict();

        }

        protected void predict()
        {
            P = np.dot(F, np.dot(P, F.T)) + Q;
        }

        public async Task Update(IModuleDataPoint dataPoint)
        {
            if (dataPoint == null)
                return;

            await Task.Run(() =>
            {
                NDArray measureND = np.array(new double[] { dataPoint.X, dataPoint.Y, dataPoint.Z });
                measureND = measureND.reshape(new int[] { 3, 1 });

                // calculate Kalman Gain

                var inverse = np.dot(H, np.dot(P, H.T)) + R;          // np.linalg.inv buggy = return null ; known problem
                inverse = np.eye(inverse.shape[1]) / inverse;

                var iter = inverse.AsIterator<double>();
                var iter2 = inverse.AsIterator<double>();
                while (iter.HasNext())
                {
                    var val = iter.MoveNext();
                    if (val.Equals(np.nan))
                    {
                        iter2.MoveNextReference() = 0;
                    }
                    else
                    {
                        iter2.MoveNext();
                    }
                }
                K = np.dot(P, np.dot(H.T, inverse));

                // estimate new Value

                x = x + np.matmul(K, measureND - np.dot(H, x));

                // I identity matrix
                NDArray I = np.eye(H.shape[1]);

                P = np.dot(I - np.dot(K, H), np.dot(P, (I - np.dot(K, H)).T + np.dot(K, np.dot(R, K.T))));

                predict();

                dataPoint.X = x[0][0];
                dataPoint.Y = x[2][0];
                dataPoint.Z = x[4][0];
            });
        }
    }
}
