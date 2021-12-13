using PTSC.Interfaces;

namespace PTSC.PubSub
{
    public class DataProcessedPayload
    {
        public DataProcessedPayload(IDriverDataModel driverDataModel)
        {
            DriverDataModel = driverDataModel;
        }

        public IDriverDataModel DriverDataModel { get; }
    }

    public class DataProcessedEvent : Prism.Events.PubSubEvent<DataProcessedPayload>
    {

    }
}
