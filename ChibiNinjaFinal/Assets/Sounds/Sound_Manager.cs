using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip footSteps;
    public AudioClip sword_1;
    public AudioClip sword_2;
    public AudioClip sword_3;
    public AudioClip sword_302;
    public AudioClip kick_1;
    public AudioClip kick_2;
    public AudioClip kick_3;
    public AudioClip slide;
    public AudioClip bombFuse;
    public AudioClip bombExplode;
    public AudioClip waspBuzz;
    public AudioClip waspDeath;
    public AudioClip jumping;
    public AudioClip jumpingMoving;
    public AudioClip jumpingAttack;
    public AudioClip jumpKick;
    public AudioClip jumpFrontFlip;
    public AudioClip jumpHighroll;
    public AudioClip shield;
    public AudioClip shieldBreak;
    public AudioClip shieldBreak_Scream;
    
    public AudioClip rolling;
    
    


    #region Player
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Idle()
    {
        audioSource.Stop();
    }

    public void Steps()
    {
        audioSource.PlayOneShot(footSteps);
    }

    public void Sword_1()
    {
        audioSource.PlayOneShot(sword_1);
    }

    public void Sword_2()
    {
        audioSource.PlayOneShot(sword_2);
    }

    public void Sword_3()
    {
        audioSource.PlayOneShot(sword_3);
    }

    public void Sword_302()
    {
        audioSource.PlayOneShot(sword_302);
    }

    public void Slide()
    {
        audioSource.PlayOneShot(slide);
    }

    public void Jumping()
    {
        audioSource.PlayOneShot(jumping);
    }

    public void JumpFrontFlip()
    {
        audioSource.PlayOneShot(jumpFrontFlip);
    }

    public void JumpHighroll()
    {
        audioSource.PlayOneShot(jumpHighroll);
    }

    public void JumpKick()
    {
        audioSource.PlayOneShot(jumpKick);
    }

    public void JumpingMoving()
    {
        audioSource.PlayOneShot(jumpingMoving);
    }

    public void JumpingAttack()
    {
        audioSource.PlayOneShot(jumpingAttack);
    }

    public void ShieldBreak()
    {
        audioSource.PlayOneShot(shieldBreak);
    }

    public void ShieldBreakScream()
    {
        audioSource.PlayOneShot(shieldBreak_Scream);
    }

    public void Kick_1()
    {
        audioSource.PlayOneShot(kick_1);
    }

    public void Kick_2()
    {
        audioSource.PlayOneShot(kick_2);
    }

    public void Kick_3()
    {
        audioSource.PlayOneShot(kick_3);
    }

    public void Rolling()
    {
        audioSource.PlayOneShot(rolling);
    }

    public void Shield()
    {
        audioSource.PlayOneShot(shield);
    }

    

    #endregion

    #region Bomb
    public void BombFuse()
    {
        audioSource.PlayOneShot(bombFuse);
    }

    public void BombExplode()
    {
        audioSource.PlayOneShot(bombExplode);
    }

    #endregion

    #region Wasp


    public void WaspBuzz()
    {
        audioSource.PlayOneShot(waspBuzz);
    }

    public void WaspDeath()
    {
        audioSource.PlayOneShot(waspDeath);
    }



    #endregion
}
