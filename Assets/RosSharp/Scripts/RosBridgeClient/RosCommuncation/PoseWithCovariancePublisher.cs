/*
 * Pose2D Publisher Script
 * Callie Kim
 */
using UnityEngine;
using System;

namespace RosSharp.RosBridgeClient
{
    public class PoseWithCovariancePublisher : UnityPublisher<MessageTypes.Geometry.PoseWithCovariance>
    {
        public Transform PublishedTransform;
        public string FrameID = "Unity";

        private MessageTypes.Geometry.PoseWithCovariance message;
        private double[] _covariance = new double[]  {0.01,  0.0,  0.0,  0.0,  0.0,  0.0,
                                                       0.0,  0.01,  0.0,  0.0,  0.0,  0.0,
                                                       0.0,   0.0, 0.01,  0.0,  0.0,  0.0,
                                                       0.0,   0.0,  0.0,  0.1,  0.0,  0.0,
                                                       0.0,   0.0,  0.0,  0.0,  0.1,  0.0,
                                                       0.0,   0.0,  0.0,  0.0,  0.0,  0.1 };

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            //message = new MessageTypes.Geometry.Pose2D
            //{
            //    x = 0.0,
            //    y = 0.0,
            //    theta = 0.0
            //};
            message = new MessageTypes.Geometry.PoseWithCovariance
            {
                covariance = _covariance
            };
            //message.covariance = _covariance;
        }

        private void UpdateMessage()
        {
            GetGeometryPoint(PublishedTransform.position.Unity2Ros(), message.pose.position);
            GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros(), message.pose.orientation);
            Publish(message);
            //Debug.Log("published");
        }

        private static void GetGeometryPoint(Vector3 position, MessageTypes.Geometry.Point geometryPoint)
        {
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
        }

        private static void GetGeometryQuaternion(Quaternion quaternion, MessageTypes.Geometry.Quaternion geometryQuaternion)
        {
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
        }

    }
}

