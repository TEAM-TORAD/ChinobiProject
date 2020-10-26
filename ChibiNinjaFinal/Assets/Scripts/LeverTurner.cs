using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTurner : MonoBehaviour
{
    public bool locked = true, canRevert;
    public WheelTurner wheelTurner;
    private Transform player;
    private Transform angleChecker;
    private Animator animator;
    private bool up = false;
    private bool hintPrinted = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        angleChecker = transform.parent.Find("AngleChecker");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hintPrinted)
        {
            if(CheckAngle())
            {
                hintPrinted = true;
                Economy.economy.InstantiateServerMessage("Use 'E' to push the handle.");
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(CheckAngle())
            {
                if (locked)
                {
                    if (wheelTurner.CheckUnlocked())
                    {
                        // Lever is unlocked
                        locked = false;
                        ChangeLever();
                        wheelTurner.LeverEfect();
                    }
                    else
                    {
                        //Lever is still locked
                        Economy.economy.InstantiateServerMessage("This lever is locked!");
                    }
                }
                else ChangeLever();
            }
            
        }
    }
    private void ChangeLever()
    {
        if (!up)
        {
            up = true;
            animator.SetBool("LeverUp", true);
        }
        else
        {
            if (canRevert)
            {
                up = false;
                animator.SetBool("LeverUp", false);
            }
            else
            {
                Economy.economy.InstantiateServerMessage("This lever is locked!");
            }
        }
    }
    private bool CheckAngle()
    {
        float distance = Vector3.Distance(angleChecker.position, player.position);
        angleChecker.position = new Vector3(angleChecker.position.x, player.position.y, angleChecker.position.z);
        Vector3 targetDir = angleChecker.position - player.position;
        float angleView = Vector3.Angle(targetDir, player.forward);
        if (distance < 2.0f && angleView < 80.0f) return true;
        else return false;
    }
}
