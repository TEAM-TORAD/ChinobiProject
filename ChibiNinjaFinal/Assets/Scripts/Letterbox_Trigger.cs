using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterbox_Trigger : MonoBehaviour
{
    public GameObject letterbox;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            letterbox.GetComponent<Letterbox>().ShowBar(105, .3f);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            letterbox.GetComponent<Letterbox>().HideBar(.3f);
        }
    }
}
