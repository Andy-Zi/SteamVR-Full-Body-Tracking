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
        public ModuleDataProcessedPayload(IModuleDataModel moduleDataModel)
        {
            ModuleDataModel = moduleDataModel;
        }

        public IModuleDataModel ModuleDataModel { get; }
    }

    public class ModuleDataProcessedEvent : Prism.Events.PubSubEvent<ModuleDataProcessedPayload>
    {
    }
}
