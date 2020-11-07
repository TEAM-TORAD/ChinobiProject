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
public class NPCInteraction : MonoBehaviour
{
    private Animator animator;
    public float rotationSpeed = 90.0f;
    public bool passive, autoStartConversation;
    private bool playerClose;
    private GameObject talkBubble;
    public Interactions[] interactions;
    private int conversationIndex = 0;
    private Transform player;
    private Vector3 startRotation;
    private float rotationBlendValue = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        talkBubble = transform.Find("TalkBubble").gameObject;
        talkBubble.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // This saves a position 1 meter in front of the player in the direction he is looking when the game starts. When player isn't close the NPC will rotate towards this position
        startRotation = transform.position + transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(passive)
        {
            if(playerClose)
            {
                LookAt(player.position);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!CursorScript.instance.conversationOpen && !CursorScript.instance.storeOpen)
                    {
                        if (interactions.Length > 0)
                        {
                            ConversationManager.Instance.StartConversation(interactions[conversationIndex].conversation); 
                            CameraLookAt.instance.Activate(transform);
                            if (talkBubble.activeSelf) talkBubble.SetActive(false);
                        }
                    }
                    else
                    {
                        if(CursorScript.instance.conversationOpen && interactions.Length > 0)
                        {
                            ConversationManager.Instance.EndConversation();
                            talkBubble.SetActive(true);
                        }
                        else if(CursorScript.instance.storeOpen)
                        {
                            if (transform.GetComponent<ShopKeeperNPC>() != null)
                            {
                                transform.GetComponent<ShopKeeperNPC>().CloseStore();
                                if (CameraLookAt.instance != null) CameraLookAt.instance.Deactivate();
                            }
                        }
                    }
                }
                else if(!CursorScript.instance.conversationOpen)
                {
                    if(interactions.Length > 0 && !talkBubble.activeSelf) talkBubble.SetActive(true);
                }
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

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = true;
            if (passive)
            {
                if (autoStartConversation && interactions.Length > 0 && !CursorScript.instance.conversationOpen)
                {
                    ConversationManager.Instance.StartConversation(interactions[conversationIndex].conversation);
                    CursorScript.instance.conversationOpen = true;
                }
                else if (!autoStartConversation && interactions.Length > 0 && !CursorScript.instance.conversationOpen)
                {
                    talkBubble.SetActive(true);
                }
            }

            //play conversation sound?

        }
        
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = false;
            if (talkBubble.activeSelf) talkBubble.SetActive(false);

        }
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
