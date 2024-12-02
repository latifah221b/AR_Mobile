using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlaneIterator : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;

    void Start()
    {
        // Get the ARPlaneManager component
        arPlaneManager = GetComponent<ARPlaneManager>();

        if (arPlaneManager == null)
        {
            //Debug.LogError("ARPlaneManager not found! Please attach the script to a GameObject with ARPlaneManager.");
            return;
        }

        // Start iterating over detected planes
        IterateOverDetectedPlanes();
    }

    void Update()
    {
        // Continuously check for planes in the update if needed
        IterateOverDetectedPlanes();
    }

    private void IterateOverDetectedPlanes()
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            // Log the plane's ID, center position, and size
           // Debug.Log($"Plane ID: {plane.trackableId}, Center: {plane.center}, Size: {plane.size}");
        }
      
        
    }
}
