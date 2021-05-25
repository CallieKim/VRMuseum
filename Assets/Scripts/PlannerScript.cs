using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerScript : MonoBehaviour
{



    public GameObject colliderCubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*var rightController = GameObject.Find("Controller (right)");
        var leftController = GameObject.Find("Controller (left)");
        var cube_size = new Vector3(50, 200, 200);

        
        var extension1 = Instantiate(colliderCubePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1), rightController.transform);*/
        //extension1.name = "extension1";
        //extension1.transform.localScale = cube_size;
        //extension1.transform.position = new Vector3(-10.6f / 1000f, 0, 0f / 1000f);
        


        /* Bat.transform.parent = rightController.transform;

         cube_size = new Vector3(50, 200, 200);

         Bat.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
         var cube1 = Instantiate(colliderCubePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1), Bat.transform);
         cube1.name = "cube1";
         cube1.transform.localScale = cube_size;
         cube1.transform.position = new Vector3(-10.6f / 1000f, 0, 0f / 1000f);
         var cube1_box_collider = cube1.GetComponent<BoxCollider>();
         cube1_box_collider.material = bounceMaterial;
         var cube1_rb = cube1.AddComponent<Rigidbody>();
         cube1_rb.isKinematic = true;
         cube1_rb.useGravity = true;*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
