using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] steps;

    [SerializeField]
    private AudioClip[] jumps;

    [SerializeField]
    private AudioClip[] attack_Screams;

    public AudioClip sword_1;
    public AudioClip sword_2;
    public AudioClip sword_3;
    public AudioClip kick_1;
    public AudioClip kick_2;
    public AudioClip kick_3;
    public AudioClip slide;
    public AudioClip bombFuse;
    public AudioClip bombExplode;
    public AudioClip waspBuzz;
    public AudioClip waspDeath;
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

    private AudioClip GetRandomClipSteps()
    {
        return steps[UnityEngine.Random.Range(0, steps.Length)];
    }

    public void Steps()
    {
        AudioClip steps = GetRandomClipSteps();
        audioSource.PlayOneShot(steps);
    }

    private AudioClip GetRandomClipJumps()
    {
        return jumps[UnityEngine.Random.Range(0, jumps.Length)];
    }

    public void Jumps()
    {
        AudioClip jumps = GetRandomClipJumps();
        audioSource.PlayOneShot(jumps);
    }

    private AudioClip GetRandomClipAttack_Screams()
    {
        return attack_Screams[UnityEngine.Random.Range(0, attack_Screams.Length)];
    }

    public void AttackScreams()
    {
        AudioClip attack_Screams = GetRandomClipAttack_Screams();
        audioSource.PlayOneShot(attack_Screams);
    }


    public void Idle()
    {
        audioSource.Stop();
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

    public void Slide()
    {
        audioSource.PlayOneShot(slide);
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
