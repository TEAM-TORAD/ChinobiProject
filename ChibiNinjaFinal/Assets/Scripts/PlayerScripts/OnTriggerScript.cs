using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerScript : MonoBehaviour
{
    public GameObject affectedGameoject;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private void Awake()
    {
        if (onEnter == null) onEnter = new UnityEvent();
        if (onExit == null) onExit = new UnityEvent();
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject == affectedGameoject)
        {
            onEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject == affectedGameoject)
        {
            onExit.Invoke();
        }
    }
}
