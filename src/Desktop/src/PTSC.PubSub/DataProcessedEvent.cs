using PTSC.Interfaces;

namespace PTSC.PubSub
{
    public class DataProcessedPayload
    {
        public DataProcessedPayload(IDriverData driverData)
        {
            DriverData = driverData;
        }

        public IDriverData DriverData { get; }
    }

    public class DataProcessedEvent : Prism.Events.PubSubEvent<DataProcessedPayload>
    {

    }
}
