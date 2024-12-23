using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public Transform MainItems;
    public Transform SideItems;
    public GameObject InventoryItem;
    public GameObject InventoryDescription;
    public Image ItemImage;
    public Text ItemDescriptionNameText;
    public Text ItemDescriptionText;
    private HashSet<string> collectedHiddenItems = new HashSet<string>();
    private AudioManager audioManager;

    private void Awake()

    {
        Instance = this;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Add(Item item)
    {
        Items.Add(item);
        if (item.itemName.StartsWith("hidden"))
        {
            audioManager.PlaySFX(audioManager.pages);
            if (!collectedHiddenItems.Contains(item.itemName))
            {
                collectedHiddenItems.Add(item.itemName);
                //Debug.Log("collected");
                CheckHiddenItemsGoal();
            }
        }
    }

    //public void Remove(Item item)
    //{
    //    Items.Remove(item);
    //}

    public void ListItems()
    {
        //if (ItemContent == null)
        //{
        //    Debug.LogError("ItemContent is not assigned in the InventoryManager!");
        //    return;
        //}

        foreach (Transform item in MainItems)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in SideItems)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            if (item.isRocketPart)
            {
                CreateInventoryItem(item, MainItems);
            }
        }

        foreach (var item in Items)
        {
            if (!item.isRocketPart)
            {
                CreateInventoryItem(item, SideItems);
            }
        }
    }

    private void CreateInventoryItem(Item item, Transform container)

    {
        GameObject obj = Instantiate(InventoryItem, container);
        var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
        var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
        Button button = obj.GetComponent<Button>();
        button.onClick.AddListener(() => ShowItemDescription(item));
    }

    public void ShowItemDescription(Item item)
    {
        //if (InventoryDescription == null)
        //{
        //    Debug.LogError("InventoryDescription is not assigned in the InventoryManager!");
        //    return;
        //}
        InventoryDescription.SetActive(true);
        ItemImage.sprite = item.icon;
        ItemDescriptionNameText.text = item.itemName;
        ItemDescriptionText.text = item.itemDescription;
    }

    public void CloseInventoryDescription()
    {
        InventoryDescription.SetActive(false);
    }

    private void CheckHiddenItemsGoal()
    {
        if (collectedHiddenItems.Count == 6)
        {
            PapersBadge.Instance.ShowBadge();
            //Debug.Log("all  collected");
        }
    }
}