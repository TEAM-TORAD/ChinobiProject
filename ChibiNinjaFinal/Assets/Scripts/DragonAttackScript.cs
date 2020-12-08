using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackScript : MonoBehaviour
{

    private GameObject meleeTrigger;
    public Transform mouth;
    public GameObject projectile;
    public int biteAttack = 20, projectileAttack = 25;
    public float projectileSpeed = 3.0f;
    public bool isFriendly = false;
    private Transform player;

    public AudioSource dragonAudio;
    public AudioClip spit;


    private void Start()
    {
        meleeTrigger = mouth.Find("MeleeAttack").gameObject;
        meleeTrigger.SetActive(false);
        meleeTrigger.GetComponent<Attack>().SetStartValues(biteAttack, isFriendly);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void ActivateMeleeTrigger()
    {
        meleeTrigger.SetActive(true);
    }
    public void DeactivateMeleeTrigger()
    {
        meleeTrigger.SetActive(false);
    }
    public void FireProjectile()
    {
        GameObject fireBall = Instantiate(projectile, mouth.position, Quaternion.identity, null);
        fireBall.GetComponent<ProjectileScript>().SetStartValues(projectileAttack, projectileSpeed, 5.0f, isFriendly, player, transform.gameObject);
        dragonAudio.PlayOneShot(spit);
    }
}
