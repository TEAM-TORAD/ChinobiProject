using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool visited = false;
    public float recoverTime = 240;
    private float timer;
    [HideInInspector]
    public Transform landingTarget;

    private void Awake()
    {
        if (transform.Find("LandingTarget") != null) landingTarget = transform.Find("LandingTarget");
       // else print(transform.name + " doesn't have a landing target!");
    }
    // Update is called once per frame
    void Update()
    {
        if(visited)
        {
            timer += Time.deltaTime;
            if(timer >= recoverTime)
            {
                visited = false;
                timer = 0;
            }
        }
    }
}
