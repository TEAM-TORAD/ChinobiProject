using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    Animator attackAnimator;
    // Start is called before the first frame update
    void Start()
    {
        attackAnimator = transform.parent.Find("Attack Trails").GetComponent<Animator>();
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
