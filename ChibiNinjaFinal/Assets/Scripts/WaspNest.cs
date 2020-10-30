using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspNest : MonoBehaviour
{
    public int hits;
    public int maxWasps = 6;
    public float spawnEveryXSeconds = 30.0f;
    private float timer;
    public GameObject waspPrefab;
    public Transform[] patrolPoints;

    public InteractionsMaster interactionsMaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>= spawnEveryXSeconds)
        {
            SpawnWasps();
        }
    }
    private void OnCollisionEnter(Collision c)
    {
        if (c.transform.CompareTag("HitBox"))
        {
            // Increse hit count
            ++hits;

            // Hit effects
            // Sound
            // Particle Effect

            //Alert nearby wasps
            AlertWasps();
            //Destroy the wasps-nest if the player hits it 10 times.
            if(hits >= 10)
            {
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("Quest Completed. Wasps-nest destroyed. Have some gold!", true);
                Economy.economy.AddGold(10);

                //Unparent all wasps that are still alive before destroying the wasps-nest
                foreach (Transform t in transform)
                {
                    //if (t.CompareTag("WaspNPC"))
                    {
                        t.transform.parent = null;
                    }
                }
                Destroy(this.gameObject);
                interactionsMaster.SetConversation("Wasp Completed");
            }
        }
    }
    void AlertWasps()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 25.0f);

        foreach (Collider col in colliders)
        {
            if(col.transform.CompareTag("WaspNPC"))
            {
                // The wasp has tho colliders. Use the one that is a child object of the wasp (because that one is the wasps body)
                if (col.transform.GetComponentInParent<WaspNPCScript>() != null)
                {
                    col.transform.GetComponentInParent<WaspNPCScript>().aware = true;
                }
                
            }
        }
    }
    void SpawnWasps()
    {
        int spawnedWasps = 0;
        foreach(Transform t in transform)
        {
            if(t.CompareTag("WaspNPC"))
            {
                ++spawnedWasps;
            }
        }
        if(spawnedWasps < maxWasps)
        {
            GameObject newWasp = Instantiate(waspPrefab, transform);
            newWasp.GetComponent<WaspNPCScript>().patrolPoints = patrolPoints;
            newWasp.transform.position = patrolPoints[0].position;
        }
    }
}
