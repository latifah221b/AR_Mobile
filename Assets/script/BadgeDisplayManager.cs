using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BadgeDisplayManager : MonoBehaviour
{
    public static BadgeDisplayManager Instance;

    [Header("UI Reference")]
    [SerializeField] private GameObject badgePanel;
    [SerializeField] private Image badgeImage;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 4f;

    private AudioManager audioManager;
    private Queue badgeQueue = new Queue();
    private bool isDisplaying = false;

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
    }

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        if (badgePanel != null)
            badgePanel.SetActive(false);
    }

    
    public void ShowBadge(Sprite badgeSprite, string achievementName)
    {
        if (badgeSprite == null) return;

        
        AchievementSaveSystem.UnlockAchievement(achievementName);

        
        badgeQueue.Enqueue(new BadgeData { sprite = badgeSprite, name = achievementName });

        if (!isDisplaying)
        {
            StartCoroutine(DisplayNextBadge());
        }
    }

    private IEnumerator DisplayNextBadge()
    {
        isDisplaying = true;

        while (badgeQueue.Count > 0)
        {
            BadgeData badge = (BadgeData)badgeQueue.Dequeue();

            
            if (badgeImage != null)
                badgeImage.sprite = badge.sprite;

            if (badgePanel != null)
                badgePanel.SetActive(true);

            
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.badge);

            
            yield return new WaitForSeconds(displayDuration);

            
            if (badgePanel != null)
                badgePanel.SetActive(false);

            
            yield return new WaitForSeconds(0.5f);
        }

        isDisplaying = false;
    }

    private struct BadgeData
    {
        public Sprite sprite;
        public string name;
    }
}