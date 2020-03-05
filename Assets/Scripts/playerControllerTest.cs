using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerTest : MonoBehaviour
{
    public int speed = 5;
    Transform vrCamera;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        vrCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        //Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // get input data from keyboard or controller
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        // update player position based on input
        //Vector3 position = transform.position;
        //position.x += moveHorizontal * speed * Time.deltaTime;
        //position.z += moveVertical * speed * Time.deltaTime;
        //transform.position = position;

        Vector3 movement = speed * new Vector3(vrCamera.TransformDirection(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).x, 0f, vrCamera.TransformDirection(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).z);
        movement.y = rb.velocity.y;
        rb.velocity = movement;

    }
}
