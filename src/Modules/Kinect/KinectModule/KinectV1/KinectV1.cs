

using Interfaces;
using KinectModule;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KinectV1
{
    public class KinectV1 : IKinectAdapter
    {

        public event OnDataProcessedHandler OnDataProcessed;
        public event OnImageProcessedHandler OnImageProcessed;

        private KinectSensor kinectSensor = null;
        private byte[] colorPixels;

        public KinectV1(bool useCamera)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    kinectSensor = potentialSensor;
                    break;
                }
            }
            if (kinectSensor == null)
            {
                throw new Exception("No Kinect-V1-Sensor found!");
            }

            kinectSensor.SkeletonStream.Enable();
            kinectSensor.SkeletonFrameReady += SensorSkeletonFrameReady;

            if (useCamera)
            {
                colorPixels = new byte[kinectSensor.ColorStream.FramePixelDataLength];
                kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinectSensor.ColorFrameReady += KinectSensor_ColorFrameReady;
            }

            
            kinectSensor.Start();

        }

        private void KinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {


            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(colorPixels);

                    // Write the pixel data into our bitmap
                    var bitmap = new Bitmap(
                        kinectSensor.ColorStream.FrameWidth,
                        kinectSensor.ColorStream.FrameHeight,
                        kinectSensor.ColorStream.FrameWidth * 4,
                        PixelFormat.Format32bppRgb,
                        Marshal.UnsafeAddrOfPinnedArrayElement(colorPixels, 0));

                    OnImageProcessed?.Invoke(bitmap);
                }

            }

        }

        public void Stop()
        {
            kinectSensor.Stop();
            kinectSensor.Dispose();
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    foreach (var skeleton in skeletons)
                    {
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            OnDataProcessed?.Invoke(TransformData(skeleton.Joints));
                        }
                    }
                }
            }
        }

        private IModuleDataModel TransformData(JointCollection positions)
        {
            List<double> TranformJoint(Joint joint)
            {
                return new List<double>() { -(joint.Position.X), joint.Position.Y, -(joint.Position.Z) };
            }

            var data = new ModuleDataModel();
            data.NOSE = TranformJoint(positions[JointType.Head]);
            data.LEFT_EYE = null;
            data.RIGHT_EYE = null;
            data.LEFT_EAR = null;
            data.RIGHT_EAR = null;
            data.LEFT_SHOULDER = TranformJoint(positions[JointType.ShoulderLeft]);
            data.RIGHT_SHOULDER = TranformJoint(positions[JointType.ShoulderRight]);
            data.LEFT_ELBOW = TranformJoint(positions[JointType.ElbowLeft]);
            data.RIGHT_ELBOW = TranformJoint(positions[JointType.ElbowRight]);
            data.LEFT_WRIST = TranformJoint(positions[JointType.WristLeft]);
            data.RIGHT_WRIST = TranformJoint(positions[JointType.WristRight]);
            data.LEFT_HIP = TranformJoint(positions[JointType.HipLeft]);
            data.RIGHT_HIP = TranformJoint(positions[JointType.HipRight]);
            data.LEFT_KNEE = TranformJoint(positions[JointType.KneeLeft]);
            data.RIGHT_KNEE = TranformJoint(positions[JointType.KneeRight]);
            data.LEFT_ANKLE = TranformJoint(positions[JointType.AnkleLeft]);
            data.RIGHT_ANKLE = TranformJoint(positions[JointType.AnkleRight]);
            data.LEFT_FOOT_INDEX = TranformJoint(positions[JointType.FootLeft]);
            data.RIGHT_FOOT_INDEX = TranformJoint(positions[JointType.FootRight]);

            return data;
        }
    }
}
