using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_PlayerAnimator : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement player;
    public PlayerJump playerJump;

    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    public void Update()
    {
        anim.SetFloat("PlayerSpeed", player.currentSpeed);
        anim.SetFloat("PlayerHeight", player.GetComponent<PlayerJump>().currentHeight);
    }
}
