using MvvmGen;
using PTSC.Interfaces;
using System.Text.Json;

namespace PTSC.Ui.Model
{
    [ViewModel]
    public partial class ModuleModel : BaseModel, IDetectionModule
    {
        [Property]
        string moduleFile;
        [Property]
        string name;
        [Property]
        string description;
        [Property]
        bool supportsImage;
        [Property]
        string process;

        internal ModuleModel Clone()
        {
            return this.MemberwiseClone() as ModuleModel;
        }

        [Property]
        string arguments;
        [Property]
        string workingDirectory;
        [Property]
        string installationScript;

        public void Save()
        {
            File.WriteAllText(moduleFile, JsonSerializer.Serialize((IDetectionModule)this, new JsonSerializerOptions() { WriteIndented = true }));
        }

        [Property]
        string installationDirectory;
    }
}