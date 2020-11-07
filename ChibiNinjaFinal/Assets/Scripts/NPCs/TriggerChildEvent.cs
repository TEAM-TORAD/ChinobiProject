using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChildEvent : MonoBehaviour
{
    private NPCInteraction nPCInteraction;
    private void Start()
    {
        nPCInteraction = GetComponentInChildren<NPCInteraction>();
    }
    private void OnTriggerEnter(Collider c)
    {
        nPCInteraction.EnteringTrigger(c);
    }
    private void OnTriggerExit(Collider c)
    {
        nPCInteraction.ExitingTrigger(c);
    }
}
