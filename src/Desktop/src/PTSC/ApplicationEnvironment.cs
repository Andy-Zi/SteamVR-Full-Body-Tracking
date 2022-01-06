using PTSC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC
{
    public class ApplicationEnvironment : IApplicationEnvironment
    {
        public string ModulesDirectory { get; set; }
        public string LoggingPath { get; set; }
        public string SettingsPath { get; set; }

        public IApplicationSettings Settings { get; set; }
    }
}
