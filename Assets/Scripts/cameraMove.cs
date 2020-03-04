using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    // Use this for initialization
    public Camera camera;
        void Start()
        {
        //camera=Gameobject.Find("Main Camera");
        //Camera mycam = GetComponent<Camera>();
    }

        // Update is called once per frame
        void FixedUpdate()
        {
            //float x = 5 * Input.GetAxis("Mouse X");
           // float y = 5 * -Input.GetAxis("Mouse Y");
            //camera.transform.Rotate(x, y, 0);
        // declare the RaycastHit variable
        //Camera mycam = GetComponent<Camera>();
        //transform.LookAt(mycam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane)), Vector3.up);
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));
    }
}
