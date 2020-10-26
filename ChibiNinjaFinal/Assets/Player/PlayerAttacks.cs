using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//attack fix
public class PlayerAttacks : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;

    public int attackValue = 15;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack");
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.ResetTrigger("Attack");
        }
       
    }

    
}
