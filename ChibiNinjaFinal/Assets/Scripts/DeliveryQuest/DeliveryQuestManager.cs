using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryQuestManager : MonoBehaviour
{
    private GameObject sushiBag;
    private GameObject talkBubble;
    
    private TMP_Text text;
    private GameObject bubble;
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

    public static DeliveryQuestManager instance = null;
   
    public void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        talkBubble = transform.Find("TalkBubble").gameObject;
        talkBubble.SetActive(false);
        sushiBag = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().backPack.gameObject;
        bubble = transform.Find("SpeachBubble").gameObject;
        text = bubble.transform.Find("Bubble").Find("Text").GetComponent<TMP_Text>();
        bubble.SetActive(false);
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
        if(active)
        {
            if (conversationOpen || deliveryQuestActive || quitWorkConversation)
            {
                ActivateQuestion();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateQuestion();
            }
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
            bubble.SetActive(true);
            text.text = "Want to deliver sushi? \n Y or N";
            conversationOpen = true;
            timer = 0;
            talkBubble.SetActive(false);
        }
        if (conversationOpen)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                text.text ="You're Hired, now get going!";
                deliveryQuestActive = true;
                Invoke("EndConversation", 3f);
                ChooseDeliveryLocation();
                PlaceDeliveryItemOnPlayer();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                text.text = "Too good for Uber hey!?";
                Invoke("EndConversation", 3f);
            }
        }
        if (deliveryQuestActive && Input.GetKeyDown(KeyCode.E))
        {
            bubble.SetActive(true);
            text.text = "Are you sick of Ubering? \n Y or N";
            talkBubble.SetActive(false);
            quitWorkConversation = true;
        }
        if (quitWorkConversation)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                text.text = "Too late... you're already FIRED!";
                locations[index].SetActive(false);
                deliveryQuestActive = false;
                Invoke("EndConversation", 3f);
                quitWorkConversation = false;
                RemoveDeliveryItemFromPlayer();
                
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                text.text = "Stop talking... get back to work!";
                Invoke("EndConversation", 3f);
                quitWorkConversation = false;
            }
        }

    }
    void EndConversation()
    {
        active = false;
        conversationOpen = false;
        bubble.SetActive(false);
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
        index = Random.Range(0, locations.Length - 1);
        locations[index].SetActive(true);
        currentLocation = locations[index];
        currentLocation.transform.GetComponent<DeliveryLocationManager>().SetAsLocation();
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
