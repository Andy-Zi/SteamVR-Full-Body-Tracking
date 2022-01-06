using MvvmGen;
using PTSC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Ui.Model
{

    [ViewModel]
    public partial class ApplicationSettingsModel : BaseModel, IApplicationSettings
    {
        [Property]
        private bool supportModuleImage;

        [Property]
        private int fPSLimit;

        [Property]
        private double scaling;

        [Property]
        private double rotation;

        [Property]
        private bool useKalmanFilter;

        [Property]
        private int kalmanFPS;

        [Property]
        private double kalmanXError;

        [Property]
        private double kalmanYError;

        [Property]
        private double kalmanZError;

        [Property]
        private double kalmanVelocityError;

        public ApplicationSettingsModel Default()
        {
            scaling = 1.0;
            rotation = 0;
            useKalmanFilter = true;
            kalmanFPS = 30;
            kalmanXError = 0.005;
            kalmanYError = 0.005;
            kalmanZError = 0.005;
            kalmanVelocityError = 1.0;
            fPSLimit = 30;
            supportModuleImage = true;
            return this;
        }

        public ApplicationSettingsModel Clone()
        {
            return (ApplicationSettingsModel)this.MemberwiseClone();
        }

    }
}
