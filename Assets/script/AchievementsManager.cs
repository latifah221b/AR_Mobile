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
    [Tooltip("Row prefab. Optional children: 'Icon' (Image) for the badge, 'Label' (Text or TextMeshProUGUI) for its name. If neither exists the badge sprite is written to the root Image, as before.")]
    public GameObject AchievementSlotPrefab;

    [Header("Category Section (optional)")]
    [Tooltip("Prefab with a child named 'Header' (Text or TextMeshProUGUI) and a child named 'Grid'. If left empty, achievements are listed flat as before.")]
    public GameObject CategorySectionPrefab;

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


        if (CategorySectionPrefab == null)
        {
            foreach (var achievement in AllAchievements)
            {
                if (achievement == null) continue;

                bool isUnlocked = unlocked.Contains(achievement.achievementName);
                CreateSlot(achievement, isUnlocked, AchievementsContainer);
            }
            return;
        }


        List<string> order = new List<string>();
        Dictionary<string, List<AchievementData>> groups = new Dictionary<string, List<AchievementData>>();

        foreach (var achievement in AllAchievements)
        {
            if (achievement == null) continue;

            string key = string.IsNullOrEmpty(achievement.category) ? "" : achievement.category;

            if (!groups.ContainsKey(key))
            {
                groups.Add(key, new List<AchievementData>());
                order.Add(key);
            }

            groups[key].Add(achievement);
        }

        foreach (string key in order)
        {
            GameObject section = Instantiate(CategorySectionPrefab, AchievementsContainer);

            Transform headerTransform = section.transform.Find("Header");
            if (headerTransform != null)
            {
                SetText(headerTransform, key);
                headerTransform.gameObject.SetActive(!string.IsNullOrEmpty(key));
            }

            Transform grid = section.transform.Find("Grid");
            Transform target = grid != null ? grid : section.transform;

            foreach (var achievement in groups[key])
            {
                bool isUnlocked = unlocked.Contains(achievement.achievementName);
                CreateSlot(achievement, isUnlocked, target);
            }
        }
    }

    private void CreateSlot(AchievementData achievement, bool isUnlocked, Transform parent)
    {
        GameObject obj = Instantiate(AchievementSlotPrefab, parent);

        Color lockedTint = new Color(0.3f, 0.3f, 0.3f, 0.5f);


        Transform iconTransform = obj.transform.Find("Icon");
        Image iconImage = iconTransform != null ? iconTransform.GetComponent<Image>() : obj.GetComponent<Image>();

        if (iconImage != null)
        {
            iconImage.sprite = achievement.badgeSprite;
            iconImage.color = isUnlocked ? Color.white : lockedTint;
        }


        Transform labelTransform = obj.transform.Find("Label");
        if (labelTransform != null)
        {
            string label = string.IsNullOrEmpty(achievement.displayName)
                ? achievement.achievementName
                : achievement.displayName;

            SetText(labelTransform, label);
            SetTextColor(labelTransform, isUnlocked ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.35f));
        }
    }

    private void SetText(Transform target, string value)
    {
        Text legacy = target.GetComponent<Text>();
        if (legacy != null) legacy.text = value;

        TMPro.TextMeshProUGUI tmp = target.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmp != null) tmp.text = value;
    }

    private void SetTextColor(Transform target, Color value)
    {
        Text legacy = target.GetComponent<Text>();
        if (legacy != null) legacy.color = value;

        TMPro.TextMeshProUGUI tmp = target.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmp != null) tmp.color = value;
    }
}

[System.Serializable]
public class AchievementData
{
    public string achievementName;
    public Sprite badgeSprite;
    public string category;
    public string displayName;
}
