using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Letterbox_Trigger : MonoBehaviour
{
    public GameObject letterbox;
    
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playLetterbox());
        letterbox.GetComponent<Letterbox>().ShowBar(150, .3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator playLetterbox()
    {
        yield return new WaitForSeconds(14);
        letterbox.GetComponent<Letterbox>().HideBar(.3f);

    }
}
