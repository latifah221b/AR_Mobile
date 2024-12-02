using System;
using TMPro;
using System.Collections;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacingObjAtLatLngAlt : MonoBehaviour
    {
        //Tracking information using GeospatialAPI
        [SerializeField] AREarthManager EarthManager;
        //Used for initializing ARCore and GeospatialAPI
        [SerializeField] GeospatialInit Initializer;
        //UI for displaying the tracking result
        [SerializeField] TMP_Text OutputText;
        //Heading accuracy (change the value in the Inspector)
        [SerializeField] double HeadingThreshold = 25;
        //Horizontal position accuracy (change the value in the Inspector)
        [SerializeField] double HorizontalThreshold = 20;

        //Latitude of the object to be placed
      double Latitude = 24.715714035782835;
        //Longitude of the object to be placed
      double Longitude = 46.64380104408511;
        //Altitude of the object to be placed [m] (geoid height + elevation. See elevation.txt for how to calculate)
        [SerializeField] double Altitude;
        //Flag to force the object to be placed on the ground. (If false, the object will be placed at the altitude specified by Altitude)
        [SerializeField] bool ForcePutOnTerrain = false;
        //Heading of the object (North = 0°)
        [SerializeField] double Heading;
        //The original data of the object to be displayed
        [SerializeField] GameObject ContentPrefab;
        //The object to be displayed
        GameObject displayObject;
        //Manager for creating anchors
        [SerializeField] ARAnchorManager AnchorManager;
        bool initialized = false;
    
    
        private ARGeospatialAnchor anchor;
        private double distance = 1000000;

    public double getdistance() {
        return this.distance;
    }


    // Update is called once per frame
    void Update()
        {
            //Return if initialization failed or tracking is not available
            if (!Initializer.IsReady || EarthManager.EarthTrackingState != TrackingState.Tracking)
            {
                //Debug.Log("EarthManager: " + EarthManager.EarthTrackingState);
               // Debug.Log("Initializer: " + EarthManager.EarthTrackingState);
            return;
            }
            //Tracking status to be displayed
            string status = "";
            //Get the tracking result
            GeospatialPose pose = EarthManager.CameraGeospatialPose;
            //The case where the tracking accuracy is worse than the threshold (the value is large)
            if (pose.OrientationYawAccuracy > HeadingThreshold ||
                  pose.HorizontalAccuracy > HorizontalThreshold)
            {
                status = "Low Tracking Accuracy Please look arround.";
            }
            else
            {
                status = "High Tracking Accuracy";
                if (!initialized)
                {
                    initialized = true;
                    //Create and place a virtual object.
                    SpawnObject(pose, ContentPrefab);
                }
            }
            //Display the tracking result
            ShowTrackingInfo(status, pose);
        }

        //Instantiate the object to be displayed
        void SpawnObject(GeospatialPose pose, GameObject prefab)
        {
         
            //Create a rotation quaternion that has the +Z axis pointing in the same direction as the heading value (heading=0 means north direction)
            //https://developers.google.com/ar/develop/unity-arf/geospatial/developer-guide-android#place_a_geospatial_anchor
            Quaternion quaternion = Quaternion.AngleAxis(180f - (float)Heading, Vector3.up);
            Altitude = pose.Altitude - 1.5f;

            if (ForcePutOnTerrain)
            {
                ResolveAnchorOnTerrainPromise terrainPromise =
                    AnchorManager.ResolveAnchorOnTerrainAsync(Latitude, Longitude, 0, quaternion);
                StartCoroutine(CheckTerrainPromise(terrainPromise));
            }
            else
            {
                anchor = AnchorManager.AddAnchor(Latitude, Longitude, Altitude, quaternion);
                if (anchor != null)
                {
                   // displayObject = Instantiate(ContentPrefab, anchor.transform);
                }
            }
        }
        private IEnumerator CheckTerrainPromise(ResolveAnchorOnTerrainPromise promise)
        {
            var retry = 0;
            while (promise.State == PromiseState.Pending)
            {
                yield return new WaitForSeconds(0.1f);
                retry = Math.Min(retry + 1, 100);
            }

            var result = promise.Result;
            if (result.TerrainAnchorState == TerrainAnchorState.Success &&
                result.Anchor != null)
            {
                //displayObject = Instantiate(ContentPrefab, result.Anchor.gameObject.transform);
              //  displayObject.transform.parent = result.Anchor.gameObject.transform;
            }
            yield break;
        }
        void ShowTrackingInfo(string status, GeospatialPose pose)
        {

        double distance = GeospatialDistance.Haversine(pose.Latitude, pose.Longitude, Latitude, Longitude); 

        if (OutputText == null) return;
        OutputText.text = string.Format(
               "\n" +
               "Latitude/Longitude {0}°, {1}°\n" +
               "Horizontal Accuracy {2}m\n" +
               "Altitude {3}m\n" +
               "Vertical Accuracy {4}m\n" +
               "Heading {5}°\n" +
               "Heading Accuracy {6}°\n" +
               "{7} \n \n" +
               "GeospatialDistance from Camera {8}m\n" + "Take Walk to minimize the distance between you and the rocket"
               ,
               pose.Latitude.ToString("F6"),  //{0}
               pose.Longitude.ToString("F6"), //{1}
               pose.HorizontalAccuracy.ToString("F6"), //{2}
               pose.Altitude.ToString("F2"),  //{3}
               pose.VerticalAccuracy.ToString("F2"),  //{4}
               pose.EunRotation.ToString("F1"),   //{5}
               pose.OrientationYawAccuracy.ToString("F1"),   //{6}
               status, //{7}
               (distance * 1000.0f) // {8}
           );
            this.distance = (distance * 1000.0f);


        }

   

}


