namespace PTSC.Communication.Model
{
    public static class DriverPipeDataModel
    {
        public const string PipeName = "PTSCDriverPipe";
        public const int BufferSize = 1000; // 1000 Bytes
        public const int ClientTimeout = 1000; // 1000ms timeout
    }
}
