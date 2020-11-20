using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NinjaStarShoot : MonoBehaviour
{
    public GameObject ninjaStar;
    public Transform hand;
    public float fireRate;
    float timeSinceLastThrow;
    public float smoothTransition = 0.1f;
    public int damage = 15;
    public float speed = 15.0f;
    public float destroyAfterSeconds = 5.0f;
    


    private void Update()
    {
        timeSinceLastThrow += Time.deltaTime;
    }

    public void ThrowNinjaStar()
    {
       
        if (timeSinceLastThrow > 1f / fireRate)
        {
            GameObject newStar = Instantiate(ninjaStar, hand.transform.position, Quaternion.LookRotation(Vector3.forward));
            newStar.transform.GetComponent<ProjectileScript>().SetStartValues(damage, speed, destroyAfterSeconds, true, null, transform.gameObject);
            timeSinceLastThrow = 0;
            print("Star thrown!");
        }
    }
}
