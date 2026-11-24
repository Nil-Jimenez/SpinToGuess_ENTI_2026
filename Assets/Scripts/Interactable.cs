using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    Outline outline;
    public string message;


    [SerializeField] private UnityEvent onInteraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        if(outline != null)
        outline.enabled = false;
    }

    public void EnableOutLine()
    {
        if (outline != null)
        outline.enabled = true;
    }
}