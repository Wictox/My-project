using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    
    void Start()
    {
        // FIX 1: Search the whole scene for the InventoryController, not just the Player!
        inventoryController = FindFirstObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Did we hit an item?
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            
            if (item != null)
            {
                // FIX 2: Give the inventory the actual object we touched
                bool itemadded = inventoryController.AddItem(collision.gameObject);

                if (itemadded)
                {
                    Destroy(collision.gameObject); // Remove the item from the floor
                }
            }
        }
    }
    
    // FIX 3: Deleted the stray { } brackets that were causing compiler errors down here
}