using UnityEngine;
using System.Collections.Generic;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager Instance;

    public List<INotifyOnTap> tapNotifiers = new List<INotifyOnTap>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterNotifier(INotifyOnTap notifier)
    {
       // Debug.Log("RegisterNotifier");

        tapNotifiers.Add(notifier);
    }

    public void UnregisterNotifier(INotifyOnTap notifier)
    {
        tapNotifiers.Remove(notifier);
    }

    public void NotifyAll(Vector2 tapPosition)
    {
       //  Debug.Log("NotifyAll");

        foreach (var notifier in tapNotifiers)
        {
            notifier.OnTap(tapPosition);
        }
    }

    public GameObject FindChildCanvas(GameObject gameObject)
    {
        // Get all child Canvas components
        Canvas[] canvases = gameObject.GetComponentsInChildren<Canvas>(true);

        // Check if any Canvas components were found
        if (canvases.Length > 0)
        {
            // Return the first child GameObject with a Canvas component
            return canvases[0].gameObject; // Change index if you want a specific Canvas
        }

        // Return null if no Canvas was found
        return null;
    }

    public GameObject GetParentGameObject(GameObject gameObject)
    {
        // Get the parent Transform
        Transform parentTransform = gameObject.transform.parent;

        // Check if there is a parent
        if (parentTransform != null)
        {
            // Return the parent GameObject
            return parentTransform.gameObject;
        }

        // Return null if there is no parent
        return null;
    }

    public Item GetItemScriptableObject(GameObject gameObject)
    {
        // Get the itemController component attached to this GameObject
        ItemController itemController = gameObject.GetComponent<ItemController>();

        // Check if the component is found and if the item reference is assigned
        if (itemController != null && itemController.Item != null)
        {
            return itemController.Item; // Return the Item ScriptableObject
        }

        // Return null if not found
        return null;
    }
}