using PTSC.Interfaces;
using System.Text.Json;

namespace PTSC.Modules
{
    public class ModuleRepository : Dictionary<string, IDetectionModule>
    {
        private readonly string moduleDirectory;
        private readonly ILogger logger;

        public ModuleRepository(string moduleDirectory, ILogger logger)
        {
            this.moduleDirectory = moduleDirectory;
            this.logger = logger;
            logger.Log($"Using '{moduleDirectory}' as ModuleDirectory.");
        }
        public void Load()
        {
            foreach(var file in Directory.GetFiles(moduleDirectory, "*.ptsc", SearchOption.AllDirectories))
            {
                var module = JsonSerializer.Deserialize<Module>(File.ReadAllText(file));
                module.WorkingDirectory = Path.GetDirectoryName(file);
                this.Add(module.Name, module);
            }
        }
    }
}
