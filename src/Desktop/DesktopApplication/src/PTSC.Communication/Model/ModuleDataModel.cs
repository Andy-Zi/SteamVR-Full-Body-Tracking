using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Communication.Model
{
    public class ModuleDataModel
    {
        public List<double> left_shoulder { get; set; }
        public List<double> right_shoulder { get; set; }
        public List<double> left_elbow { get; set; }
        public List<double> right_elbow { get; set; }
        public List<double> left_wrist { get; set; }
        public List<double> right_wrist { get; set; }
        public List<double> left_hip { get; set; }
        public List<double> right_hip { get; set; }
        public List<double> left_knee { get; set; }
        public List<double> right_knee { get; set; }
        public List<double> left_ankle { get; set; }
        public List<double> right_ankle { get; set; }
        public List<double> left_heel { get; set; }
        public List<double> right_heel { get; set; }
        public List<double> left_foot_index { get; set; }
        public List<double> right_foot_index { get; set; }
    }
}
