namespace PTSC.Communication.Model
{
    public static class DriverPipeDataModel
    {
        public static string PipeName => "DriverPipe";
        public static int BufferSize => 1000; // 1000 Bytes
        public static int ClientTimeout => 1000; // 1000ms timeout
    }
}
