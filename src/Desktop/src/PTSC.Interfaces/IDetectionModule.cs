namespace PTSC.Interfaces
{
    public interface IDetectionModule
    {
        public string Name { get; }
        public string Description { get; }
        public bool SupportsImage { get; }
        public string Process { get; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public string InstallationScript { get; set; }
        public string InstallationDirectory { get; set; }
    }
}