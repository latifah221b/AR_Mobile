//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using TMPro;

//public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    public bool hovered;
//    private Item heldItem;
//    private Color opaque = new Color(1, 1, 1, 1);
//    private Color transparent = new Color(1, 1, 1, 0);
//    private Image thisSlotImage;
//    public TMP_Text thisSlotQuantityText;
//    public void initialiseSlot()
//    {
//        thisSlotImage = gameObject.GetComponent<Image>();
//        thisSlotQuantityText = transform.GetChild(0).GetComponent<TMP_Text>();
//        thisSlotImage.sprite = null;
//        thisSlotImage.color = transparent;
//        setItem(null);
//    }
//    public void setItem(Item item)
//    {
//        heldItem = item;
//        if (item != null)
//        {
//            thisSlotImage.sprite = heldItem.icon;
//            thisSlotImage.color = opaque;
//            updateData();
//        }
//        else
//        {
//            thisSlotImage.sprite = null;
//            thisSlotImage.color = transparent;
//            updateData();
//        }
//    }
//    public Item getItem()
//    {
//        return heldItem;
//    }
//    public void updateData()
//    {
//        if (heldItem != null)
//            thisSlotQuantityText.text = heldItem.currentQuantity.ToString();
//        else
//            thisSlotQuantityText.text = "";
//    }
//    public void OnPointerEnter(PointerEventData pointerEventData)
//    {
//        hovered = true;
//    }
//    public void OnPointerExit(PointerEventData pointerEventData)
//    {
//        hovered = false;
//    }
//}


// this system is an alternitive system for managing a slot in a ui inventory system
// the class, named slot, is designed to handle items in a slot, responed to pointer events, and manage the visual representation of the slots content
// needs debug and build reshape