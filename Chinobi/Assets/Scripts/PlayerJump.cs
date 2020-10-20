using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight;
    public float jumpDistance;
    public float delay;

    private Rigidbody rb;
    private Animator anim;
    public bool isGrounded;

    private PlayerMovement PM;

    [Header("Current Height")]
    public float currentHeight;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        PM = GetComponent<PlayerMovement>();
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
        if(isGrounded)
        {
            
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            StartCoroutine(DelayedForwardForce());
        }
    }
    IEnumerator DelayedForwardForce()
    {
        // Wait until the player is no longer grounded before adding forward speed, in case different materials affect the jump distance while player is grounded (guessing here)
        yield return new WaitUntil(() => !isGrounded);
        // Multiply the jumpDistance with the velocity (meters per second that the player is moving)
        float newJumpDistance = jumpDistance * PM.velocity;
        rb.AddForce(transform.forward * newJumpDistance, ForceMode.Impulse);

        print("Added " + PM.velocity + " multiplied with " + jumpDistance + " = " + newJumpDistance + ".");


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
