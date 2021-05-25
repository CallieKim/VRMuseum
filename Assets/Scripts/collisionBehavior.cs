using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionBehavior : MonoBehaviour
{

    //public GameObject extension_left;

    public GameObject locator;

    public bool in_collision = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Matrix4x4 Transform(GameObject Parent, Vector3 trans, Quaternion rotn)
    {
        var WorldToParent = Matrix4x4.TRS(Parent.transform.position, Parent.transform.rotation, new Vector3(1, 1, 1));
        GameObject empty = new GameObject();
        Transform ParentChild = empty.transform;

        ParentChild.position = trans;
        ParentChild.rotation = rotn;
        var ParentToChildMatrix = Matrix4x4.TRS(trans, rotn, new Vector3(1, 1, 1));

        var WorldToChild = WorldToParent * ParentToChildMatrix;
        UnityEngine.Object.Destroy(empty);

        return WorldToChild;
    }

    public static Vector3 ExtractPosition(Matrix4x4 M)
    {
        Vector3 position;
        position.x = M.m03;
        position.y = M.m13;
        position.z = M.m23;
        return position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Contains("extension"))
        {
            var posn = collision.gameObject.transform.position;              ///////////SHIFT AHEAD
            Debug.Log("old " + posn);

            posn=ExtractPosition(Transform(collision.gameObject, new Vector3(0, 0, 0.65f/2), new Quaternion(0, 0, 0, 1)));




            Debug.Log("new " + posn);

            in_collision = true;

            var my_collider = gameObject.GetComponent<Collider>();
            var closest_point = my_collider.ClosestPoint(posn);

            Debug.Log("ENTER:" + gameObject.name + "   " + collision.gameObject.name + closest_point );
            locator.transform.position = closest_point;
            locator.transform.rotation = gameObject.transform.rotation;
        }

        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name.Contains("extension"))
        {
            var posn = collision.gameObject.transform.position;
            Debug.Log("old " + posn);
            posn = ExtractPosition(Transform(collision.gameObject, new Vector3(0, 0, 0.65f/2), collision.gameObject.transform.rotation));
            Debug.Log("new " + posn);
            in_collision = true;
            var my_collider = gameObject.GetComponent<Collider>();
            var closest_point = my_collider.ClosestPoint(posn);



            Debug.Log("STAY:" + gameObject.name + "   " + collision.gameObject.name + closest_point);
            locator.transform.position = closest_point;
            locator.transform.rotation = gameObject.transform.rotation;
        }

    }



    private void OnTriggerExit(Collider collision)
    {
        in_collision = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
