using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollisions : MonoBehaviour
{
    
    void Start()
    {
        
        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(8, 11);
    }

   
}
