using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerTarget : MonoBehaviour
{
    public string racerTag = "Player";
    public RacerNavMeshMovement racerController;
    public RacerTarget nextTarget;
    Target targetMarker;
    // Start is called before the first frame update
    void Start()
    {
        targetMarker = transform.GetComponent<Target>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(racerTag))
        {
            targetMarker.enabled = false;
            if (nextTarget != null) nextTarget.gameObject.SetActive(true);
            else
            {
                if(racerController != null)
                {
                    if(!racerController.raceFinished) racerController.playerFinished = true;
                }
            }
        }
    }
}
