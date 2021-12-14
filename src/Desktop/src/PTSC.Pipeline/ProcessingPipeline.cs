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

        }
        protected SubscriptionToken SubscriptionToken;
        protected ImageProcessedEvent ImageProcessedEvent;
        protected DataProcessedEvent DataProcessedEvent;

        public void Start()
        {
            SubscriptionToken = EventAggregator.GetEvent<DataRecievedEvent>().Subscribe(async (payload) =>  await Process(payload));
            ImageProcessedEvent = EventAggregator.GetEvent<ImageProcessedEvent>();
            DataProcessedEvent = EventAggregator.GetEvent<DataProcessedEvent>();
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
                var moduledata = payload.ModuleDataModel;
                var bitmap = OpenCVService.Mat2Bitmap(payload.Image);
                payload.Image.Dispose();
                return bitmap;
                }
            );
        }

        async Task<IDriverDataModel> ProcessData(DataRecievedPayload payload)
        {
            //TODO Filter and Convert Data;
            return await Task.Run(() =>
            {
                var moduledata = payload.ModuleDataModel;
                return new DriverDataModel();
            });
        }
    }
}