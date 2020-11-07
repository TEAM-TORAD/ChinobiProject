using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWayControl : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("DoorwayStay", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("DoorwayStay", false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("DoorwayStay", true);
        }
    }
}
