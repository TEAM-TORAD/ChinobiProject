using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackValue = 15;
    public bool friendly = false;

    public void SetStartValues(int _attackValue, bool _friendly)
    {
        attackValue = _attackValue;
        friendly = _friendly;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            if (friendly)
            {
                if (other.transform.CompareTag("WaspNPC"))
                {
                    if (other.transform.GetComponentInParent<Health>() != null) other.transform.GetComponentInParent<Health>().TakeDamage(attackValue);
                    else Debug.LogError("No health script on " + other.transform.parent.name + ".");
                    transform.gameObject.SetActive(false);
                }
                else if (other.transform.CompareTag("ExplodingNPC"))
                {
                    if (other.transform.GetComponent<Health>() != null) other.transform.GetComponent<Health>().TakeDamage(attackValue);
                    else Debug.LogError("No health script on " + other.transform.name + ".");
                    transform.gameObject.SetActive(false);
                }
                else print(other.transform.name + " with tag " + other.transform.tag);
            }
            else
            {
                if (other.transform.CompareTag("Player"))
                {
                    if (other.transform.GetComponent<Health>() != null) other.transform.GetComponent<Health>().TakeDamage(attackValue);
                    else Debug.LogError("No health script on " + other.transform.name + ".");
                    transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
