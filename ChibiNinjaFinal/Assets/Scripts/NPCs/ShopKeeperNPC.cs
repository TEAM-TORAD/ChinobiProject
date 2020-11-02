using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ShopKeeperNPC : MonoBehaviour
{
    public bool autoStartConversation = true;
    public float rotationSpeed = 4.0f;
    public NPCConversation conversation;
    public Item[] items;
    private Animator animator;
    private bool playerClose, passive = true;
    private Transform player;
    public Transform lookAtObject;
    private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(passive)
        {
            if (playerClose)
            {
                LookAt(player);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!CursorScript.instance.storeOpen) StartConversation();
                    else
                    {
                        CloseStore();
                        Economy.economy.InstantiateServerMessage("Press 'E' to talk to the shop keeper.", true);
                    }
                }
            }
            else
            {
                // Rotate towards the rotation the NPC had when the game started
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotationSpeed * 10 * Time.deltaTime);
            }
        }
        
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("Player"))
        {
            playerClose = true;
            if(autoStartConversation)
            {
                if(!CursorScript.instance.storeOpen) StartConversation();
            }
            else
            {
                if(!CursorScript.instance.storeOpen) Economy.economy.InstantiateServerMessage("Press 'E' to talk to the shop keeper.", true);
            }
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            playerClose = false;
            //if (Economy.economy.storePanel.gameObject.activeSelf) CloseStore();
        }
    }
    private void StartConversation()
    {
        ConversationManager.Instance.StartConversation(conversation);
    }
    public void OpenStore()
    {
        Economy.economy.PopulateStore(this);
        Economy.economy.storePanel.gameObject.SetActive(true);
        CursorScript.instance.storeOpen = true;
        ConversationManager.Instance.EndConversation();
    }
    public void CloseStore()
    {
        Economy.economy.storePanel.gameObject.SetActive(false);
        CursorScript.instance.storeOpen = false;
    }
    private void LookAt(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // Set animator based on the angle between the current rotation and the target rotation
        float angleToTarget = Vector3.Angle(targetDirection, transform.forward);
        if (angleToTarget > 2) animator.SetFloat("Blend", 0.3f);
        else animator.SetFloat("Blend", 0.0f);

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
