using UnityEngine;
using UnityEngine.Android;

public class PermissionRequester : MonoBehaviour
{
    private const string ActivityRecognitionPermission = "android.permission.ACTIVITY_RECOGNITION";

    void Start()
    {
        // Check and request step counter and location permissions on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // Request Activity Recognition permission (for step counter)
            if (!Permission.HasUserAuthorizedPermission(ActivityRecognitionPermission))
            {
                Permission.RequestUserPermission(ActivityRecognitionPermission);
            }

            // Request Fine Location permission (for GPS)
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }

            // Request Coarse Location permission (for GPS)
            if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            {
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }
        }
    }
}
