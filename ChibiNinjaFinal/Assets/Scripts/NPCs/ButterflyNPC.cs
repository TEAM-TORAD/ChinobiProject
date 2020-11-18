using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButterflyNPC : MonoBehaviour
{
    #region Public variables
    [Tooltip("Sets the speed of the nav mesh agent.")]
    public float speed = 2;
    [Tooltip("The time the NPC will be idle if it can't find a fresh flower to go to. When the time runs out it will check again.")]
    public float waitTime = 20;
    [Tooltip("The time the butter-fly will take to drink nectar from a flower.")]
    public float drinkTime = 10;
    [Tooltip("The hight above the ground that the butter-fly will travel.")]
    public float travelHeight = 1.5f;
    [Tooltip("The max distance the butterfly will search for a fresh flower.")]
    public float searchDistance = 20.0f;
    #endregion

    #region Logic variables
    private bool idle, drinkingNectar;
    private float timer;
    private Flower targetFlower;
    private NavMeshAgent agent;
    private Animator animator;
    private Transform rig;
    private GameObject[] flowers;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rig = transform.Find("Rig");
        rig.localPosition = new Vector3(0, 0, 0);

        animator = rig.GetComponent<Animator>();
        flowers = GameObject.FindGameObjectsWithTag("Flower");
        FindFlower();
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        if (idle)
        {
            timer += Time.deltaTime;

            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 0.0f, 1.0f * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);

            if (timer >= waitTime)
            {
                FindFlower();
            }
        }
        else if (drinkingNectar)
        {
            timer += Time.deltaTime;

            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 0.0f, 1.0f * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);

            if (timer >= drinkTime)
            {
                drinkingNectar = false;
                FindFlower();
            }
        }
        // Butterfly is traveling
        else 
        {
            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 1.0f, 1.5f * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);

            // Check distance to the flower without considering height
            float distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetFlower.transform.position.x, targetFlower.transform.position.z));
            
            // Butterfly has reached its' target
            if(distanceToTarget < 0.33f)
            {
                print("Target reached.");
                // The butterfly is decending towards the local offset 
                if (Vector3.Distance(rig.position, targetFlower.landingTarget.position) > 0.05f)
                {
                    rig.position = Vector3.MoveTowards(rig.position, targetFlower.landingTarget.position, speed * 0.5f * Time.deltaTime);
                }
                // The butterlfy has reached the flower
                else
                {
                    print("Drink nectar.");
                    DrinkNectar();
                }
            }
            // Butterfly is traveling to the target
            else
            {
                // If the butterfly is to low it needs to travel up before starting to travel to the next flower
                if(rig.localPosition.y < travelHeight -0.1f)
                {
                    rig.localPosition = Vector3.Lerp(rig.localPosition, new Vector3(0, travelHeight, 0), speed * 0.5f * Time.deltaTime);
                }
                // Butterfly has the right height and can travel to the target
                else
                {
                    //
                }
            }
        }
    }
    #endregion

    #region custom methods
    private void DrinkNectar()
    {
        agent.speed = 0;
        agent.enabled = false;
        timer = 0;
        idle = false;
        drinkingNectar = true;
        targetFlower.visited = true;
    }
    private void WaitHere()
    {
        agent.speed = 0;
        agent.enabled = false;
        timer = 0;
        idle = true;
        drinkingNectar = false;
    }
    private void TravelToFlower(Flower _flower)
    {
        agent.enabled = true;
        agent.speed = speed;
        idle = false;
        drinkingNectar = false;
        targetFlower = _flower;
        agent.destination = targetFlower.transform.position;
    }
    private void FindFlower()
    {
        float closest = Mathf.Infinity;
        int closestIndex = -1;
        for(int i = 0; i < flowers.Length; ++i)
        {
            if (flowers[i].transform.GetComponent<Flower>() != null)
            {
                // Make sure that the flower hasn't been visited reasently
                if (!flowers[i].transform.GetComponent<Flower>().visited)
                {
                    if (flowers[i].transform.GetComponent<Flower>().landingTarget != null)
                    {
                        // If the distance to the flower is less then the search distance and the current shortest distance, this is the new target
                        float distance = Vector3.Distance(transform.position, flowers[i].transform.position);
                        if (distance <= searchDistance && distance < closest)
                        {
                            // Check if the target flowe can be reached by the navmesh agent
                            NavMeshPath path = new NavMeshPath();
                            NavMesh.CalculatePath(transform.position, flowers[i].transform.position, NavMesh.AllAreas, path);
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                // Check that the point is actually on the mesh (and not slightly outside)
                                NavMeshHit hit;
                                if (NavMesh.SamplePosition(flowers[i].transform.position, out hit, 0.5f, NavMesh.AllAreas))
                                {
                                    // CHeck distance in x and z (not height)
                                    float xZDistance = Vector2.Distance(new Vector2(flowers[i].transform.position.x, flowers[i].transform.position.z), new Vector2(hit.position.x, hit.position.z));
                                    if(xZDistance < 0.2f)
                                    {
                                        closest = distance;
                                        closestIndex = i;
                                    }
                                    //else print("Index " + i + ": " + flowers[i].transform.name + " is " + xZDistance + " away, out of max 0.2 from closest point on the navmesh.");
                                }
                                //else print("Index " + i + ": " + flowers[i].transform.name + " is outside the navmesh.");
                            }
                            //else print("Index " + i + ": " + flowers[i].transform.name + " can't be reached by the navmesh agent. No valid path.");
                        }
                    }
                    //else print("Index " + i + ": Can't find a landingTarget on " + flowers[i].transform.name + ".");
                }
            }
            //else print("Index " + i + ": Can't find a flower script on " + flowers[i].transform.name + ".");
        }
        // If closestIndex is 0 or greater it means it found a flower close by, now go to it.
        if (closestIndex >= 0)
        {
            Flower thisFlower = flowers[closestIndex].transform.GetComponent<Flower>();
            TravelToFlower(thisFlower);
        }
        // There is no flower with nectar nearby, wait for a while
        else
        {
            WaitHere();
            print(transform.name + " can't find any flower to drink from.");
        }
    }
    #endregion
}
