using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FriendlyNPC : MonoBehaviour
{


    NavMeshAgent navMeshAgent;
    Animator animator;
    
    private Vector3 randomPosition;
    private Vector3 anchorPosition;

    [Header ("NPC Movement")]
    public float distanceRadius;
    public int minWaitTime;
    public int maxWaitTime;
    private int waitTime;

    private bool moveAction;

    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Nav mesh component not attached to" + gameObject.name);
        }

        anchorPosition = gameObject.transform.position;

       
        navMeshAgent.angularSpeed = 360;
        moveAction = true;

        
    }

    void Update()
    {
        navMeshAgent.SetDestination(randomPosition);

        if (moveAction)
        {
            StartCoroutine(MoveNPC());
        }
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    IEnumerator MoveNPC()
    {
        moveAction = false;

        GenerateRandom();

        yield return new WaitForSeconds(waitTime);

        moveAction = true;
    }

    void GenerateRandom()
    {
        float x = Random.Range(-distanceRadius, distanceRadius);
        float z = Random.Range(-distanceRadius, distanceRadius);

        Vector3 newRandomPosition = new Vector3(x, anchorPosition.y, z);
        randomPosition = newRandomPosition;

        int newWaitTime = Random.Range(minWaitTime, maxWaitTime);
        waitTime = newWaitTime;

        //print("random position = " + randomPosition);
        //print("wait time = " + waitTime);
    }
}