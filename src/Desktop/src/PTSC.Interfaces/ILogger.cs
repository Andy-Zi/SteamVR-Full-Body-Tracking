using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.Interfaces
{
    public interface ILogger
    {
        void Log(string message, LogEventLevel logLevel = LogEventLevel.Information);
    }
}
