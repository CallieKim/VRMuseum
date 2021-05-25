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
            public fixed float position[3];
            public fixed float rotation[4];
            public int i;
            //public char c;
        }

        public UDP<PoseMessage> newConnection;
        GameObject origin;
        // Tracker is Device 4
        public GameObject tracker;
        public GameObject tracker2;
        public GameObject lController;
        public GameObject rController;
        public GameObject hmd;
        public GameObject trackerWrist;
        public GameObject trackerShoulder;


        //public Valve.VR.SteamVR_TrackedObject lControllerTrackedObjectScript;
        //public Valve.VR.SteamVR_TrackedObject rControllerTrackedObjectScript;
        public Valve.VR.SteamVR_TrackedObject trackerTrackedObjectScript;
        public Valve.VR.SteamVR_TrackedObject tracker2TrackedObjectScript;
        public Valve.VR.SteamVR_TrackedObject hmdTrackedObjectScript;


        Pos originPos;
        Pos trackerPos;
        Pos tracker2Pos;
        Pos lPos;
        Pos rPos;
        Pos hPos;
        Pos collisionObjectPosition;
        Pos trackerWristPos;
        Pos trackerShoulderPos;


        Pos prev_trackerPos;
        Pos prev_tracker2Pos;
        Pos prev_lPos;
        Pos prev_rPos;
        Pos prev_hPos;
        Pos prev_trackerWristPos;
        Pos prev_trackerShoulderPos;

        float prev_time_hPos = 0;
        float prev_time_trackerPos = 0;
        float prev_time_tracker2Pos = 0;
        float prev_time_lPos = 0;
        float prev_time_rPos = 0;
        float prev_time_trackerWristPos = 0;
        float prev_time_trackerShoulderPos = 0;




        public GameObject locator;

        void Awake()
        {
            //origin = GameObject.Find("Origin");
            //tracker = GameObject.Find("Controller (left)");
            //tracker = GameObject.Find("Controller (left)");
        }

        // Start is called before the first frame update
        void Start()
        {

            float m = (float)Time.deltaTime;
            
            
            
            newConnection = new UDP<PoseMessage>();
            newConnection.StartConnection("192.168.0.150", 60010, 6020);      //   .43.79

            //StartCoroutine(ExecuteAfterTime(10));
            // instantiate Pos message
            //originPos = new Pos();
            trackerPos = new Pos();
            tracker2Pos = new Pos();
            trackerWristPos = new Pos();
            trackerShoulderPos = new Pos();


            prev_hPos = new Pos();
            prev_lPos = new Pos();
            prev_rPos = new Pos();
            prev_tracker2Pos = new Pos();
            prev_trackerPos = new Pos();
            prev_trackerWristPos = new Pos();
            prev_trackerShoulderPos = new Pos();

            trackerPos.i = 3;
            tracker2Pos.i = 4;
            lPos = new Pos();
            lPos.i = 1;
            rPos = new Pos();
            rPos.i = 2;
            hPos = new Pos();
            hPos.i = 0;
            collisionObjectPosition = new Pos();
            collisionObjectPosition.i = 5;

            // send position and rotation of origin
            //GetPos(origin, originPos);
            //newConnection.Send<Pos>(originPos);



           
          
            


            lPos = GetPos(lController, lPos);
            rPos = GetPos(rController, rPos);
            hPos = GetPos(hmd, hPos);
            trackerPos = GetPos(tracker, trackerPos);
            tracker2Pos = GetPos(tracker2, tracker2Pos);


            prev_lPos = GetPos(lController, prev_lPos);
            prev_rPos = GetPos(rController, prev_rPos);
            prev_hPos = GetPos(hmd, prev_hPos);
            prev_trackerPos = GetPos(tracker, prev_trackerPos);
            prev_tracker2Pos = GetPos(tracker2, prev_tracker2Pos);


            prev_time_hPos = Time.time;
            prev_time_trackerPos = Time.time;
            prev_time_tracker2Pos = Time.time;
            prev_time_lPos = Time.time;
            prev_time_rPos = Time.time;




        }

        //IEnumerator ExecuteAfterTime(float time)
        //{
        //    yield return new WaitForSeconds(time);
        //}

        unsafe Pos GetPos(GameObject g, Pos p)
        {
            p.position[0] = g.transform.position.Unity2Ros().x;
            //p.position[0] = g.transform.position.x;
            p.position[1] = g.transform.position.Unity2Ros().y;
            p.position[2] = g.transform.position.Unity2Ros().z;

            p.rotation[0]= g.transform.rotation.Unity2Ros().x;
            p.rotation[1] = g.transform.rotation.Unity2Ros().y;
            p.rotation[2] = g.transform.rotation.Unity2Ros().z;
            p.rotation[3] = g.transform.rotation.Unity2Ros().w;

            //p.c = g.gameObject.tag.ToCharArray()[0];
            //Debug.Log(p.i);

            //p.i = 

            return p;
        }


        float EulerDistance(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.x - p2.x,2) + Math.Pow(p1.y - p2.y,2) + Math.Pow(p1.z - p2.z,2));
        }

        unsafe float EulerDistance(Pos p1, Pos p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.position[0] - p2.position[0], 2) + Math.Pow(p1.position[1] - p2.position[1], 2) + Math.Pow(p1.position[2] - p2.position[2], 2));
        }

        unsafe Pos DeepCopy(Pos p1,Pos p2)
        {
            p2.position[0] = p1.position[0];
            p2.position[1] = p1.position[1];
            p2.position[2]= p1.position[2];


            p2.rotation[0] = p1.rotation[0];
            p2.rotation[1] = p1.rotation[1];
            p2.rotation[2] = p1.rotation[2];
            p2.rotation[3] = p1.rotation[3];
            return p2;



        }

        // Update is called once per frame
        unsafe void Update()
        {
            // get position of gameobject and store it in Pos


            //Debug.Log(EulerDistance(tracker.transform.position, tracker2.transform.position));

            if ( true)
            {
                hPos = GetPos(hmd, hPos);
               
                //if (EulerDistance(prev_hPos, hPos) < 0.5     ||   Time.time- prev_time_hPos>2)
                {
                    newConnection.Send<Pos>(hPos);
                    prev_hPos = DeepCopy(prev_hPos, hPos);
                    hmdTrackedObjectScript.isUpdated = false;
                    //Debug.Log("Sent HMD");
                    prev_time_hPos = Time.time;
                }
                
            }

            if (true)
            {
                trackerPos = GetPos(tracker, trackerPos);
                //if (EulerDistance(prev_trackerPos, trackerPos) < 0.5 || Time.time - prev_time_trackerPos > 2)
                {
                    newConnection.Send<Pos>(trackerPos);
                    prev_trackerPos = DeepCopy(prev_trackerPos, trackerPos);
                    trackerTrackedObjectScript.isUpdated = false;
                    //Debug.Log("Sent Tracker");
                    prev_time_trackerPos = Time.time;
                }
            }



            if (true)
            {
                tracker2Pos = GetPos(tracker2, tracker2Pos);
                //if (EulerDistance(prev_tracker2Pos, tracker2Pos) < 0.5 || Time.time - prev_time_tracker2Pos > 2)
                {
                    newConnection.Send<Pos>(tracker2Pos);
                    prev_tracker2Pos = DeepCopy(prev_tracker2Pos, tracker2Pos);
                    tracker2TrackedObjectScript.isUpdated = false;
                    prev_time_tracker2Pos = Time.time;
                }
                //Debug.Log("Sent Tracker");
            }
            // send Pos message

            //if (lControllerTrackedObjectScript.isUpdated)
            {
                lPos = GetPos(lController, lPos);
                //if (EulerDistance(prev_lPos, lPos) < 0.5 || Time.time - prev_time_lPos > 2)
                {
                    newConnection.Send<Pos>(lPos);
                    prev_lPos = DeepCopy(prev_lPos, lPos);
                    prev_time_lPos = Time.time;
                    //lControllerTrackedObjectScript.isUpdated = false;
                }
            }



            //if (rControllerTrackedObjectScript.isUpdated)
            {
                rPos = GetPos(rController, rPos);
                //if (EulerDistance(prev_rPos, rPos) < 0.5 || Time.time - prev_time_rPos > 2)
                {
                    newConnection.Send<Pos>(rPos);
                    prev_rPos = DeepCopy(prev_rPos, rPos);
                    prev_time_rPos = Time.time;
                    //rControllerTrackedObjectScript.isUpdated = false;
                }
            }





            var allCollisionBehaviors = FindObjectsOfType<collisionBehavior>();
            bool collision_detected = false;
            foreach (var cb in allCollisionBehaviors)
            {
                
                if (cb.in_collision)
                {
                    Debug.Log("In collision with " + cb.gameObject.name + " and " + cb.gameObject.name);
                    collisionObjectPosition.position[0] = locator.transform.position.Unity2Ros().x;//g.transform.position.Unity2Ros().x;
                    collisionObjectPosition.position[1] = locator.transform.position.Unity2Ros().y;
                    collisionObjectPosition.position[2] = locator.transform.position.Unity2Ros().z;


                    collisionObjectPosition.rotation[0] = locator.transform.rotation.Unity2Ros().x;
                    collisionObjectPosition.rotation[1] = locator.transform.rotation.Unity2Ros().y;
                    collisionObjectPosition.rotation[2] = locator.transform.rotation.Unity2Ros().z;
                    collisionObjectPosition.rotation[3] = locator.transform.rotation.Unity2Ros().w;

                    collision_detected = true;

                }
            }
            var dist = EulerDistance(trackerWrist.transform.position, trackerShoulder.transform.position);
            
           /* if( (dist < 0.15 || dist > 0.35))
            {
                collisionObjectPosition.position[0] = -500f;
                collisionObjectPosition.position[1] = -500f;
                collisionObjectPosition.position[2] = -500f;
                Debug.Log("One of the shoulder or wrist tracker has lost control:"+dist);
            }
            
            
            else*/ if (!collision_detected      )
            {
                collisionObjectPosition.position[0] =-100f;
                collisionObjectPosition.position[1] = -100f;
                collisionObjectPosition.position[2] = -100f;
            }

            newConnection.Send<Pos>(collisionObjectPosition);



        }

        void OnApplicationQuit()
        {
            newConnection.Stop();
        }


    }
}