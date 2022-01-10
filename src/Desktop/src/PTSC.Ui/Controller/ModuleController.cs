using PTSC.Ui.Model;
using PTSC.Ui.Modules;
using PTSC.Ui.View;
using Unity;

namespace PTSC.Ui.Controller
{
    public class ModuleController : BaseController<ModuleModel, ModuleView>
    {
        [Dependency] public ModuleRepository ModuleRepository { get; set; }

        ModuleModel Model;

        public ModuleController(ModuleView view) : base(view)
        {

        }

        public ModuleController WithModule(ModuleModel model)
        {
            Model = ((ModuleModel)model).Clone();
            Model.ResetState();
            return this;
        }

        internal void OnOk()
        {
            Model.Save();
            ModuleRepository.Remove(Model.Name);
            ModuleRepository.Add(Model.Name, Model);
            this.View.DialogResult = DialogResult.OK;
        }

        protected override void BindData()
        {
            ViewBindings.BindView(this.View, Model);
            base.BindData();
        }

        internal void OnCancel()
        {
            this.View.Close();
        }
    }
}
