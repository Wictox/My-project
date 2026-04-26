using UnityEngine;

public class Chess : MonoBehaviour , IInteractable
{
    public bool IsOpened {get ; private set;}
    public string ChestId {get ; private set;}
    public GameObject itemPrefab;
    public Sprite openedSprite;

    public void Interact()
    {
        if (!CanInteract())
            return;

        OpenChest();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestId ??= GlobalHelper.GenerateUniqueID(gameObject);
    }
    
    public bool CanInteract()
    {
        return !IsOpened;
    }

    private void OpenChest()
    {
        SetOpened(true);
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>()?.StartBounce();
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}
