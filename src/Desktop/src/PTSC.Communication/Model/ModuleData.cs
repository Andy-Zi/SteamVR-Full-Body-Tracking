using PTSC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Communication.Model
{
    public class ModuleData : Dictionary<string, IModuleDataPoint> , IModuleData
    {
        public ModuleData(ModuleDataModel moduleDataModel)
        {
            this.Add("NOSE",new ModuleDataPoint(moduleDataModel.NOSE));
            this.Add("LEFT_EYE", new ModuleDataPoint(moduleDataModel.LEFT_EYE));
            this.Add("RIGHT_EYE", new ModuleDataPoint(moduleDataModel.RIGHT_EYE));
            this.Add("LEFT_EAR", new ModuleDataPoint(moduleDataModel.LEFT_EAR));
            this.Add("RIGHT_EAR", new ModuleDataPoint(moduleDataModel.RIGHT_EAR));
            this.Add("LEFT_SHOULDER", new ModuleDataPoint(moduleDataModel.LEFT_SHOULDER));
            this.Add("RIGHT_SHOULDER", new ModuleDataPoint(moduleDataModel.RIGHT_SHOULDER));
            this.Add("LEFT_ELBOW", new ModuleDataPoint(moduleDataModel.LEFT_ELBOW));
            this.Add("RIGHT_ELBOW", new ModuleDataPoint(moduleDataModel.RIGHT_ELBOW));
            this.Add("LEFT_WRIST", new ModuleDataPoint(moduleDataModel.LEFT_WRIST));
            this.Add("RIGHT_WRIST", new ModuleDataPoint(moduleDataModel.RIGHT_WRIST));
            this.Add("LEFT_HIP", new ModuleDataPoint(moduleDataModel.LEFT_HIP));
            this.Add("RIGHT_HIP", new ModuleDataPoint(moduleDataModel.RIGHT_HIP));
            this.Add("LEFT_KNEE", new ModuleDataPoint(moduleDataModel.LEFT_KNEE));
            this.Add("RIGHT_KNEE", new ModuleDataPoint(moduleDataModel.RIGHT_KNEE));
            this.Add("LEFT_ANKLE", new ModuleDataPoint(moduleDataModel.LEFT_ANKLE));
            this.Add("RIGHT_ANKLE", new ModuleDataPoint(moduleDataModel.RIGHT_ANKLE));
        }
    }
}
