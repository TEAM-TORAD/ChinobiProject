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
    public bool lookAtPlayer, passive = true;
    public float rotationSpeed = 90.0f;
    public float angleToTarget;
    // Start is called before the first frame update
    void Awake()
    {
        if (transform.GetComponent<NPCInteraction>() != null) transform.GetComponent<NPCInteraction>().passive = passive;
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(!passive)
        {

        }
    }
    private void LookAt(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
        // Set animator based on the angle between the current; rotation and the target rotation
        if (Mathf.Abs(angle) > 5)
        {
            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 0.2f, 2 * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);
            Vector3 newRot = transform.rotation.eulerAngles;
            if (angle > 0) newRot.y -= Time.deltaTime * rotationSpeed;
            else newRot.y += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(newRot);
        }

        else
        {
            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 0.0f, 2* Time.deltaTime);
            animator.SetFloat("Blend",blendValue);
        }
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
