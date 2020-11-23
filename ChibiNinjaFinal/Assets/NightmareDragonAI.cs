using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.AI;

public class NightmareDragonAI : MonoBehaviour
{

    private NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, rangedAttackRange;
    public bool playerInSightRange, playerInAttackRange, playerInRangedAttackRange, playerInSight;

    //Timers
    public float chaseTime;
    public float patrolTime;

    public float rbSpeed;

    //Settings
    public float walkSpeed;
    public float runSpeed;
    public float maxChaseTime;
 
    private Animator anim;
    private Rigidbody rb;
    private Health health;

    public Collider[] aliveColliders;
    public Collider[] deadColliders;

    public bool enemyCanFly;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        health.onTakeDamage.AddListener(TakeDamage);
        health.onDeath.AddListener(Die);
        aliveColliders = GetComponents<Collider>();
        deadColliders = transform.Find("Root_Pelvis").GetComponents<Collider>();
    }
    public void SetColliders(Collider[] colliders, bool value)
    {
        foreach(Collider c in colliders)
        {
            c.enabled = value;
        }
    }
    public void Update()
    {
        if(health.alive)
        {
            rbSpeed = agent.velocity.magnitude;
            anim.SetFloat("Speed", rbSpeed);

            SightChecker();
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            playerInRangedAttackRange = Physics.CheckSphere(transform.position, rangedAttackRange, whatIsPlayer);

            if (playerInSight)
            {
                chaseTime = 0;
                if (playerInAttackRange) MeleeAttack();
                else if (playerInRangedAttackRange) RangedAttack();
                else ChasePlayer();
            }
            else if (chaseTime <= maxChaseTime)
            {
                ChasePlayer();
            }
            else
            {
                Patroling();
            }
        }
        
    }
    public void Die()
    {
        anim.SetBool("Alive", false);
        anim.SetTrigger("TakeDamage");
        agent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        //rb.constraints = RigidbodyConstraints.None;
        SetColliders(aliveColliders, false);
        SetColliders(deadColliders, true);
    }
    public void TakeDamage()
    {
        anim.SetTrigger("TakeDamage");
    }
    private void Patroling()
    {
        patrolTime += Time.deltaTime;
        agent.speed = walkSpeed;

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f) walkPointSet = false;     
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        //Checks that the point is valid walkpoint
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        chaseTime += Time.deltaTime;
        patrolTime = 0;
        agent.SetDestination(player.position);
       
        TurnToLookAt();
        agent.speed = runSpeed;

        if (chaseTime >= maxChaseTime) EnemyFrustrated();
    }
    private void EnemyFrustrated()
    {
        agent.SetDestination(transform.position);
        TurnToLookAt();
        //if (enemyCanFly) Invoke(nameof(RangedAttack), 1f);
        //else 
        anim.SetTrigger("Scream");
    }
    private void MeleeAttack()
    {
        //Make sure enemy doesn't move
        TurnToLookAt();
        agent.SetDestination(transform.position);
        anim.SetTrigger("Attack");
        
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void RangedAttack()
    {
        agent.SetDestination(player.position);
        TurnToLookAt();
        anim.SetTrigger("RangedAttack");
        anim.SetBool("TargetInRange", true);

        // Add projectile effect here

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void SightChecker()
    {
        Vector3 targetDir = player.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        
        if (angleToPlayer >= -60 && angleToPlayer <= 60 && playerInSightRange) // 120° FOV
        {
            playerInSight = true;
        }
        else
        {
            playerInSight = false;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        anim.ResetTrigger("Attack");
        anim.ResetTrigger("RangedAttack");
    }

    private void TurnToLookAt()
    {
        Vector3 relativePos = player.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 2 * Time.deltaTime);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
    }
    
}
