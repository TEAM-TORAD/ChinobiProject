using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackValue = 15;
    public bool friendly = false;
    private bool hasHitATarget;
    public void SetStartValues(int _attackValue, bool _friendly)
    {
        attackValue = _attackValue;
        friendly = _friendly;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            Health healthOther = null;
            if (other.transform.GetComponent<Health>() != null) healthOther = other.transform.GetComponent<Health>();
            else if (other.transform.GetComponentInParent<Health>() != null) healthOther = other.transform.GetComponentInParent<Health>();
            if(healthOther != null)
            {
                if (friendly != healthOther.friendly && !hasHitATarget)
                {
                    hasHitATarget = true;
                    healthOther.TakeDamage(attackValue);
                    print("Attacked " + other.name + " with " + attackValue + ".");
                }
            }
            else Debug.Log("Player hit " + other.transform.name + " with tag " + other.transform.tag + ". The gameobject doesn't have a health script.");
        }
    }
    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        hasHitATarget = false;
    }
}
