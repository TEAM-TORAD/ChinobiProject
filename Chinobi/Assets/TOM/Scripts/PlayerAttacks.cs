using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//attack fix
public class PlayerAttacks : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;

    public bool isLeft;
    public bool isRight;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("AttackNormal");
        }
       
    }

    
}
