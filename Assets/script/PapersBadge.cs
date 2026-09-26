using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PapersBadge : MonoBehaviour
{
    public static PapersBadge Instance;

    [Header("Badge GameObjects")]
    public GameObject badgeSprite;
    public GameObject badgeSprite200;
    public GameObject badgeSprite300;

    [Header("Badge Popup Card (optional)")]
    [Tooltip("When set, badges appear on this shared card and stay until the player presses Close. When empty, the old 4-second auto-hide behaviour is used.")]
    public BadgePopupCard popupCard;

    [Header("Badge Names")]
    public string badgeName100 = "2 Coins";
    public string badgeName200 = "4 Coins";
    public string badgeName300 = "6 Coins";

    [Header("Badge Descriptions (optional)")]
    [TextArea] public string badgeDescription100 = "";
    [TextArea] public string badgeDescription200 = "";
    [TextArea] public string badgeDescription300 = "";

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

        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null) audioManager = audioObject.GetComponent<AudioManager>();

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
            ShowBadgeInternal(badgeSprite, badgeName100, badgeDescription100);
        }
    }

    private int collectedPapers = 0;

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

    public int GetCollectedPapers()
    {
        return collectedPapers;
    }

    private void CheckPapers()
    {
        if (!triggered100 && collectedPapers >= 2)
        {
            triggered100 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_100");
            ShowBadgeInternal(badgeSprite, badgeName100, badgeDescription100);
        }

        if (!triggered200 && collectedPapers >= 4)
        {
            triggered200 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_200");
            ShowBadgeInternal(badgeSprite200, badgeName200, badgeDescription200);
        }

        if (!triggered300 && collectedPapers >= 6)
        {
            triggered300 = true;
            AchievementSaveSystem.UnlockAchievement("Papers_300");
            ShowBadgeInternal(badgeSprite300, badgeName300, badgeDescription300);
        }
    }

    private void ShowBadgeInternal(GameObject badge, string badgeName, string description)
    {
        if (popupCard != null)
        {
            Sprite sprite = null;
            if (badge != null)
            {
                Image source = badge.GetComponent<Image>();
                if (source != null) sprite = source.sprite;
            }

            popupCard.Show(sprite, badgeName, description);

            if (audioManager != null)
                audioManager.PlaySFX(audioManager.badge);

            return;
        }

        if (badge != null)
        {
            badge.SetActive(true);

            if (audioManager != null)
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
