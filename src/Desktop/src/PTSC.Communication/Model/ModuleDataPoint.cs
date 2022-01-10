using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    public class ModuleDataPoint : IModuleDataPoint
    {

        private List<double> values = new();
        private int length = 0;

        public double X
        {
            get
            {
                if (length >= 1)
                {
                    return values[0];
                }
                return 0;
            }
            set
            {
                if (length >= 1)
                {
                    values[0] = value;
                }
            }
        }

        public double Y
        {
            get
            {
                if (length >= 2)
                {
                    return values[1];
                }
                return 0;
            }
            set
            {
                if (length >= 2)
                {
                    values[1] = value;
                }
            }
        }

        public double Z
        {
            get
            {
                if (length >= 3)
                {
                    return values[2];
                }
                return 0;
            }
            set
            {
                if (length >= 3)
                {
                    values[2] = value;
                }
            }
        }

        public double Visibility
        {
            get
            {
                if (length >= 4)
                {
                    return values[3];
                }
                return 0;
            }
        }
        public ModuleDataPoint(IEnumerable<double> data)
        {
            Update(data);
        }
        public List<double> GetValues() => values;

        public void Update(IEnumerable<double> newData)
        {
            values.Clear();
            if(newData != null)
            {
                values.AddRange(newData);
            }
            length = values.Count;
        }
    }
}
