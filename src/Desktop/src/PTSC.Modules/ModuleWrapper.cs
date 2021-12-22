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
        public ModuleWrapper()
        {

        }

        public void Start(IDetectionModule detectionModule)
        {
            if(CurrentDetectionModule != null)
            {
                if (CurrentDetectionModule.Name.Equals(detectionModule.Name))
                    return;

                Stop();
                CurrentProcess?.WaitForExit();
            }

            var startinfo = new ProcessStartInfo()
            {
                WorkingDirectory = detectionModule.WorkingDirectory,
                CreateNoWindow = true,  
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = detectionModule.Process,
                Arguments = detectionModule.Arguments,
            };

            CurrentProcess = new Process();
            CurrentProcess.StartInfo = startinfo;
            CurrentProcess.ErrorDataReceived += CurrentProcess_ErrorDataReceived;
            CurrentProcess.OutputDataReceived += CurrentProcess_OutputDataReceived;
            CurrentProcess.EnableRaisingEvents = true;
            CurrentProcess.Start();
            CurrentProcess.BeginErrorReadLine();
            CurrentProcess.BeginOutputReadLine();
            CurrentDetectionModule = detectionModule;   
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
