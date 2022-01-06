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
        public string LoggingPath { get; protected set; }
        public Logger(string path,string logFileName = "log.txt")
        {
            LoggingPath = Path.Combine(path, logFileName);
            serilogLogger = new LoggerConfiguration()
                .WriteTo.File(LoggingPath)
                .CreateLogger();      
        }

        public void Log(string message,LogEventLevel logLevel = LogEventLevel.Information)
        {
            serilogLogger.Write(logLevel, message);
        }
    }
}
