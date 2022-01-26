using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTSC.Interfaces;

namespace PTSC.Communication.Model
{
    public class DriverDataPoint : IDriverDataPoint
    {
        private readonly List<double> values = new() { 0,0,0,0,0,0,0 };

        public DriverDataPoint(List<double> position)
        {
            if(position.Count != 0)
            {
                this.X = position[0];
                this.Y = position[1];
                this.Z = position[2];
            }
            else
            {
                this.X = 0;
                this.Y = 0;
                this.Z = 0;
            }
        }

        public double X 
        { 
            get
            {
                return values[0];
            }
            set
            {
                values[0] = value;
            }
        }

        public double Y
        {
            get
            {
                return values[1];
            }
            set
            {
                values[1] = value;
            }
        }

        public double Z
        {
            get
            {
                return values[2];
            }
            set
            {
                values[2] = value;
            }
        }
        public double rotationW
        {
            get
            {
                return values[3];
            }
            set
            {              
                values[3] = value;
            }
        }

        public double rotationX
        {
            get
            {
                return values[4];
            }
            set
            {
                values[4] = value;
            }
        }

        public double rotationY
        {
            get
            {
                return values[5];
            }
            set
            {
                values[5] = value;
            }
        }

        public double rotationZ
        {
            get
            {
                return values[6];
            }
            set
            {
                values[6] = value;
            }
        }        
    }
}
