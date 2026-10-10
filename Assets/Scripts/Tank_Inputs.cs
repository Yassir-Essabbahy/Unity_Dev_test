using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Tank_Inputs : MonoBehaviour
{

    public Camera camera1;

    public Vector3 reticlePosition;
    public Vector3 ReticlePosition
    {
        get { return reticlePosition; }
    }

    private Vector3 reticleNormal;
    public Vector3 ReticleNormal
    {
        get { return reticleNormal; }
    }

    private float forwardInput;
    public float ForwardInput
    {
        get { return forwardInput; }
    }

    private float rotationInput;
    public float RotationInput
    {
        get { return rotationInput; }
    }

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 500f;

    [SerializeField] private InputActionAsset controls;

    [SerializeField] private float stickDeadzone = 0.3f;
    [SerializeField] private float aimDistance = 10f;

    private Vector2 aimInput;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction fireAction;

    public bool isShooting {get; private set;}

    private void Awake()
    {
        if (controls == null) return;

        //setup control actions
        moveAction = controls.FindAction("Move");
        aimAction = controls.FindAction("Aim");
        fireAction = controls.FindAction("Fire");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        aimAction?.Enable();
        fireAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        aimAction?.Disable();
        fireAction?.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (camera1 == null)
        {
            camera1 = Camera.main;
        }

        // Move
        if (moveAction != null)
        {
            Vector2 move = moveAction.ReadValue<Vector2>();
            rotationInput = move.x;
            forwardInput = move.y;
        }

        // Fire
        if (fireAction != null)
        {
            isShooting = fireAction.IsPressed();
        }

        // Right stickv to aim
        Vector2 stickAim = Vector2.zero;
        if (aimAction != null && aimAction.activeControl != null && aimAction.activeControl.device is Gamepad)
        {
            stickAim = aimAction.ReadValue<Vector2>();
        }
        else if (Gamepad.current != null)
        {
            stickAim = Gamepad.current.rightStick.ReadValue();
        }

        // right stick priority if > stick deadzone
        if (stickAim.magnitude > stickDeadzone)
        {
            Vector3 aimDir = new Vector3(stickAim.x, 0f, stickAim.y).normalized;
            reticlePosition = transform.position + (aimDir * aimDistance);
            reticleNormal = Vector3.up;
        }
        else
        {
            // if mouse not used
            HandleInputs();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(reticlePosition, 0.5f);
    }

    protected virtual void HandleInputs()
    {
        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : (Vector2)Input.mousePosition;
        Ray screenRay = camera1.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if(Physics.Raycast(screenRay, out hit, rayDistance, groundLayer))
        {
            reticlePosition = hit.point;
            reticleNormal = hit.normal;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            if (groundPlane.Raycast(screenRay, out float distance))
            {
                reticlePosition = screenRay.GetPoint(distance);
                reticleNormal = Vector3.up;
            }
        }
}
}
