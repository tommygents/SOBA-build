using UnityEngine;
using TMPro;

public class InGameLogger : MonoBehaviour
{
    // Singleton instance
    public static InGameLogger Instance { get; private set; }

    // Reference to the TextMeshProUGUI component for displaying logs
    [SerializeField]
    private TextMeshProUGUI debugText;

    // Maximum number of log entries to keep
    [SerializeField]
    private int maxLogEntries = 50;

    // Internal storage for log messages
    private readonly System.Collections.Generic.Queue<string> logQueue = new System.Collections.Generic.Queue<string>();

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Validate that the debugText component is assigned
        if (debugText == null)
        {
            debugText = GetComponent<TextMeshProUGUI>();
            if (debugText == null)
            {
                //Debug.LogError("InGameLogger: No TextMeshProUGUI component found. Please assign it in the Inspector.");
            }
        }

        Debug.Log("InGameLogger initialized");
        if (!Debug.isDebugBuild)
        {
            debugText.enabled = false;
        }
    }

    /// <summary>
    /// Logs a message to the in-game logger UI.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void Log(string message)
    {
        if (debugText == null)
        {
            //Debug.LogError("InGameLogger: debugText is not assigned.");
            return;
        }
        

        string timeStampedMessage = $"[{System.DateTime.Now:HH:mm:ss}] {message}";
        logQueue.Enqueue(timeStampedMessage);

        // Ensure we don't exceed the maximum number of log entries
        if (logQueue.Count > maxLogEntries)
        {
            logQueue.Dequeue();
        }

        // Update the debugText UI
        debugText.text = string.Join("\n", logQueue.ToArray());
    
    }

    /// <summary>
    /// Clears all log messages from the in-game logger.
    /// </summary>
    public void Clear()
    {
        logQueue.Clear();
        if (debugText != null)
        {
            debugText.text = string.Empty;
        }
    }

    /// <summary>
    /// Optionally, you can add different log levels (Info, Warning, Error) by overloading the Log method.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    public void LogInfo(string message)
    {
        Log($"INFO: {message}");
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    public void LogWarning(string message)
    {
        Log($"WARNING: {message}");
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    public void LogError(string message)
    {
        Log($"ERROR: {message}");
    }
}
