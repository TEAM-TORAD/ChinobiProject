using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeliveryLocationManager : MonoBehaviour
{
    public TMP_Text text;
    public DeliveryQuestManager manager;
    public bool active;
    public bool closeEnough;

    private void Start()
    {
        active = false;
        text.gameObject.SetActive(false);
        
    }
    private void Update()
    {
        if(manager.currentLocation == gameObject && manager.deliveryQuestActive)
        {
            
            active = true;
        }
        if(manager.currentLocation != gameObject)
        {
            text.gameObject.SetActive(false);
            active = false;
            closeEnough = false;
        }
        if(closeEnough && active)
        {
            //E to talk sprite pop up

            if (Input.GetKeyDown(KeyCode.E))
            {
                text.gameObject.SetActive(true);
                text.text = "Thanks for the Sushi!";
                Invoke("DeactivateText", 3f);
                manager.SushiDelivered();
                active = false;
                closeEnough = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && active && manager.deliveryQuestActive)
        {
            closeEnough = true;
            text.gameObject.SetActive(true);
            text.text = "Where's my Uber Eats?";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && active && manager.deliveryQuestActive)
        {
            text.text = "Damn, thought you had my Uber Eats";
            Invoke("DeactivateText", 3f);
            closeEnough = false;
        }
    }

    public void DeactivateText()
    {
        text.gameObject.SetActive(false);
        closeEnough = false;
    }

}
