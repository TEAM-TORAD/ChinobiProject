using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HostileNPC : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    Animator animator;
    GameObject player;

    private Vector3 randomPosition;
    private Vector3 anchorPosition;

    [Header("NPC Attributes")]
    public int health;

    [Header("NPC Movement")]
    [Tooltip("Distance NPC can roam from placement")]
    public float distanceRadius;
    public float npcSpeed;
    public int minWaitTime;
    public int maxWaitTime;
    private int waitTime;

    [Header ("Attack Variables")]
    [Tooltip("Distance NPC will aggro to player")]
    public float aggroRange;
    private float aggroDistance;
    [Tooltip("Time between attacks")]
    public float attackInterval;

    private bool attacking;
    private bool attackAction;
    private bool moveAction;
    
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        anchorPosition = gameObject.transform.position;

        navMeshAgent.speed = npcSpeed;
        navMeshAgent.angularSpeed = 360;
        navMeshAgent.stoppingDistance = 1.5f;
        moveAction = true;
        attacking = false;
        attackAction = false;
    }

    void Update()
    {
        if (!attacking)
        {
            navMeshAgent.SetDestination(randomPosition);

            if (moveAction)
            {
                StartCoroutine(MoveNPC());
            }
        }
        
        if (attacking)
        {
            navMeshAgent.SetDestination(player.transform.position);

            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);

            if (attackAction)
            {
                StartCoroutine(AttackMode());
                StartCoroutine(MoveNPC());
            }
        }

        Animations();
        DistanceCalculations();
        Debug.Log(aggroDistance);
    }

    IEnumerator AttackMode()
    {
        attackAction = false;

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(1);

        animator.SetBool("Attack", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(attackInterval - 1);

        attackAction = true;
    }

    IEnumerator MoveNPC()
    {
        moveAction = false;

        GenerateRandom();

        yield return new WaitForSeconds(waitTime);

        moveAction = true;
    }

    void Animations()
    {
        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("Walk", true);
        }

        if (navMeshAgent.velocity == Vector3.zero)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
        }
    }

    void DistanceCalculations()
    {
        aggroDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (aggroDistance <= aggroRange && attacking == false)
        {
            attackAction = true;
            attacking = true;
        }

        else if (aggroDistance >= aggroRange && attacking == true)
        {
            attacking = false;
            moveAction = true;
        }
    }

    void GenerateRandom()
    {
        float x = Random.Range(-distanceRadius, distanceRadius);
        float z = Random.Range(-distanceRadius, distanceRadius);

        Vector3 newRandomPosition = new Vector3(x, anchorPosition.y, z);
        randomPosition = newRandomPosition;

        int newWaitTime = Random.Range(minWaitTime, maxWaitTime);
        waitTime = newWaitTime;

        print("random position = " + randomPosition);
        print("wait time = " + waitTime);
    }
}