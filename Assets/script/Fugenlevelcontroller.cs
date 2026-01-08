using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controller for Fu_Gen_Lvl scene
/// Handles hidden item collection and triggers rocket fly sequence
/// Includes rocket transparency based on collected items
/// </summary>
public class FuGenLevelController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private int targetHiddenItems = 10;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI hiddenItemCountText;
    [SerializeField] private GameObject[] notificationDialogs;

    [Header("Rocket References")]
    [SerializeField] private GameObject rocket;
    [SerializeField] private Animator rocketAnimator;

    [Header("Rocket Transparency Settings")]
    [Tooltip("All renderers on the rocket that should fade in")]
    [SerializeField] private Renderer[] rocketRenderers;
    [Tooltip("Starting alpha (0.5 = 50% visible)")]
    [Range(0f, 1f)]
    [SerializeField] private float minAlpha = 0.5f;
    [Tooltip("Final alpha when all items collected (1 = 100% visible)")]
    [Range(0f, 1f)]
    [SerializeField] private float maxAlpha = 1.0f;

    [Header("Scene References")]
    [SerializeField] private sceneLoader sceneLoaderRef;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private GameObject rewardsScreen;
    [SerializeField] private Image resultStarImage;
    [SerializeField] private TextMeshProUGUI resultStepCountText;
    [SerializeField] private TextMeshProUGUI resultItemCountText;
    [SerializeField] private TextMeshProUGUI resultTimeTakenText;

    [Header("Badge References")]
    [SerializeField] private GameObject L1SpriteBadge;
    [SerializeField] private GameObject S1SpriteBadge;
    [SerializeField] private GameObject S2SpriteBadge;
    [SerializeField] private GameObject P1SpriteBadge;
    [SerializeField] private GameObject P2SpriteBadge;

    private int currentHiddenItemCount = 0;
    private bool goalReached = false;
    private bool rocketCanFly = false;
    private AudioManager audioManager;
    private float startTime;

    public static FuGenLevelController Instance { get; private set; }

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
        startTime = Time.time;

        // If rocketRenderers is empty, try to get all renderers from rocket
        if ((rocketRenderers == null || rocketRenderers.Length == 0) && rocket != null)
        {
            rocketRenderers = rocket.GetComponentsInChildren<Renderer>();
        }

        // Initialize rocket transparency
        UpdateRocketTransparency();
        UpdateUI();
    }

    /// <summary>
    /// Update rocket transparency based on collected items
    /// </summary>
    private void UpdateRocketTransparency()
    {
        if (rocketRenderers == null || rocketRenderers.Length == 0) return;

        // Calculate alpha: lerp from minAlpha to maxAlpha based on progress
        float progress = (float)currentHiddenItemCount / targetHiddenItems;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, progress);

        Debug.Log($"Rocket Alpha: {alpha * 100:F0}% (Items: {currentHiddenItemCount}/{targetHiddenItems})");

        foreach (Renderer renderer in rocketRenderers)
        {
            if (renderer == null) continue;

            foreach (Material mat in renderer.materials)
            {
                if (mat == null) continue;

                // For Toon Shader with _Alpha property
                if (mat.HasProperty("_Alpha"))
                {
                    mat.SetFloat("_Alpha", alpha);
                }

                // For shaders with Color property (like your Toon shader)
                if (mat.HasProperty("Color"))
                {
                    Color color = mat.GetColor("Color");
                    color.a = alpha;
                    mat.SetColor("Color", color);
                }

                // Fallback for standard shaders
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.GetColor("_Color");
                    color.a = alpha;
                    mat.SetColor("_Color", color);
                }

                if (mat.HasProperty("_BaseColor"))
                {
                    Color color = mat.GetColor("_BaseColor");
                    color.a = alpha;
                    mat.SetColor("_BaseColor", color);
                }
            }
        }
    }

    /// <summary>
    /// Set rocket to full visibility
    /// </summary>
    private void SetRocketFullyVisible()
    {
        if (rocketRenderers == null) return;

        foreach (Renderer renderer in rocketRenderers)
        {
            if (renderer == null) continue;

            foreach (Material mat in renderer.materials)
            {
                if (mat == null) continue;

                if (mat.HasProperty("_Alpha"))
                    mat.SetFloat("_Alpha", 1f);

                if (mat.HasProperty("Color"))
                {
                    Color color = mat.GetColor("Color");
                    color.a = 1f;
                    mat.SetColor("Color", color);
                }

                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.GetColor("_Color");
                    color.a = 1f;
                    mat.SetColor("_Color", color);
                }

                if (mat.HasProperty("_BaseColor"))
                {
                    Color color = mat.GetColor("_BaseColor");
                    color.a = 1f;
                    mat.SetColor("_BaseColor", color);
                }
            }
        }
    }

    /// <summary>
    /// Call this method when a hidden item is collected
    /// </summary>
    public void OnHiddenItemCollected()
    {
        if (goalReached) return;

        currentHiddenItemCount++;
        UpdateUI();
        UpdateRocketTransparency();

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.pages);
        }

        Debug.Log($"Hidden item collected! Count: {currentHiddenItemCount}/{targetHiddenItems}");

        if (currentHiddenItemCount >= targetHiddenItems)
        {
            goalReached = true;
            SetRocketFullyVisible();
            StartCoroutine(TriggerGoalReachedSequence());
        }
    }

    private void UpdateUI()
    {
        if (hiddenItemCountText != null)
        {
            hiddenItemCountText.text = currentHiddenItemCount.ToString();
        }
    }

    private IEnumerator TriggerGoalReachedSequence()
    {
        Debug.Log("Goal reached! Starting notification sequence...");

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.clear);
        }

        yield return new WaitForSeconds(2f);

        if (notificationDialogs.Length > 0 && notificationDialogs[0] != null)
        {
            notificationDialogs[0].SetActive(true);
            yield return new WaitForSeconds(3f);
            notificationDialogs[0].SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        if (notificationDialogs.Length > 1 && notificationDialogs[1] != null)
        {
            notificationDialogs[1].SetActive(true);
            yield return new WaitForSeconds(3f);
            notificationDialogs[1].SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        if (notificationDialogs.Length > 2 && notificationDialogs[2] != null)
        {
            notificationDialogs[2].SetActive(true);
            yield return new WaitForSeconds(3f);
            notificationDialogs[2].SetActive(false);
        }

        rocketCanFly = true;
        Debug.Log("Rocket is now ready to fly! Tap on it.");
    }

    public void OnRocketTapped()
    {
        if (!goalReached)
        {
            Debug.Log("Cannot fly yet! Collect all hidden items first.");
            return;
        }

        if (!rocketCanFly)
        {
            Debug.Log("Wait for notifications to finish...");
            return;
        }

        StartCoroutine(RocketFlySequence());
    }

    private IEnumerator RocketFlySequence()
    {
        rocketCanFly = false;

        Debug.Log("Rocket is flying!");

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.clear);
        }

        if (rocketAnimator != null)
        {
            rocketAnimator.enabled = true;
        }

        StartCoroutine(ShowResultsPanelAfterDelay(6f));

        yield return new WaitForSeconds(6f);

        if (notificationDialogs.Length > 3 && notificationDialogs[3] != null)
        {
            notificationDialogs[3].SetActive(true);
            yield return new WaitForSeconds(3f);
            notificationDialogs[3].SetActive(false);
        }

        if (notificationDialogs.Length > 4 && notificationDialogs[4] != null)
        {
            notificationDialogs[4].SetActive(true);
        }
    }

    private IEnumerator ShowResultsPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var stepCounter = FindObjectOfType<StepCounterIOSScript>();
        int stepsNow = 0;
        if (stepCounter != null)
        {
            stepsNow = stepCounter.GetCurrentStepCount();
            if (resultStepCountText != null)
            {
                resultStepCountText.text = stepsNow.ToString();
            }
        }

        if (resultItemCountText != null)
        {
            resultItemCountText.text = currentHiddenItemCount.ToString();
        }

        if (resultTimeTakenText != null)
        {
            float totalTime = Time.time - startTime;
            resultTimeTakenText.text = FormatTime(totalTime);
        }

        if (L1SpriteBadge != null)
        {
            L1SpriteBadge.SetActive(goalReached);
        }

        if (S1SpriteBadge != null && S2SpriteBadge != null)
        {
            if (currentHiddenItemCount >= targetHiddenItems)
            {
                S1SpriteBadge.SetActive(true);
                S2SpriteBadge.SetActive(false);
            }
            else
            {
                S1SpriteBadge.SetActive(false);
                S2SpriteBadge.SetActive(true);
            }
        }

        if (P1SpriteBadge != null && P2SpriteBadge != null)
        {
            if (stepsNow >= 300)
            {
                P1SpriteBadge.SetActive(true);
                P2SpriteBadge.SetActive(false);
            }
            else
            {
                P1SpriteBadge.SetActive(false);
                P2SpriteBadge.SetActive(true);
            }
        }

        if (rewardsScreen != null)
        {
            rewardsScreen.SetActive(true);
        }

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Getters
    public int GetCurrentHiddenItemCount() => currentHiddenItemCount;
    public int GetTargetHiddenItems() => targetHiddenItems;
    public bool IsGoalReached() => goalReached;
    public bool CanRocketFly() => rocketCanFly;
}