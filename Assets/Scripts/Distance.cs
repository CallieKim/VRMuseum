using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class Distance : MonoBehaviour
    {
        public GameObject tracker;
        public GameObject origin;
        Transform gameobject1;
        Transform gameobject2;

        // Start is called before the first frame update
        void Start()
        {
            tracker = GameObject.Find("Tracker");
            origin = GameObject.Find("Base1");
            gameobject1 = tracker.transform;
            gameobject2 = origin.transform;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(gameobject1.position - gameobject2.position);
            Debug.Log("Position : "+tracker.transform.position.Unity2Ros());

            Debug.Log("Rotation :" + tracker.transform.rotation.Unity2Ros().eulerAngles);
        }
    }
}