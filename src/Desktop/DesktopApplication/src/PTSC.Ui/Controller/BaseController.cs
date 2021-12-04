using PTSC.Interfaces;

namespace PTSC.Ui.Controller
{
    public abstract class BaseController<TModel, TView> : IController<TModel, TView>
        where TModel : IModel
        where TView : IView
    {
        public TView View { get; protected set; }

        public BaseController(TView view)
        {
            View = view;
        }
        public virtual void Dispose()
        {
            //Close Stuff here
        }

        public virtual BaseController<TModel, TView> Initialize()
        {
            View.Initialize(this);
            return this;
        }
    }
}
