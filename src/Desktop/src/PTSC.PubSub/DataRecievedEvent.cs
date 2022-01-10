using OpenCvSharp;
using PTSC.Interfaces;

namespace PTSC.PubSub
{
    public class DataRecievedPayload
    {
        public DataRecievedPayload(IModuleData moduleData,Mat image)
        {
            ModuleData = moduleData;
            Image = image;
        }

        public IModuleData ModuleData { get; }
        public Mat Image { get; }
    } 
    public class DataRecievedEvent : Prism.Events.PubSubEvent<DataRecievedPayload>
    {

    }
}