using System.Collections;
using UnityEngine;

public class PapersBadge : MonoBehaviour
{
    public static PapersBadge Instance;
    public GameObject badgeSprite;
    private AudioManager audioManager;

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
    }

    public void ShowBadge()
    {
        if (badgeSprite != null)
        {
            badgeSprite.SetActive(true);
            audioManager.PlaySFX(audioManager.badge);
            //Debug.Log("badge displayed!");
            StartCoroutine(HideBadgeAfterDelay(5f));
        }
    }

    private IEnumerator HideBadgeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        badgeSprite.SetActive(false);
        //Debug.Log("hidden after 5 seconds.");
    }
}