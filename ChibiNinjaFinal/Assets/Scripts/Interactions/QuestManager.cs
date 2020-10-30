using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public DummyHitDetection dummy;
    public InteractionsMaster interactionsMaster;
    public bool attackTutorialStarted, waspKillerStarted;


    private void Start()
    {
        //dummyText = GameObject.FindGameObjectWithTag("DummyText").transform;
        //dummyText.gameObject.SetActive(false);
    }
    private void Update()
    {

      
    }
    public void StartAttackTutorial()
    {
        if(!attackTutorialStarted)
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
        if(!waspKillerStarted)
        {
            waspKillerStarted = true;
            ClearMessages();
            Economy.economy.InstantiateServerMessage("Go and destroy the wasps-nest in the village!", false);
            interactionsMaster.SetConversation("Wasp Accepted");
        }
    }
}
