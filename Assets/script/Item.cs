using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public string itemDescription;
    //public Image itemDescriptionImage;
    //public TMP_Text ItemDescriptionNameText;
    //public TMP_Text ItemDescriptionText;
    internal object currentQuantity;
    public string description;
}