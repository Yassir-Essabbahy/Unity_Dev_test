using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DebugOverlay : MonoBehaviour
{
    public GameObject debugPanel;
    public TextMeshProUGUI debugText;
    public Tank_Inputs tankInputs;
    public Transform tank;

    float smoothDelta = 0.016f;
    float smoothSpeed = 0f;
    Vector3 lastPosition;

    void Start()
    {
        debugPanel.SetActive(false);
        lastPosition = tank.position;
    }

    void Update()
    {
        // F3 show panel
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }

        if (!debugPanel.activeSelf) return;

        // Smooth the frame time so the FPS number doesn't flicker
        smoothDelta = Mathf.Lerp(smoothDelta, Time.unscaledDeltaTime, 0.1f);
        float fps = 1f / smoothDelta;
        float ms = smoothDelta * 1000f;

        float speed = (tank.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = tank.position;
        smoothSpeed = Mathf.Lerp(smoothSpeed, speed, 0.1f);

        string device = "Keyboard / Mouse";
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            device = "Gamepad";
        }

        int shells = GameObject.FindGameObjectsWithTag("Projectile").Length;

        debugText.text =
            "Debug Overlay\n\n" +
            "FPS: " + Mathf.Ceil(fps) + " (" + ms.ToString("F1") +
            "Input Device: " + device + "\n" +
            "Tank Speed: " + speed.ToString("F2") + " m/s\n" +
            "Movement: " + tankInputs.RotationInput.ToString("F1") + ", " + tankInputs.ForwardInput.ToString("F1") + "\n" +
            "Is Shooting: " + tankInputs.isShooting + "\n" +
            "Active Shells: " + shells;
    }
}