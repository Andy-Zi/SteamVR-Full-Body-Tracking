using PTSC.Interfaces;
using System.Diagnostics;

namespace PTSC.Modules
{
    public class ModuleWrapper : IDisposable
    {
        public delegate void OnMessageHandler(string message);
        public event OnMessageHandler OnMessage;

        public delegate void OnErrorHandler(string message);
        public event OnErrorHandler OnError;

        public Process CurrentProcess { get; protected set; }
        public IDetectionModule CurrentDetectionModule { get; protected set; }
        private Task ModuleTask;
        public ModuleWrapper()
        {

        }

        public void Start(IDetectionModule detectionModule)
        {
            // Check for venv folder
            string venvPath = Path.Combine(detectionModule.WorkingDirectory, detectionModule.InstallationDirectory);
            if (!Directory.Exists(venvPath))
            {
                var setupStartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = detectionModule.WorkingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "powershell.exe",
                    Arguments = detectionModule.InstallationScript,
                };
                // Start new Task Chain
                ModuleTask =  Task.Factory.StartNew(() => StartProcess(detectionModule, setupStartInfo, true));
            }

            var runStartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = detectionModule.WorkingDirectory,
                CreateNoWindow = true,  
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = Path.Combine(detectionModule.WorkingDirectory, detectionModule.Process),
                Arguments = detectionModule.Arguments,
            };

            // Virtual environment existing, only execute module
            if (ModuleTask == null)
            {
                ModuleTask = Task.Factory.StartNew(() => StartProcess(detectionModule, runStartInfo, false));
            }
            else // After environment setup continute with module execution
            {
                ModuleTask.ContinueWith(t => StartProcess(detectionModule, runStartInfo, false), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void StartProcess(IDetectionModule detectionModule, ProcessStartInfo startInfo, bool waitForExit)
        {
            CurrentProcess = new Process();
            CurrentProcess.StartInfo = startInfo;
            CurrentProcess.ErrorDataReceived += CurrentProcess_ErrorDataReceived;
            CurrentProcess.OutputDataReceived += CurrentProcess_OutputDataReceived;
            CurrentProcess.EnableRaisingEvents = true;
            CurrentProcess.Start();
            CurrentProcess.BeginErrorReadLine();
            CurrentProcess.BeginOutputReadLine();
            CurrentDetectionModule = detectionModule;
            if (waitForExit)
            {
                CurrentProcess.WaitForExit();
            }
        }

        private void CurrentProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OnMessage != null)
                OnMessage(e.Data);
        }

        private void CurrentProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OnError != null)
                OnError(e.Data);
        }

        public void Stop()
        {
            CurrentProcess?.Kill();
            CurrentDetectionModule = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
