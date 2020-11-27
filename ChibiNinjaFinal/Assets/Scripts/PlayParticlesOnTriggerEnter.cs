using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticlesOnTriggerEnter : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public bool playOnlyOnce = true;
    private bool hasPlayed = false;
    public string[] targetTags;
    private void OnTriggerEnter(Collider other)
    {
        if(playOnlyOnce && !hasPlayed)
        {
            PlayTheParticle(other);
        }
        else if(!playOnlyOnce)
        {
            PlayTheParticle(other);
        }
    }
    void PlayTheParticle(Collider other)
    {
        foreach (string s in targetTags)
        {
            if (other.CompareTag(s))
            {
                hasPlayed = true;
                if (!particleSystem.isEmitting) particleSystem.Play();
            }
        }
    }
}
