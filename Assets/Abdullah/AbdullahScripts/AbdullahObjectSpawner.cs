using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class Works fine, but needs minor modifications
/// First object spawns perfectly, stays at place and rotates. However, the next ones have issues such as:
/// -spawnning upon each others 
/// -Minor driftig
/// -Objects need to be offseted randomly 
/// </summary>
public class AbdullahObjectSpawner : MonoBehaviour
{
    public GameObject[] spawnableObjects;  
    public double bottomLeftLatitude;      
    public double bottomLeftLongitude;     
    public double topRightLatitude;        
    public double topRightLongitude;       
    public float spawnCooldown = 10f;      // Time in seconds between spawns
    private float lastSpawnTime = -10f;    
    public GameObject player;

    public Text debugText;                 

    void Start()
    {
        UpdateDebugText("Waiting for the spawn condition to be met...");
    }

    void Update()
    {
        
        double currentLatitude = GPSLocationService.latitude;
        double currentLongitude = GPSLocationService.longitude;

        // Check coordinates are within the defined range and if cooldown has passed
        if (IsWithinRange(currentLatitude, currentLongitude) && Time.time >= lastSpawnTime + spawnCooldown)
        {
            SpawnRandomObject();
            lastSpawnTime = Time.time;  
        }
    }

    private bool IsWithinRange(double latitude, double longitude)
    {
        bool withinRange = (latitude >= bottomLeftLatitude && latitude <= topRightLatitude &&
                            longitude >= bottomLeftLongitude && longitude <= topRightLongitude);
        UpdateDebugText($"Latitude: {latitude:F6}, Longitude: {longitude:F6}\nWithin Range: {withinRange}");
        return withinRange;
    }

    private void SpawnRandomObject()
    {
        if (spawnableObjects.Length == 0)
        {
            UpdateDebugText("No objects to spawn.");
            return;
        }

        
        int randomIndex = Random.Range(0, spawnableObjects.Length);
        GameObject objectToSpawn = spawnableObjects[randomIndex];

        // spawn position 
        Vector3 spawnPosition = player.transform.position; 

        
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        UpdateDebugText($"Spawned: {objectToSpawn.name} at position {spawnPosition}");
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
        else
        {
            Debug.LogWarning("Debug Text UI element is not assigned.");
        }
    }
}
