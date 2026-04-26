using UnityEngine;
using UnityEngine.EventSystems;

public class itemdraghandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform OriginalParent;
    CanvasGroup canvasGroup;
    
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
            // 1. Move/Swap items between slots
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
        else if (eventData.pointerEnter != null)
        {
            // 2. We hit the UI (Inventory background, buttons, etc). Snap back.
            transform.SetParent(OriginalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
        }
        else
        {
            // 3. We are hovering over the world/grass. Drop it!
            DropItem(originalSlot);
        }
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null; 

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Could not find Player! Make sure your Player object has the tag 'Player'.");
            return;
        }

        Vector2 randomOffset = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Vector2 dropPosition = (Vector2)player.transform.position + randomOffset;

        if (physicalItemPrefab != null)
        {
            // SPAWN ONLY ONE THING: The floor item prefab
            GameObject droppedItem = Instantiate(physicalItemPrefab, dropPosition, Quaternion.identity);
            
            // Clean up the name and fix scale
            droppedItem.name = physicalItemPrefab.name.Replace("(Clone)", ""); 
            droppedItem.transform.localScale = Vector3.one; 

            // Apply the bounce effect to the NEW floor item
            if (droppedItem.TryGetComponent<BounceEffect>(out BounceEffect bounce))
            {
                bounce.StartBounce();
            }
        }
        else
        {
            Debug.LogWarning("You forgot to assign the physicalItemPrefab on " + gameObject.name);
        }

        // Destroy the UI item from the inventory
        Destroy(gameObject);
    }   
}