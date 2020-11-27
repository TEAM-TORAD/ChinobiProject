using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;

public class Civilian : MonoBehaviour
{
    public float rotationSpeed = 120.0f;
    public float walkSpeed = 2, runSpeed = 5, patrolPointReachedDistance = 0.7f;
    //public NPCConversation conversation;
    public bool passive = true;
    public Transform[] patrolPoints;
    private bool playerClose, fleeing;
    private Animator animator;
    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody RB;
    public bool overrideRun = false;
    public float velocityPerSecond = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        else if (GetComponentInChildren<Animator>() != null) animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (transform.GetComponent<NPCInteraction>() != null) transform.GetComponent<NPCInteraction>().passive = passive;
        if (!passive)
        {
            if (patrolPoints.Length > 0)
            {
                passive = false;
                agent.SetDestination(patrolPoints[0].position);
            }
            else
            {
                passive = true; 
                if (transform.GetComponent<NPCInteraction>() != null) transform.GetComponent<NPCInteraction>().passive = true;
                Debug.LogError(transform.name + " is set to NOT be passive, but it doesn't have any patrol points to go to. Passive bool will be set to true to avoid breaking the game.");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!passive)
        {
            if (fleeing)
            {
                agent.speed = runSpeed;
            }
            else
            {
                agent.speed = walkSpeed;
                if (DistanceToAgentTarget() <= patrolPointReachedDistance)
                {
                    int randomDestination = Random.Range(0, (patrolPoints.Length - 1));
                    agent.destination = patrolPoints[randomDestination].position;
                }
            }
            animator.SetFloat("Blend", LocomotionBlendValue());
        }
    }
    float LocomotionBlendValue()
    {
        // Works with blend with 3 stages (idle, walk and run) 
        velocityPerSecond = Mathf.Lerp(velocityPerSecond, agent.speed, 0.4f);
        if (velocityPerSecond <= walkSpeed)
        {
            float returnValue = (velocityPerSecond / walkSpeed) * 0.5f;
            return returnValue;
        }
        else
        {
            float returnValue = ((velocityPerSecond - walkSpeed) / (runSpeed - walkSpeed) * .5f) + .5f;
            return returnValue;
        }
    }
    
    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }
    
}
