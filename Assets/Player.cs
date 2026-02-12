using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private CharacterController characterController;
    public float characterSpeed = 5f;
    private float gravity = -9.81f;
    private float verticalVelocity;

    private float characterJumpHeight = 2f;
    private Transform cameraTransform;

    private Animator characterAnimator;

    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    /**
        * This method is responsible for moving the character based on player input.
        * It handles both horizontal movement and jumping mechanics.
    */
    void MoveCharacter()
    {
        // Getting input from the player
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 normalizedMovement = new Vector3(horizontalAxis, 0, verticalAxis).normalized;

        bool isRunning = normalizedMovement.magnitude > 0.1f;
        characterAnimator.SetBool("IsRunning", isRunning);

        if (isRunning)
        {
          float targetAngle =
           Mathf.Atan2(normalizedMovement.x, normalizedMovement.z) * Mathf.Rad2Deg
           + cameraTransform.eulerAngles.y;  

           transform.rotation = Quaternion.Euler(0, targetAngle, 0);
           movementDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }

        else
        {
            movementDirection = Vector3.zero;
        }

        characterController.Move(movementDirection * characterSpeed * Time.deltaTime);

        // Apply gravity
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
      
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * verticalVelocity * Vector3.up);
      
        // Jump action
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            characterAnimator.SetTrigger("Jump");
            verticalVelocity = Mathf.Sqrt(characterJumpHeight * 2f * -gravity);
            
        }
    }

}
