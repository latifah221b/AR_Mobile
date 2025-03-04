using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class StepCounterIOSScript : MonoBehaviour
{
    public GameObject cube;
    public Vector3 rotate;
    public GUIStyle msgStyle;

    [SerializeField] private TMP_Text step_counter_txt;

    [SerializeField] private GameObject[] papers;
    private int currentStepCount = 0;
    private int nextPaperStep = 8;
    private int paperIndex = 0;
    private const float maxSpawnDistance = 2.0f;
    private int maxPapers = 6;
    private const int stepBadgeGoal = 100;

    private bool hasTriggeredBadge = false;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UnityOnStart();
#endif

    void Start()
    {
        _msg = "0";

#if UNITY_IOS && !UNITY_EDITOR
        UnityOnStart();
#endif
    }

    private string _msg;

    private void OnMessageReceived(string msg)
    {
        _msg = msg;

        if (int.TryParse(_msg, out int steps))
        {
            if (step_counter_txt != null)
            {
                step_counter_txt.text = steps.ToString();
                HandleStepCount(steps);
            }
        }
        else
        {
            Debug.LogWarning("DEBUG: Could not parse step count from msg: " + msg);
        }
    }

    private void HandleStepCount(int steps)
    {
        currentStepCount = steps;
        Debug.Log("DEBUG: StepCounter - currentStepCount = " + currentStepCount);

        // Example logic: spawn a paper every 5 steps after 8, etc.
        if (paperIndex < maxPapers && currentStepCount >= nextPaperStep && paperIndex < papers.Length)
        {
            SpawnPaper();
            nextPaperStep += 5;
        }

        // If >= 300 steps, trigger a badge
        if (currentStepCount >= stepBadgeGoal && !hasTriggeredBadge)
        {
            TriggerStepBadge();
        }
    }

    private void SpawnPaper()
    {
        if (paperIndex >= maxPapers) return;

        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 randomDirection = Random.insideUnitSphere * maxSpawnDistance;
        randomDirection.y = 0;
        Vector3 spawnPosition = playerPosition + randomDirection;

        Instantiate(papers[paperIndex], spawnPosition, Quaternion.identity);
        paperIndex++;
        Debug.Log("DEBUG: Spawned paper index " + paperIndex + " at " + spawnPosition);
    }

    private void TriggerStepBadge()
    {
        StepsBadge.Instance.ShowBadge();
        Debug.Log("DEBUG: StepBadge triggered at stepCount = " + currentStepCount);
        hasTriggeredBadge = true;
    }

    void Update()
    {
        if (step_counter_txt != null)
        {
            step_counter_txt.text = _msg;
        }
    }

    public int GetCurrentStepCount()
    {
        return currentStepCount;
    }
}
