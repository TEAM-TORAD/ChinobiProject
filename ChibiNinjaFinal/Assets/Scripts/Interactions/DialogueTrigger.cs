using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public DialogueManager dialogueManager;

    public Dialogue dialogue;

    [HideInInspector] public bool dialogueOpen;

    private bool playerClose;
    public bool autoTalk = false;

     void Start()
    {
        dialogueOpen = false;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!dialogueOpen)
            {
                dialogueManager.StartDialogue(dialogue);
                dialogueOpen = true;
            }
        }
        if(Input.anyKeyDown && dialogueOpen)
        {
            dialogueManager.DisplayNextSentence();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player") && !dialogueOpen)
        {
            playerClose = true;
            if(autoTalk)
            {
                dialogueManager.StartDialogue(dialogue);
                dialogueOpen = true;
                autoTalk = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") && !dialogueOpen)
        {
            playerClose = false;
            dialogueManager.EndDialogue();
            dialogueOpen = false;
        }
    }
}
