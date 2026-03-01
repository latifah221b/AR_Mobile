using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance;

    [Header("All Achievements")]
    public AchievementData[] AllAchievements;

    [Header("UI References")]
    public Transform AchievementsContainer;
    public GameObject AchievementSlotPrefab;

    [Header("Achievements Panel")]
    public GameObject AchievementsPanel;

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

    private void Start()
    {
        if (AchievementsPanel != null)
            AchievementsPanel.SetActive(false);
    }

    public void OpenAchievements()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.click);

        if (AchievementsPanel != null)
            AchievementsPanel.SetActive(true);

        ListAchievements();
    }

    public void CloseAchievements()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.disclick);

        if (AchievementsPanel != null)
            AchievementsPanel.SetActive(false);
    }

    public void ListAchievements()
    {
        if (AchievementsContainer == null || AchievementSlotPrefab == null || AllAchievements == null) return;

        
        foreach (Transform child in AchievementsContainer)
        {
            Destroy(child.gameObject);
        }

        
        List<string> unlocked = AchievementSaveSystem.GetAllUnlockedAchievements();

        
        foreach (var achievement in AllAchievements)
        {
            if (achievement == null) continue;

            bool isUnlocked = unlocked.Contains(achievement.achievementName);
            CreateSlot(achievement, isUnlocked);
        }
    }

    private void CreateSlot(AchievementData achievement, bool isUnlocked)
    {
        GameObject obj = Instantiate(AchievementSlotPrefab, AchievementsContainer);

        Image iconImage = obj.GetComponent<Image>();

        if (iconImage != null)
        {
            iconImage.sprite = achievement.badgeSprite;

            if (isUnlocked)
            {
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            }
        }
    }
}

[System.Serializable]
public class AchievementData
{
    public string achievementName;
    public Sprite badgeSprite;
}