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
    public float playerSpeed = 6;
    public float currentSpeed;

    public bool canMove = true;

    private float inputForwardBack;
    private float inputLeftRight;
    private Vector3 moveDirection;
    private float turnSmoothTime;
    float turnSmoothVelocity;
    private PlayerJump jump;
    private Animator animator;

    //Calculating velocity
    Vector3 lastpos;
    public float velocity;

    private void Awake()
    {
        // Get Player RigidBody from Self
        rb = GetComponent<Rigidbody>();
        // Rotation Smoothing Settings
        turnSmoothTime = 0.1f;
        jump = GetComponent<PlayerJump>();

        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void FixedUpdate()
    {
        //Calculating velocity is the first thing that happens
        //Movement since last frame
        Vector3 movementSinceLastFrame = transform.position - lastpos;
        // Framerate since last frame
        float framerate = 1 / Time.deltaTime;
        //Velocity meters per second (ignoring z)
        velocity = Mathf.Pow( (Mathf.Pow(movementSinceLastFrame.x, 2) + Mathf.Pow(movementSinceLastFrame.z, 2) ), 0.5f) * framerate ;


        MovePlayer();

        // Updating the last position is the last thing that happens
        lastpos = transform.position;
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

        bool rotateCamera = (inputForwardBack != 0 || inputLeftRight != 0);
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        if (rotateCamera) 
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        rb.MovePosition(transform.position + transform.forward.normalized * playerSpeed * currentSpeed * Time.deltaTime);

        #endregion
    }
    public void PlayerSpeed()
    {
        // If the animator is in the PlayerMovement state (blend tree), do regular speed acceleration/deceleration
        if(canMove)
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
        // Player movement is not allowed (player is in another state than PlayerMovement, primarily attacking, )
        else
        {
            if (currentSpeed > 0) currentSpeed -= .04f;
            if (currentSpeed < 0) currentSpeed = 0;
        }
    }



}

