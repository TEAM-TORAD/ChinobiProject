using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

public class RacerNavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    //public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;
    public Rigidbody rb;

    //waypoints
    [SerializeField]
    Transform[] waypoints;
    int waypointIndex = 0;

    //To add multiple checkpoint destinations
    //public Transform startPoint;
    public Transform endPoint;
    private Vector3 distanceToDestination;
    private bool destinationReached;


    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        CheckDistance();

        //if(!destinationReached) MoveToDestination();

        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (!destinationReached || !agent.pathPending)
            MoveToDestination();

    }
    private void MoveToDestination()
    {
        agent.speed = 6.5f;
        agent.destination = waypoints[waypointIndex].transform.position;
        waypointIndex = (waypointIndex += 1);
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

        FaceDestination();
    }

    float DistanceToAgentTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.destination.x, agent.destination.z));
    }

    private void FaceDestination()
    {
        Vector3 relativePos = endPoint.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
    }

    void CheckDistance()
    {
        distanceToDestination = transform.position - endPoint.position;
        if (distanceToDestination.magnitude < 1f)
        {
            destinationReached = true;
            Debug.Log("Destination Reached");
            agent.isStopped = true;
        }
    }

}
