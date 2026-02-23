using UnityEngine;


public class HiddenItemPickupFuGen : MonoBehaviour
{
    public Item Item;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void OnMouseDown()
    {
        Pickup();
    }

    void Pickup()
    {
        if (Item == null)
        {
            Debug.LogError("[HiddenItemPickup] Item is NULL! Assign the ScriptableObject.");
            return;
        }

        Debug.Log("[HiddenItemPickup] Picking up: " + Item.itemName);

        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.Add(Item);
            InventoryManager.Instance.ShowItemDescription(Item);
        }

        
        InventorySaveSystem.SaveCollectedItem(Item.itemName);
        Debug.Log("[HiddenItemPickup] SAVED to PlayerPrefs: " + Item.itemName);

        
        if (FuGenLevelController.Instance != null)
        {
            FuGenLevelController.Instance.OnHiddenItemCollected();
        }

        
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.partsitems);
        }

        
        Destroy(gameObject);
    }
}