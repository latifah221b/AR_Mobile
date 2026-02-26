using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllData : MonoBehaviour
{
    
    void Start()
    {
        AchievementSaveSystem.ClearAllAchievements();
        InventorySaveSystem.ClearAllItems();
        PlayerPrefs.DeleteAll();
        Debug.Log("ALL DATA CLEARED!");
    }
    
}
