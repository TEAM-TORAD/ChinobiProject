using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth.health < playerHealth.maxHealth)
            {
                playerHealth.AddHealth(15);
                Destroy(transform.parent.gameObject);

            }
        }
   
    }
}
