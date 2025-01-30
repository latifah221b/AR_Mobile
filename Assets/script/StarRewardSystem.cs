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
    }

    private void CheckReward()
    {
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
        //yield return new WaitForSeconds(5f);
        audioManager.PlaySFX(audioManager.badge);
        starRewardImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        starRewardImage.gameObject.SetActive(false);
    }

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
}