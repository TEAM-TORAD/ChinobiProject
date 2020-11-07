using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeliveryLocationManager : MonoBehaviour
{
    private GameObject bubble;
    private TMP_Text text;
    public bool active;
    public bool closeEnough;
    private Target target;
    private Transform player;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = transform.GetComponent<Target>();
        target.enabled = false;
        bubble = transform.Find("SpeachBubble").gameObject;
        text = bubble.transform.Find("Bubble").Find("Text").GetComponent<TMP_Text>();
        active = false;
        bubble.SetActive(false);
        
    }
    private void Update()
    {
        if (DeliveryQuestManager.instance.currentLocation == gameObject)
        {
            if (DeliveryQuestManager.instance.deliveryQuestActive)
            {
                target.enabled = true;
                active = true;
            }
        }
        else
        {
            bubble.SetActive(false);
            active = false;
        }
        if (DeliveryQuestManager.instance.currentLocation == gameObject && !DeliveryQuestManager.instance.deliveryQuestActive)
        {
            gameObject.GetComponent<Target>().enabled = false;
        }
        if (active)
        {
            //E to talk sprite pop up
            if(closeEnough)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    bubble.SetActive(true);
                    text.text = "Thanks for the Sushi!";
                    Invoke("DeactivateText", 3f);
                    DeliveryQuestManager.instance.SushiDelivered();
                    active = false;
                    target.enabled = false;
                    closeEnough = false;
                }
            }
        }
        else
        {
            if (!renderer.isVisible)
            {
                transform.gameObject.SetActive(false);
            }
        }
    }
   
    public void SetAsLocation()
    {
        active = true;
        if(target != null) target.enabled = true;
    }
    public void CancelDelivery()
    {
        active = false;
        if (target != null) target.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            closeEnough = true;
            if(active)
            {
                text.text = "Is that my Uber Eats?";
                bubble.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && active && DeliveryQuestManager.instance.deliveryQuestActive)
        {
            text.text = "Damn, thought you had my Uber Eats";
            Invoke("DeactivateText", 3f);
            closeEnough = false;
        }
    }

    public void DeactivateText()
    {
        bubble.SetActive(false);
        closeEnough = false;
    }

}
