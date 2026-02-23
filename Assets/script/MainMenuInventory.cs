using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuInventory : MonoBehaviour
{
    public static MainMenuInventory Instance;

    [Header("All Items Database")]
    [Tooltip("Drag ALL your Item ScriptableObjects here")]
    public Item[] AllItems;

    [Header("UI References")]
    public Transform SideItems;
    public GameObject InventoryItem;

    [Header("Item Description Panel")]
    public GameObject InventoryDescription;
    public Image ItemImage;
    public Text ItemDescriptionNameText;
    public Text ItemDescriptionText;

    [Header("Inventory Panel")]
    public GameObject InventoryPanel;

    [Header("Counter (Optional)")]
    public Text CollectedCountText;

    private AudioManager audioManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (InventoryPanel != null)
            InventoryPanel.SetActive(false);

        if (InventoryDescription != null)
            InventoryDescription.SetActive(false);

        
        List<string> saved = InventorySaveSystem.GetAllCollectedItems();
        Debug.Log("[MainMenuInventory] Saved items count: " + saved.Count);
        foreach (string s in saved)
        {
            Debug.Log("[MainMenuInventory] Saved item: " + s);
        }
    }

    
    public void OpenInventory()
    {
        Debug.Log("[MainMenuInventory] OpenInventory called");

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.click);

        if (InventoryPanel != null)
            InventoryPanel.SetActive(true);

        ListItems();
    }

    
    public void CloseInventory()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.disclick);

        if (InventoryPanel != null)
            InventoryPanel.SetActive(false);

        if (InventoryDescription != null)
            InventoryDescription.SetActive(false);
    }

    
    public int GetItemCount()
    {
        return InventorySaveSystem.GetCollectedCount();
    }

    
    public void ListItems()
    {
        Debug.Log("[MainMenuInventory] ListItems called");

        
        if (SideItems == null)
        {
            Debug.LogError("[MainMenuInventory] SideItems is NULL!");
            return;
        }
        if (InventoryItem == null)
        {
            Debug.LogError("[MainMenuInventory] InventoryItem prefab is NULL!");
            return;
        }
        if (AllItems == null || AllItems.Length == 0)
        {
            Debug.LogError("[MainMenuInventory] AllItems is empty! Drag your ScriptableObjects.");
            return;
        }

        
        foreach (Transform child in SideItems)
        {
            Destroy(child.gameObject);
        }

        
        List<string> collectedItems = InventorySaveSystem.GetAllCollectedItems();
        Debug.Log("[MainMenuInventory] Collected items from save: " + collectedItems.Count);

        
        if (CollectedCountText != null)
        {
            CollectedCountText.text = collectedItems.Count + "/" + AllItems.Length;
        }

        
        foreach (var item in AllItems)
        {
            if (item == null)
            {
                Debug.LogWarning("[MainMenuInventory] Found NULL item in AllItems array!");
                continue;
            }

            bool isCollected = collectedItems.Contains(item.itemName);
            Debug.Log("[MainMenuInventory] Item: " + item.itemName + " | Collected: " + isCollected);

            CreateInventoryItem(item, isCollected);
        }
    }

    
    private void CreateInventoryItem(Item item, bool isCollected)
    {
        GameObject obj = Instantiate(InventoryItem, SideItems);

        Text itemName = obj.transform.Find("ItemName")?.GetComponent<Text>();
        Image itemIcon = obj.transform.Find("ItemIcon")?.GetComponent<Image>();
        Button button = obj.GetComponent<Button>();

        if (itemName == null)
            Debug.LogWarning("[MainMenuInventory] ItemName not found in prefab!");
        if (itemIcon == null)
            Debug.LogWarning("[MainMenuInventory] ItemIcon not found in prefab!");

        if (isCollected)
        {
            
            if (itemName != null) itemName.text = item.itemName;
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
                itemIcon.color = Color.white;
            }

            if (button != null)
            {
                button.interactable = true;
                button.onClick.AddListener(() => ShowItemDescription(item));
            }
        }
        else
        {
            
            if (itemName != null) itemName.text = "???";
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
                itemIcon.color = new Color(0.3f, 0.3f, 0.3f, 0.5f); // Dark gray
            }

            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    
    public void ShowItemDescription(Item item)
    {
        if (InventoryDescription == null) return;

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.click);

        InventoryDescription.SetActive(true);

        if (ItemImage != null) ItemImage.sprite = item.icon;
        if (ItemDescriptionNameText != null) ItemDescriptionNameText.text = item.itemName;
        if (ItemDescriptionText != null) ItemDescriptionText.text = item.itemDescription;
    }

    
    public void CloseInventoryDescription()
    {
        if (InventoryDescription != null)
        {
            InventoryDescription.SetActive(false);
        }
    }

    
    public void ResetAllProgress()
    {
        InventorySaveSystem.ClearAllItems();
        ListItems();
        Debug.Log("[MainMenuInventory] All progress reset!");
    }
}