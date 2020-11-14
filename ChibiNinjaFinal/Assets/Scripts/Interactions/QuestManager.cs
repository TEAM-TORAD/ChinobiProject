using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;
    public DummyHitDetection dummy;
    private NPCInteraction interactionsMaster;
    public bool attackTutorialStarted, waspKillerStarted;
    public bool attackTutorialCompleted, wasKillerCompleted;
    public Transform waspNest;
    private Target waspNestTarget;
    Target ninjaMasterTarget;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        Transform ninjaMaster = GameObject.FindGameObjectWithTag("NinjaMaster").transform;
        interactionsMaster = ninjaMaster.GetComponent<NPCInteraction>();
        ninjaMasterTarget = ninjaMaster.Find("Target").GetComponent<Target>();
        waspNestTarget = waspNest.GetComponent<Target>();
        waspNestTarget.enabled = false;
        waspNest.gameObject.SetActive(false);
        if(!attackTutorialStarted)
        {
            Economy.economy.InstantiateServerMessage("Speak to the Ninja Master.", true);
            ninjaMasterTarget.enabled = true;
        }
        else
        {

            ninjaMasterTarget.enabled = false;
        }
    }
    private void Update()
    {


    }
    public void CloseCOnversation()
    {
        if(CursorScript.instance.conversationOpen)
        {
            ConversationManager.Instance.EndConversation();
            CameraLookAt.instance.Deactivate();
        }
        else Debug.LogError("conversationOpen is set to false in CursorScript.instace . Can't end a conversation if none is open.");

    }
    public void StartConversation(NPCConversation conversation, Transform lookAtTarget)
    {
        if (!CursorScript.instance.conversationOpen)
        {
            ConversationManager.Instance.StartConversation(conversation);
            CameraLookAt.instance.Activate(lookAtTarget);
        }
        else Debug.LogError("conversationOpen is already set to true in the CursorScript.instance . Can't start a new conversation while a conversation is already open.");
    }

    public void StartAttackTutorial()
    {
        if (!attackTutorialStarted)
        {
            attackTutorialStarted = true;
            ninjaMasterTarget.enabled = false;
            dummy.StartTutorial();
        }
    }
    public void CompleteAttackTutorial()
    {
        if(!attackTutorialCompleted)
        {
            attackTutorialCompleted = true;
            ninjaMasterTarget.enabled = true;
        }
    }
    public void ClearMessages()
    {
        Economy.economy.DestroyOldMessages();
    }
    public void StartWaspKiller()
    {
        if (!waspKillerStarted)
        {
            if (ninjaMasterTarget.enabled) ninjaMasterTarget.enabled = false;
            waspKillerStarted = true;
            ClearMessages();
            waspNest.gameObject.SetActive(true);
            waspNestTarget.enabled = true;
            Economy.economy.InstantiateServerMessage("Destroy the wasps-nest in the village!", true);
            interactionsMaster.SetConversation("Wasp Accepted");
        }
    }
    public void CompleteWaspKiller()
    {
        if (!waspKillerStarted)
        {
            wasKillerCompleted = true;
        }
    }
}
