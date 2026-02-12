using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float rotationSpeed = 700f;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
         controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
         float horizontal = -Input.GetAxis("Vertical");
        float vertical = Input.GetAxis("Horizontal");


        // Calculate movement direction
        moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection.Normalize(); // Make sure diagonal movement isn't faster
        
        // Apply gravity
        moveDirection.y = -9.81f;
        
        // Move the player
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        // Rotate player to face movement direction
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
