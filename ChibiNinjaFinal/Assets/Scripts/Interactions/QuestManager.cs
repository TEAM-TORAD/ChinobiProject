using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;
    public DummyHitDetection dummy;
    public InteractionsMaster interactionsMaster;
    public bool attackTutorialStarted, waspKillerStarted;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    private void Update()
    {


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
            Economy.economy.InstantiateServerMessage("Destroy the wasps-nest in the village!", false);
            interactionsMaster.SetConversation("Wasp Accepted");
        }
    }
}
