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
    public Transform[] patrolPoints;
    public Collider aliveCollider, deadCollider;

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


    private float deathTimer;

    public float detectionAngle = 35.0f;

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
        aliveCollider.enabled = true;
        deadCollider.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            velocity = RB.velocity.magnitude;
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
                    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                    if (distanceToPlayer < detectionDistance)
                    {
                        //Check the angle between the player and the wasp's forward position
                        Vector3 targetPos = player.position - transform.position;
                        float angle = Vector3.Angle(targetPos, transform.forward);
                        // If the angle is less than the detection-angle set in the inspector, detect the player
                        if(angle < detectionAngle )
                        {
                            LayerMask layerMask = 1 >> LayerMask.GetMask("Enemy");
                            RaycastHit hit;
                            // Does the ray intersect any objects excluding the player layer
                            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, detectionDistance, layerMask))
                            {
                                if (hit.transform.CompareTag("Player"))
                                {
                                    Debug.Log("Did Hit Player");
                                    aware = true;
                                }
                                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                                else
                                {
                                    Debug.Log("Did Hit " + hit.transform.tag);
                                }

                            }
                        }
                        //If the player is less than 1 meter away, the wasp will detect them even if the angle is greater than the detection angle
                        else if(distanceToPlayer < 1) aware = true;

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
        else
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= 3.0f) DropItem();
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
        aliveCollider.enabled = false;
        deadCollider.enabled = true;

    }
    void DropItem()
    {
        RandomReward RR = RandomReward.RR;

        int num = Random.Range(0, RR.itemDrops.Count);
        Instantiate(RR.itemDrops[num], transform.position, transform.rotation);
        Destroy(transform.gameObject);
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
