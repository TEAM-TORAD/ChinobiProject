using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float timer;
  
    void Update()
    {
        timer += 1f * Time.deltaTime;
        if(timer > 34f)
        {
            Destroy(gameObject);
        }
    }
}
