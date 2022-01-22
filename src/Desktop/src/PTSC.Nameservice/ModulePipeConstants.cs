namespace PTSC.Nameservice
{
    public static class ModulePipeConstants
    {
        public const string PipeName = "PTSCModulePipe";
        public const int JsonBufferSize = 8192;
        public const int ImageBufferSize = 1024 * 1024 * 4;
        public const int BufferSize = JsonBufferSize + ImageBufferSize;
        public static readonly string[] SkeletonParts = new string[] {
        "NOSE",
        "LEFT_EYE",
        "RIGHT_EYE",
        "LEFT_EAR",
        "RIGHT_EAR",
        "LEFT_SHOULDER",
        "RIGHT_SHOULDER",
        "LEFT_ELBOW",
        "RIGHT_ELBOW",
        "LEFT_WRIST",
        "RIGHT_WRIST",
        "LEFT_HIP",
        "RIGHT_HIP",
        "LEFT_KNEE",
        "RIGHT_KNEE",
        "LEFT_ANKLE",
        "RIGHT_ANKLE",
        "LEFT_TOES",
        "RIGHT_TOES",
        }; 
    }
}
