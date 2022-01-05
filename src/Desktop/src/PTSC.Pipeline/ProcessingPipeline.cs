using OpenCvSharp;
using Prism.Events;
using PTSC.Communication.Model;
using PTSC.Interfaces;
using PTSC.OpenCV;
using PTSC.PubSub;
using System.Drawing;
using Unity;

namespace PTSC.Pipeline
{
    /// <summary>
    /// Definition of the Processing Steps performed on the Data and Image
    /// </summary>
    public class ProcessingPipeline
    {

        [Dependency] public IEventAggregator EventAggregator { get; set; }

        public ProcessingPipeline()
        {
            const double dt = 0.01677;
            this.FilterModel = new KalmanFilterModel(dt);
        }
        protected SubscriptionToken SubscriptionToken;
        protected ImageProcessedEvent ImageProcessedEvent;
        protected DataProcessedEvent DataProcessedEvent;
        protected ModuleDataProcessedEvent ModuleDataProcessedEvent;
        public void Start()
        {
            SubscriptionToken = EventAggregator.GetEvent<DataRecievedEvent>().Subscribe(async (payload) =>  await Process(payload));
            ImageProcessedEvent = EventAggregator.GetEvent<ImageProcessedEvent>();
            DataProcessedEvent = EventAggregator.GetEvent<DataProcessedEvent>();
            ModuleDataProcessedEvent = EventAggregator.GetEvent<ModuleDataProcessedEvent>();
        }

        public void Stop()
        {
            SubscriptionToken.Dispose();
        }

        async Task Process(DataRecievedPayload payload)
        {
            Bitmap processedImage = null;
            if(payload.Image != null)
            {
                processedImage = await ProcessImage(payload);
            }

            var processedData = await ProcessData(payload);

            await Task.Run(() => ImageProcessedEvent.Publish(new ImageProcessedPayload(processedImage)));
            await Task.Run(() => DataProcessedEvent.Publish(new DataProcessedPayload(processedData)));
        }

        async Task<Bitmap> ProcessImage(DataRecievedPayload payload)
        {
            //TODO Paint skeleton on Image + Flip and Resize Image
            return await Task.Run(() => {
                try
                {
                    var moduledata = payload.ModuleDataModel;
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
            return await Task.Run(() =>
            {
                var moduledata = payload.ModuleDataModel;
                moduledata = FilterData(moduledata);

                Task.Run(() => ModuleDataProcessedEvent.Publish(new(moduledata)));
                return MapData(moduledata);
            });
        }

        private IDriverDataModel MapData(IModuleDataModel moduledata)
        {
            List<double> zeroList = new List<double> { 0.0f, 0.0f, 0.0f };
            DriverDataModel driverData = new DriverDataModel();
            // set nose as head for driver
            driverData.head = moduledata.NOSE ?? zeroList;
            // set center point of hips as the waist point fro driver
            driverData.waist = CalculateCenter3D(moduledata.LEFT_HIP ?? zeroList, moduledata.RIGHT_HIP ?? zeroList);
            // set ancles as foots for driver
            driverData.left_foot = moduledata.LEFT_ANKLE ?? zeroList;
            driverData.right_foot = moduledata.RIGHT_ANKLE ?? zeroList;
            return driverData;
        }

        protected KalmanFilterModel FilterModel;
        private IModuleDataModel FilterData(IModuleDataModel moduledata)
        {

            return this.FilterModel.update(moduledata); ;
        }

        private static List<double> CalculateCenter3D(List<double> point1, List<double> point2)
        {
            var x_new = (point1[0] + point2[0]) / 2;
            var y_new = (point1[1] + point2[1]) / 2;
            var z_new = (point1[2] + point2[2]) / 2;
            return new List<double> { x_new, y_new, z_new };
        }
    }
}