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

    void Awake()
    { 
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter();
        animateCharacter();
        // Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        // characterController.Move(movement * Time.deltaTime * characterSpeed);
    }

    /**
        * This method is responsible for moving the character based on player input.
        * It handles both horizontal movement and jumping mechanics.
    */
    private void moveCharacter()
    {
        // Getting input from the player
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 normalizedMovement = new Vector3(horizontalAxis, 0, verticalAxis).normalized;

        // Normal player movement
        if (normalizedMovement.magnitude > 0.1f)
        {
          float targetAngle =
           Mathf.Atan2(normalizedMovement.x, normalizedMovement.z) * Mathf.Rad2Deg
           + cameraTransform.eulerAngles.y;  

           transform.rotation = Quaternion.Euler(0, targetAngle, 0);
           movementDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

           characterController.Move(movementDirection * characterSpeed * Time.deltaTime);
        }

        // Apply gravity
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
      
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
      
        // Jump action
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            characterAnimator.SetTrigger("Jump");
            verticalVelocity = Mathf.Sqrt(characterJumpHeight * 2f * -gravity);
            
        }
    }

    private void animateCharacter()
    {
        float currentCharacterSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        characterAnimator.SetFloat("Speed", 1f * currentCharacterSpeed);
        characterAnimator.SetBool("IsGrounded", characterController.isGrounded);
    }
}
