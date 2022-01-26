using System.Diagnostics;
using System.Drawing;
using Prism.Events;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using PTSC.OpenCV;
using PTSC.PubSub;
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

        async Task<IDriverData> ProcessData(DataRecievedPayload payload)
        {
            return await Task.Run(async () =>
            {
                var moduledata = payload.ModuleData;
                //moduledata = correctYAxis(moduledata);
                moduledata = await FilterData(moduledata);

                await Task.Run(() => ModuleDataProcessedEvent.Publish(new(moduledata)));

                var driverdata = MapData(moduledata);
                // calculate destination for waist and feet
                CalculateRotation(driverdata);
                // scale coordinate before serializing
                ScaleData(driverdata);
                // rotate coordinate before serializing
                RotateData(driverdata);
                return driverdata;
            });
        }

        private IDriverData MapData(IModuleData moduledata)
        {
            DriverDataModel driverDataModel = new DriverDataModel();
            // set nose as head for driver
            driverDataModel.head = moduledata["NOSE"].GetValues() ?? zeroList;
            // set hips for dirver data
            driverDataModel.left_hip = moduledata["LEFT_HIP"].GetValues() ?? zeroList;
            driverDataModel.right_hip = moduledata["RIGHT_HIP"].GetValues() ?? zeroList;
            // set center point of hips as the waist point for driver
            driverDataModel.waist = CalculateHipCenter3D(driverDataModel.right_hip, driverDataModel.left_hip);
            // set knees for dirver data
            driverDataModel.left_knee = moduledata["LEFT_KNEE"].GetValues() ?? zeroList;
            driverDataModel.right_knee = moduledata["RIGHT_KNEE"].GetValues() ?? zeroList;
            // set ancles as foots for driver
            driverDataModel.left_foot = moduledata["LEFT_ANKLE"].GetValues() ?? zeroList;
            driverDataModel.right_foot = moduledata["RIGHT_ANKLE"].GetValues() ?? zeroList;
            // set toes for dirver data
            driverDataModel.left_foot_toes = moduledata["LEFT_FOOT_INDEX"].GetValues() ?? zeroList;
            driverDataModel.right_foot_toes = moduledata["RIGHT_FOOT_INDEX"].GetValues() ?? zeroList;
            return new DriverData(driverDataModel);
        }

        private async Task<IModuleData> FilterData(IModuleData moduledata)
        {
            if(UseKalmanFilter)
                return await KalmanFilterModel.Update(moduledata);
            return moduledata;
        }

        private static List<double> CalculateHipCenter3D(List<double> rightHip, List<double> leftHip)
        {
            if(leftHip.Count < 3 || rightHip.Count < 3)
                return new List<double> { 0, 0, 0 };

            double waistX = (leftHip[0] + rightHip[0]) / 2;
            double waistY = (leftHip[1] + rightHip[1]) / 2;
            double waistZ = (leftHip[2] + rightHip[2]) / 2;
            return new List<double> { waistX, waistY, waistZ };
        }

        private void ScaleData(IDriverData driverData)
        {
            ScaleProperty(driverData["waist"]);
            ScaleProperty(driverData["left_foot"]);
            ScaleProperty(driverData["right_foot"]);
        }

        private void ScaleProperty(IDriverDataPoint dataPoint)
        {
            dataPoint.X = dataPoint.X * ScalingOffset;
            dataPoint.Y = dataPoint.Y * ScalingOffset;
            dataPoint.Z = dataPoint.Z * ScalingOffset;
        }

        private void RotateData(IDriverData driverData)
        {
            double rotation = ToRadians(RotationOffset);
            RotateProperty(driverData["waist"], rotation);
            RotateProperty(driverData["left_foot"], rotation);
            RotateProperty(driverData["right_foot"], rotation);
        }

        private void RotateProperty(IDriverDataPoint dataPoint, double rotation)
        {
            dataPoint.X = Math.Cos(rotation) * dataPoint.X - Math.Sin(rotation) * dataPoint.Z;
            dataPoint.Y = Math.Sin(rotation) * dataPoint.X + Math.Cos(rotation) * dataPoint.Z;
            dataPoint.Z = dataPoint.Y;
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private void CalculateRotation(IDriverData driverData)
        {
            SetHeadRotation(driverData["head_rotation"]);
            // calculate waist's direction as a normal vector of the vector from right to left hip
            CalculateWaistRotation(driverData["waist_rotation"], driverData["right_hip"], driverData["left_hip"]);
            // set feet directions as the knee's direction from the hip
            CalculateFootRotation(driverData["left_foot_rotation"], driverData["left_foot"], driverData["left_foot_toes"]);
            CalculateFootRotation(driverData["right_foot_rotation"], driverData["right_foot"], driverData["right_foot_toes"]);
        }

        private void SetHeadRotation(IDriverDataPoint headRotation)
        {
            headRotation.rotationW = 0;
            headRotation.rotationX = 0;
            headRotation.rotationY = 0;
            headRotation.rotationZ = 0;
        }

        private void CalculateFootRotation(IDriverDataPoint footRotation, IDriverDataPoint ankle, IDriverDataPoint toes)
        {
            footRotation.rotationW = 0;
            // calculate the foot direction as the vector from ankle to toes
            footRotation.rotationX = toes.X - ankle.X;
            footRotation.rotationY = toes.Y - ankle.Y;
            footRotation.rotationZ = toes.Z - ankle.Z;
            // normalize the vector to a length of 1
            NormalizeVector(footRotation);
        }

        private void CalculateWaistRotation(IDriverDataPoint waistRotation, IDriverDataPoint rightHip, IDriverDataPoint leftHip)
        {
            waistRotation.rotationW = 0;
            // calculate vector (x, y, z) -> normal vector (z, y, -x)
            waistRotation.rotationX = leftHip.Z - rightHip.Z; // x value of the normal vector is the z value of the hip vector
            waistRotation.rotationY = 0; // set y value to 0 to get a horizontal vector
            waistRotation.rotationZ = -(leftHip.X - rightHip.X); // z value of the normal vector is the negative x value of the hip vector
            // normalize the vector to a length of 1
            NormalizeVector(waistRotation);
        }

        private void NormalizeVector(IDriverDataPoint driverDataPoint)
        {
            double length = Math.Sqrt(Math.Pow(driverDataPoint.X, 2) + Math.Pow(driverDataPoint.Y, 2) + Math.Pow(driverDataPoint.Z, 2));
            driverDataPoint.X /= length;
            driverDataPoint.Y /= length;
            driverDataPoint.Z /= length;
        }
    }
}