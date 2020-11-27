using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using DialogueEditor;
using UnityEngine.Events;

// This script is attached to NPC's and requires that you assign the HintButton and SpeechBubble prefabs to the script in the inspector
// You need to make sure there is a TRIGGER on the object you attach this script to
public class DisplayInteractions : MonoBehaviour
{
    #region Public Variables
    [HideInInspector]
    public GameObject hintButtonPrefab;
    private GameObject speechBubble;
    [HideInInspector]
    public TMP_Text text;
    private NPCInteraction NPCInteraction;
    private PopupsScript popupScript;
    #endregion

    #region Public Boolean States
    public bool hintButtonShowing;
    public bool speechBubbleShowing;
    public bool interactionOpen;
    [HideInInspector]
    public UnityEvent OnSpeachBubbleOpen;
    [HideInInspector]
    public UnityEvent OnSpeachBubbleClose;
    #endregion

    #region Initialized Start values
    private void Awake()
    {
        if (OnSpeachBubbleOpen == null)  OnSpeachBubbleOpen = new UnityEvent();
        if (OnSpeachBubbleClose == null) OnSpeachBubbleClose = new UnityEvent();
        if (transform.Find("InteractionPopups").Find("HintButtonPrefab") != null) hintButtonPrefab = transform.Find("InteractionPopups").Find("HintButtonPrefab").gameObject;
        if(transform.Find("InteractionPopups").Find("SpeechBubble") != null) speechBubble = transform.Find("InteractionPopups").Find("SpeechBubble").gameObject;
        if (speechBubble != null) text = speechBubble.transform.Find("Bubble").GetComponentInChildren<TMP_Text>();
        if (transform.GetComponent<NPCInteraction>() != null) NPCInteraction = transform.GetComponent<NPCInteraction>();
        if (transform.Find("InteractionPopups").GetComponent<PopupsScript>() != null) popupScript = transform.Find("InteractionPopups").GetComponent<PopupsScript>();

        if (NPCInteraction.bubbleTextStart != "") text.text = NPCInteraction.bubbleTextStart;
        else text.text = "I've got notting to say.";

        hintButtonShowing = false;
        speechBubbleShowing = false;
        interactionOpen = false;
        hintButtonPrefab.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(false);
    }
    #endregion

    #region Update methods
    private void Update()
    {
        if (hintButtonShowing) MakeHintButtonFaceCamera();
        if(NPCInteraction.playerClose)
        {
            if (NPCInteraction.speachState == NPCSpeechState.interacts_UI)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (CursorScript.instance.conversationOpen)
                    {
                        ConversationManager.Instance.EndConversation();
                        hintButtonPrefab.SetActive(true);
                    }
                    else if (CursorScript.instance.storeOpen)
                    {
                        if (transform.GetComponent<ShopKeeperNPC>() != null)
                        {
                            transform.GetComponent<ShopKeeperNPC>().CloseStore();
                            if (CameraLookAt.instance != null) CameraLookAt.instance.Deactivate();
                        }
                    }
                    else if (NPCInteraction.interactions.Length > 0 && hintButtonPrefab.activeSelf)
                    {
                        ConversationManager.Instance.StartConversation(NPCInteraction.interactions[NPCInteraction.conversationIndex].conversation);
                        CameraLookAt.instance.Activate(transform);
                        if (hintButtonPrefab.activeSelf) HideHintButton();
                        CursorScript.instance.conversationOpen = true;
                    }
                }
                else if (!CursorScript.instance.conversationOpen)
                {
                    if (NPCInteraction.interactions.Length > 0 && !hintButtonPrefab.activeSelf) ShowHintButton();
                }
            }
            else if (NPCInteraction.speachState == NPCSpeechState.interacts_speech_bubble)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!speechBubbleShowing)
                    {
                        ShowSpeechBubble();
                    }
                    else
                    {
                        HideSpeechBubble();
                    }
                }
            }
        }
        else
        {
            HideSpeechBubble();
            HideHintButton();
        }
        
    }
    #endregion

    #region Handle Trigger enter/exit to show/hide the Hint Button
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hintButtonShowing) ShowHintButton();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hintButtonShowing || speechBubbleShowing)
        {
            HideHintButton();
            HideSpeechBubble();
        }
    }
    #endregion

    #region Method to handle showing/hiding the Hint Button
    //Activates the Hint Button to show to the player
    public void ShowHintButton()
    {
        popupScript.ResetPosition();
        hintButtonPrefab.gameObject.SetActive(true);
        hintButtonShowing = true;
    }
    //Deactivates the Hint Button to hide it from player
    public void HideHintButton()
    {
        hintButtonPrefab.gameObject.SetActive(false);
        hintButtonShowing = false;
    }
    #endregion
    void ShowSpeechBubble()
    {
        HideHintButton();
        popupScript.ResetPosition();
        OnSpeachBubbleOpen.Invoke();
        speechBubble.gameObject.SetActive(true);
        speechBubbleShowing = true;
    }
    public void HideSpeechBubble()
    {
        ShowHintButton();
        speechBubble.gameObject.SetActive(false);
        speechBubbleShowing = false;
        OnSpeachBubbleClose.Invoke();
    }

    #region Hint Button Behavioural Methods
    //Makes the Hint Button face the players camera
    private void MakeHintButtonFaceCamera()
    {
        hintButtonPrefab.transform.LookAt(Camera.main.transform);
    }
    #endregion
  
}
