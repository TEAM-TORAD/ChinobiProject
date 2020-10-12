using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaspNPCScript : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody RB;
    private Health health;

    private AudioSource audio;

    // Public variables
    public float walkSpeed = 2, runSpeed = 5, detectionDistance = 3, reachedTargetDistance = 0.3f, hooverDistanceMax = 2.5f, hooverDistanceMin = 1.5f;
    public int explosionDamageValue = 20;
    public Transform[] patrolPoints;

    // Handled by logic
    [HideInInspector]
    public bool aware;
    [HideInInspector]
    public bool alive = true;
    private float velocity;
    private int patrolIndex = 0;
    //[HideInInspector]
    public bool isAttacking = false, isTakingDamage = false;
    private Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponentInChildren<Rigidbody>();
        audio = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        coll = transform.GetComponentInChildren<MeshCollider>();

        // Set destination to the first patrol point in the array
        agent.SetDestination(patrolPoints[patrolIndex].position);

    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            velocity = RB.velocity.magnitude;
            if (Input.GetKeyDown(KeyCode.B)) Die();
            if (!isTakingDamage && !isAttacking)
            {
                if (!aware)
                {
                    agent.speed = walkSpeed;

                    // If the agent has reached the patrol point
                    if (DistanceToAgentTarget() < reachedTargetDistance)
                    {
                        if (patrolIndex + 1 < patrolPoints.Length) ++patrolIndex;
                        else patrolIndex = 0;
                        agent.SetDestination(patrolPoints[patrolIndex].position);
                    }
                    // If agent is close enough to the player to discover them
                    if (Vector3.Distance(transform.position, player.position) < detectionDistance)
                    {
                        aware = true;
                    }
                }
                // Agent is aware
                else
                {
                    agent.speed = runSpeed;
                    agent.SetDestination(player.position);

                    if (DistanceToAgentTarget() < reachedTargetDistance && !isAttacking)
                    {
                        isAttacking = true;
                        animator.SetTrigger("Attack");
                    }
                }
            }
        }
    }
    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }
    public void Die()
    {
        alive = false;
        animator.SetTrigger("Die");
        agent.enabled = false;
        RB.constraints = RigidbodyConstraints.None;
        RB.isKinematic = false;
        RB.useGravity = true;
        //RB.AddForce(transform.forward * velocity, ForceMode.Impulse);
        transform.GetComponentInParent<Collider>().enabled = false;
    }
    public void TakeDamage(int value)
    {
        if (!isAttacking)
        {
            isTakingDamage = true;
            //animator.SetTrigger("TakeDamage");
            agent.speed = 0.0f;
            health.TakeDamage(value);
        }
    }
}
