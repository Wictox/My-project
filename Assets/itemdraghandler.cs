using UnityEngine;
using UnityEngine.EventSystems;

public class itemdraghandler : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform OriginalParent;
    CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;  // Save the original parent of the item
        
        // FIX 1: Move the item to the root Canvas so it renders ON TOP of all other UI slots
        transform.SetParent(transform.root); 
        
        canvasGroup.blocksRaycasts = false; // Disable raycast blocking 
        canvasGroup.alpha = 0.6f;           // Make the item semi-transparent while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Move the item with the mouse cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Re-enable raycast blocking
        canvasGroup.alpha = 1f;            // Restore the item's opacity

        // THE FIX: Use GetComponentInParent! 
        // This way, if your mouse hits the item currently in the slot, it will look up and grab the slot holding it.
        Slot dropslot = eventData.pointerEnter?.GetComponentInParent<Slot>(); 
        Slot originalSlot = OriginalParent.GetComponent<Slot>();      

        if (dropslot != null)
        {  
            // We only swap if there IS an item already in the drop slot
            if (dropslot.currentItem != null)
            {
                // Move the existing item into our old slot
                dropslot.currentItem.transform.SetParent(originalSlot.transform); 
                originalSlot.currentItem = dropslot.currentItem; 
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
            }
            else
            {
                // If the slot was empty, just clear our old slot's memory
                originalSlot.currentItem = null; 
            }

            // Move the dragged item into the new slot
            transform.SetParent(dropslot.transform); 
            dropslot.currentItem = gameObject; 
        }
        else
        {
            // If we dropped it on a wall or outside a slot entirely, send it back
            transform.SetParent(OriginalParent);
        }

        // Snap the item perfectly to the middle of whichever slot it ended up in
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
    }
}