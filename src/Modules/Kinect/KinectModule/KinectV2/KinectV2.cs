
using Interfaces;
using KinectModule;
using Microsoft.Kinect;
using System.Collections.Generic;
using Kinect = Microsoft.Kinect;

namespace KinectV2
{
    public class KinectV2 : IKinectAdapter
    {
        public event OnDataProcessedHandler OnDataProcessed;
        public event OnImageProcessedHandler OnImageProcessed;

        private Kinect.KinectSensor kinectSensor = null;
        private Kinect.CoordinateMapper coordinateMapper = null;
        private Kinect.BodyFrameReader bodyFrameReader = null;
        private ColorFrameReader colorFrameReader = null;
        private Kinect.Body[] bodies = null;

        public KinectV2(bool useCamera)
        {
            kinectSensor = Kinect.KinectSensor.GetDefault();
            coordinateMapper = kinectSensor.CoordinateMapper;
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            if (useCamera)
            {
                colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
                colorFrameReader.FrameArrived += ColorFrameReader_FrameArrived;
            }
       
            kinectSensor.Open();

            if (bodyFrameReader != null)
            {
                bodyFrameReader.FrameArrived += Reader_FrameArrived;
            }

        }

        private void ColorFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
           //TODO
        }

        public void Stop()
        {
            colorFrameReader?.Dispose();
            bodyFrameReader?.Dispose();
            kinectSensor.Close();
        }


        private void Reader_FrameArrived(object sender, Kinect.BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (Kinect.BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Kinect.Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                foreach (var body in bodies)
                {
                    if (body.IsTracked)
                    {
                        OnDataProcessed?.Invoke(TransformData(body.Joints));
                        break;
                    }
                }
            }
        }

        private IModuleDataModel TransformData(IReadOnlyDictionary<JointType, Joint> positions)
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
            data.LEFT_TOES = TranformJoint(positions[JointType.FootLeft]);
            data.RIGHT_TOES = TranformJoint(positions[JointType.FootRight]);

            return data;
        }
    }
}
