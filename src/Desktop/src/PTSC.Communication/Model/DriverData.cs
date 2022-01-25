using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTSC.Interfaces;

namespace PTSC.Communication.Model
{
    public class DriverData : Dictionary<string, IDriverDataPoint>, IDriverData
    {
        public DriverData(DriverDataModel driverDataModel)
        {
            List<double> zeroList = new List<double> { 0, 0, 0, 0 };
            this.Add("head", new DriverDataPoint(driverDataModel.head));
            this.Add("waist", new DriverDataPoint(driverDataModel.waist));
            this.Add("left_hip", new DriverDataPoint(driverDataModel.left_hip));
            this.Add("right_hip", new DriverDataPoint(driverDataModel.right_hip));
            this.Add("left_knee", new DriverDataPoint(driverDataModel.left_knee));
            this.Add("right_knee", new DriverDataPoint(driverDataModel.right_knee));
            this.Add("left_foot", new DriverDataPoint(driverDataModel.left_foot));
            this.Add("right_foot", new DriverDataPoint(driverDataModel.right_foot));
            this.Add("left_foot_toes", new DriverDataPoint(driverDataModel.left_foot_toes));
            this.Add("right_foot_toes", new DriverDataPoint(driverDataModel.right_foot_toes));
            this.Add("head_rotation", new DriverDataPoint(zeroList));
            this.Add("waist_rotation", new DriverDataPoint(zeroList));
            this.Add("left_foot_rotation", new DriverDataPoint(zeroList));
            this.Add("right_foot_rotation", new DriverDataPoint(zeroList));
        }
    }
}
