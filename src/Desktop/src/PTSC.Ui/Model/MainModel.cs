using MvvmGen;

namespace PTSC.Ui.Model
{
    [ViewModel]
    public partial class MainModel : BaseModel
    {
        [Property]
        private int fpsLimit;
    }
}
