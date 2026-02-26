using System.Collections;
using UnityEngine;

public class PapersBadge : MonoBehaviour
{
    public static PapersBadge Instance;

    [Header("Badge GameObjects")]
    public GameObject badgeSprite;
    public GameObject badgeSprite200;
    public GameObject badgeSprite300;

    private AudioManager audioManager;

    private int collectedPapers = 0;

    private bool triggered100 = false;
    private bool triggered200 = false;
    private bool triggered300 = false;

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
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        
        triggered100 = AchievementSaveSystem.IsAchievementUnlocked("Papers_100");
        triggered200 = AchievementSaveSystem.IsAchievementUnlocked("Papers_200");
        triggered300 = AchievementSaveSystem.IsAchievementUnlocked("Papers_300");
    }

    
    public void ShowBadge()
    {
        if (!triggered100)
        {
            triggered100 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_100");
            ShowBadgeInternal(badgeSprite);
        }
    }

    
    public void CollectPaper()
    {
        collectedPapers++;
        CheckPapers();
    }

    
    public void SetPaperCount(int count)
    {
        collectedPapers = count;
        CheckPapers();
    }

    private void CheckPapers()
    {
        if (!triggered100 && collectedPapers >= 2)
        {
            triggered100 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_100");
            ShowBadgeInternal(badgeSprite);
        }

        if (!triggered200 && collectedPapers >= 4)
        {
            triggered200 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_200");
            ShowBadgeInternal(badgeSprite200);
        }

        if (!triggered300 && collectedPapers >= 6)
        {
            triggered300 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_300");
            ShowBadgeInternal(badgeSprite300);
        }
    }

    private void ShowBadgeInternal(GameObject badge)
    {
        if (badge != null)
        {
            badge.SetActive(true);
            audioManager.PlaySFX(audioManager.badge);
            StartCoroutine(HideBadgeAfterDelay(badge, 4f));
        }
    }

    private IEnumerator HideBadgeAfterDelay(GameObject badge, float delay)
    {
        yield return new WaitForSeconds(delay);
        badge.SetActive(false);
    }

    public int GetCollectedPapers()
    {
        return collectedPapers;
    }
}