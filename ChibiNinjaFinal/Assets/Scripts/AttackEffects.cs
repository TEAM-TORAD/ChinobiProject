using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    Animator attackAnimator;
    GameObject attackTrails;
    // Start is called before the first frame update
    void Start()
    {
        attackTrails = gameObject.GetComponent<GameObject>();
        attackAnimator = attackTrails.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ColliderOn()
    {
        attackAnimator.SetTrigger("S1");
    }
    public void ColliderOff()
    {
        //
    }

    public void Collider2On()
    {

        attackAnimator.SetTrigger("S1");
    }
    public void Collider2Off()
    {
        //
    }
}
