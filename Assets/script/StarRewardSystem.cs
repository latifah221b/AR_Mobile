using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarRewardSystem : MonoBehaviour
{
    [SerializeField] private Image starRewardImage;
    [SerializeField] private Sprite oneStarSprite;
    [SerializeField] private Sprite twoStarsSprite;
    [SerializeField] private Sprite threeStarsSprite;

    private int rocketPartCount = 0;
    private int itemCount = 0;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void CollectRocketPart()
    {
        rocketPartCount++;
        CheckReward();
    }

    public void CollectItem()
    {
        itemCount++;
        // Debug.Log("DEBUG: CollectItem => itemCount = " + itemCount);
    }

    private void CheckReward()
    {
        // This logic only updates the "starRewardImage" if rocketPartCount hits 3
        // Then checks how many side items for the 1/2/3 star award
        if (rocketPartCount == 3)
        {
            if (itemCount >= 6)
            {
                ShowStarReward(threeStarsSprite);
            }
            else if (itemCount >= 3)
            {
                ShowStarReward(twoStarsSprite);
            }
            else
            {
                ShowStarReward(oneStarSprite);
            }
        }
    }

    private void ShowStarReward(Sprite starSprite)
    {
        starRewardImage.sprite = starSprite;
        StartCoroutine(ShowRewardWithDelay());
    }

    private IEnumerator ShowRewardWithDelay()
    {
        audioManager.PlaySFX(audioManager.badge);
        starRewardImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        starRewardImage.gameObject.SetActive(false);
    }

    // =============== GETTERS ===============

    // If rocketPartCount < 3, we haven't triggered any star threshold
    public Sprite GetCurrentRewardSprite()
    {
        if (rocketPartCount < 3)
        {
            return null; // Or a default sprite if you prefer
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
