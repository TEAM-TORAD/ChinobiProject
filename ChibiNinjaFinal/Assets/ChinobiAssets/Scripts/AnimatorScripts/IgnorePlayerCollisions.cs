using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollisions : MonoBehaviour
{
    
    void Start()
    {
        Physics.IgnoreLayerCollision(2, 8);
        Physics.IgnoreLayerCollision(2, 10);
    }

   
}
