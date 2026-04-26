using UnityEngine;
using UnityEngine.EventSystems;

public class itemdraghandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform OriginalParent;
    CanvasGroup canvasGroup;
    
    // NEW: You need to drag your actual physical item prefab into this slot in the Unity Inspector!
    public GameObject physicalItemPrefab; 
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;  
        transform.SetParent(transform.root); 
        
        canvasGroup.blocksRaycasts = false; 
        canvasGroup.alpha = 0.6f;          
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.alpha = 1f;            

        Slot dropslot = eventData.pointerEnter?.GetComponentInParent<Slot>(); 
        Slot originalSlot = OriginalParent.GetComponent<Slot>();      

        if (dropslot != null)
        {  
            if (dropslot.currentItem != null)
            {
                dropslot.currentItem.transform.SetParent(originalSlot.transform); 
                originalSlot.currentItem = dropslot.currentItem; 
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
            }
            else
            {
                originalSlot.currentItem = null; 
            }

            transform.SetParent(dropslot.transform); 
            dropslot.currentItem = gameObject; 
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
        }
        else
        {
            // FIX: The logic here is cleaned up.
            if (!IsWithinInventory(eventData.position, eventData.pressEventCamera))
            {
                // We are OUTSIDE the inventory. Throw it!
                DropItem(originalSlot);
            }
            else
            {
                // We are INSIDE the inventory, but missed a slot. Snap it back.
                transform.SetParent(OriginalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
            }
        }
    }

    // FIX: Changed mousePosition to position, and added camera support for better UI detection
    bool IsWithinInventory(Vector2 position, Camera eventCamera)
    {
        RectTransform inventoryRect = OriginalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, position, eventCamera);
    }

    void DropItem(Slot originalSlot)
    {
        // 1. Clear the UI slot
        originalSlot.currentItem = null; 

        // 2. Find the player (Make sure your player has the tag "Player" in Unity!)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Could not find Player to drop item next to!");
            return;
        }

        // 3. Random drop position (Spawns slightly around the player so it doesn't get stuck inside them)
        // Since you are making a 2D top-down game, we use X and Y.
        Vector2 randomOffset = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        Vector2 dropPosition = (Vector2)player.transform.position + randomOffset;

        // 4. Instantiate the physical item in the world
        if (physicalItemPrefab != null)
        {
            Instantiate(physicalItemPrefab, dropPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("You forgot to assign the physicalItemPrefab on " + gameObject.name);
        }

        // 5. Destroy this UI item
        Destroy(gameObject);
    }   
}