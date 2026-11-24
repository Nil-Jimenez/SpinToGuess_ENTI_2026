using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity;
    public float gamepadSensitivity = 100f;
    private float xRotation;
    public bool canMove;
    [SerializeField] private Transform playerTransform;
    public Vector3 LookDirection;

    private InputSystem_Actions input;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canMove = true;

        input = new();
        input.Player.Look.Enable();
    }

    void Update()
    {
        if (!canMove || Time.timeScale <= 0) return;

        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>() * mouseSensitivity;
        bool usingController = input.Player.Look.activeControl?.device is Gamepad;
        float sensitivity = usingController ? gamepadSensitivity : mouseSensitivity;
       
        lookInput *= sensitivity;
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerTransform.Rotate(Vector3.up * lookInput.x);
        LookDirection = transform.forward;
    }

    public void ResetXrotation()
    {
        xRotation = 0;
    }

  
    private void OnDestroy()
    {
        if (input != null) input.Player.Look.Disable();
    }
}