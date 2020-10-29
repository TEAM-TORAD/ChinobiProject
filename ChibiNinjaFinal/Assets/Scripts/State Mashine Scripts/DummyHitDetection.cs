using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DummyHitDetection : MonoBehaviour
{
    public float hits;
    public TextMeshPro text;
    private void Start()
    {
        hits = 0;
    }
    public void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hits == 1)
            {
                text.SetText("Ow, Hit me again!", true);
            }
            if (hits == 2)
            {
                text.SetText("Eek, your enjoying this, go again!", true);
            }
            if (hits == 3)
            {
                text.SetText("Ouch, and again?", true);
            }
            if (hits == 4)
            {
                text.SetText("And Again!", true);
            }

            if (hits == 5)
            {
                text.SetText("Ok, ok, ok, STOP!!", true);
            }
            if(hits == 0)
            {
                text.SetText("Go on Hit me Dummy!", true);
            }
            if(hits == 6)
            {
                text.SetText("You are sadistic!!", true);
            }
            if (hits == 7)
            {
                text.SetText("You sicko!!", true);
            }
            if (hits == 8)
            {
                text.SetText("OK, Jokes Over!! Move along..", true);
            }
            if (hits == 9)
            {
                text.SetText("No, seriously, go talk to the Master!", true);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitBox"))
        {
            hits += 1;
        }

        
    }
}
