using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Communication.Model
{
    public class ModulePipeDataModel
    {
        public static string PipeName => "ModulePipe";
        public static int BufferSize => 1000; // 1000 Bytes
        public static int ClientTimeout => 1000; // 1000ms timeout
    }
}
