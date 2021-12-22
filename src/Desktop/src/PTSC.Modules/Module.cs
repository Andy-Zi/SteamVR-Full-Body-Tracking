using PTSC.Interfaces;

namespace PTSC.Modules
{
    public class Module : IDetectionModule
    {
        public Module()
        {

        }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SupportsImage{ get; set; }
        public string Process { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public string InstallationDirectory { get; set; }
        public string InstallationArguments{ get; set; }

    }
}