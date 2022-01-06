namespace PTSC.Interfaces
{
    public interface IApplicationEnvironment
    {
        string ModulesDirectory { get; set; }
        string LoggingPath { get; set; }
        string SettingsPath { get; set; }
        IApplicationSettings Settings { get; set; }
    }
}
