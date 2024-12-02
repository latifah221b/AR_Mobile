using UnityEngine;
using System.Collections;

public class GeolocationManager : MonoBehaviour
{
    private void Start()
    {
        // Start the location service
        StartLocationService();
    }

    private void StartLocationService()
    {
        // Check if location service is enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled by the user.");
            return;
        }

        // Start the location service
        Input.location.Start(1f, 0.1f); // Start with desired accuracy and distance

        // Start a coroutine to get the location data
        StartCoroutine(GetLocationData());
    }

    private IEnumerator GetLocationData()
    {
        // Wait until the location service is initialized
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }

        // Check if there was an error while initializing
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location.");
            yield break;
        }

        // Access the device's location data
        while (true)
        {
            // Get the current location
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float altitude = Input.location.lastData.altitude;
            float horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
            float verticalAccuracy = Input.location.lastData.verticalAccuracy;
            System.DateTime timestamp = System.DateTime.Now;

            // Log the location data
           // Debug.Log($"Latitude: {latitude}, Longitude: {longitude}, Altitude: {altitude}");
            //Debug.Log($"Horizontal Accuracy: {horizontalAccuracy}, Vertical Accuracy: {verticalAccuracy}, Timestamp: {timestamp}");

            // Wait for a specified interval before checking again
            yield return new WaitForSeconds(5); // Adjust as needed
        }
    }

    private void OnDisable()
    {
        // Stop the location service when the script is disabled
        Input.location.Stop();
    }
}