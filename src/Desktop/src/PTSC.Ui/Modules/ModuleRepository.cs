using PTSC.Interfaces;
using PTSC.Ui.Model;
using System.Text.Json;

namespace PTSC.Ui.Modules
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
            if(!Directory.Exists(moduleDirectory))
                Directory.CreateDirectory(moduleDirectory);

            foreach (var file in Directory.GetFiles(moduleDirectory, "*.ptsc", SearchOption.AllDirectories))
            {
                var module = JsonSerializer.Deserialize<ModuleModel>(File.ReadAllText(file));
                module.ModuleFile = file;
                module.WorkingDirectory = Path.GetDirectoryName(file);
                Add(module.Name, module);
            }
        }
    }
}
