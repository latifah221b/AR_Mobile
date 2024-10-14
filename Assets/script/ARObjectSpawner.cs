using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;





public class ARObjectSpawner : MonoBehaviour
{
    public static int number_of_enemy = 2;
    [SerializeField] private GameObject objectsToSpawn;
    private ARPlaneManager aRPlaneManager;

    private int count = 0;
    [SerializeField] private sceneLoader _sceneLoaderRef;
    [SerializeField] private GameObject _tappingCanvas;
    private Camera arCamera;
    private float maxDistance = 2f;
    private Vector2 requiredSize = new Vector2(0.3f, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to ARPlaneManager
        aRPlaneManager = GetComponent<ARPlaneManager>();

        aRPlaneManager.planesChanged += OnPlanesChanged;

        arCamera = Camera.main;
        var enemyController = objectsToSpawn.GetComponent<EnemyController>();
        enemyController.set_Scene_Loader(_sceneLoaderRef);

    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null && args.added.Count > 0)
        {
            var detectedPlane = args.added[0];
            // check if the detectedPlane close enough to the camera and its space is big enough to visualize the enemy. 

            if (IsPlaneSuitable(detectedPlane))
            {
               
                PositionObjectOnPlane(detectedPlane);

                count++;
                if (count == 1) { _tappingCanvas.SetActive(true); }
                if (number_of_enemy == count)
                {
                    aRPlaneManager.planesChanged -= OnPlanesChanged;

                }
            }


        }
    }

    void OnDestroy()
    {
        aRPlaneManager.planesChanged -= OnPlanesChanged;
    }

    void PositionObjectOnPlane(ARPlane plane)
    {
        Vector3 spawnPosition = plane.center;
        // add an offset 
        spawnPosition.y += 0.1f;

        Quaternion planeRotation = Quaternion.LookRotation(plane.normal, Vector3.up);

        var obj = Instantiate(objectsToSpawn, spawnPosition, planeRotation);
        MakeObjectLookAtCamera(obj.transform);
        var enemyController = obj.GetComponent<EnemyController>();
        //enemyController.set_transform(obj.transform);
    }

    // Function to check distance 
    private bool IsPlaneCloseEnough(ARPlane arPlane, float maxDistance)
    {
        // calculate the distance between the AR Camera and the plane's center 
        float distance = Vector3.Distance(arCamera.transform.position, arPlane.center);

        // check if the distance is less than the max distance allowed 
        if (distance <= maxDistance)
        {
            Debug.Log("Plane is close enough.");
            Debug.Log("distance:" + string.Format("{0:N3}", distance));

            return true;
        }
        Debug.Log("Plane is too far.");
        Debug.Log("distance:" + string.Format("{0:N3}", distance));
        return false;
    }

    private bool IsPlaneLargeEnough(ARPlane arPlane, Vector2 requiredSize)
    {
        // Get the plane's current size (in meters)
        Vector2 planeSize = arPlane.size;

        // check if the plane's dimensions are greater than the required size 
        if (planeSize.x >= requiredSize.x && planeSize.y >= requiredSize.y)
        {
            Debug.Log("Plane is large enough");
            return true;
        }
        Debug.Log("Plane is too small. ");
        return false;
    }

    // check if the plane is suitable (both close enough and large enough) 

    private bool IsPlaneSuitable(ARPlane arPlane)
    {
        // check if the plane is close enough 
        bool isCloseEnough = IsPlaneCloseEnough(arPlane, maxDistance);

        // check if the plane is large enough 
        bool isLargeEnough = IsPlaneLargeEnough(arPlane, requiredSize);

        // Return true only if both conditions are met 

        return isCloseEnough && isLargeEnough;
    }


    void MakeObjectLookAtCamera(Transform objectTransform)
    {
        objectTransform.forward = -objectTransform.forward;
        Vector3 cameraPosition = arCamera.transform.position;
        objectTransform.LookAt(cameraPosition);
        objectTransform.rotation = Quaternion.Euler(0f, objectTransform.rotation.eulerAngles.y, 0f);

        objectTransform.forward = -objectTransform.forward;

    }

    IEnumerator final_logic()
    {
        {
            yield return new WaitForSecondsRealtime(2);
            _sceneLoaderRef.LoadA("scene4");

        }
    }
}
