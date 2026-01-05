using UnityEngine;

public class HiddenItemListener : MonoBehaviour
{
    private HiddenItemCompletionController completionController;

    private void Start()
    {
        completionController = FindObjectOfType<HiddenItemCompletionController>();
    }

    public void OnHiddenItemAdded()
    {
        if (completionController == null) return;

        int count = InventoryManager.Instance.GetItemCount();
        completionController.OnHiddenItemCollected(count);
    }
}
