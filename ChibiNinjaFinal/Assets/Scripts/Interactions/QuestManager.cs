using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public NPCConversation startDialog;
    public static QuestManager instance = null;
    public DummyHitDetection dummy;
    private NPCInteraction interactionsMaster;
    public bool attackTutorialStarted, waspKillerStarted;
    public bool attackTutorialCompleted, wasKillerCompleted;
    public Transform waspNest;
    private Target waspNestTarget;
    Target ninjaMasterTarget;
    private bool mouseTutorialActive, mouseRotateLeft, mouseRotateRight;
    private bool moveTutorialActive, w, a, s, d;
    public GameObject sprintTutorialTrigger;
    private bool sprintTutorialActive;
    private Animator playerAnimator;
    private bool jumpTutorialStarted;

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
        sprintTutorialTrigger.SetActive(false);
        playerAnimator = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Animator>();
    }
    private void Start()
    {
        StartConversation(startDialog, null);

    }
    private void Update()
    {
        if(mouseTutorialActive)
        {
            float xAxis = Input.GetAxis("Mouse X");
            if (xAxis > 0) mouseRotateRight = true;
            else if (xAxis < 0) mouseRotateLeft = true;
            if(mouseRotateLeft && mouseRotateRight)
            {
                mouseTutorialActive = false;
                moveTutorialActive = true;
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("Well done. Use WASD keys to move.", false);
            }
        }
        else if(moveTutorialActive)
        {
            var input = Input.inputString;
            print(input);
            switch (input)
            {
                case "w":
                    w = true;
                    break;
                case "a":
                    a = true;
                    break;
                case "s":
                    s = true;
                    break;
                case "d":
                    d = true;
                    break;
            }
            if(w && a && s && d)
            {
                moveTutorialActive = false;
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("Head over to the Ninja Master", false);
                ninjaMasterTarget.enabled = true;
                sprintTutorialTrigger.SetActive(true);
                interactionsMaster.SetConversation("Dummy");
            }
        }
        else if(sprintTutorialActive)
        {
           if(playerAnimator.GetFloat("Speed") >= 1.8f)
            {
                sprintTutorialActive = false;
                jumpTutorialStarted = true;
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("You're like the wind! Press space to jump.", false);
            }
        }
        else if(jumpTutorialStarted)
        {
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("jump_start") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jumping") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Power jump_start"))
            {
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("Nice, but be careful! If you fall from a high place you might die!", true);
                jumpTutorialStarted = false;

            }
        }


    }
    public void StartSprintTutorial()
    {
        sprintTutorialActive = true;
        Economy.economy.DestroyOldMessages();
        Economy.economy.InstantiateServerMessage("Press left shift while moving to sprint", false);
    }
    private void StartMouseMoveTutorial()
    {
        Economy.economy.DestroyOldMessages();
        Economy.economy.InstantiateServerMessage("Use the mouse to rotate.", false);
        interactionsMaster.SetConversation("Walk before you talk");
        mouseTutorialActive = true;
    }
    public void DoTutorial(bool value)
    {
        if (value)
        {
            StartMouseMoveTutorial();
        }
        else
        {
            CompleteAttackTutorial(true);
        }
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
            interactionsMaster.SetConversation("Dummy started");
        }
    }
    public void CompleteAttackTutorial(bool skipTutorial)
    {
        if(!attackTutorialCompleted)
        {
            if (skipTutorial)
            {
                interactionsMaster.SetConversation("Tutorial Skipped");
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage("Speak to the Ninja Master.", true);
            }
            else interactionsMaster.SetConversation("Tutorial Completed");
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
            Economy.economy.DestroyOldMessages();
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
