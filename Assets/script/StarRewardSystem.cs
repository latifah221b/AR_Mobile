using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarRewardSystem : MonoBehaviour
{
    [SerializeField] private Image starRewardImage;
    [SerializeField] private Sprite oneStarSprite;
    [SerializeField] private Sprite twoStarsSprite;
    [SerializeField] private Sprite threeStarsSprite;

    [Header("Reward Popup Card (optional)")]
    [Tooltip("When set, the reward appears on this shared card and stays until the player presses Close. When empty, the old 5-second auto-hide image is used.")]
    public BadgePopupCard popupCard;

    [Header("Reward Popup Text")]
    public string rewardTitle = "Level Complete!";
    public string rewardSubtitle = "Here is how you did";
    public string oneStarName = "1 Star";
    public string twoStarsName = "2 Stars";
    public string threeStarsName = "3 Stars";

    private int rocketPartCount = 0;
    private int itemCount = 0;
    private AudioManager audioManager;

    private void Start()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null) audioManager = audioObject.GetComponent<AudioManager>();
    }

    public void CollectRocketPart()
    {
        rocketPartCount++;
        CheckReward();
    }

    public void CollectItem()
    {
        itemCount++;
    }

    private void CheckReward()
    {
        if (rocketPartCount == 3)
        {
            if (itemCount >= 6)
            {
                ShowStarReward(threeStarsSprite, threeStarsName);
            }
            else if (itemCount >= 3)
            {
                ShowStarReward(twoStarsSprite, twoStarsName);
            }
            else
            {
                ShowStarReward(oneStarSprite, oneStarName);
            }
        }
    }

    private void ShowStarReward(Sprite starSprite, string rewardName)
    {
        if (popupCard != null)
        {
            popupCard.Show(starSprite, rewardName, "", rewardTitle, rewardSubtitle);

            if (audioManager != null)
                audioManager.PlaySFX(audioManager.badge);

            return;
        }

        if (starRewardImage == null) return;

        starRewardImage.sprite = starSprite;
        StartCoroutine(ShowRewardWithDelay());
    }

    private IEnumerator ShowRewardWithDelay()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.badge);

        starRewardImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        starRewardImage.gameObject.SetActive(false);
    }

    // =============== GETTERS ===============

    public Sprite GetCurrentRewardSprite()
    {
        if (rocketPartCount < 3)
        {
            return null;
        }

        if (itemCount >= 6)
        {
            return threeStarsSprite;
        }
        else if (itemCount >= 3)
        {
            return twoStarsSprite;
        }
        else
        {
            return oneStarSprite;
        }
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    public int GetRocketPartCount()
    {
        return rocketPartCount;
    }
}
