using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ShopKeeperNPC : MonoBehaviour
{
    public Item[] items;
    public bool passive = true;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetComponentInChildren<NPCInteraction>() != null) transform.GetComponentInChildren<NPCInteraction>().passive = passive;
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
