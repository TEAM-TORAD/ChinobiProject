using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Transform dummyText;
    public bool attackTutorialStarted;


    private void Start()
    {
        dummyText = GameObject.FindGameObjectWithTag("DummyText").transform;
        dummyText.gameObject.SetActive(false);
    }
    public void StartAttackTutorial()
    {
        if(!attackTutorialStarted)
        {
            attackTutorialStarted = true;
            print("Attack tutorial started.");
            dummyText.gameObject.SetActive(true);
        }
    }
}
