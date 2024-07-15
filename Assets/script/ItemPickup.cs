//using UnityEngine;
//public class ItemPickup : MonoBehaviour
//{
//    public Item Item;
//    void Update()
//    {
//        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit))
//            {
//                if (hit.collider.gameObject == gameObject)
//                {
//                    Pickup();
//                }
//            }
//        }
//    }
//    void Pickup()
//    {
//        InventoryManager.Instance.Add(Item);
//        Destroy(gameObject);
//    }
//}
//public class ItemPickup : MonoBehaviour
//{
//    public Item Item;
//    void Pickup()
//    {
//        InventoryManager.Instance.Add(Item);
//        Destroy(gameObject);
//    }
//    private void OnMouseDown()
//    {
//        Pickup();

//    }
//}