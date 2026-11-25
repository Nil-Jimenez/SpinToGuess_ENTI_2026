using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float playerReach = 10f;      
    [SerializeField] private LayerMask interactionMask = ~0; 

    private Interactable currentInteractable;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        CheckInteraction();

        if (Mouse.current.leftButton.wasPressedThisFrame && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            DisableCurrentInteractable();
            return;
        }

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, playerReach, interactionMask)
            && hit.collider.CompareTag("Interactable"))
        {
            Interactable newInteractable = hit.collider.GetComponent<Interactable>();

            if (newInteractable != null && newInteractable.enabled)
            {
                SetNewCurrentInteractable(newInteractable);
                return;
            }
        }

        DisableCurrentInteractable();
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        if (currentInteractable != null && currentInteractable != newInteractable)
        {
            currentInteractable.DisableOutline();
        }

        currentInteractable = newInteractable;
        currentInteractable.EnableOutLine();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();

        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}