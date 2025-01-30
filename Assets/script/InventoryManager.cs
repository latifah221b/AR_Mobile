using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [Header("Inventory Data")]
    public List<Item> Items = new List<Item>();
    [Header("UI References")]
    public Transform MainItems;
    public Transform SideItems;
    public GameObject InventoryItem;
    [Header("Item Description Panel")]
    public GameObject InventoryDescription;
    public Image ItemImage;
    public Text ItemDescriptionNameText;
    public Text ItemDescriptionText;
    private HashSet<string> collectedHiddenItems = new HashSet<string>();
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
    public void Add(Item item)
    {
        Items.Add(item);
        if (item.itemName.StartsWith("hidden"))
        {
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.pages);
            if (!collectedHiddenItems.Contains(item.itemName))
            {
                collectedHiddenItems.Add(item.itemName);
                CheckHiddenItemsGoal();
            }
        }
        ListItems();
    }
    public void ListItems()
    {
        foreach (Transform child in MainItems)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in SideItems)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in Items)
        {
            if (item.isRocketPart)
            {
                CreateInventoryItem(item, MainItems);
            }
            else
            {
                CreateInventoryItem(item, SideItems);
            }
        }
    }
    private void CreateInventoryItem(Item item, Transform container)
    {
        GameObject obj = Instantiate(InventoryItem, container);
        Text itemName = obj.transform.Find("ItemName")?.GetComponent<Text>();
        Image itemIcon = obj.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemName != null) itemName.text = item.itemName;
        if (itemIcon != null) itemIcon.sprite = item.icon;
        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => ShowItemDescription(item));
        }
    }
    public void ShowItemDescription(Item item)
    {
        if (InventoryDescription == null) return;
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
    private void CheckHiddenItemsGoal()
    {
        if (collectedHiddenItems.Count >= 6)
        {
            PapersBadge.Instance.ShowBadge();
        }
    }
}