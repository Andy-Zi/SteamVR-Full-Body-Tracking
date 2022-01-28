using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Windows;
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

                DriverData driverdata = MapData(moduledata);
                // calculate destination for waist and feet
                CalculateRotations(driverdata, moduledata);
                return driverdata;
            });
        }

        private DriverData MapData(IModuleData moduledata)
        {
            // scale coordinate before serializing
            ScaleData(moduledata);
            // rotate coordinate before serializing
            RotateData(moduledata);
            DriverData driverData = new DriverData();
            // set center point of hips as the waist point for driver
            driverData.waist = CalculateHipCenter3D(moduledata["RIGHT_HIP"].GetValues(), moduledata["LEFT_HIP"].GetValues());
            // set ancles as foots for driver
            driverData.left_foot = new DriverDataPoint("left_foot");
            driverData.left_foot.setPosition(moduledata["LEFT_ANKLE"].GetValues() ?? zeroList);
            driverData.right_foot = new DriverDataPoint("right_foot");
            driverData.right_foot.setPosition(moduledata["RIGHT_ANKLE"].GetValues() ?? zeroList);

            return driverData;
        }

        private async Task<IModuleData> FilterData(IModuleData moduledata)
        {
            if(UseKalmanFilter)
                return await KalmanFilterModel.Update(moduledata);
            return moduledata;
        }

        private static DriverDataPoint CalculateHipCenter3D(List<double> rightHip, List<double> leftHip)
        {
            DriverDataPoint driverdatapoint = new DriverDataPoint("waist");

            if (leftHip.Count < 3 || rightHip.Count < 3)
                return new DriverDataPoint("waist");

            driverdatapoint.X = (leftHip[0] + rightHip[0]) / 2;
            driverdatapoint.Y = (leftHip[1] + rightHip[1]) / 2;
            driverdatapoint.Z = (leftHip[2] + rightHip[2]) / 2;
            
            return driverdatapoint;
        }

        private void ScaleData(IModuleData moduledata)
        {
            foreach(KeyValuePair<string, IModuleDataPoint> entry in moduledata)
{
                ScaleProperty(moduledata[entry.Key]);
            }
        }

        private void ScaleProperty(IModuleDataPoint dataPoint)
        {
            dataPoint.X = dataPoint.X * ScalingOffset;
            dataPoint.Y = dataPoint.Y * ScalingOffset;
            dataPoint.Z = dataPoint.Z * ScalingOffset;
        }

        private void RotateData(IModuleData moduledata)
        {
            double rotation = ToRadians(RotationOffset);
            foreach (KeyValuePair<string, IModuleDataPoint> entry in moduledata)
            {
                RotateProperty(moduledata[entry.Key],rotation);
            }
        }

        private void RotateProperty(IModuleDataPoint dataPoint, double rotation)
        {
            double x = dataPoint.X;
            double y = dataPoint.Y;
            double z = dataPoint.Z;
            dataPoint.X = Math.Cos(rotation) * x - Math.Sin(rotation) * z;
            dataPoint.Y = dataPoint.Y;
            dataPoint.Z = Math.Cos(rotation) * z + Math.Sin(rotation) * x;
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private void CalculateRotations(IDriverData driverData, IModuleData moduleData)
        {
            // calculate waist's direction as a normal vector of the vector from right to left hip
            driverData.waist.setRotation(CalculateRotation(moduleData["RIGHT_HIP"], moduleData["LEFT_HIP"]));
            // set feet directions as the knee's direction from the hip
            driverData.left_foot.setRotation(CalculateRotation(moduleData["LEFT_ANKLE"], moduleData["LEFT_FOOT_INDEX"]));
            driverData.right_foot.setRotation(CalculateRotation(moduleData["RIGHT_ANKLE"], moduleData["RIGHT_FOOT_INDEX"]));
        }

        private Quaternion CalculateRotation(IModuleDataPoint startPoint, IModuleDataPoint endPoint)
        {
            Vector3 V1 = new Vector3((float)startPoint.X, (float)startPoint.Y, (float)startPoint.Z);
            Vector3 V2 = new Vector3((float)endPoint.X, (float)endPoint.Y, (float)endPoint.Z);
            Vector3 v = Vector3.Cross(V2 - V1, new Vector3(0,1,0));
            Quaternion q;
            q.X = v.X;
            q.Y = v.Y;
            q.Z = v.Z;
            q.W = 0;//(float)Math.Sqrt((V1.LengthSquared()) * (V2.LengthSquared())) + Vector3.Dot(V1, V2);
            q = Quaternion.Normalize(q);

            return q;
        }

    }
}