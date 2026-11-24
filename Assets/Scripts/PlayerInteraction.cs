using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    private InputSystem_Actions input;
    Interactable currentInteractable;
    private float playerReach = 3f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        input = new();
        input.Enable();
    }

    void Update()
    {
        CheckInteraction();
        if(input.Player.Interact.WasPressedThisFrame() && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);   
        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }


    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutLine();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();

        if(currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
        

    }

    private void OnDestroy()
    {
        if (input != null) input.Disable();
    }
}
