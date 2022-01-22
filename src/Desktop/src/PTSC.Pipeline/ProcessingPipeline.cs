using OpenCvSharp;
using Prism.Events;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using PTSC.Nameservice;
using PTSC.OpenCV;
using PTSC.Pipeline.Kalman;
using PTSC.PubSub;
using System.Diagnostics;
using System.Drawing;
using Unity;

namespace PTSC.Pipeline
{
    /// <summary>
    /// Definition of the Processing Steps performed on the Data and Image
    /// </summary>
    public class ProcessingPipeline
    {
        public bool UseKalmanFilter;
        public double RotationOffset = 0;
        public double ScalingOffset = 1;
        public List<double> zeroList = new List<double>() { 0.0, 0.0, 0.0 };
        [Dependency] public IEventAggregator EventAggregator { get; set; }
        [Dependency] public IKalmanFilterModel KalmanFilterModel { get; set; }
        public ProcessingPipeline(IApplicationEnvironment applicationEnvironment)
        {
            var settings = applicationEnvironment.Settings;
            RotationOffset = settings.Rotation;
            ScalingOffset = settings.Scaling;
        }

        protected SubscriptionToken SubscriptionToken;
        protected ImageProcessedEvent ImageProcessedEvent;
        protected DataProcessedEvent DataProcessedEvent;
        protected ModuleDataProcessedEvent ModuleDataProcessedEvent;
        protected PipelineLatencyEvent PipelineLatencyEvent;

        public void Start()
        {
            SubscriptionToken = EventAggregator.GetEvent<DataRecievedEvent>().Subscribe(async (payload) =>  await Process(payload));
            ImageProcessedEvent = EventAggregator.GetEvent<ImageProcessedEvent>();
            DataProcessedEvent = EventAggregator.GetEvent<DataProcessedEvent>();
            ModuleDataProcessedEvent = EventAggregator.GetEvent<ModuleDataProcessedEvent>();
            PipelineLatencyEvent = EventAggregator.GetEvent<PipelineLatencyEvent>();
        }

        public void Stop()
        {
            SubscriptionToken.Dispose();
        }

        async Task Process(DataRecievedPayload payload)
        {
            var watch = Stopwatch.StartNew();
            Bitmap processedImage = null;
            if(payload.Image != null)
            {
                processedImage = await ProcessImage(payload);
            }

            var processedData = await ProcessData(payload);

            await Task.Run(() => ImageProcessedEvent.Publish(new ImageProcessedPayload(processedImage)));
            await Task.Run(() => DataProcessedEvent.Publish(new DataProcessedPayload(processedData)));
            watch.Stop();
            await Task.Run(() => PipelineLatencyEvent.Publish(new LatencyPayload(watch.ElapsedMilliseconds)));
        }

        async Task<Bitmap> ProcessImage(DataRecievedPayload payload)
        {
            //TODO Paint skeleton on Image + Flip and Resize Image
            return await Task.Run(() => {
                try
                {
                    var moduledata = payload.ModuleData;
                    var img = payload.Image;
                    var bitmap = OpenCVService.Mat2Bitmap(img);
                    img.Dispose();
                    return bitmap;
                }
                catch
                {
                    return default;
                }

                }
            );
        }

        async Task<IDriverDataModel> ProcessData(DataRecievedPayload payload)
        {
            return await Task.Run(async () =>
            {
                var moduledata = payload.ModuleData;
                //moduledata = correctYAxis(moduledata);
                moduledata = await FilterData(moduledata);

                await Task.Run(() => ModuleDataProcessedEvent.Publish(new(moduledata)));

                var driverdata = MapData(moduledata);
                // calculate destination for waist and feet
                driverdata = CalculateRotation(driverdata);
                // scale coordinate before serializing
                driverdata = ScaleData(driverdata);
                // rotate coordinate before serializing
                driverdata = RotateData(driverdata);
                return driverdata;
            });
        }

        private IDriverDataModel MapData(IModuleData moduledata)
        {
            DriverDataModel driverData = new DriverDataModel();
            // set nose as head for driver
            driverData.head = moduledata["NOSE"].GetValues() ?? zeroList;
            // set hips for dirver data
            driverData.left_hip = moduledata["LEFT_HIP"].GetValues() ?? zeroList;
            driverData.right_hip = moduledata["RIGHT_HIP"].GetValues() ?? zeroList;
            // set center point of hips as the waist point for driver
            driverData.waist = CalculateCenter3D(moduledata["LEFT_HIP"].GetValues() ?? zeroList, moduledata["RIGHT_HIP"].GetValues() ?? zeroList);
            // set knees for dirver data
            driverData.left_knee = moduledata["LEFT_KNEE"].GetValues() ?? zeroList;
            driverData.right_knee = moduledata["RIGHT_KNEE"].GetValues() ?? zeroList;
            // set ancles as foots for driver
            driverData.left_foot = moduledata["LEFT_ANKLE"].GetValues() ?? zeroList;
            driverData.right_foot = moduledata["RIGHT_ANKLE"].GetValues() ?? zeroList;
            // set toes for dirver data
            driverData.left_foot_toes = moduledata["LEFT_TOES"].GetValues() ?? zeroList;
            driverData.right_foot_toes = moduledata["RIGHT_TOES"].GetValues() ?? zeroList;
            return driverData;
        }

        private async Task<IModuleData> FilterData(IModuleData moduledata)
        {
            if(UseKalmanFilter)
                return await KalmanFilterModel.Update(moduledata);
            return moduledata;
        }

        private static List<double> CalculateCenter3D(List<double> point1, List<double> point2)
        {
            if(point1.Count < 3|| point2.Count < 3)
                return new List<double>() { 0.0,0.0,0.0};
            var x_new = (point1[0] + point2[0]) / 2;
            var y_new = (point1[1] + point2[1]) / 2;
            var z_new = (point1[2] + point2[2]) / 2;
            return new List<double> { x_new, y_new, z_new };
        }

        private IDriverDataModel ScaleData(IDriverDataModel driverData)
        {
            driverData.waist = ScaleProperty(driverData.waist);
            driverData.left_foot = ScaleProperty(driverData.left_foot);
            driverData.right_foot = ScaleProperty(driverData.right_foot);
            return driverData;
        }

        private List<double> ScaleProperty(List<double> coordinate)
        {
            double newX = coordinate[0] * ScalingOffset;
            double newY = coordinate[1] * ScalingOffset;
            double newZ = coordinate[2] * ScalingOffset;
            return new List<double> { newX, newY, newZ };
        }

        private IDriverDataModel RotateData(IDriverDataModel driverData)
        {
            double rotation = ToRadians(RotationOffset);
            driverData.waist = RotateProperty(driverData.waist, rotation);
            driverData.left_foot = RotateProperty(driverData.left_foot, rotation);
            driverData.right_foot = RotateProperty(driverData.right_foot, rotation);
            return driverData;
        }

        private List<double> RotateProperty(List<double> coodinate, double rotation)
        {
            double x = coodinate[0];
            double y = coodinate[2];
            double newX = Math.Cos(rotation) * x - Math.Sin(rotation) * y;
            double newY = Math.Sin(rotation) * x + Math.Cos(rotation) * y;
            double newZ = coodinate[1];
            return new List<double> { newX, newY, newZ };
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private IDriverDataModel CalculateRotation(IDriverDataModel driverData)
        {
            driverData.head_rotation = new List<double>() { 0.0, 0.0, 0.0, 0.0 };
            // calculate waist's direction as a normal vector of the vector from right to left hip
            driverData.waist_rotation = CalculateWaistRotation(driverData.right_hip, driverData.left_hip);
            // set feet directions as the knee's direction from the hip
            driverData.left_foot_rotation = CalculateFootRotation(driverData.left_foot, driverData.left_foot_toes);
            driverData.right_foot_rotation = CalculateFootRotation(driverData.right_foot, driverData.right_foot_toes);
            return driverData;
        }

        private List<double> CalculateFootRotation(List<double> ankle, List<double> toes)
        {

            double foot_vector_x = toes[0] - ankle[0];
            double foot_vector_y = toes[1] - ankle[1];
            double foot_vector_z = toes[2] - ankle[2];
            double foot_vector_w = 0;
            List<double> normalized_foot_vector = NormalizeVector(foot_vector_x, foot_vector_y, foot_vector_z);
            return new List<double> { foot_vector_w, normalized_foot_vector[0], normalized_foot_vector[1], normalized_foot_vector[2] };
        }

        private List<double> CalculateWaistRotation(List<double> right_hip, List<double> left_hip)
        {
            // calculate vector (x, y, z) -> normal vector (z, y, -x)
            double hip_normal_vector_x = left_hip[2] - right_hip[2]; // x value of the normal vector is the z value of the hip vector
            double hip_normal_vector_y = 0; // set y value to 0 to get a horizontal vector
            double hip_normal_vector_z = -(left_hip[0] - right_hip[0]); // z value of the normal vector is the negative x value of the hip vector
            double hip_normal_vector_w = 0;
            List<double> normalized_hip_vector = NormalizeVector(hip_normal_vector_x, hip_normal_vector_y, hip_normal_vector_z);
            return new List<double> { hip_normal_vector_w, normalized_hip_vector[0], normalized_hip_vector[1], normalized_hip_vector[2] };
        }

        private List<double> NormalizeVector(double x, double y, double z)
        {
            double length = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            return new List<double> { x / length, y / length, z / length };
        }
    }
}