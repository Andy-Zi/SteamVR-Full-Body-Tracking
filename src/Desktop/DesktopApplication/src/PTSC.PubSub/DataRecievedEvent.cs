using OpenCvSharp;
using PTSC.Interfaces;

namespace PTSC.PubSub
{
    public class DataRecievedPayload
    {
        public DataRecievedPayload(IModuleDataModel moduleDataModel,Mat image)
        {
            ModuleDataModel = moduleDataModel;
            Image = image;
        }

        public IModuleDataModel ModuleDataModel { get; }
        public Mat Image { get; }
    } 
    public class DataRecievedEvent : Prism.Events.PubSubEvent<DataRecievedPayload>
    {

    }
}