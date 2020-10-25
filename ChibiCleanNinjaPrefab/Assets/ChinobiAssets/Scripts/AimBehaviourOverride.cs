using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AimBehaviourOverride : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AimBehaviourBasic>().AimOn();
        if (Input.GetButtonUp("Fire1"))
        {
            animator.gameObject.GetComponent<NinjaStarShoot>().ThrowNinjaStar();          
        }
    } 
}
