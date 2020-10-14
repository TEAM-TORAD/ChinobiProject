using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public Animator anim;
    public bool isBlocking;
    private Stamina stamina;
    public GameObject blockSphere;
    //public GameObject onSphereDrained;
    private MeshRenderer thisMesh;

    //http://gyanendushekhar.com/2018/09/16/change-material-and-its-properties-at-runtime-unity-tutorial/
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stamina = GetComponent<Stamina>();
        thisMesh = blockSphere.GetComponent<MeshRenderer>();
        blockSphere.SetActive(false);
    }

    public void Update()
    {
        if(isBlocking)
        {
            if(stamina.stamina > 0)
            {
                //thisMesh.material.SetFloat("_NormalStrength2", 1 - (stamina.stamina / stamina.maxStamina));
            }
            else
            {
                /*GameObject drainSphere = Instantiate(onSphereDrained, blockSphere.transform.parent);
                drainSphere.transform.localPosition = blockSphere.transform.localPosition;*/
                StopBlock();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && stamina.stamina > 5)
        {
            anim.SetBool("Block", true);
            isBlocking = true;
            stamina.usingStamina = true;
            blockSphere.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopBlock();
        }
    }
    private void StopBlock()
    {
        anim.SetBool("Block", false);
        isBlocking = false;
        stamina.usingStamina = false;

        blockSphere.SetActive(false);
    }
}
