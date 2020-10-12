using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int regularDamage = 15, powerDamage = 25;
    public bool friendly = false;
    private int attackValue;
    // Start is called before the first frame update
    void Start()
    {
        attackValue = regularDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider c)
    {
        // Make sure that the collider is NOT a trigger. We don't want to register doubble hit's on characters that has a weapon attack trigger active when we hit them.
        if (!c.isTrigger)
        {
            if (friendly) // A friendly (player, ninja master etc.) is holding the weapon, attack enemies
            {
                if (c.attachedRigidbody != null) // The object that was hit has an attached rigidbody
                {
                    if (c.attachedRigidbody.CompareTag("ExplodingNPC"))
                    {
                        Debug.Log("Player hit Exploding NPC");
                        c.attachedRigidbody.GetComponent<ExplodingNPCController>().TakeDamage(attackValue);
                        Debug.Log("Exploding NPC has " + c.attachedRigidbody.GetComponent<Health>().health + " health left.");
                    }
                }
                else
                {
                    if (c.transform.CompareTag("WaspNPC"))
                    {
                        Debug.Log("Player hit Wasp NPC");
                        Health waspHealth = c.transform.GetComponent<Health>();
                        waspHealth.TakeDamage(regularDamage);
                        Debug.Log("Wasp NPC has " + waspHealth.health + " health left.");
                    }
                    else
                    {
                        if (c.attachedRigidbody != null) Debug.Log("Player hit (tag on attachedRigidbody) " + c.attachedRigidbody.tag + ". Name " + c.attachedRigidbody.name); // Debug Test
                        else if (c.transform.tag != null) Debug.Log("Player hit (tag on transform) " + c.transform.tag); // Debug test
                        else Debug.Log("Player hit (name) " + c.transform.name); // Debug test
                    }
                }
            }
            else // If an enemy is holding the weapon, attack friendlies
            {
                if(c.attachedRigidbody != null)
                {
                    if (c.attachedRigidbody.CompareTag("Player"))
                    {
                        Debug.Log("Player hit by enemy!");
                        Health playerHealth = c.attachedRigidbody.GetComponent<Health>();
                        Stamina playerStamina = c.attachedRigidbody.GetComponent<Stamina>();
                        PlayerBlock playerBlock = c.attachedRigidbody.GetComponent<PlayerBlock>();

                        if (playerStamina != null && playerBlock != null && playerHealth != null)
                        {
                            if (playerBlock.isBlocking)
                            {
                                playerStamina.StaminaDamage(regularDamage);
                            }
                            else
                            {
                                playerHealth.TakeDamage(regularDamage);
                            }
                        }
                        else Debug.LogError("Some scripts are missing. Make sure Health.cs, Stamina.cs and PlayerBlock.cs are attatched to the player!");
                    }
                }
            }
        }
    }
}
