using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

[System.Serializable]
public class Mission
{
    public string missionName;
    public NPCConversation conversation;
    public bool completed = false;
}
public class InteractionsMaster : MonoBehaviour
{
    public Mission[] missions;
    public bool autoStartConversation = true;
    private bool playerClose;
    private int activeMissionIndex = 0;

    private void Update()
    {
        if(playerClose && Input.GetKeyDown(KeyCode.E))
        {
            StartACtiveConversation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
            if (autoStartConversation) StartACtiveConversation();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerClose = false;
    }
    public void SetConversation(string name)
    {
        bool matchFound = false;
        int i = 0;
        foreach(Mission m in missions)
        {
            if(name == m.missionName)
            {
                activeMissionIndex = i;
                matchFound = true;
                break;
            }
            ++i;
        }
        if (!matchFound) Debug.LogError("No matching mission-name found!");
    }
    void StartACtiveConversation()
    {
        ConversationManager.Instance.StartConversation(missions[activeMissionIndex].conversation);

    }
}
