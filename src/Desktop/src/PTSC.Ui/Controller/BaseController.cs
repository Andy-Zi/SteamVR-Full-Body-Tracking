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


        protected virtual void BindData()
        {

        }

        public virtual BaseController<TModel, TView> Initialize()
        {
            BindData();
            View.Initialize(this);
            return this;
        }
    }
}
