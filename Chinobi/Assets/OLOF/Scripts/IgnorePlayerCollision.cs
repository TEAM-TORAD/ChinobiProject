using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<Rigidbody>() != null)
        {
            GetComponentInChildren<Rigidbody>().detectCollisions = false;
            //Ignore the collisions between Player layer and Wasp layer
            Physics.IgnoreLayerCollision(8, 9);
        }
    }
}
