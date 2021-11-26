using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Interfaces
{
    public interface IController : IDisposable
    {

    }

    public interface IController<TModel, TView> : IController
    where TModel : IModel
    where TView : IView
    {

    }
}
