using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight;
    public float delay;

    private Rigidbody rb;
    private Animator anim;
    public bool isGrounded;

    

    [Header("Current Height")]
    public float currentHeight;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        delay = 0.45f;

    }

    public void Update()
    {
        GroundCheck();

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Jump");
                StartCoroutine(DelayJump());
            }
        }
    }
    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(delay);
        Jump();
    }
    private void Jump()
    {
        if (isGrounded)
        {
            //rb.velocity = Vector3.up * jumpHeight;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        

    }
    public void GroundCheck()
    {
        Vector3 downVector = transform.TransformDirection(Vector3.down) * 100f;
        // Debug.DrawRay(transform.position, downVector, Color.green);

        if (Physics.Raycast(transform.position, (downVector), out RaycastHit hit))
        {
            currentHeight = hit.distance;

            if (hit.distance > 0.1f)
            {
                isGrounded = false;
                anim.SetBool("isGrounded", false);
            }
            if (hit.distance < 0.1f)
            {
                isGrounded = true;
                anim.SetBool("isGrounded", true);
            }
        }
    }
}
