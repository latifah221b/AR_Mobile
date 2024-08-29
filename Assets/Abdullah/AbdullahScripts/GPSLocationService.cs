using UnityEngine;
using UnityEngine.UI;  
using System.Collections;

public class GPSLocationService : MonoBehaviour
{
    public static float latitude;
    public static float longitude;
    public Text coordinatesText;  
    public float updateInterval = 1f; // Time in seconds between updates
    private float nextUpdateTime = 0f; 

    void Start()
    {
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
       
        if (!Input.location.isEnabledByUser)
        {
           // Debug.Log("Location services are not enabled by the user.");
            yield break;
        }

        Input.location.Start(1f, 1f); // Start location service with desired accuracy and update distance

        // Wait until the service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

       
        if (maxWait <= 0)
        {
            //Debug.Log("Timed out while initializing location services.");
            yield break;
        }

        
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //Debug.Log("Unable to determine device location.");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            UpdateCoordinatesDisplay();
        }

        // we can stop here, we keep it running to continuously get updates
    }

    void Update()
    {
        
        if (Input.location.status == LocationServiceStatus.Running)
        {
            if (Time.time >= nextUpdateTime)
            {
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
                UpdateCoordinatesDisplay();
                nextUpdateTime = Time.time + updateInterval; // Schedule the next update
            }
        }
    }

    private void UpdateCoordinatesDisplay()
    {
        if (coordinatesText != null)
        {
            coordinatesText.text = $"Latitude: {latitude:F6}\nLongitude: {longitude:F6}";
        }
    }
}
