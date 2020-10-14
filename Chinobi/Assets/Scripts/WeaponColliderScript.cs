using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    
    public WeaponType type;
    public Transform weapon;
    private Collider weaponsCollider;
    // Start is called before the first frame update
    void Start()
    {
        weaponsCollider = weapon.GetComponent<Collider>();
        weaponsCollider.enabled = false;
        //weapon = transform.Find("WeaponSlot").transform;
    }

    public void ColliderOn()
    {
        weaponsCollider.enabled = true;
    }
    public void ColliderOff()
    {
        weaponsCollider.enabled = false;
    }
    
}
