using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    private InputMaster _controls;
    private UnityEngine.Vector2 _direction;
    static private float xRotation = 0f;

    private void OnEnable()
    {
        _controls = new InputMaster();
        _controls.PlayerActions.CameraMovement.performed += Look;
        _controls.Enable();
    }

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }

    void Look(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<UnityEngine.Vector2>() * mouseSensitivity * Time.deltaTime;

        /*We split up the axis to be able to handle the events seperately.
         in X axis the playerbody rotates and in Y axis the Camera rotates
         */
        float mouseX = _direction.x;
        float mouseY = _direction.y;

        xRotation -= mouseY;
        /*Clamping makes sure the player can't make a 360 turn upwards or downwards*/
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = UnityEngine.Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(UnityEngine.Vector3.up * mouseX);
    }
    private void OnDisable()
    {
        /*Clean Up*/
        _controls.PlayerActions.CameraMovement.performed -= Look;
        _controls.Disable();
    }
}




