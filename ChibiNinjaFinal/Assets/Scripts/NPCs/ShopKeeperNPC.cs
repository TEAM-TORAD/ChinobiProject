using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ShopKeeperNPC : MonoBehaviour
{
    public bool autoStartConversation = false;
    public NPCConversation conversation;
    public Item[] items;
    private Animator animator;
    public bool passive = true;
    private Transform player;
    public Transform lookAtObject;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (transform.GetComponent<NPCInteraction>() != null) transform.GetComponent<NPCInteraction>().passive = passive;
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
}
