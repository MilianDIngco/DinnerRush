using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputActionAsset controls;
    public GameObject camera;

    private InputAction actionMove;
    private InputAction actionLook;
    private InputAction actionLook3D;
    private InputAction actionHeadPosition;
    private InputAction actionInteract;
    private InputAction actionInteractLeft;
    private InputAction actionInteractRight;
    private InputAction actionJump;

    public CharacterController characterController;
    float speed = 4f;

    // Mouse/keyboard, gamepad
    float mouseSensitivity = 35.0f;
    Vector2 moveInput;
    Vector3 moveDirection;
    Vector2 lookInput;
    float cameraAngle;

    // VR
    Vector3 lookDirection;
    Vector3 headPos;

    public void Awake()
    {
        actionMove = controls.FindAction("Move");
        actionLook = controls.FindAction("Look");
        actionLook3D = controls.FindAction("Look3D");
        actionHeadPosition = controls.FindAction("HeadPosition");
        actionInteract = controls.FindAction("Interact");
        actionInteractLeft = controls.FindAction("InteractLeft");
        actionInteractRight = controls.FindAction("InteractRight");
        actionJump = controls.FindAction("Jump");
    }

    public void OnEnable()
    {
        actionMove.Enable();
        actionLook.Enable();
        actionLook3D.Enable();
        actionHeadPosition.Enable();
        actionInteract.Enable();
        actionInteractLeft.Enable();
        actionInteractRight.Enable();
        actionJump.Enable();
    }

    public void OnDisable()
    {
        actionMove.Disable();
        actionLook.Disable();
        actionLook3D.Disable();
        actionHeadPosition.Disable();
        actionInteract.Disable();
        actionInteractLeft.Disable();
        actionInteractRight.Disable();
        actionJump.Disable();
    }

    public void Update()
    {
        // Movement
        moveInput = actionMove.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        characterController.Move(moveDirection * speed * Time.deltaTime);

        // Camera movement (mouse/gamepad)
        lookInput = actionLook.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, (lookInput.x * mouseSensitivity * Time.deltaTime));
        cameraAngle -= lookInput.y * mouseSensitivity * Time.deltaTime;
        cameraAngle = Mathf.Clamp(cameraAngle, -80, 80);
        camera.transform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);

        // Camera movement (VR)
            // Rotation
        lookDirection = actionLook3D.ReadValue<Vector3>();
        if (lookDirection != Vector3.zero)
        {
            camera.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
            // Vertical position
        headPos = actionHeadPosition.ReadValue<Vector3>();
        camera.transform.localPosition = headPos;
    }
}