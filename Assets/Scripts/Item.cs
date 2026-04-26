using UnityEngine;
using UnityEngine.UI; // REQUIRED: You need this to use 'Image'

public class Item : MonoBehaviour
{
    public int ID; 
    public string Name;
    public int quantity = 1;

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }

    public virtual void PickUp()
    {
        // Get the Sprite from the SpriteRenderer (assuming this is a ground item)
        // Note: If you use 'Image' here, it only works for UI elements, not ground items.
        // If this is a ground item, use SpriteRenderer instead!
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite itemIcon = (sr != null) ? sr.sprite : null;

        // Use 'FindAnyObjectByType' if you aren't using a Singleton 'Instance' pattern
        ItemPickupUIController uiController = FindAnyObjectByType<ItemPickupUIController>();

        if (uiController != null)
        {
            uiController.ShowItemPickup(itemIcon, Name);
        }
        else
        {
            Debug.LogWarning("ItemPickupUIController not found in the scene.");
        } 
    }
}