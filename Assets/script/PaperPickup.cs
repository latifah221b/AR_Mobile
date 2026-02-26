using UnityEngine;


public class PaperPickup : MonoBehaviour
{
    private void OnMouseDown()
    {
        Pickup();
    }

    void Pickup()
    {
        
        if (PapersBadge.Instance != null)
        {
            PapersBadge.Instance.CollectPaper();
        }

        
        AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.coin);
        }

        Destroy(gameObject);
    }
}