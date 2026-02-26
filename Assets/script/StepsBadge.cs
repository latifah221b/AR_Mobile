using System.Collections;
using UnityEngine;

public class StepsBadge : MonoBehaviour
{
    public static StepsBadge Instance;

    [Header("Badge GameObjects")]
    public GameObject badgeSprite;
    public GameObject badgeSprite200;
    public GameObject badgeSprite300;

    private AudioManager audioManager;

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

        
        triggered100 = AchievementSaveSystem.IsAchievementUnlocked("Steps_100");
        triggered200 = AchievementSaveSystem.IsAchievementUnlocked("Steps_200");
        triggered300 = AchievementSaveSystem.IsAchievementUnlocked("Steps_300");
    }

    
    public void ShowBadge()
    {
        if (!triggered100)
        {
            triggered100 = true;
            AchievementSaveSystem.UnlockAchievement("Steps_100");
            ShowBadgeInternal(badgeSprite);
        }
    }

    
    public void CheckSteps(int stepCount)
    {
        if (!triggered100 && stepCount >= 40)
        {
            triggered100 = true;
            AchievementSaveSystem.UnlockAchievement("Steps_100");
            ShowBadgeInternal(badgeSprite);
        }

        if (!triggered200 && stepCount >= 60)
        {
            triggered200 = true;
            AchievementSaveSystem.UnlockAchievement("Steps_200");
            ShowBadgeInternal(badgeSprite200);
        }

        if (!triggered300 && stepCount >= 80)
        {
            triggered300 = true;
            AchievementSaveSystem.UnlockAchievement("Steps_300");
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
}