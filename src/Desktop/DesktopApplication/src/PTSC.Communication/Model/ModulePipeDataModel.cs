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
        public static int JsonBufferSize => 8192;
        public static int ImageBufferSize => 1024 * 1024 * 4;
        public static int BufferSize => JsonBufferSize + ImageBufferSize;
        public static int ClientTimeout => 1000; // 1000ms timeout
    }
}
