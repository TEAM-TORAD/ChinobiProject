using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingNPCController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody RB;
    private Health health;

    // Public variables
    public AudioClip footstep, explosion;
    public float walkSpeed = 2, runSpeed = 5, detectionDistance = 3, reachedTargetDistance = 0.3f, explodeDistance = 0.5f, explosionRadius = 2.0f, explosionForce = 100.0f;
    public int explosionDamageValue = 20;
    public Transform[] patrolPoints;
    public Material deadMaterial;

    // Handled by logic
    [HideInInspector]
    public bool aware;
    private float velocity;
    private int patrolIndex = 0;
    [HideInInspector]
    public bool isAttacking = false, isTakingDamage = false;
    private Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(patrolPoints[patrolIndex].position);
        coll = transform.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        velocity = RB.velocity.magnitude;
        animator.SetBool("Moving", true);
        if (Input.GetKeyDown(KeyCode.B)) Die();
        if (!isTakingDamage && !isAttacking)
        {
            if (!aware)
            {
                agent.speed = walkSpeed;
                animator.speed = 0.75f;

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
                animator.speed = 1.0f;
                agent.SetDestination(player.position);

                if (DistanceToAgentTarget() < explodeDistance && !isAttacking)
                {
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                }
            }
        }

    }
    public void Explode()
    {
        //Debug.Log("Explosion started");
        Vector3 explosionCenter = coll.bounds.center;
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, explosionRadius);
        coll.enabled = false;
        //Debug.Log(transform.position + " " + explosionCenter);
        foreach (Collider c in colliders)
        {
            print("Collidername: " + c.transform.name + " Collider tag: " + c.transform.tag + " Collider is trigger: " + c.isTrigger + " Rigidbody null: " + c.attachedRigidbody);
            //Rigidbody rb = c.transform.GetComponent<Rigidbody>();
            if(!c.isTrigger)
            {

                Rigidbody rb = c.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, 1.0f, ForceMode.Impulse);
                    if (c.transform.CompareTag("Player"))
                    {
                        Debug.Log("Player hit by explosion!");
                        Health playerHealth = c.GetComponent<Health>();
                        Stamina playerStamina = c.GetComponent<Stamina>();
                        PlayerBlock playerBlock = c.GetComponent<PlayerBlock>();

                        if (playerStamina != null && playerBlock != null && playerHealth != null)
                        {
                            if (playerBlock.isBlocking)
                            {
                                playerStamina.StaminaDamage(explosionDamageValue);
                            }
                            else
                            {
                                playerHealth.TakeDamage(explosionDamageValue);
                            }
                        }
                        else Debug.LogError("Some scripts are missing. Make sure Health.cs, Stamina.cs and PlayerBlock.cs are attatched to the player!");

                    }
                    else if (c.transform.CompareTag("WaspNPC"))
                    {
                        //Effects on wasp
                        Debug.Log("wasp hit by explosion");
                        Health waspHealth = null;
                        if (c.GetComponent<Health>() != null) waspHealth = c.GetComponent<Health>();
                        else if (c.transform.parent.GetComponent<Health>() != null) waspHealth = c.transform.parent.GetComponent<Health>();
                        if(waspHealth != null) waspHealth.TakeDamage(explosionDamageValue * 2);
                    }
                    else if (c.transform.CompareTag("ExplodingNPC") && c.transform != transform)
                    {
                        ExplodingNPCController hitBombGuy =  c.transform.GetComponent<ExplodingNPCController>();
                        hitBombGuy.isAttacking = true;
                        hitBombGuy.animator.SetTrigger("Attack");
                    }
                }
            }
            
        }
        // Destroy the NPC after 2 seconds
        Destroy(transform.gameObject, 2);
    }
    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }
    public void Die()
    {
        if (!isAttacking)
        {
            float velocity = RB.velocity.magnitude;
            if (velocity < 2.5f) velocity = 2.5f;

            transform.GetComponentInChildren<SkinnedMeshRenderer>().material = deadMaterial;
            agent.enabled = false;
            animator.enabled = false;
            RB.constraints = RigidbodyConstraints.None;
            RB.AddForce(transform.forward * velocity, ForceMode.Impulse);
        }
    }
    public void TakeDamage(int value)
    {
        if(!isAttacking)
        {
            isTakingDamage = true;
            animator.SetTrigger("TakeDamage");
            agent.speed = 0.0f;
            health.TakeDamage(value);
        }
    }
    
}
