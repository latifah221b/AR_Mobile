using System.Collections.Generic;
using UnityEngine;


public static class AchievementSaveSystem
{
    private const string SAVE_KEY = "UnlockedAchievements";

    
    public static void UnlockAchievement(string achievementName)
    {
        if (string.IsNullOrEmpty(achievementName)) return;

        List<string> unlockedAchievements = GetAllUnlockedAchievements();

        if (!unlockedAchievements.Contains(achievementName))
        {
            unlockedAchievements.Add(achievementName);
            SaveAchievementsList(unlockedAchievements);
        }
    }

    
    public static bool IsAchievementUnlocked(string achievementName)
    {
        List<string> unlockedAchievements = GetAllUnlockedAchievements();
        return unlockedAchievements.Contains(achievementName);
    }

    
    public static List<string> GetAllUnlockedAchievements()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");

        if (string.IsNullOrEmpty(json))
        {
            return new List<string>();
        }

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data != null ? data.achievements : new List<string>();
    }

    
    public static int GetUnlockedCount()
    {
        return GetAllUnlockedAchievements().Count;
    }

    
    public static void ClearAllAchievements()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
    }

    private static void SaveAchievementsList(List<string> achievements)
    {
        SaveData data = new SaveData { achievements = achievements };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class SaveData
    {
        public List<string> achievements = new List<string>();
    }
}