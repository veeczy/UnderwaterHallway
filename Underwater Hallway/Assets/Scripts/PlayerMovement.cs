using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
//using System.Collections.Specialized;

public class PlayerMovement : MonoBehaviour
{
    //variables related to movement speed
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpPower = 7f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float gravity = 10f;

    //variables related to keybinds
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode runKey = KeyCode.LeftShift;

    //variables misc
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    public CharacterController characterController;
    public Camera playerCamera;

    private bool canMove = true;

    //start is called at beginning
    private void Start()
    {
        //characterController = GetComponent<characterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //update is called every frame
    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);



        bool isRunning = Input.GetKey(runKey);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;

        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)

        {

            moveDirection.y = jumpPower;

        }

        else

        {

            moveDirection.y = movementDirectionY;

        }



        if (!characterController.isGrounded)

        {

            moveDirection.y -= gravity * Time.deltaTime;

        }



        if (Input.GetKey(crouchKey) && canMove)

        {

            characterController.height = crouchHeight;

            walkSpeed = crouchSpeed;

            runSpeed = crouchSpeed;



        }

        else

        {

            characterController.height = defaultHeight;

            walkSpeed = 6f;

            runSpeed = 10f;

        }



        characterController.Move(moveDirection * Time.deltaTime);



        if (canMove)
        {

            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;

            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        }


    }

    //fixed update is related to rigidbody and physics
    private void FixedUpdate()
    {
        
    }
}