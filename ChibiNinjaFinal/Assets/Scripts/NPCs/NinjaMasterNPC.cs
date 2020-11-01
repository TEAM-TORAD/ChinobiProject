using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NinjaMasterNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody RB;
    private Animator animator;
    private Transform player;
    public Transform passiveLookAt;
    public bool lookAtPlayer, passive = true;
    public float rotationSpeed = 3.0f;
    public float angleToTarget;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer) {
            LookAt(player);
        }
        else
        {
            if (passive)
            {
                if (passiveLookAt != null) LookAt(passiveLookAt);
                else animator.SetFloat("Blend", 0.0f);
            }
        }
    }
    private void LookAt(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // Set animator based on the angle between the current rotation and the target rotation
        angleToTarget = Vector3.Angle(targetDirection, transform.forward);
        if (angleToTarget > 2) animator.SetFloat("Blend", 0.3f);
        else animator.SetFloat("Blend", 0.0f);

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.transform.CompareTag("Player"))
        {
            if (passive) lookAtPlayer = true;
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.transform.CompareTag("Player"))
        {
            if (passive) lookAtPlayer = false;
        }
    }
}
