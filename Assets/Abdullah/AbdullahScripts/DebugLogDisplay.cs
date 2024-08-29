using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugLogDisplay : MonoBehaviour
{
    public Text debugText; // The UI Text component to display logs
    private Queue<string> logQueue = new Queue<string>();
    public int maxMessages = 20; // Maximum number of log messages to display

    void OnEnable()
    {
        // Subscribe to Unity's log message events
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Unsubscribe from Unity's log message events
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Add new log message to the queue
        logQueue.Enqueue(logString);

        // Remove oldest message if the queue exceeds max size
        if (logQueue.Count > maxMessages)
        {
            logQueue.Dequeue();
        }

        // Update the displayed text with the concatenated log messages
        debugText.text = string.Join("\n", logQueue.ToArray());
    }
}
