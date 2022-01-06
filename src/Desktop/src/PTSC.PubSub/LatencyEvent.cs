using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.PubSub
{

    public class LatencyPayload
    {
        public LatencyPayload(double value)
        {
            Latency = value;
        }

        public double Latency { get; }
    }

    public class PipelineLatencyEvent : Prism.Events.PubSubEvent<LatencyPayload>
    {

    }

    public class ModuleLatencyEvent : Prism.Events.PubSubEvent<LatencyPayload>
    {

    }
}
