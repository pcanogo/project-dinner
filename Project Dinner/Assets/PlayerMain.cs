using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    public CharacterController controller;
    private InputMaster _controls;
    public Transform groundCheck;
    public LayerMask goundMask;
    

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float grounDistance = 0.4f;

    private Vector2 _moveAxis;
    private Vector3 _movement;
    private Vector3 _velocity;
    private bool _isGrounded;

    private void OnEnable()
    {
        /*InputMaster is a class created by the Input System.*/
        _controls = new InputMaster();
        /*We aggregate methods to the InputMaster and enable it */
        _controls.PlayerActions.Jump.performed += Jump;
        _controls.PlayerActions.MoveUpDown.performed += MoveUpDown;
        _controls.PlayerActions.MoveLeftRight.performed += MoveLeftRight;
        _controls.Enable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        /*Creates sphere that checks if the player is touching the ground.*/
        _isGrounded = Physics.CheckSphere(groundCheck.position, grounDistance, goundMask);

        /* Condition to reset the Y velocity once the player is grounded.
        When the player is grounded the velocity becomes negative if not reseted.*/
        if (_isGrounded && _velocity.y < 0)
        {   
           /* -2f is used since it works a bit better than 0 to make sure the 
            player is touching the ground */
            _velocity.y = -2f;
        }

        /*Will merge these into 1 vector3 in future pr.*/
        _movement = transform.right * _moveAxis.x + transform.forward * _moveAxis.y;
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_movement * speed * Time.deltaTime);
        controller.Move(_velocity * Time.deltaTime);

    }

    /*Movement is partitioned into two methods for better input feedback.
     Although in theory, the newq input master has the ability to handle 2D 
    Vector inputs, This worked better.
     */
    private void MoveUpDown(InputAction.CallbackContext ctx)
    {
        _moveAxis.y = ctx.ReadValue<float>();
        //Debug.Log($"Y axis {_moveAxis}");
    }    
    private void MoveLeftRight(InputAction.CallbackContext ctx)
    {
        _moveAxis.x = ctx.ReadValue<float>();
        //Debug.Log($"X axis {_moveAxis}");
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void OnDisable()
    {
        /*Clean up to prevent memrory leak*/
        _controls.PlayerActions.Jump.performed -= Jump;
        _controls.PlayerActions.MoveUpDown.performed -= MoveUpDown;
        _controls.PlayerActions.MoveLeftRight.performed -= MoveLeftRight;
        _controls.Disable();
    }
}
