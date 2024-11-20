using System.Collections;
using UnityEngine;

public class StepsBadge : MonoBehaviour
{
    public static StepsBadge Instance;
    public GameObject badgeSprite;

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

    public void ShowBadge()
    {
        if (badgeSprite != null)
        {
            badgeSprite.SetActive(true);
            //Debug.Log("badge displayed!");
            StartCoroutine(HideBadgeAfterDelay(5f));
        }
    }

    private IEnumerator HideBadgeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        badgeSprite.SetActive(false);
        //Debug.Log("badge hidden.");
    }
}
