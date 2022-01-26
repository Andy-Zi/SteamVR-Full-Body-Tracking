
using Interfaces;
using System.Collections.Generic;

namespace KinectModule
{
    public class ModuleDataModel : IModuleDataModel
    {
        private List<double> Normlize(List<double> values, List<double> offset)
        {
            return new List<double>() { values[0] - offset[0], values[1] - offset[1], values[2] - offset[2] };
        }
        public void NormalizeToHead()
        {
            var offset = NOSE;
            NOSE = new List<double>() { 0, 0, 0, };
            LEFT_SHOULDER = Normlize(LEFT_SHOULDER, offset);
            RIGHT_SHOULDER = Normlize(RIGHT_SHOULDER, offset);
            LEFT_ELBOW = Normlize(LEFT_ELBOW, offset);
            RIGHT_ELBOW = Normlize(RIGHT_ELBOW, offset);
            LEFT_WRIST = Normlize(LEFT_WRIST, offset);
            RIGHT_WRIST = Normlize(RIGHT_WRIST, offset);
            LEFT_HIP = Normlize(LEFT_HIP, offset); ;
            RIGHT_HIP = Normlize(RIGHT_HIP, offset);
            LEFT_KNEE = Normlize(LEFT_KNEE, offset);
            RIGHT_KNEE = Normlize(RIGHT_KNEE, offset);
            LEFT_ANKLE = Normlize(LEFT_ANKLE, offset);
            RIGHT_ANKLE = Normlize(RIGHT_ANKLE, offset);
            LEFT_FOOT_INDEX = Normlize(LEFT_FOOT_INDEX, offset);
            RIGHT_FOOT_INDEX = Normlize(RIGHT_FOOT_INDEX, offset);
        }


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
        public List<double> LEFT_FOOT_INDEX { get; set; }
        public List<double> RIGHT_FOOT_INDEX { get; set; }
    }
}