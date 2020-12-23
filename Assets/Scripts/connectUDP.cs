using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RosSharp.RosBridgeClient
{
    public class connectUDP : MonoBehaviour
    {
        public unsafe struct PoseMessage// info pranav needs to display pose of tool
        {
            public UInt32 message_id; //Numeric code for Pose message type, 0x0300
            public double timestamp;//Time of the position estimate, in UNIX epoch seconds
            public UInt32 error_code;//Value is 0 if there is no error and non-zero iif the base station’s high level state is ERROR
                                     // 0: No Error
                                     // 1: Error during system initialization (most likely PLL failed to lock)
                                     // 2: Unable to get any background data
                                     // 4: Invalid Filter Parameters (check the settings by using the Config Updater tool)
                                     // 5: Error establishing or maintaining digitizer comms (could be network or DCC issue)
                                     // 6: Received insufficient number of bad backgrounds and exceeded all retries
                                     // 7: Cannot establish data output interface
                                     // 8: Cannot establish command & control interface
            public UInt32 message_size;//Size, in bytes, of this message payload
            public UInt32 pose_id;//Unique identifier for pose.  Matches subscription ID defined in SetPoseSubscription.
            public fixed double position[3];
            //public char toolname1; public char toolname2; public char toolname3; public char toolname4; public char toolname5; public char toolname6; public char toolname7; public char toolname8; public char toolname9; public char toolname10;
            public bool GimbleLock;
            public fixed double rotate[9];
            //Vector3D X, Y, Z position of the object [meters]
            public fixed double orientation[3]; //RollPitchYawD Roll, pitch, yaw representing orientation [radians]
            public fixed double linearVelocity[3];// Vector3D X, Y, Z linear velocity of the object [m/s]
            public fixed double angularVelocity[3];// RollPitchYawD Angular velocity [radians per second]
            public fixed double measurement_quality[9]; //MeasurementQualityList Quality of underlying range measurements
            public fixed double position_quality[9];//PositionQualityList Collection of information describing the quality of the position estimates used to generate the pose estimate.
            public fixed double pose_quality[9]; //PoseQualityList Quality of the pose estimate and all other pose estimates used (for relative pose cases)
            public fixed char toolname[10];
        }

        public unsafe struct Pos
        {
            public fixed double position[2];
            public double delta;
        }

        public UDP<PoseMessage> newConnection;
        GameObject origin;
        GameObject tracker;
        Pos originPos;
        Pos trackerPos;

        void Awake()
        {
            origin = GameObject.Find("Origin");
            tracker = GameObject.Find("Tracker");
        }

        // Start is called before the first frame update
        void Start()
        {
            newConnection = new UDP<PoseMessage>();
            newConnection.StartConnection("192.168.0.201", 5003, 50002);

            // instantiate Pos message
            originPos = new Pos();
            trackerPos = new Pos();

            // send position and rotation of origin
            GetPos(origin, originPos);
            newConnection.Send<Pos>(originPos);
        }

        unsafe void GetPos(GameObject g, Pos p)
        {
            p.position[0] = g.transform.position.Unity2Ros().x;
            p.position[1] = g.transform.position.Unity2Ros().y;

            p.delta= g.transform.rotation.Unity2Ros().eulerAngles.y;
        }

        // Update is called once per frame
        void Update()
        {
            // get position of gameobject and store it in Pos
            GetPos(tracker, trackerPos);

            // send position and rotation of tracker each frame
            newConnection.Send<Pos>(trackerPos);
        }


    }
}