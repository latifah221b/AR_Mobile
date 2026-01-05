using UnityEngine;

public class HiddenItemCompletionController : MonoBehaviour
{
    [SerializeField] private int targetHiddenItems = 6;
    [SerializeField] private Animator rocketAnimator;
    [SerializeField] private AudioManager audioManager;

    private bool completed = false;

    public void OnHiddenItemCollected(int currentCount)
    {
        if (completed) return;

        if (currentCount >= targetHiddenItems)
        {
            completed = true;
            TriggerCompletion();
        }
    }

    private void TriggerCompletion()
    {
        Debug.Log("Hidden items goal reached!");

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.clear);

        if (rocketAnimator != null)
            rocketAnimator.enabled = true;
    }
}
