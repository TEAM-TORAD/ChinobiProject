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
    private Transform rayTarget, rayOrigin;
    private bool hasExploded;

    // Public variables

    [Tooltip("The speed when traveling between patrol points.")]
    public float walkSpeed = 2;
    [Tooltip("The speed the NPC will travel when it has detected the player")]
    public float runSpeed = 5;
    [Tooltip("The max distance the NPC can detect the player")]
    public float detectionDistance = 10;
    [Tooltip("The distance to the patrol point in x and z that the NPC will travel before considering the target to be reached.")]
    public float reachedTargetDistance = 0.3f;
    [Tooltip("The distance to the player the aware NPC will travel before attacking.")]
    public float explodeDistance = 0.5f;
    [Tooltip("The radius from the NPC that will be affected by the explossion")]
    public float explosionRadius = 2.0f;
    [Tooltip("The force of the explosion. The further away from the center of the explosion the less force will be added to rigidbodies within the explosion radius.")]
    public float explosionForce = 100.0f;
    [Tooltip("The damage the player will recieve from the explosion")]
    public int explosionDamageValue = 20;
    [Tooltip("The material that will be set when the NPC dies (unless it dies from exploding).")]
    public Material deadMaterial;
    [Tooltip("The angle in which the NPC can detect the player. If the player is within the detection distance it will check the angle. Last step a raycast will check if something is between the NPC and the player")]
    public float detectionAngle = 35.0f;
    [Tooltip("The patrol points the NPC will travel between. Starts at the first, goes to the second until last element in the array. Then it will go back to the first element in the array.")]
    public Transform[] patrolPoints;

    // Handled by logic
    [HideInInspector]
    public bool aware;
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
        rayTarget = player.transform.Find("EnemyRayTarget");
        rayOrigin = transform.Find("RaycastOrigin");
        agent.SetDestination(patrolPoints[patrolIndex].position);
        coll = transform.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Moving", true);
        //if (Input.GetKeyDown(KeyCode.B)) Die();
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
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer < detectionDistance)
                {
                    //Check the angle between the player and the wasp's forward position
                    Vector3 targetPos = rayTarget.position - rayOrigin.position;
                    float angle = Vector3.Angle(targetPos, rayOrigin.forward);
                    // If the angle is less than the detection-angle set in the inspector, detect the player
                    if (angle < detectionAngle)
                    {

                        //Debug.DrawRay(rayOrigin.position, targetPos, Color.green);

                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        if (Physics.Raycast(rayOrigin.position, targetPos, out hit, Mathf.Infinity))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                aware = true;
                            }
                            else
                            {
                                //Debug.Log("Did Hit tag: " + hit.transform.tag + " name: " + hit.transform.name );
                            }
                        }
                    }
                    //If the player is less than 1 meter away, the wasp will detect them even if the angle is greater than the detection angle
                    else if (distanceToPlayer < 1) aware = true;
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
        hasExploded = true;
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
                        PlayerInputs playerInputs = c.GetComponent<PlayerInputs>();

                        if (playerStamina != null && playerInputs != null && playerHealth != null)
                        {
                            if (playerInputs.isHoldingBlock)
                            {
                                playerStamina.StaminaDamage(explosionDamageValue);
                            }
                            else
                            {
                                playerHealth.TakeDamage(explosionDamageValue);
                            }
                        }
                        else Debug.LogError("Some scripts are missing. Make sure Health.cs, Stamina.cs and PlayerInputs.cs are attatched to the player!");

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
        if (!hasExploded)
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
