using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DeliveryQuestManager : MonoBehaviour
{
    private GameObject sushiBag;
    
    private TMP_Text text;
    public bool deliveryQuestActive;
    public bool conversationOpen;
    public bool quitWorkConversation;
    public bool active;
    public GameObject[] locations;
    int index;
    public GameObject currentLocation;
    private float deliveryDistance;
    private int reward;
    public float goldRate = 0.5f, bonusRate = 100.0f;
    public float timer;
    private NPCInteraction nPCInteraction;
    private DisplayInteractions displayInteractions;
    private Transform player;

    public static DeliveryQuestManager instance = null;
   
    public void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (transform.GetComponent<NPCInteraction>() != null) nPCInteraction = transform.GetComponent<NPCInteraction>();

        if (transform.GetComponent<DisplayInteractions>() != null) displayInteractions = transform.GetComponent<DisplayInteractions>();
        displayInteractions.OnSpeachBubbleOpen.AddListener(ActivateQuestion);
        displayInteractions.OnSpeachBubbleClose.AddListener(EndConversation);

        sushiBag = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().backPack.gameObject;
        text = transform.GetComponent<DisplayInteractions>().text;
        deliveryQuestActive = false;
        conversationOpen = false;
        locations = GameObject.FindGameObjectsWithTag("DeliveryLocation");
        foreach(GameObject g in locations)
        {
            g.SetActive(false);
        }
        sushiBag.SetActive(false);
       
    }

    public void Update()
    {
        if (deliveryQuestActive)
        {
            timer += 1f * Time.deltaTime;
        }
        if(conversationOpen)
        {
            if (!deliveryQuestActive)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    text.text = "You're Hired, now get going!";
                    deliveryQuestActive = true;
                    ChooseDeliveryLocation();
                    PlaceDeliveryItemOnPlayer();
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    text.text = "Too good for Uber hey!?";
                }
            }
            if (quitWorkConversation)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    text.text = "Too late... you're already FIRED!";
                    locations[index].SetActive(false);
                    deliveryQuestActive = false;
                    quitWorkConversation = false;
                    RemoveDeliveryItemFromPlayer();

                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    text.text = "Stop talking... get back to work!";
                    quitWorkConversation = false;

                }
            }
        }
    }


    #region Conversation to activate quest
    public void ActivateQuestion()
    {
        if (!deliveryQuestActive && Input.GetKeyDown(KeyCode.E))
        {
            text.text = "Want to deliver sushi? \n Y or N";
            conversationOpen = true;
            timer = 0;
        }
        if (deliveryQuestActive)
        {
            text.text = "Are you sick of Ubering? \n Y or N";
            conversationOpen = true;
            quitWorkConversation = true;
        }
    }
    public void EndConversation()
    {
        active = false;
        conversationOpen = false;
    }
    #endregion
    public void ActivateDelivery()
    {
        ChooseDeliveryLocation();
        PlaceDeliveryItemOnPlayer();
        deliveryQuestActive = true;
        
    }
    public void DeactivateDelivery()
    {
        RemoveDeliveryItemFromPlayer();
        deliveryQuestActive = false;
        timer = 0;
    }
    public void ChooseDeliveryLocation()
    {
        index = Random.Range(0, locations.Length - 1);
        locations[index].SetActive(true);
        currentLocation = locations[index];
        currentLocation.transform.GetComponent<DeliveryLocationManager>().SetAsLocation();
        deliveryDistance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(currentLocation.transform.position.x, currentLocation.transform.position.z));
    }

    public void PlaceDeliveryItemOnPlayer()
    {
        sushiBag.SetActive(true);
        
    }
    public void RemoveDeliveryItemFromPlayer()
    {
        sushiBag.SetActive(false);
    }
    public void SushiDelivered()
    {
        RemoveDeliveryItemFromPlayer();
        deliveryQuestActive = false;
        // Calculate the reward based on the average speed + a bonus based on the distance
        
        float myRweard = (deliveryDistance / timer) * goldRate + (deliveryDistance / bonusRate);
        reward = Mathf.RoundToInt(myRweard);
        if (reward <= 0) reward = 1;
        Economy.economy.InstantiateServerMessage("You completed the delivery. Your reward is: " + reward + "!", true);
        Economy.economy.AddGold(reward);

        // just need to add a Sprite saying Delivered Successfully in 'timer' seconds.
        // and reward the player Distance / Timer x Gold Reward
    }
}
