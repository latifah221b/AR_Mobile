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

    [Header("Description Panel")]
    public GameObject DescriptionPanel;
    public Image AchievementImage;
    public Text AchievementNameText;
    public Text AchievementDescriptionText;

    [Header("Achievements Panel")]
    public GameObject AchievementsPanel;

    [Header("Counter (Optional)")]
    public Text UnlockedCountText;

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

        if (DescriptionPanel != null)
            DescriptionPanel.SetActive(false);
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

        if (DescriptionPanel != null)
            DescriptionPanel.SetActive(false);
    }

    public void ListAchievements()
    {
        if (AchievementsContainer == null || AchievementSlotPrefab == null || AllAchievements == null) return;

        
        foreach (Transform child in AchievementsContainer)
        {
            Destroy(child.gameObject);
        }

        
        List<string> unlocked = AchievementSaveSystem.GetAllUnlockedAchievements();

        
        if (UnlockedCountText != null)
        {
            UnlockedCountText.text = unlocked.Count + "/" + AllAchievements.Length;
        }

        
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

        Text nameText = obj.transform.Find("ItemName")?.GetComponent<Text>();
        Image iconImage = obj.transform.Find("ItemIcon")?.GetComponent<Image>();
        Button button = obj.GetComponent<Button>();

        if (isUnlocked)
        {
            if (nameText != null) nameText.text = achievement.displayName;
            if (iconImage != null)
            {
                iconImage.sprite = achievement.badgeSprite;
                iconImage.color = Color.white;
            }

            if (button != null)
            {
                button.interactable = true;
                AchievementData captured = achievement;
                button.onClick.AddListener(() => ShowDescription(captured));
            }
        }
        else
        {
            if (nameText != null) nameText.text = "???";
            if (iconImage != null)
            {
                iconImage.sprite = achievement.badgeSprite;
                iconImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            }

            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    public void ShowDescription(AchievementData achievement)
    {
        if (DescriptionPanel == null) return;

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.click);

        DescriptionPanel.SetActive(true);

        if (AchievementImage != null) AchievementImage.sprite = achievement.badgeSprite;
        if (AchievementNameText != null) AchievementNameText.text = achievement.displayName;
        if (AchievementDescriptionText != null) AchievementDescriptionText.text = achievement.description;
    }

    public void CloseDescription()
    {
        if (DescriptionPanel != null)
            DescriptionPanel.SetActive(false);
    }

    public void ResetAllAchievements()
    {
        AchievementSaveSystem.ClearAllAchievements();
        ListAchievements();
    }
}

[System.Serializable]
public class AchievementData
{
    public string achievementName;
    public string displayName;
    public string description;
    public Sprite badgeSprite;
}