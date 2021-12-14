using NUnit.Framework;
using OpenCvSharp;
using PTSC.OpenCV;
using System.Drawing;

namespace _PTSC.Tests
{
    public class Tests
    {

        Mat TestImage;

        [SetUp]
        public void Setup()
        {
            TestImage = new Mat(new OpenCvSharp.Size(1280,720),MatType.CV_8UC3);
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void DecodeImage_Should_Decode()
        {
            Cv2.ImEncode(".jpg", TestImage, out var encoded);
            Mat decoded = null;
            Assert.DoesNotThrow(() =>
            {
                decoded = OpenCVService.DecodeImage(encoded);
            });
            Assert.Multiple(() =>
            {
                Assert.AreEqual(TestImage.Height, decoded.Height);
                Assert.AreEqual(TestImage.Width, decoded.Width);
                Assert.AreEqual(TestImage.Channels(), decoded.Channels());
            });
        }

        [Test]
        public void Mat2Bitmap_Should_Create_Bitmap()
        {

            Bitmap result = null;
            Assert.DoesNotThrow(() =>
            {
                result = OpenCVService.Mat2Bitmap(TestImage);
            });
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(TestImage.Height, result.Height);
                Assert.AreEqual(TestImage.Width, result.Width);
            });
        }
    }
}