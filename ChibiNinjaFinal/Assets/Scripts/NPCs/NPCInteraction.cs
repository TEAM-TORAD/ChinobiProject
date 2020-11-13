using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;

[System.Serializable]
public class Interactions
{
    public string interactionName;
    public NPCConversation conversation;
    public bool completed = false;
}
[System.Serializable]
public enum NPCSpeechState
{
    silent,
    interacts_UI,
    interacts_speech_bubble
}
public class NPCInteraction : MonoBehaviour
{
    private Animator animator;
    public float rotationSpeed = 90.0f;
    public bool passive, autoStartConversation;
    [HideInInspector]
    public bool playerClose;
    public NPCSpeechState speachState = NPCSpeechState.silent;
    private Transform player;
    private Vector3 startRotation;
    private float rotationBlendValue = 0.3f;
    public Interactions[] interactions;
    public string bubbleTextStart = "";
    [HideInInspector]
    public int conversationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // This saves a position in front of the player in the direction he is looking when the game starts. When player isn't close the NPC will rotate towards this position
        startRotation = transform.position + transform.forward * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(passive)
        {
            if(playerClose)
            {
                LookAt(player.position);
            }
            else
            {
                LookAt(startRotation);
            }
        }
    }
    public void SetConversation(string name)
    {
        bool matchFound = false;
        int i = 0;
        foreach (Interactions m in interactions)
        {
            if (name == m.interactionName)
            {
                conversationIndex = i;
                matchFound = true;
                break;
            }
            ++i;
        }
        if (!matchFound) Debug.LogError("No matching mission-name found!");
    }

    public void EnteringTrigger(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = true;

            //play conversation sound?

        }
    }
    public void ExitingTrigger(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = false;
            //if (talkBubble.activeSelf) talkBubble.SetActive(false);

        }
    }
    private void OnTriggerEnter(Collider c)
    {
        EnteringTrigger(c);
    }
    private void OnTriggerExit(Collider c)
    {
        ExitingTrigger(c);
    }
    private void LookAt(Vector3 _targetPosition)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = _targetPosition - transform.position;

        float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
        // Set animator based on the angle between the current; rotation and the target rotation
        if (Mathf.Abs(angle) > 5)
        {
            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), rotationBlendValue, 2 * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);
            Vector3 newRot = transform.rotation.eulerAngles;
            if (angle > 0) newRot.y -= Time.deltaTime * rotationSpeed;
            else newRot.y += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(newRot);
        }
        else
        {
            float blendValue = Mathf.Lerp(animator.GetFloat("Blend"), 0.0f, 2 * Time.deltaTime);
            animator.SetFloat("Blend", blendValue);
        }
    }
}
