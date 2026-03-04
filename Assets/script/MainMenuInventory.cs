using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main Menu Inventory - Side Items Only
/// Shows all collected items from all scenes using saved data
/// </summary>
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
    
    private const int MAX_ITEMS = 31;
    
    
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
    }
    
    /// <summary>
    /// Open inventory panel - Call from Button OnClick
    /// </summary>
    public void OpenInventory()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.click);
            
        if (InventoryPanel != null)
            InventoryPanel.SetActive(true);
            
        ListItems();
    }
    
    /// <summary>
    /// Close inventory panel - Call from Button OnClick
    /// </summary>
    public void CloseInventory()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.disclick);
            
        if (InventoryPanel != null)
            InventoryPanel.SetActive(false);
            
        if (InventoryDescription != null)
            InventoryDescription.SetActive(false);
    }
    
    /// <summary>
    /// Get count of collected items
    /// </summary>
    public int GetItemCount()
    {
        return Mathf.Min(InventorySaveSystem.GetCollectedCount(), MAX_ITEMS);
    }
    
    /// <summary>
    /// List all items - collected shown normal, uncollected shown grayed
    /// </summary>
    public void ListItems()
    {
        if (SideItems == null || InventoryItem == null || AllItems == null) return;
        
        // Clear existing items
        foreach (Transform child in SideItems)
        {
            Destroy(child.gameObject);
        }
        
        // Get saved collected items
        List<string> collectedItems = InventorySaveSystem.GetAllCollectedItems();
        
        // Update counter (capped at 32)
        if (CollectedCountText != null)
        {
            int count = Mathf.Min(collectedItems.Count, MAX_ITEMS);
            CollectedCountText.text = count + "/" + MAX_ITEMS;
        }
        
        // Create UI for all items
        foreach (var item in AllItems)
        {
            if (item == null) continue;
            
            bool isCollected = collectedItems.Contains(item.itemName);
            CreateInventoryItem(item, isCollected);
        }
    }
    
    /// <summary>
    /// Create inventory item slot
    /// </summary>
    private void CreateInventoryItem(Item item, bool isCollected)
    {
        GameObject obj = Instantiate(InventoryItem, SideItems);
        
        Text itemName = obj.transform.Find("ItemName")?.GetComponent<Text>();
        Image itemIcon = obj.transform.Find("ItemIcon")?.GetComponent<Image>();
        Button button = obj.GetComponent<Button>();
        
        if (isCollected)
        {
            // Show collected item normally
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
            // Show uncollected item grayed out
            if (itemName != null) itemName.text = "???";
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
                itemIcon.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            }
            
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }
    
    /// <summary>
    /// Show item description popup
    /// </summary>
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
    
    /// <summary>
    /// Close item description - Call from Button OnClick
    /// </summary>
    public void CloseInventoryDescription()
    {
        if (InventoryDescription != null)
        {
            InventoryDescription.SetActive(false);
        }
    }
    
    
    /// <summary>
    /// Reset all collected items (for testing)
    /// </summary>
    public void ResetAllProgress()
    {
        InventorySaveSystem.ClearAllItems();
        ListItems();
    }
}