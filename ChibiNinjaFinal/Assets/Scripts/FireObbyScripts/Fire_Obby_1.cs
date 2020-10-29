using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Obby_1 : MonoBehaviour
{
    public float scaleSize;
    public float scaleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(2.136063f, scaleSize * Mathf.Sin(Time.time * scaleSpeed), 1.804361f);
    }
}
