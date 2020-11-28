using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCluck : MonoBehaviour
{
    public AudioClip cluck, distress;
    public float randomTimeMin = 10f, randomTimeMax = 40f;
    private float timer, timeToNext;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        timeToNext = SetRandomTime();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= timeToNext)
        {
            audio.clip = cluck;
            audio.Play();
            timeToNext = SetRandomTime();
            timer = 0;
        }
        timer += Time.deltaTime;
    }
    float SetRandomTime()
    {
        return Random.Range(randomTimeMin, randomTimeMax);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            timer = 0;
            timeToNext = SetRandomTime();
            if(!audio.isPlaying)
            {
                audio.clip = cluck;
                audio.Play();
            }
            
        }
        else if(collision.transform.CompareTag("HitBox"))
        {
            timer = 0;
            timeToNext = SetRandomTime();
            if (!audio.isPlaying)
            {
                audio.clip = distress;
                audio.Play();
            }
        }
    }
}
