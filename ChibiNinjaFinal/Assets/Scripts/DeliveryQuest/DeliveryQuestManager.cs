using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryQuestManager : MonoBehaviour
{
    private GameObject sushiBag;
    private GameObject talkBubble;
    
    public TMP_Text speachBubble;
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

   
    public void Start()
    {
        talkBubble = transform.Find("TalkBubble").gameObject;
        talkBubble.SetActive(false);
        sushiBag = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().backPack.gameObject;
        speachBubble = transform.Find("SpeachBubble").GetComponent<TMP_Text>();
        deliveryQuestActive = false;
        speachBubble.gameObject.SetActive(false);
        conversationOpen = false;
        locations = GameObject.FindGameObjectsWithTag("DeliveryLocation");
        currentLocation = locations[index];
        sushiBag.SetActive(false);
       
    }

    public void Update()
    {
       if(active)
        {
            ActivateQuestion();
        }
        if (deliveryQuestActive)
        {
            timer += 1f * Time.deltaTime;
        }
    }

    #region Trigger Conversation
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            talkBubble.SetActive(true);
            active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(talkBubble.activeSelf) talkBubble.SetActive(false);
            EndConversation();
        }
    }
    #endregion

    #region Conversation to activate quest
    public void ActivateQuestion()
    {
        if (!deliveryQuestActive && Input.GetKeyDown(KeyCode.E))
        {
            speachBubble.gameObject.SetActive(true);
            speachBubble.text = "Want to deliver sushi? \n Y or N";
            conversationOpen = true;
            timer = 0;
            talkBubble.SetActive(false);
        }
        if (conversationOpen)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                speachBubble.text ="You're Hired, now get going!";
                deliveryQuestActive = true;
                Invoke("EndConversation", 3f);
                ChooseDeliveryLocation();
                PlaceDeliveryItemOnPlayer();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                speachBubble.text = "Too good for Uber hey!?";
                Invoke("EndConversation", 3f);
            }
        }
        if (deliveryQuestActive && Input.GetKeyDown(KeyCode.E))
        {
            speachBubble.gameObject.SetActive(true);
            speachBubble.text = "Are you sick of Ubering? \n Y or N";
            quitWorkConversation = true;
        }
        if (quitWorkConversation)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                speachBubble.text = "Too late... you're already FIRED!";
                deliveryQuestActive = false;
                Invoke("EndConversation", 3f);
                quitWorkConversation = false;
                RemoveDeliveryItemFromPlayer();
                
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                speachBubble.text = "Stop talking... get back to work!";
                Invoke("EndConversation", 3f);
                quitWorkConversation = false;
            }
        }

    }
    void EndConversation()
    {
        active = false;
        conversationOpen = false;
        speachBubble.gameObject.SetActive(false);
        if (active) talkBubble.SetActive(true);
    }
    #endregion
    public void ActivateDelivery()
    {
        ChooseDeliveryLocation();
        PlaceDeliveryItemOnPlayer();
        deliveryQuestActive = true;
        deliveryDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(locations[index].transform.position.x, locations[index].transform.position.z));
    }
    public void DeactivateDelivery()
    {
        RemoveDeliveryItemFromPlayer();
        deliveryQuestActive = false;
        timer = 0;
    }
    public void ChooseDeliveryLocation()
    {
        index = Random.Range(0, locations.Length);
        currentLocation = locations[index];
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
        reward = Mathf.RoundToInt((deliveryDistance / timer) * goldRate + (deliveryDistance / bonusRate));
        if (reward == 0) reward = 1;
        Economy.economy.InstantiateServerMessage("You completed the delivery. Your reward is: " + reward + "!", true);
        Economy.economy.AddGold(reward);

        // just need to add a Sprite saying Delivered Successfully in 'timer' seconds.
        // and reward the player Distance / Timer x Gold Reward
    }
}
