using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Transform interactionsPanel;

    public Animator animator;

    [HideInInspector] public DialogueTrigger dialogueTriggerScript;

    private Queue<string> sentences;

    public void Start()
    {
        sentences = new Queue<string>();
        dialogueTriggerScript = FindObjectOfType<DialogueTrigger>();
        if (animator == null) interactionsPanel.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (animator != null) animator.SetBool("IsOpen", true);
        else interactionsPanel.gameObject.SetActive(true);

        nameText.text = dialogue.NPCName;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        if (animator != null) animator.SetBool("IsOpen", false);
        else interactionsPanel.gameObject.SetActive(false);
        dialogueTriggerScript.dialogueOpen = false;
    }

    public void Update()
    {
        Debug.Log(sentences.Count);
        
    }
}
