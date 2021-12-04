using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Communication.Model
{
    [Serializable]

    public class DriverDataModel
    {
        public List<double> LEFT_HIP { get; set; }
        public List<double> RIGHT_HIP { get; set; }
        public List<double> LEFT_ANKLE { get; set; }
        public List<double> RIGHT_ANKLE { get; set; }
        public List<double> LEFT_WRIST { get; set; }
        public List<double> RIGHT_WRIST { get; set; }
        public List<double> NOSE { get; set; }
        public List<double> LEFT_FOOT_INDEX { get; set; }
        public List<double> RIGHT_FOOT_INDEX { get; set; }

        //public List<double> LEFT_SHOULDER { get; set; }
        //public List<double> RIGHT_SHOULDER { get; set; }
        //public List<double> LEFT_ELBOW { get; set; }
        //public List<double> RIGHT_ELBOW { get; set; }
        //public List<double> LEFT_KNEE { get; set; }
        //public List<double> RIGHT_KNEE { get; set; }
        //public List<double> LEFT_HEEL { get; set; }
        //public List<double> RIGHT_HEEL { get; set; }
        //public string image_id { get; set; }
        //public int? image_width { get; set; }
        //public int? image_height { get; set; }
    }
}
