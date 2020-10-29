using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    
    public float speed = .1f;
    
    public GameObject hitParticle;
   
    public LayerMask playerMask;
    
    private float dist;
    private Vector3 target;
    private Vector3 lastPos;


    private bool collided;
    private void Awake()
    {
        collided = false;
        RayCheck();
        
       
    }
    private void Update()
    {

        if (!collided)
        {
            MoveProjectile();
        }
        
        
    }
    
    private void MoveProjectile()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        lastPos = transform.position;
        transform.LookAt(target);
    }
    private void RayCheck()
    {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit hit, 100, playerMask);
            target = hit.point;
            
            dist = hit.distance;
        
        Debug.Log("Hit distance = " + dist);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            collided = true;
            Destroy(gameObject.GetComponentInChildren<TrailRenderer>());
            //gameObject.transform.parent = collision.gameObject.transform;
            GameObject particle = Instantiate(hitParticle, lastPos, Quaternion.identity) as GameObject;
            Destroy(particle, 0.1f);
            Destroy(gameObject, 5);
        }
       


    }
    
}
