using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtParent : MonoBehaviour
{
    
    private void Awake()
    {
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}
