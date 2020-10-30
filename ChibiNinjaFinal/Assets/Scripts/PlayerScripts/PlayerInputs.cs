using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    // Default Button Strings (May need to re-assign in Input Manager).
    #region Button String Assignments
    public string jumpButton = "Jump";              
    public string leftMouse = "Fire1";
    public string rightMouse = "Aim";
    public string middleMouse = "Fire3";
    public string sprintButton = "Sprint";
    #endregion

    #region Other
    public bool conversationOpen = false, menuOpen = false;
    private bool cursorLocked;
    #endregion

    // Bools for Holding Button States.
    #region Button Held Bools
    public bool isHoldingJump;                          
    public bool isHoldingAttack;
    public bool isHoldingKick;
    public bool isHoldingBlock;
    public bool isHoldingSprint;
    #endregion

    // Component
    #region Components Used
    private Animator anim;

    private Stamina stamina;
    #endregion

    private void Start()
    {
        // Find animator on current GameObject
        #region Locate Initial Component
        anim = gameObject.GetComponent<Animator>();
        stamina = GetComponent<Stamina>();
        #endregion
    }
    private void Update()
    {
        print("Cursor locked = " + CursorScript.instance.cursorLocked);
        if(CursorScript.instance.cursorLocked)
        {
            #region Jump Button - SpaceBar
            if (Input.GetButton(jumpButton))
            {
                HoldingJump();
            }
            if (Input.GetButtonDown(jumpButton))
            {
                anim.SetTrigger("JumpPressed");
            }
            if (Input.GetButtonUp(jumpButton))
            {
                isHoldingJump = false;
                anim.SetBool("JumpHeld", false);
                anim.ResetTrigger("JumpPressed");
            }
            #endregion
            #region Attack Button - LeftMouse
            if (Input.GetButton(leftMouse))
            {
                HoldingAttack();
            }
            if (Input.GetButtonDown(leftMouse))
            {
                anim.SetTrigger("AttackPressed");
            }
            if (Input.GetButtonUp(leftMouse))
            {
                isHoldingAttack = false;
                anim.SetBool("AttackHeld", false);
                anim.ResetTrigger("AttackPressed");
            }
            #endregion
            #region Block Button - RightMouse

            if (Input.GetButtonDown(rightMouse))
            {
                anim.SetTrigger("BlockPressed");
                HoldingBlock();
            }
            /*
            else if (Input.GetButton(rightMouse) && stamina.stamina > 0)
            {
                HoldingBlock();
            }*/
            if (Input.GetButtonUp(rightMouse))
            {
                StopBlocking();
            }
            if (stamina.stamina <= 0 && isHoldingBlock)
            {
                anim.SetBool("ShieldBreak", true);
                StopBlocking();
            }
            #endregion
            #region Kick Button - MiddleMouse
            if (Input.GetButton(middleMouse))
            {
                HoldingKick();
            }
            if (Input.GetButtonDown(middleMouse))
            {
                anim.SetTrigger("KickPressed");
            }
            if (Input.GetButtonUp(middleMouse))
            {
                isHoldingKick = false;
                anim.SetBool("KickHeld", false);
                anim.ResetTrigger("KickPressed");
            }
            #endregion
            #region Sprint Button - LeftShift
            if (Input.GetButton(sprintButton))
            {
                HoldingSprint();
            }
            if (Input.GetButtonDown(sprintButton))
            {
                anim.SetTrigger("SprintPressed");
            }
            if (Input.GetButtonUp(sprintButton))
            {
                isHoldingSprint = false;
                anim.SetBool("SprintHeld", false);
                anim.ResetTrigger("SprintPressed");
            }
            #endregion
        }
        // Listens for player Inputs Pressed or Held, and lets the Animator know to Trigger if only pressed

    }

    // Methods for handling Held Button inputs and letting the Animator know its been Held
    #region Held Button Methods
    void HoldingJump()
    {
        if(stamina.stamina > 0)
        {
            isHoldingJump = true;
            anim.SetBool("JumpHeld", true);
        }  
        else
        {
            isHoldingJump = false;
            anim.SetBool("JumpHeld", false);
        }
    }
    void HoldingAttack()
    {
        isHoldingAttack = true;
        anim.SetBool("AttackHeld", true);        
    }

    void HoldingBlock()
    {
        isHoldingBlock = true;
        anim.SetBool("BlockHeld", true);
    }
    public void StopBlocking()
    {
        isHoldingBlock = false;
        anim.SetBool("BlockHeld", false);
        anim.ResetTrigger("BlockPressed");
        stamina.usingStamina = false;
    }
    public void UseShield()
    {
        stamina.usingStamina = true;
    }

    void HoldingKick()
    {
        isHoldingKick = true;
        anim.SetBool("KickHeld", true);
    }

    void HoldingSprint()
    {
        isHoldingSprint = true;
        anim.SetBool("SprintHeld", true);
    }
    #endregion
}
