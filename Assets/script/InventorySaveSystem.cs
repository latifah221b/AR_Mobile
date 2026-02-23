using System.Collections.Generic;
using UnityEngine;


public static class InventorySaveSystem
{
    private const string SAVE_KEY = "CollectedItems";

    
    public static void SaveCollectedItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("[InventorySaveSystem] Cannot save empty item name!");
            return;
        }

        List<string> collectedItems = GetAllCollectedItems();

        if (!collectedItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            SaveItemsList(collectedItems);
            Debug.Log("[InventorySaveSystem] SAVED: " + itemName + " | Total: " + collectedItems.Count);
        }
        else
        {
            Debug.Log("[InventorySaveSystem] Already saved: " + itemName);
        }
    }

    
    public static bool IsItemCollected(string itemName)
    {
        List<string> collectedItems = GetAllCollectedItems();
        return collectedItems.Contains(itemName);
    }

    
    public static List<string> GetAllCollectedItems()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("[InventorySaveSystem] No saved data found.");
            return new List<string>();
        }

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null || data.items == null)
        {
            Debug.Log("[InventorySaveSystem] SaveData is null.");
            return new List<string>();
        }

        Debug.Log("[InventorySaveSystem] Loaded " + data.items.Count + " items from save.");
        return data.items;
    }

    
    public static int GetCollectedCount()
    {
        return GetAllCollectedItems().Count;
    }

    
    public static void ClearAllItems()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
        Debug.Log("[InventorySaveSystem] All items CLEARED!");
    }

    private static void SaveItemsList(List<string> items)
    {
        SaveData data = new SaveData { items = items };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("[InventorySaveSystem] Data saved to PlayerPrefs.");
    }

    [System.Serializable]
    private class SaveData
    {
        public List<string> items = new List<string>();
    }
}