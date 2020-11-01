using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspNest : MonoBehaviour
{
    // texture from https://hillsteadblog.files.wordpress.com/2009/11/img_23021.jpg
    public int hits;
    public int maxWasps = 6;
    public float spawnEveryXSeconds = 30.0f;
    private float timer;
    private Transform spawnPoint;
    public GameObject waspPrefab;
    public Transform[] patrolPoints;
    private ParticleSystem PS;

    public InteractionsMaster interactionsMaster;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.parent.Find("Wasp Spawnpoint");
        PS = transform.parent.Find("Particle System").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnEveryXSeconds)
        {
            SpawnWasps(false);
            timer = 0;
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
            PS.Play();

            //Alert nearby wasps
            AlertWasps();

            //Spawn a few extra enemies the first few times the player hits the nest
            if (hits < 5)
            {
                SpawnWasps(true);
            }

            //Destroy the wasps-nest if the player hits it 10 times.
            if (hits >= 10)
            {
                if (QuestManager.instance.waspKillerStarted)
                {
                    Economy.economy.DestroyOldMessages();
                    Economy.economy.InstantiateServerMessage("Quest Completed. Wasps-nest destroyed. Have some gold!", true);
                    Economy.economy.AddGold(10);
                }

                //Unparent all wasps that are still alive before destroying the wasps-nest
                foreach (Transform t in transform)
                {
                    t.parent = null;
                }
                Destroy(transform.parent.gameObject);
                interactionsMaster.SetConversation("Wasp Completed");
            }
        }
    }
    void AlertWasps()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 25.0f);

        foreach (Collider col in colliders)
        {
            if (col.transform.CompareTag("WaspNPC"))
            {
                // The wasp has tho colliders. Use the one that is a child object of the wasp (because that one is the wasps body)
                if (col.transform.GetComponentInParent<WaspNPCScript>() != null)
                {
                    col.transform.GetComponentInParent<WaspNPCScript>().aware = true;
                }

            }
        }
    }
    void SpawnWasps(bool _aware)
    {
        int spawnedWasps = 0;
        foreach (Transform t in transform)
        {
            if (t.CompareTag("WaspNPC"))
            {
                ++spawnedWasps;
            }
        }
        if (spawnedWasps < maxWasps)
        {
            GameObject newWasp = Instantiate(waspPrefab, spawnPoint.position, spawnPoint.rotation, transform);
            WaspNPCScript wasp = newWasp.GetComponent<WaspNPCScript>();
            wasp.patrolPoints = patrolPoints;
            wasp.aware = _aware;
        }
    }
}
