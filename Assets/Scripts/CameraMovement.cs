using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxYaw = 8f;     
    [SerializeField] private float maxPitch = 5f;   

    [SerializeField] private float smoothSpeed = 6f; 
    [Range(0f, 0.5f)]
    [SerializeField] private float deadZone = 0.05f; 

    [SerializeField] private CursorLockMode cursorMode = CursorLockMode.Confined;

    public bool canMove;
    public Vector3 LookDirection;

    private Quaternion baseRotation;
    private Vector2 currentOffset;

    void Start()
    {
        canMove = true;
        baseRotation = transform.localRotation;

        Cursor.visible = true;
        Cursor.lockState = cursorMode; 
    }

    void Update()
    {
        if (!canMove || Time.timeScale <= 0) return;
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 normalized = new Vector2(
            mousePos.x / Screen.width,
            mousePos.y / Screen.height) * 2f - Vector2.one;
        normalized = Vector2.ClampMagnitude(normalized, 1f);

        normalized.x = ApplyDeadZone(normalized.x);
        normalized.y = ApplyDeadZone(normalized.y);

        Vector2 target = new Vector2(normalized.x * maxYaw, normalized.y * maxPitch);

        float t = 1f - Mathf.Exp(-smoothSpeed * Time.unscaledDeltaTime);
        currentOffset = Vector2.Lerp(currentOffset, target, t);

        transform.localRotation = baseRotation * Quaternion.Euler(-currentOffset.y, currentOffset.x, 0f);
        LookDirection = transform.forward;
    }

    private float ApplyDeadZone(float value)
    {
        if (Mathf.Abs(value) < deadZone) return 0f;
        return Mathf.Sign(value) * (Mathf.Abs(value) - deadZone) / (1f - deadZone);
    }

    public void ResetXrotation()
    {
        currentOffset = Vector2.zero;
        transform.localRotation = baseRotation;
    }

    public void SetBaseRotation(Quaternion newBase)
    {
        baseRotation = newBase;
    }
}