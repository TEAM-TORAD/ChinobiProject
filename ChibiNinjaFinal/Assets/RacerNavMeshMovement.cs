using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

public class RacerNavMeshMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    //public LayerMask whatIsGround, whatIsPlayer;

    private Animator anim;
    private Rigidbody rb;

    public bool active = false;

    //waypoints
    [SerializeField]
    Transform[] waypoints;
    public int waypointIndex = 0;

    //To add multiple checkpoint destinations
    private float distanceToDestination;
    public bool raceFinished;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0;
        agent.destination = waypoints[waypointIndex].transform.position;
        print("Distance to next target: " + DistanceToAgentTarget());
    }
    private void Update()
    {
        if(active)
        {
            CheckDistance();
            if (!agent.pathPending) MoveToDestination();
            else print("Path is penging");
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
        

    }
    private void MoveToDestination()
    {
        if (!raceFinished) agent.speed = 6.5f;
        else
        {
            agent.speed = 0;

        }
        //waypoints
        /*transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, agent.speed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }

        if (waypointIndex == waypoints.Length)
        {
            destinationReached = true;
        }*/
        //agent.SetDestination(endPoint.position);

        //FaceDestination();
    }

    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }

    private void FaceDestination()
    {
        Vector3 relativePos = waypoints[waypointIndex].transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
    }

    void CheckDistance()
    {
        if (DistanceToAgentTarget() < 1.0f)
        {
            Debug.Log("Destination Reached");
            if ( waypointIndex < waypoints.Length - 1 )
            {
                waypointIndex += 1;
                agent.destination = waypoints[waypointIndex].transform.position;
                print("Distance to next target: " + DistanceToAgentTarget());
            }
            else
            {
                // Race over
                print("Completed the race. I assume you lost.");
                raceFinished = true;
            }
            
        }
    }

}
