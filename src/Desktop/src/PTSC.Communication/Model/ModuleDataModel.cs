using PTSC.Interfaces;
using System.Windows.Forms.DataVisualization.Charting;

namespace PTSC.Communication.Model
{

    public class ModuleDataModel : IModuleDataModel
    {
        public List<double> NOSE { get; set; }

        public List<double> LEFT_EYE { get; set; }
        public List<double> RIGHT_EYE { get; set; }

        public List<double> LEFT_EAR { get; set; }
        public List<double> RIGHT_EAR { get; set; }

        public List<double> LEFT_SHOULDER { get; set; }
        public List<double> RIGHT_SHOULDER { get; set; }

        public List<double> LEFT_ELBOW { get; set; }
        public List<double> RIGHT_ELBOW { get; set; }

        public List<double> LEFT_WRIST { get; set; }
        public List<double> RIGHT_WRIST { get; set; }

        public List<double> LEFT_HIP { get; set; }
        public List<double> RIGHT_HIP { get; set; }

        public List<double> LEFT_KNEE { get; set; }
        public List<double> RIGHT_KNEE { get; set; }

        public List<double> LEFT_ANKLE { get; set; }
        public List<double> RIGHT_ANKLE { get; set; }

        public Dictionary<string, List<double>> GetData()
        {
            return new()
            {  
                {nameof(NOSE), NOSE },
                { nameof(LEFT_EYE), LEFT_EYE },
                { nameof(RIGHT_EYE), RIGHT_EYE },
                { nameof(LEFT_EAR), LEFT_EAR },
                { nameof(RIGHT_EAR), RIGHT_EAR },
                { nameof(LEFT_SHOULDER), LEFT_SHOULDER },
                { nameof(RIGHT_SHOULDER), RIGHT_SHOULDER },
                { nameof(LEFT_ELBOW), LEFT_ELBOW },
                { nameof(RIGHT_ELBOW), RIGHT_ELBOW },
                { nameof(LEFT_WRIST), LEFT_WRIST },
                { nameof(RIGHT_WRIST), RIGHT_WRIST },
                { nameof(LEFT_HIP), LEFT_HIP },
                { nameof(RIGHT_HIP), RIGHT_HIP },
                { nameof(LEFT_KNEE), LEFT_KNEE },
                { nameof(RIGHT_KNEE), RIGHT_KNEE },
                { nameof(LEFT_ANKLE), LEFT_ANKLE },
                { nameof(RIGHT_ANKLE), RIGHT_ANKLE },
            };
        }
    }
}
