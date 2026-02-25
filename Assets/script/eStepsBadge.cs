using UnityEngine;


public class eStepsBadge : MonoBehaviour
{
    public static eStepsBadge Instance;

    [Header("Badge Sprites")]
    [SerializeField] private Sprite badge100Steps;
    [SerializeField] private Sprite badge200Steps;
    [SerializeField] private Sprite badge300Steps;

    [Header("Achievement Names")]
    [SerializeField] private string achievement100Name = "Steps_100";
    [SerializeField] private string achievement200Name = "Steps_200";
    [SerializeField] private string achievement300Name = "Steps_300";

    private bool unlocked100 = false;
    private bool unlocked200 = false;
    private bool unlocked300 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
        unlocked100 = AchievementSaveSystem.IsAchievementUnlocked(achievement100Name);
        unlocked200 = AchievementSaveSystem.IsAchievementUnlocked(achievement200Name);
        unlocked300 = AchievementSaveSystem.IsAchievementUnlocked(achievement300Name);
    }

    
    public void CheckStepBadges(int currentSteps)
    {
        
        if (!unlocked100 && currentSteps >= 20)
        {
            unlocked100 = true;
            ShowBadge(badge100Steps, achievement100Name);
        }

        
        if (!unlocked200 && currentSteps >= 40)
        {
            unlocked200 = true;
            ShowBadge(badge200Steps, achievement200Name);
        }

        
        if (!unlocked300 && currentSteps >= 60)
        {
            unlocked300 = true;
            ShowBadge(badge300Steps, achievement300Name);
        }
    }

    private void ShowBadge(Sprite badge, string achievementName)
    {
        if (BadgeDisplayManager.Instance != null)
        {
            BadgeDisplayManager.Instance.ShowBadge(badge, achievementName);
        }
    }
}