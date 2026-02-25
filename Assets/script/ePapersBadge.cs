using UnityEngine;


public class ePapersBadge : MonoBehaviour
{
    public static ePapersBadge Instance;

    [Header("Badge Sprites")]
    [SerializeField] private Sprite badge100Papers;
    [SerializeField] private Sprite badge200Papers;
    [SerializeField] private Sprite badge300Papers;

    [Header("Achievement Names")]
    [SerializeField] private string achievement100Name = "Papers_100";
    [SerializeField] private string achievement200Name = "Papers_200";
    [SerializeField] private string achievement300Name = "Papers_300";

    private bool unlocked100 = false;
    private bool unlocked200 = false;
    private bool unlocked300 = false;

    private int collectedPapers = 0;

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

    
    public void CollectPaper()
    {
        collectedPapers++;
        CheckPaperBadges();
    }

    
    public void SetPaperCount(int count)
    {
        collectedPapers = count;
        CheckPaperBadges();
    }

    
    private void CheckPaperBadges()
    {
        
        if (!unlocked100 && collectedPapers >= 2)
        {
            unlocked100 = true;
            ShowBadge(badge100Papers, achievement100Name);
        }

        
        if (!unlocked200 && collectedPapers >= 4)
        {
            unlocked200 = true;
            ShowBadge(badge200Papers, achievement200Name);
        }

        
        if (!unlocked300 && collectedPapers >= 6)
        {
            unlocked300 = true;
            ShowBadge(badge300Papers, achievement300Name);
        }
    }

    private void ShowBadge(Sprite badge, string achievementName)
    {
        if (BadgeDisplayManager.Instance != null)
        {
            BadgeDisplayManager.Instance.ShowBadge(badge, achievementName);
        }
    }

    public int GetCollectedPapers()
    {
        return collectedPapers;
    }
}