using System;

namespace Interfaces
{
    public delegate void OnDataProcessedHandler(IModuleDataModel data);
    public interface IKinectAdapter
    {
        event OnDataProcessedHandler OnDataProcessed;
    }
}
