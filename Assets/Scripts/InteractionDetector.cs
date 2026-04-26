using UnityEngine;
using UnityEngine.InputSystem; // Required for the input button!

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon; // Assign this in the inspector with your interaction icon


    void Start()
    {
        interactionIcon.SetActive(false); // Hide the interaction icon at the start
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    // When something enters our player's bubble...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
            {
                interactableInRange = interactable;
                interactionIcon.SetActive(true); // Show the interaction icon
                return; // Don't set interactableInRange if the chest is already opened
            }
    }
    

    // When we walk away from the NPC...
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable== interactableInRange)
            {
                interactableInRange = null;
                interactionIcon.SetActive(false); // Show the interaction icon
    }

    // This is the function your button press will actually trigger!
    }
}