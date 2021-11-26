using PTSC.Interfaces;
using PTSC.Ui.Model;
using PTSC.Ui.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PTSC.Ui.Controller
{
    public class MainController : BaseController<MainModel, MainView>
    {
        [Dependency] public ILogger Logger { get; set; }
        public MainController(MainView view) : base(view)
        {

        }

        public void SaySomething()
        {
            Logger.Log("foobar");
        }
    }
}
