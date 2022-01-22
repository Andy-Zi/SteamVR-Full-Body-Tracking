using System;
using System.Drawing;

namespace Interfaces
{
    public delegate void OnDataProcessedHandler(IModuleDataModel data);
    public delegate void OnImageProcessedHandler(Bitmap image);
    public interface IKinectAdapter
    {
        event OnDataProcessedHandler OnDataProcessed;
        event OnImageProcessedHandler OnImageProcessed;
    }
}
