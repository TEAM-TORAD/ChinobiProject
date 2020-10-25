using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.XR;

public class NinjaStarShoot : MonoBehaviour
{
    public GameObject ninjaStar;
    public Transform hand;
    public float fireRate;
    float timeSinceLastThrow;
    public float smoothTransition = 0.1f;
    


    private void Update()
    {
        timeSinceLastThrow += Time.deltaTime;
    }

    public void ThrowNinjaStar()
    {
       
        if (timeSinceLastThrow > 1f / fireRate)
        {
            Instantiate(ninjaStar, hand.transform.position, Quaternion.LookRotation(Vector3.forward));
            timeSinceLastThrow = 0;
        }
    }
}
