using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public GameObject player;
    public Transform target;

    public Rigidbody rb;

    public float distance;
    public float minDistance;
    public float maxDistance;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Find Target
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        CheckTarget();
    }
    //Simple is target visible or in range
    private void CheckTarget()
    {
        if(distance < minDistance)
        {
            MoveToTarget();
        }
        if (distance > maxDistance) ;
        {
            //don't see player
        }
    }

    private void MoveToTarget()
    {
        Vector3 moveToward = target.position - transform.position; //Used to face the AI in the direction of the target
        //Vector3 moveAway = transform.position - target.position; //Used to face the AI away from the target when running away
        distance = Vector3.Distance(transform.position, target.position);
        transform.LookAt(target);
        rb.MovePosition(moveToward * Time.deltaTime); //Move the AI towards target
    }

    //private void MoveAwayFromTarget()
   // {

   // }
    //Rotate Towards Target
    //Move Towards Target
    //Attack Target
}
