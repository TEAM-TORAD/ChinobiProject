using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTurner : MonoBehaviour
{
    private Animator animator;
    private Transform angleChecker;
    public WheelTurner wheelTurner;
    public Transform player;
    private bool hintPrinted = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        angleChecker = transform.parent.Find("AngleChecker");
    }

    // Update is called once per frame
    void Update()
    {
        if(!hintPrinted && CheckAngle())
        {
            hintPrinted = true;
            Economy.economy.InstantiateServerMessage("Use 'E' to push the button!");
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(CheckAngle())
            {
                animator.SetTrigger("ButtonPress");
                wheelTurner.NextStatue();
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
