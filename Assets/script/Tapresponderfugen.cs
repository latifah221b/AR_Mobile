using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Tap Responder for Fu_Gen_Lvl scene
/// Handles taps on hidden items and rocket
/// </summary>
public class TapResponderFuGen : MonoBehaviour, INotifyOnTap
{
    [SerializeField] private TextMeshProUGUI hiddenItemCountText;

    private AudioManager audioManager;

    private void Start()
    {
        GameObjectManager.Instance.RegisterNotifier(this);
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    public void OnTap(Vector2 tapPosition)
    {
        Collider hitCollider = CheckTapPosition(tapPosition);

        if (hitCollider != null)
        {
            HandleTag(hitCollider);
        }
    }

    private void HandleTag(Collider collider)
    {
        switch (collider.tag)
        {
            case "hidden_item":
                // Hidden items are handled by HiddenItemPickupFuGen via OnMouseDown
                // But we can also handle it here if needed
                Debug.Log("Hit a hidden item!");
                break;

            case "star_box":
                // Handle collectible star boxes (side items)
                audioManager?.PlaySFX(audioManager.coin);
                Destroy(collider.transform.parent?.gameObject);
                Destroy(collider);
                break;

            case "rocket":
                Debug.Log("Rocket tapped!");

                // Check with FuGenLevelController if we can fly
                if (FuGenLevelController.Instance != null)
                {
                    FuGenLevelController.Instance.OnRocketTapped();
                }
                else
                {
                    Debug.LogWarning("FuGenLevelController not found!");
                }
                break;

            default:
                break;
        }
    }

    private Collider CheckTapPosition(Vector2? tapPosition)
    {
        if (tapPosition == null) return null;

        RaycastHit hit;
        Vector2 screenPos = (Vector2)tapPosition;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null)
            {
                return hit.collider;
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        if (GameObjectManager.Instance != null)
        {
            GameObjectManager.Instance.UnregisterNotifier(this);
        }
    }
}