using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToSwim : MonoBehaviour
{
    public GameObject playerMesh;
    public GameObject snorkle;
    public GameObject sushiBag;
    public GameObject armature;
    public GameObject slash;
    public GameObject player;
    public AudioSource snorkleSource;
    public AudioClip toonExplosion;


    private void Start()
    {
        snorkle.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerMesh.SetActive(false);
            snorkle.SetActive(true);
            sushiBag.SetActive(false);
            armature.SetActive(false);
            player.GetComponent<PlayerInputs>().enabled = false;
            player.GetComponent<AudioSource>().enabled = false;
            snorkleSource.PlayOneShot(toonExplosion);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerMesh.SetActive(true);
            snorkle.SetActive(false);
            sushiBag.SetActive(true);
            armature.SetActive(true);
            player.GetComponent<PlayerInputs>().enabled = true;
            player.GetComponent<AudioSource>().enabled = true;
        }
     
    }


}
