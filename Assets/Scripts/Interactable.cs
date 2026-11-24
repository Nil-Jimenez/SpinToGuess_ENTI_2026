using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    [SerializeField] private Outline outline;
    [HideInInspector] public string message;


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
        outline.enabled = false;
    }

    public void EnableOutLine()
    {
        outline.enabled = true;
    }
}