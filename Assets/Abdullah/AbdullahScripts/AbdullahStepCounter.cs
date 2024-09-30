using UnityEngine;
using TMPro;


public class AbdullahStepCounter : MonoBehaviour
{
    private AndroidJavaObject sensorManager;
    private AndroidJavaObject stepCounterSensor;
    private AndroidJavaObject unityActivity;
    private StepCounterListener stepCounterListener;
    private int stepsSinceStart = 0;
    private int initialStepCount = -1; // To store the initial step count when the app starts
    private bool isCounting = false;

    [SerializeField] private TMP_Text stepsText; // UI Text to display the step count

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            InitializeStepCounter();
        }
        else
        {
            Debug.LogWarning("Step counter is only supported on Android devices.");
        }
    }

    private void InitializeStepCounter()
    {
        // Get the current activity context
        unityActivity = GetUnityActivity();

        // Get the SensorManager from the Android context
        sensorManager = unityActivity.Call<AndroidJavaObject>("getSystemService", "sensor");

        // Check if the step counter sensor is available
        stepCounterSensor = sensorManager.Call<AndroidJavaObject>("getDefaultSensor", 19); // TYPE_STEP_COUNTER = 19

        if (stepCounterSensor == null)
        {
            Debug.LogError("Step Counter Sensor not available on this device.");
            return;
        }

        // Create a SensorEventListener using AndroidJavaProxy
        stepCounterListener = new StepCounterListener(this);

        // Register the listener with SENSOR_DELAY_FASTEST
        isCounting = sensorManager.Call<bool>("registerListener", stepCounterListener, stepCounterSensor, SensorDelay.FASTEST);

        if (isCounting)
        {
            Debug.Log("Step counter initialized and listening for steps.");
        }
        else
        {
            Debug.LogError("Failed to register step counter listener.");
        }
    }

    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    public void OnStepCounterUpdate(int steps)
    {
        Debug.LogWarning("OnStepCounterUpdate");
        if (initialStepCount == -1)
        {
            // Set the initial step count the first time a step count is received
            initialStepCount = steps;
        }

        // Calculate steps since the app started
        stepsSinceStart = steps - initialStepCount;

        UpdateStepsText();
    }

    private void UpdateStepsText()
    {
        if (stepsText != null)
        {
            stepsText.text =  stepsSinceStart.ToString();
        }
        else
        {
            Debug.LogWarning("Steps Text UI element is not assigned.");
        }
    }

    void LateUpdate()
    {
        UpdateStepsText();
    }

    void OnDestroy()
    {
        // Unregister the listener when the script is destroyed
        if (isCounting)
        {
            sensorManager.Call("unregisterListener", stepCounterListener, stepCounterSensor);
        }
    }

    private class StepCounterListener : AndroidJavaProxy
    {
        private AbdullahStepCounter stepCounter;

        public StepCounterListener(AbdullahStepCounter stepCounter) : base("android.hardware.SensorEventListener")
        {
            this.stepCounter = stepCounter;
        }

        void onSensorChanged(AndroidJavaObject sensorEvent)
        {
            // Get the values from the sensor event
            float[] values = sensorEvent.Get<float[]>("values");
            if (values != null && values.Length > 0)
            {
                int steps = (int)values[0];
                stepCounter.OnStepCounterUpdate(steps);
            }
            else
            {
                Debug.LogWarning("Step counter values are null or empty.");
            }
        }

        void onAccuracyChanged(AndroidJavaObject sensor, int accuracy)
        {
            // Not needed for step counter
        }
    }

    // Custom enumeration for sensor delay
    private static class SensorDelay
    {
        public const int FASTEST = 0; // SENSOR_DELAY_FASTEST constant for highest frequency updates
        public const int GAME = 1; // SENSOR_DELAY_GAME
        public const int UI = 2; // SENSOR_DELAY_UI
        public const int NORMAL = 3; // SENSOR_DELAY_NORMAL
    }
}
