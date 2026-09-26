using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Shared "Badge Unlocked!" card. One per scene. Badges are queued so that two
/// unlocks in the same frame are both seen: the next one appears when the
/// player closes the current card.
/// </summary>
public class BadgePopupCard : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The root object that is switched on and off. Defaults to this GameObject.")]
    public GameObject panel;
    public Image badgeImage;
    public Text nameText;
    public Text descriptionText;

    private readonly Queue<BadgeEntry> queue = new Queue<BadgeEntry>();
    private bool showing = false;

    private struct BadgeEntry
    {
        public Sprite sprite;
        public string name;
        public string description;
    }

    private void Awake()
    {
        if (panel == null) panel = gameObject;

        // Awake runs the first time this object is activated, which is the moment
        // ShowNext() switches the card on. Only hide here when we are NOT in the
        // middle of showing a badge, otherwise the card would close itself
        // immediately after opening.
        if (!showing) panel.SetActive(false);
    }

    /// <summary>Queue a badge. Shows immediately when nothing else is on screen.</summary>
    public void Show(Sprite sprite, string badgeName, string description)
    {
        BadgeEntry entry = new BadgeEntry();
        entry.sprite = sprite;
        entry.name = badgeName;
        entry.description = description;
        queue.Enqueue(entry);

        if (!showing) ShowNext();
    }

    /// <summary>Call from the Close button.</summary>
    public void Close()
    {
        showing = false;

        if (queue.Count > 0)
        {
            ShowNext();
            return;
        }

        if (panel != null) panel.SetActive(false);
    }

    private void ShowNext()
    {
        if (queue.Count == 0)
        {
            showing = false;
            if (panel != null) panel.SetActive(false);
            return;
        }

        BadgeEntry entry = queue.Dequeue();
        showing = true;

        if (badgeImage != null)
        {
            badgeImage.sprite = entry.sprite;
            badgeImage.enabled = entry.sprite != null;
        }

        if (nameText != null)
            nameText.text = entry.name == null ? "" : entry.name;

        if (descriptionText != null)
        {
            bool hasDescription = !string.IsNullOrEmpty(entry.description);
            descriptionText.text = hasDescription ? entry.description : "";
            descriptionText.gameObject.SetActive(hasDescription);
        }

        if (panel != null) panel.SetActive(true);
    }
}
