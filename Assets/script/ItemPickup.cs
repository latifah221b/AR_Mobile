using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (Item.isRocketPart)
        {
            FindObjectOfType<StarRewardSystem>().CollectRocketPart();
            audioManager.PlaySFX(audioManager.partsitems);
        }
        else if (Item.isSideItem)
        {
            FindObjectOfType<StarRewardSystem>().CollectItem();
            audioManager.PlaySFX(audioManager.partsitems);
        }

        
        InventorySaveSystem.SaveCollectedItem(Item.itemName);

        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}