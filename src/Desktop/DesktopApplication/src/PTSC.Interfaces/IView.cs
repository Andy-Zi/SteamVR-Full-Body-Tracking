using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Interfaces
{
    public interface IView : IDisposable
    {
        public IView Initialize(IController controller);
    }
}
