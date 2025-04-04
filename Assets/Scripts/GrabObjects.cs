using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabObjects : MonoBehaviour
{
    public InputActionAsset controls;
    public GameObject camera;

    private InputAction actionInteract;
    private InputAction actionInteractLeft;
    private InputAction actionInteractRight;

    public GameObject hand;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject holdingObjectHand;
    public GameObject holdingObjectLeftHand;
    public GameObject holdingObjectRightHand;

    private bool holding = false;
    private bool holdingLeft = false, holdingRight = false;

    public void Awake()
    {
        actionInteract = controls.FindAction("Interact");
        actionInteractLeft = controls.FindAction("InteractLeft");
        actionInteractRight = controls.FindAction("InteractRight");
    }

    public void OnEnable()
    {
        actionInteract.Enable();
        actionInteractLeft.Enable();
        actionInteractRight.Enable();
    }

    public void OnDisable()
    {
        actionInteract.Disable();
        actionInteractLeft.Disable();
        actionInteractRight.Disable();
    }

    public void Update()
    {
        if (actionInteract.WasPressedThisFrame())
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                if (hit.collider.gameObject.tag == "Ingredient") {
                    holding = true;
                    holdingObjectHand = hit.collider.gameObject;
                    holdingObjectHand.transform.SetParent(hand.transform);
                    holdingObjectHand.transform.localPosition = Vector3.zero;
                    holdingObjectHand.transform.rotation = Quaternion.Euler(Vector3.zero);
                    holdingObjectHand.GetComponent<Collider>().enabled = false;
                    Rigidbody rb = holdingObjectHand.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
        else if (actionInteract.WasReleasedThisFrame())
        {
            if (holding)
            {
                holding = false;
                holdingObjectHand.transform.SetParent(null);
                holdingObjectHand.GetComponent<Collider>().enabled = true;
                holdingObjectHand.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}