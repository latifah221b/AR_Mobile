using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Shared "Badge Unlocked!" card. One per scene. Entries are queued so that two
/// unlocks in the same frame are both seen: the next one appears when the
/// player presses Close. Also used for star rewards, which pass their own
/// title and subtitle.
/// </summary>
public class BadgePopupCard : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The root object that is switched on and off. Defaults to this GameObject.")]
    public GameObject panel;
    public Image badgeImage;
    public Text nameText;
    public Text descriptionText;

    [Header("Optional headings")]
    [Tooltip("When set, callers can override the heading per entry. Left alone, the card keeps whatever the prefab says.")]
    public TMPro.TextMeshProUGUI titleText;
    public TMPro.TextMeshProUGUI subtitleText;

    private readonly Queue<BadgeEntry> queue = new Queue<BadgeEntry>();
    private bool showing = false;
    private string defaultTitle = null;
    private string defaultSubtitle = null;
    private bool defaultsCaptured = false;

    private struct BadgeEntry
    {
        public Sprite sprite;
        public string name;
        public string description;
        public string title;
        public string subtitle;
    }

    private void Awake()
    {
        if (panel == null) panel = gameObject;
        CaptureDefaults();

        // Awake runs the first time this object is activated, which is the moment
        // ShowNext() switches the card on. Only hide here when we are NOT in the
        // middle of showing an entry, otherwise the card would close itself
        // immediately after opening.
        if (!showing) panel.SetActive(false);
    }

    private void CaptureDefaults()
    {
        if (defaultsCaptured) return;
        if (titleText != null) defaultTitle = titleText.text;
        if (subtitleText != null) defaultSubtitle = subtitleText.text;
        defaultsCaptured = true;
    }

    /// <summary>True while a badge is on screen or still waiting in the queue.</summary>
    public bool IsShowing
    {
        get { return showing || queue.Count > 0; }
    }

    /// <summary>Queue an entry using the card's own heading.</summary>
    public void Show(Sprite sprite, string badgeName, string description)
    {
        Show(sprite, badgeName, description, null, null);
    }

    /// <summary>Queue an entry with its own heading. Pass null to keep the card's heading.</summary>
    public void Show(Sprite sprite, string badgeName, string description, string title, string subtitle)
    {
        CaptureDefaults();

        BadgeEntry entry = new BadgeEntry();
        entry.sprite = sprite;
        entry.name = badgeName;
        entry.description = description;
        entry.title = title;
        entry.subtitle = subtitle;
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

        if (titleText != null)
            titleText.text = entry.title == null ? (defaultTitle == null ? titleText.text : defaultTitle) : entry.title;

        if (subtitleText != null)
            subtitleText.text = entry.subtitle == null ? (defaultSubtitle == null ? subtitleText.text : defaultSubtitle) : entry.subtitle;

        if (panel != null) panel.SetActive(true);
    }
}
