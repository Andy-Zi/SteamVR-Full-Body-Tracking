namespace PTSC.Communication.Model
{
    public static class ModulePipeDataModel
    {
        public const string PipeName = "PTSCModulePipe";
        public const int JsonBufferSize = 8192;
        public const int ImageBufferSize = 1024 * 1024 * 4;
        public const int BufferSize = JsonBufferSize + ImageBufferSize;
    }
}
