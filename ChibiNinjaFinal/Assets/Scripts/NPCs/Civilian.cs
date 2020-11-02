using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;

public class Civilian : MonoBehaviour
{
    public float rotationSpeed = 4.0f;
    public float walkSpeed = 2, runSpeed = 5, patrolPointReachedDistance = 0.7f;
    public NPCConversation conversation;
    public bool passive = true, autoStartConversation = true;
    public Transform[] patrolPoints;
    private bool playerClose, fleeing;
    private Animator animator;
    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody RB;
    private Quaternion startRotation;
    public bool overrideRun = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        else if (GetComponentInChildren<Animator>() != null) animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startRotation = transform.rotation;
        if(!passive)
        {
            if (patrolPoints.Length > 0)
            {
                passive = false;
                agent.SetDestination(patrolPoints[0].position);
            }
            else
            {
                passive = true;
                Debug.LogError(transform.name + " is set to not be passive, but has no patrol points to go to. Setting as passive.");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(passive)
        {
            if(playerClose)
            {
                LookAt(player);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if (!CursorScript.instance.conversationOpen)
                    {
                        ConversationManager.Instance.StartConversation(conversation);
                        CursorScript.instance.conversationOpen = true;
                    }
                    else
                    {
                        ConversationManager.Instance.EndConversation();
                        CursorScript.instance.conversationOpen = false;
                        Economy.economy.InstantiateServerMessage("Press 'E' to talk to Girl.", true);
                    }
                }
            }
            else
            {
                // Rotate towards the rotation the NPC had when the game started
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotationSpeed * 10 * Time.deltaTime);
            }
        }
        else
        {
            if(fleeing)
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
        //Only check velocity in x and z (Ignore up and down)
        //float velocity = Mathf.Pow(Mathf.Pow(RB.velocity.x, 2) + Mathf.Pow(RB.velocity.z, 2), 0.5f);
        float velocity = Mathf.Pow(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2), 0.5f);
        if (velocity <= walkSpeed)
        {
            float returnValue = (velocity / walkSpeed) * 0.5f;
            return returnValue;
        }
        else
        {
            float returnValue = ((velocity - walkSpeed) / (runSpeed - walkSpeed) * .5f) + .5f;
            return returnValue;
        }
    }
    private void LookAt(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;


        // Set animator based on the angle between the current rotation and the target rotation
        float angleToTarget = Vector3.Angle(targetDirection, transform.forward);
        if (angleToTarget > 3)
        {
            animator.SetFloat("Blend", 0.2f);
            print("angle to player greater than 3");
        }
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
    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("Player"))
        {
            playerClose = true;
            if(passive)
            {
                if(autoStartConversation && conversation != null && !CursorScript.instance.conversationOpen)
                {
                    ConversationManager.Instance.StartConversation(conversation);
                    CursorScript.instance.conversationOpen = true;
                }
                else if(!autoStartConversation && conversation != null && !CursorScript.instance.conversationOpen)
                {
                    Economy.economy.InstantiateServerMessage("Press 'E' to talk to Girl.", true);
                }
            }

            //play giggle sound

        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = false;


        }
    }
}
