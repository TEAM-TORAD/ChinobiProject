using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RacerController : MonoBehaviour
{
    DisplayInteractions displayInteractions;
    private TMP_Text text;
    public bool raceActive, conversationOpen;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetComponent<DisplayInteractions>() != null) displayInteractions = transform.GetComponent<DisplayInteractions>();
        displayInteractions.OnSpeachBubbleOpen.AddListener(ActivateQuestion);
        displayInteractions.OnSpeachBubbleClose.AddListener(EndConversation);
        text = transform.GetComponent<DisplayInteractions>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateQuestion()
    {
        /*
        if (!raceActive && Input.GetKeyDown(KeyCode.E))
        {
            text.text = "Wanna race me around the bay? \n Y or N";
            conversationOpen = true;
        }
        if (deliveryQuestActive)
        {
            text.text = "Are you sick of Ubering? \n Y or N";
            conversationOpen = true;
            quitWorkConversation = true;
        }*/
    }
    public void EndConversation()
    {
        //active = false;
        conversationOpen = false;
    }
}
