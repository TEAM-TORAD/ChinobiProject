using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using DialogueEditor;

public class RacerNavMeshMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    //public LayerMask whatIsGround, whatIsPlayer;

    private Animator anim;
    //private Rigidbody rb;


    private TMP_Text text;
    private DisplayInteractions displayInteractions;

    public bool raceActive = false, active = true;
    public int waypointIndex = 0;
    public bool conversationOpen;
    public bool playerFinished = false;
    public RacerTarget firstRaceTarget;
    public float runSpeed = 5.0f;

    //waypoints
    [SerializeField]
    Transform[] waypoints;

    //To add multiple checkpoint destinations
   // private float distanceToDestination;
    public bool raceFinished;

    private void Awake()
    {

        text = transform.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        displayInteractions = GetComponent<DisplayInteractions>();
        anim = GetComponentInChildren<Animator>();
       // rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0;
        agent.destination = waypoints[waypointIndex].transform.position;
        //print("Distance to next target: " + DistanceToAgentTarget());
        displayInteractions.OnSpeachBubbleOpen.AddListener(ActivateQuestion);
        displayInteractions.OnSpeachBubbleClose.AddListener(EndConversation);
    }
    private void Update()
    {
        if(conversationOpen)
        {
            if(!raceActive)
            {
                if(Input.GetKeyDown(KeyCode.Y))
                {
                    text.text = "Alright! Get ready! \n Follow the orange targets!";
                    StartCoroutine(StartRace());
                }
                else if(Input.GetKeyDown(KeyCode.N))
                {
                    text.text = "To bad. I thought you where though.";
                }
            }
        }
        if(raceActive)
        {
            CheckDistance();
            if (!agent.pathPending) MoveToDestination();
        }
        anim.SetFloat("Blend", agent.velocity.magnitude);


    }
    private void LateUpdate()
    {
        if(raceActive && !raceFinished)
        {
            if (conversationOpen) ConversationManager.Instance.EndConversation();
            else if (displayInteractions.hintButtonPrefab.activeSelf) displayInteractions.hintButtonPrefab.SetActive(false);
        }
    }
    public void ActivateQuestion()
    {
        if (active)
        {
            if (!raceActive)
            {
                text.text = "Wanna race around the bay? \n Y or N";
                conversationOpen = true;
            }
            else
            {
                if (playerFinished) text.text = "Well... I guess you won...";
                else text.text = "Ha! You never stood a chance!";
            }
        }
    }
    public void EndConversation()
    {
        conversationOpen = false;
    }
    IEnumerator StartRace()
    {
        CursorScript.instance.eventStoppingPlayer = true;
        Economy.economy.InstantiateServerMessage("3", true);
        yield return new WaitForSeconds(1);
        Economy.economy.InstantiateServerMessage("2", true);
        yield return new WaitForSeconds(1);
        Economy.economy.InstantiateServerMessage("1", true);
        yield return new WaitForSeconds(1);
        Economy.economy.InstantiateServerMessage("GO!", true);
        CursorScript.instance.eventStoppingPlayer = false;
        raceActive = true;
        transform.GetComponent<NPCInteraction>().passive = false;
        firstRaceTarget.gameObject.SetActive(true);
    }
    private void MoveToDestination()
    {
        if (!raceFinished) agent.speed = runSpeed;
        else
        {
            agent.speed = 0;
        }
    }

    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }

    void CheckDistance()
    {
        if (DistanceToAgentTarget() < 1.75f)
        {
            //Debug.Log("Destination Reached");
            if ( waypointIndex < waypoints.Length - 1 )
            {
                waypointIndex += 1;
                agent.destination = waypoints[waypointIndex].transform.position;
            //    print("Distance to next target: " + DistanceToAgentTarget());
            }
            else
            {
                if(!raceFinished)
                {
                    // Race over
                    raceFinished = true;
                    transform.GetComponent<NPCInteraction>().passive = true;
                    if (playerFinished)
                    {
                        // NPC lost
                        Economy.economy.InstantiateServerMessage("Congrats! You won the race. Have some gold", true);
                        Economy.economy.AddGold(15);
                    }
                    else
                    {
                        //NPC won
                        Economy.economy.InstantiateServerMessage("Ough! You lost the race. A well, you can always race with the chickens.", true);
                    }
                }
                
            }
            
        }
    }

}
