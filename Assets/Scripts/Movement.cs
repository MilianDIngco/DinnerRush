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
    private InputAction actionLook3D;
    private InputAction actionHeadPosition;

    public CharacterController characterController;
    float speed = 4f;

    // Mouse/keyboard, gamepad
    float mouseSensitivity = 35.0f;
    Vector2 moveInput;
    Vector3 moveDirection;
    Vector2 lookInput;
    float cameraAngle;

    // VR
    Quaternion lookDirection;
    Vector3 headPos;
    Vector3 eulerAngles;
    float pitch;

    public void Awake()
    {
        actionMove = controls.FindAction("Move");
        actionLook = controls.FindAction("Look");
        actionLook3D = controls.FindAction("Look3D");
        actionHeadPosition = controls.FindAction("HeadPosition");

        Cursor.visible = false;
        Screen.lockCursor = true;
    }

    public void OnEnable()
    {
        actionMove.Enable();
        actionLook.Enable();
        actionLook3D.Enable();
        actionHeadPosition.Enable();
    }

    public void OnDisable()
    {
        actionMove.Disable();
        actionLook.Disable();
        actionLook3D.Disable();
        actionHeadPosition.Disable();
    }

    public void Update()
    {
        //transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        // Movement
        moveInput = actionMove.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        characterController.Move(moveDirection * speed * Time.deltaTime);

        if (!VR)
        {
            // Camera movement (mouse/gamepad)
            lookInput = actionLook.ReadValue<Vector2>();
            if (lookInput != Vector2.zero)
            {
                transform.Rotate(Vector3.up, (lookInput.x * mouseSensitivity * Time.deltaTime));
                cameraAngle -= lookInput.y * mouseSensitivity * Time.deltaTime;
                cameraAngle = Mathf.Clamp(cameraAngle, -80, 80);
                camera.transform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);
            }
        }
        else {
            // Camera movement (VR)
                // Rotation
            lookDirection = actionLook3D.ReadValue<Quaternion>();
            eulerAngles = lookDirection.eulerAngles;
            pitch = eulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
            transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0); 
            cameraAngle = Mathf.Clamp(pitch, -80f, 80f);
            camera.transform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);

                // Camera position
            headPos = actionHeadPosition.ReadValue<Vector3>();
            camera.transform.localPosition = headPos;
        }


        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
}