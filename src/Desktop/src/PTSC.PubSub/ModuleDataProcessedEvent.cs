using PTSC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.PubSub
{

    public class ModuleDataProcessedPayload
    {
        public ModuleDataProcessedPayload(IModuleData moduleData)
        {
            ModuleData = moduleData;
        }

        public IModuleData ModuleData { get; }
    }

    public class ModuleDataProcessedEvent : Prism.Events.PubSubEvent<ModuleDataProcessedPayload>
    {
    }
}
