using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DummyHitDetection : MonoBehaviour
{
    public float hits;
    public InteractionsMaster interactionsMaster;
    private bool playerClose;
    public bool started, firstHint, secondHint, thirdHint, fourthHint, fifthHint, complete;
    private PlayerInputs playerInputs;
    private Animator playerAnimator;
    private float timer;
    private string currentHint = "";
    private ParticleSystem PS;
    private Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PS = transform.parent.Find("Particle System").GetComponent<ParticleSystem>();
        playerInputs = player.GetComponent<PlayerInputs>();
        playerAnimator = player.GetComponent<Animator>();
        hits = 0;

    }
    private void Update()
    {
        if(playerClose && started)
        {
            if(firstHint && !secondHint)
            {
                if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ) {
                    secondHint = true;
                    currentHint = "Hold left mouse button to do a combo attack!!";
                    Economy.economy.DestroyOldMessages();
                    Economy.economy.InstantiateServerMessage(currentHint, false);
                }
            }
            else if(secondHint && !thirdHint)
            {
                // Check if player is holding down the left mouse button and the animator state is in the third combo
                if (Input.GetButton(playerInputs.leftMouse) && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                {
                        thirdHint = true;
                        currentHint = "Nice! Click the middle mouse button to kick.";
                        Economy.economy.DestroyOldMessages();
                        Economy.economy.InstantiateServerMessage(currentHint, false);
                }
            }
            else if(thirdHint && !fourthHint)
            {
                if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Kick1"))
                {
                    fourthHint = true;
                    currentHint = "Hold the middle-mouse-button to do a kick-combo.";
                    Economy.economy.InstantiateServerMessage("You're a champ!", true);
                    Economy.economy.DestroyOldMessages();
                    Economy.economy.InstantiateServerMessage(currentHint, false);
                }
            }
            else if(fourthHint && !fifthHint)
            {
                // Check if player is holding down the middle mouse button and the animator state is in the third combo
                if (Input.GetButton(playerInputs.middleMouse) && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Kick3"))
                {
                    timer = 0;
                    fifthHint = true;
                    currentHint = "Hold the right mouse button while standing still to block.";
                    Economy.economy.DestroyOldMessages();
                    Economy.economy.InstantiateServerMessage("You are just like Jet Li!", true);
                    Economy.economy.InstantiateServerMessage(currentHint, false);
                }
            }
            else if(fifthHint && !complete)
            {
                // Check if player is holding down the right mouse button for a while...
                if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShieldBreak"))
                {
                    timer = 0;
                    complete = true;
                    currentHint = "You've completed the basics. Return to the Master.";
                    Economy.economy.DestroyOldMessages();
                    Economy.economy.InstantiateServerMessage("The shield is using stamina. If you run out, the shield breaks.", true);
                    Economy.economy.InstantiateServerMessage(currentHint, false);

                    // Set new conversation on the master
                    interactionsMaster.SetConversation("Dummy completed");
                }
            }
        }
        else if(!playerClose && started && !complete && !CursorScript.instance.conversationOpen)
        {
            if(Input.GetButtonDown(playerInputs.leftMouse) || Input.GetButtonDown(playerInputs.middleMouse) || Input.GetButtonDown(playerInputs.rightMouse))
            {
                Economy.economy.InstantiateServerMessage("Get closer to the dummy!", true);
            }
        }
    }
    private void OnCollisionEnter(Collision c)
    {
        
        if (c.transform.CompareTag("HitBox") )
        {
            // Set the rotation of the particle effect to face player
            Vector3 newRot = PS.transform.rotation.eulerAngles;
            newRot.y = player.rotation.eulerAngles.y;
            newRot.y -= 180;
            PS.transform.rotation = Quaternion.Euler(newRot);

            PS.Play();

            if(started) ++hits;
        }
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("Player"))
        {
            playerClose = true;
            if(!firstHint && started)
            {
                currentHint = "Press left mouse-button to attack.";
                Economy.economy.DestroyOldMessages();
                Economy.economy.InstantiateServerMessage(currentHint, false);
                firstHint = true;
            }
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if(c.CompareTag("Player"))
        {
            playerClose = false;
            if (started && !complete) Economy.economy.InstantiateServerMessage("Where are you going? Training isn't completed yet!", true);
        }
    }

    public void StartTutorial()
    {
        started = true;
        Economy.economy.InstantiateServerMessage("Head over to the training dummy!", true);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitBox"))
        {
            hits += 1;
            if (hits == 1)
            {
                text.SetText("Ow, Hit me again!", true);
            }
            if (hits == 2)
            {
                text.SetText("Eek, your enjoying this, go again!", true);
            }
            if (hits == 3)
            {
                text.SetText("Ouch, and again?", true);
            }
            if (hits == 4)
            {
                text.SetText("And Again!", true);
            }

            if (hits == 5)
            {
                text.SetText("Ok, ok, ok, STOP!!", true);
            }
            if (hits == 0)
            {
                text.SetText("Go on Hit me Dummy!", true);
            }
            if (hits == 6)
            {
                text.SetText("You are sadistic!!", true);
            }
            if (hits == 7)
            {
                text.SetText("You sicko!!", true);
            }
            if (hits == 8)
            {
                text.SetText("OK, Jokes Over!! Move along..", true);
            }
            if (hits == 9)
            {
                text.SetText("No, seriously, go talk to the Master!", true);
            }
        }
    }*/
}
