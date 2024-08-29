using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject InventoryDescription;
    public Image ItemImage;
    public Text ItemDescriptionNameText;
    public Text ItemDescriptionText;
    private void Awake()
    {
        Instance = this;
    }
    public void Add(Item item)
    {
        Items.Add(item);
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
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => ShowItemDescription(item));
        }
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
}