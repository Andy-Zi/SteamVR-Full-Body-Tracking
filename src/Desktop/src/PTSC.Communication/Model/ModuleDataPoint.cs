using PTSC.Interfaces;

namespace PTSC.Communication.Model
{

    public class ModuleDataPoint : IModuleDataPoint
    {
        object LockObject = new object();

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
                    lock(LockObject)
                        values[0] = Math.Round(value, 3);
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
                    lock (LockObject)
                        values[1] = Math.Round(value, 3);
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
                    lock (LockObject)
                        values[2] = Math.Round(value,3);
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
                return UsesVisabilty ? 0 : 1;
            }
        }


        public bool IsVisible(double threshold = 0.2)
        {
            return Visibility > threshold;
        }

        private bool UsesVisabilty = true;
        public ModuleDataPoint(List<double> data)
        {
            lock (LockObject)
            {
                if (data != null)
                {
                    if (data.Count <= 3)
                    {
                        UsesVisabilty = false;
                    }
                    foreach(var value in data)
                    {
                        values.Add(Math.Round(value, 3));
                    }
                }
                length = values.Count;
            }
            
        }
        public List<double> GetValues() => values;

        public IModuleDataPoint Clone()
        {
            var clone = new List<double>();
            lock (LockObject)
            {
                foreach (var value in values)
                {
                    clone.Add(value);
                }
            }
            return new ModuleDataPoint(clone);
        }
    }
}
