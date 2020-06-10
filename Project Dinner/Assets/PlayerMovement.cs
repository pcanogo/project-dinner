using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask goundMask;


    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private InputMaster _controls;
    private Vector2 _moveAxis;
    private Vector3 _direction;
    private Vector3 _movement;
    private Vector3 _velocity;
    private bool _isGrounded;
    private const float groundCheckMod = -2f;

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

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float groundDistance = 0.4f;
        /*Creates sphere that checks if the player is touching the ground.*/
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, goundMask);

        /* Condition to reset the Y velocity once the player is grounded.
        When the player is grounded the velocity becomes negative if not reseted.*/
        if (_isGrounded && _velocity.y < 0)
        {
            /* -2f is used since it works a bit better than 0 to make sure the 
             player is touching the ground */
            _velocity.y = groundCheckMod;
        }

        /*Will merge these into 1 vector3 in future pr.*/
        _direction = new Vector3(_moveAxis.x, 0f, _moveAxis.y);
        _velocity.y += gravity * Time.deltaTime;

        /*Rotate the player in the direction it is moving in*/
        if (_direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            _movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(_movement.normalized * speed * Time.deltaTime);
        }
        controller.Move(_velocity * Time.deltaTime);
    }

    /*Movement is partitioned into two methods for better input feedback.
     Although in theory, the new input master has the ability to handle 2D 
    Vector inputs, This worked better.
     */
    private void MoveUpDown(InputAction.CallbackContext ctx)
    {
        _moveAxis.y = ctx.ReadValue<float>();
        /*Debug.Log($"Y axis {_moveAxis}");*/
    }
    private void MoveLeftRight(InputAction.CallbackContext ctx)
    {
        _moveAxis.x = ctx.ReadValue<float>();
        /*Debug.Log($"X axis {_moveAxis}");*/
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * groundCheckMod * gravity);
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
