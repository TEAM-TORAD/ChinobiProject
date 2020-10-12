using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb;
    public Transform playerCamera;

    [Header("Player Settings")]
    public float playerSpeed;
    public float currentSpeed;

    private float inputForwardBack;
    private float inputLeftRight;
    private Vector3 moveDirection;
    private float turnSmoothTime;
    float turnSmoothVelocity;
    private PlayerJump jump;

    private void Awake()
    {
        // Get Player RigidBody from Self
        rb = GetComponent<Rigidbody>();
        // Player Speed Settings
        playerSpeed = 6;
        // Rotation Smoothing Settings
        turnSmoothTime = 0.1f;
        jump = GetComponent<PlayerJump>();
    }
    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        #region Player Movement and Rotation

        inputForwardBack = Input.GetAxisRaw("Vertical");
        inputLeftRight = Input.GetAxisRaw("Horizontal");

        //move position
        moveDirection = new Vector3(inputLeftRight, 0f, inputForwardBack).normalized;
        //change rotation
       
        PlayerSpeed();
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveCamDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.MovePosition(transform.position + moveCamDirection.normalized * playerSpeed * currentSpeed * Time.deltaTime);   

        #endregion
    }
    public void PlayerSpeed()
    {
        if (moveDirection.magnitude > 0.1)
        {



            if (currentSpeed < 1f)
            {
                currentSpeed += .03f;

            }

            if (currentSpeed > 1f)
            {
                currentSpeed += .01f;

            }

            if (currentSpeed > 2f)
            {
                currentSpeed = 2f;

            }

        }
        else if (moveDirection.magnitude < 0.1f)
        {
            if (currentSpeed > 0)
            {

                if (jump.isGrounded)
                {
                    currentSpeed -= 0.05f;
                }
                else
                {
                    currentSpeed -= 0.001f;
                }
            }
            if (currentSpeed < 0.001)
            {
                currentSpeed = 0f;
            }

        }

    }



}

