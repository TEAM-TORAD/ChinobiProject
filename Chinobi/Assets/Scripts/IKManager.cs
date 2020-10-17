using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IKManager : MonoBehaviour
{
    // https://docs.unity3d.com/Manual/InverseKinematics.html

    public bool activeIK = false;
    public bool lookAtTarget = false;
    public Transform rightHandObj = null, lookAt = null;
    public float translateWeight = 1.0f;
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.CompareTag("Player"))
        {
            animator = transform.GetComponent<Animator>();
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if(activeIK)
            {

                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, translateWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, translateWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    //animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
                if (lookAtTarget && lookAt != null)
                {

                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookAt.position);
                }
                else
                {
                    animator.SetLookAtWeight(0);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
