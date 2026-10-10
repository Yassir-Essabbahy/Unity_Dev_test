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

    [SerializeField] private float stickDeadzone = 0.25f;
    [SerializeField] private float aimDistance = 10f;

    private Vector2 aimInput;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction fireAction;

    public bool isShooting { get; private set; }

    private bool isUsingGamepadAim = false;
    private Vector3 lastAimDir = Vector3.forward;

    private void Awake()
    {
        if (controls == null) return;

        // setup control actions
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

        // Move with deadzone 
        if (moveAction != null)
        {
            Vector2 move = moveAction.ReadValue<Vector2>();
            rotationInput = Mathf.Abs(move.x) > 0.15f ? move.x : 0f;
            forwardInput = Mathf.Abs(move.y) > 0.15f ? move.y : 0f;
        }

        // Fire detection 
        bool firePressed = fireAction != null && fireAction.IsPressed();
        bool mouseFire = Mouse.current != null && Mouse.current.leftButton.isPressed;
        isShooting = firePressed || mouseFire;

        // Right stick aim reading
        Vector2 stickAim = Vector2.zero;
        if (aimAction != null && aimAction.activeControl != null && aimAction.activeControl.device is Gamepad)
        {
            stickAim = aimAction.ReadValue<Vector2>();
        }
        else if (Gamepad.current != null)
        {
            stickAim = Gamepad.current.rightStick.ReadValue();
        }

        // Aim logic
        if (stickAim.magnitude > stickDeadzone)
        {
            isUsingGamepadAim = true;
            lastAimDir = new Vector3(stickAim.x, 0f, stickAim.y).normalized;
            reticlePosition = transform.position + (lastAimDir * aimDistance);
            reticleNormal = Vector3.up;
        }
        else if (isUsingGamepadAim)
        {
            // When right stick returns to neutral keep last aim
            reticlePosition = transform.position + (lastAimDir * aimDistance);
            reticleNormal = Vector3.up;

            // switch
            if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 4f)
            {
                isUsingGamepadAim = false;
                HandleInputs();
            }
        }
        else
        {
            // Mouse aiming
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
        if (camera1 == null) return;

        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : (Vector2)Input.mousePosition;
        Ray screenRay = camera1.ScreenPointToRay(mousePos);
        float effectiveRayDist = Mathf.Max(rayDistance, 500f);

        // Raycast against groundLayer
        if (Physics.Raycast(screenRay, out RaycastHit hit, effectiveRayDist, groundLayer))
        {
            if (!hit.transform.IsChildOf(transform))
            {
                reticlePosition = hit.point;
                reticleNormal = hit.normal;
                return;
            }
        }

        // Fallback to ground plane at tank base position
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
        if (groundPlane.Raycast(screenRay, out float distance))
        {
            reticlePosition = screenRay.GetPoint(distance);
            reticleNormal = Vector3.up;
        }
    }
}
