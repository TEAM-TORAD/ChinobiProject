using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedActivation : MonoBehaviour
{
    public float delayTime;
    public GameObject[] objects;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }
    void Start()
    {
        Invoke("Activate", delayTime);
    }
    private void Activate()
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
