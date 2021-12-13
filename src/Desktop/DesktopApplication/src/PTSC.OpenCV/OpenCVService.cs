using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;

namespace PTSC.OpenCV
{
    public static class OpenCVService
    {
        public static Mat DecodeImage(Span<byte> imageData)
        {
            var mat = Cv2.ImDecode(imageData, ImreadModes.Color);
            return mat;
        }

        public static Bitmap Mat2Bitmap(Mat image)
        {
            return BitmapConverter.ToBitmap(image);
        }
    }
}