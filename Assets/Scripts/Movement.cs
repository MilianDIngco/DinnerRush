using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public bool VR = false;
    public InputActionAsset controls;
    public GameObject camera;

    private InputAction actionMove;
    private InputAction actionLook;

    public CharacterController characterController;
    float speed = 4f;

    // Mouse/keyboard, gamepad
    float mouseSensitivity = 35.0f;
    Vector2 moveInput;
    Vector3 moveDirection;
    Vector2 lookInput;
    float cameraAngle;

    public void Awake()
    {
        actionMove = controls.FindAction("Move");
        actionLook = controls.FindAction("Look");

        Cursor.visible = false;
        Screen.lockCursor = true;
    }

    public void OnEnable()
    {
        actionMove.Enable();
        actionLook.Enable();
    }

    public void OnDisable()
    {
        actionMove.Disable();
        actionLook.Disable();
    }

    public void Update()
    {
        // Stop character input if UI is up
        if (UIManager.Instance.StopCamera()) 
        {
            // Lock camera if stopping
            Cursor.visible = UIManager.Instance.StopCamera();
            Cursor.lockState = UIManager.Instance.StopCamera() ? CursorLockMode.None : CursorLockMode.Locked;  
            return;
        }


        //transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        // Movement
        moveInput = actionMove.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        characterController.Move(moveDirection * speed * Time.deltaTime);

        lookInput = actionLook.ReadValue<Vector2>();
        if (lookInput != Vector2.zero)
        {
            transform.Rotate(Vector3.up, (lookInput.x * mouseSensitivity * Time.deltaTime));
            cameraAngle -= lookInput.y * mouseSensitivity * Time.deltaTime;
            cameraAngle = Mathf.Clamp(cameraAngle, -80, 80);
            camera.transform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);
        }

        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
}