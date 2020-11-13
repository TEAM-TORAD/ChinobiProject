using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeliveryLocationManager : MonoBehaviour
{
    public bool active;
    public bool conversationOpen;
    private Target target;
    private Renderer renderer;
    private DisplayInteractions displayInteractions;
    private bool askedAboutSushi, replied;
    private TMP_Text text;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        displayInteractions = transform.GetComponent<DisplayInteractions>();
        displayInteractions.OnSpeachBubbleOpen.AddListener(ActivateQuestion);
        displayInteractions.OnSpeachBubbleClose.AddListener(CloseConversation);
        target = transform.GetComponent<Target>();
        text = transform.GetComponent<DisplayInteractions>().text;
        target.enabled = false;
        active = false;
        
    }
    private void Update()
    {
        if (DeliveryQuestManager.instance.currentLocation == gameObject && DeliveryQuestManager.instance.deliveryQuestActive)
        {
            target.enabled = true;
            active = true;
        }
        else
        {
            active = false;
            gameObject.GetComponent<Target>().enabled = false;
        }
        if (active)
        {
            if(askedAboutSushi && !replied && conversationOpen)
            {
                if(Input.GetKeyDown(KeyCode.Y))
                {
                    DeliveryComplete();
                }
                else if(Input.GetKeyDown(KeyCode.N))
                {
                    text.text = "Damn, thought you had my sushi...";
                    replied = true;
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
    public void ActivateQuestion()
    {
        if(active)
        {
            conversationOpen = true;
            if (!askedAboutSushi)
            {
                text.text = "Is that my sushi? \n Y or N";
                askedAboutSushi = true;
            }
        }
        
    }
    public void CloseConversation()
    {
        conversationOpen = false;
        askedAboutSushi = false;
        replied = false;
    }
    public void DeliveryComplete()
    {
        DeliveryQuestManager.instance.SushiDelivered();
        replied = true;
        active = false;
        target.enabled = false;
        conversationOpen = false;
        text.text = "Thanks for the Sushi!";
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


}
