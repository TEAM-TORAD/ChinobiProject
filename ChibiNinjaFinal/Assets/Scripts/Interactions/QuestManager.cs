using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;
    public DummyHitDetection dummy;
    public NPCInteraction interactionsMaster;
    public bool attackTutorialStarted, waspKillerStarted;
    public Transform waspNest;
    private Target waspNestTarget;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        waspNestTarget = waspNest.GetComponent<Target>();
        waspNestTarget.enabled = false;
        waspNest.gameObject.SetActive(false);
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
            dummy.StartTutorial();
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
            waspKillerStarted = true;
            ClearMessages();
            waspNest.gameObject.SetActive(true);
            waspNestTarget.enabled = true;
            Economy.economy.InstantiateServerMessage("Destroy the wasps-nest in the village!", true);
            interactionsMaster.SetConversation("Wasp Accepted");
        }
    }
}
