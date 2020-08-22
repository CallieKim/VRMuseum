/*
 * Pose2D Publisher Script
 * Callie Kim
 */
using UnityEngine;
using System;

namespace RosSharp.RosBridgeClient
{
    public class Pose2DPublisher : UnityPublisher<MessageTypes.Geometry.Pose2D>
    {
        public Transform PublishedTransform;
        public string FrameID = "Unity";

        private MessageTypes.Geometry.Pose2D message;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Debug.Log(message.x);
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Pose2D
            {
                x = 0.0,
                y = 0.0,
                theta = 0.0
            };
        }

        private void UpdateMessage()
        {
            //message.x = 1.0;
            //message.y = 1.0;
            //message.theta = 0.0;

            message.x = PublishedTransform.position.Unity2Ros().x;
            message.y = PublishedTransform.position.Unity2Ros().y;
            message.theta = GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros());
            //Debug.Log(message.x);
            Publish(message);
            //Debug.Log("published");
        }

        //private static void GetGeometryPoint(Vector3 position, MessageTypes.Geometry.Point geometryPoint)
        //{
        //    geometryPoint.x = position.x;
        //    geometryPoint.y = position.y;
        //    geometryPoint.z = position.z;
        //}

        private static double GetGeometryQuaternion(Quaternion q)
        {
            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            double yaw = Math.Atan2(siny_cosp, cosy_cosp);

            return yaw;
        }
    }
}

