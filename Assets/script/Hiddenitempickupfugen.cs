using UnityEngine;

/// <summary>
/// Pickup script for hidden items in Fu_Gen_Lvl scene
/// Attach this to hidden item GameObjects instead of ItemPickup
/// </summary>
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
        // Add to inventory
        if (InventoryManager.Instance != null && Item != null)
        {
            InventoryManager.Instance.Add(Item);
            InventoryManager.Instance.ShowItemDescription(Item);
            Debug.Log($"Hidden item '{Item.itemName}' added to inventory!");
        }

        // Notify the FuGenLevelController
        if (FuGenLevelController.Instance != null)
        {
            FuGenLevelController.Instance.OnHiddenItemCollected();
        }
        else
        {
            Debug.LogWarning("FuGenLevelController not found in scene!");
        }

        // Destroy the picked up item
        Destroy(gameObject);
    }
}