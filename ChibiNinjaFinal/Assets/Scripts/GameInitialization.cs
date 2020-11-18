using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class GameInitialization : MonoBehaviour
{
    public NPCConversation startPanel;


    // Start is called before the first frame update
    void Start()
    {
        QuestManager.instance.StartConversation(startPanel, null);
    }

    public void DoTutorial(bool value)
    {
        if(value) QuestManager.instance.StartAttackTutorial();
        else
        {

        }
    }
}
