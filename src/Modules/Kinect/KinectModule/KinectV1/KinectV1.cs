

using Interfaces;
using KinectModule;
using Microsoft.Kinect;
using System.Collections.Generic;

namespace KinectV1
{
    public class KinectV1 : IKinectAdapter
    {

        public event OnDataProcessedHandler OnDataProcessed;

        private KinectSensor kinectSensor = null;

        public KinectV1()
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    kinectSensor = potentialSensor;
                    break;
                }
            }
            kinectSensor.SkeletonStream.Enable();

            kinectSensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            kinectSensor.Start();

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

                    foreach(var skeleton in skeletons)
                    {
                        if(skeleton.TrackingState == SkeletonTrackingState.Tracked)
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
                return new List<double>() { joint.Position.X, joint.Position.Y, joint.Position.Z };
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

            return data;
        }
    }
}
