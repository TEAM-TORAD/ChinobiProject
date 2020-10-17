using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
 *  The WeaponColliderScript should be placed on the game object that has the Animator Controller!!! 
 *  The animations will call the methods in this script!!!
 * 
 * 
 * 
 */
public enum WeaponType
{
    SWORD,
    TROWING_STAR
}
public class WeaponColliderScript : MonoBehaviour
{
    
    public Transform weapon1;
    public WeaponType typeWeapon1;

    public Transform weapon2;
    public WeaponType typeWeapon2;

    private Collider weaponsCollider1 = null, weaponsCollider2 = null;

    // Start is called before the first frame update
    void Start()
    {
        weaponsCollider1 = weapon1.GetComponent<Collider>();
        weaponsCollider1.enabled = false;

        if(weapon2 != null)
        {
            weaponsCollider2 = weapon2.GetComponent<Collider>();
            weaponsCollider2.enabled = false;
        }
    }

    public void ColliderOn()
    {
        weaponsCollider1.enabled = true;
    }
    public void ColliderOff()
    {
        weaponsCollider1.enabled = false;
    }

    public void Collider2On()
    {
        weaponsCollider2.enabled = true;
    }
    public void Collider2Off()
    {
        weaponsCollider2.enabled = false;
    }

}
