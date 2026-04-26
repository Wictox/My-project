using UnityEngine;
using UnityEngine.UI;
using TMPro; // Delete this line if you aren't using TextMeshPro

public class ItemPickupUIController : MonoBehaviour
{
    // This allows Item.cs to see the function
    public void ShowItemPickup(Sprite itemSprite, string itemName)
    {
        Debug.Log("UI logic would go here for: " + itemName);
        // You will eventually add your animation/text code here
    }
}