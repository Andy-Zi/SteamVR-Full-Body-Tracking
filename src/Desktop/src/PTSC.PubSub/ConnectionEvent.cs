using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.PubSub
{

    public class ConnectionPayload
    {
        public ConnectionPayload(bool isConnected)
        {
            IsConnected = isConnected;
        }

        public bool IsConnected { get; }
    }
    public class ModuleConnectionEvent : Prism.Events.PubSubEvent<ConnectionPayload>
    {

    }

    public class DriverConnectionEvent : Prism.Events.PubSubEvent<ConnectionPayload>
    {

    }
}
