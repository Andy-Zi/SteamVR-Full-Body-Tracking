using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC
{
    public class Logger : Interfaces.ILogger
    {
        Serilog.Core.Logger serilogLogger;
        public Logger(string path,string logFileName = "log.txt")
        {
            serilogLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(path), logFileName))
                .CreateLogger();

            
        }

        public void Log(string message,LogEventLevel logLevel = LogEventLevel.Information)
        {
            serilogLogger.Write(logLevel, message);
        }
    }
}
