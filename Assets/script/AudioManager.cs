using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Reflection;
#endif

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Badge")]
    public AudioClip badge;
    [Range(0f, 3f)] public float badgeVolume = 1f;
    [Range(-3f, 3f)] public float badgePitch = 1f;

    [Header("Clear")]
    public AudioClip clear;
    [Range(0f, 3f)] public float clearVolume = 1f;
    [Range(-3f, 3f)] public float clearPitch = 1f;

    [Header("Click")]
    public AudioClip click;
    [Range(0f, 3f)] public float clickVolume = 1f;
    [Range(-3f, 3f)] public float clickPitch = 1f;

    [Header("Disclick")]
    public AudioClip disclick;
    [Range(0f, 3f)] public float disclickVolume = 1f;
    [Range(-3f, 3f)] public float disclickPitch = 1f;

    [Header("Correct")]
    public AudioClip correct;
    [Range(0f, 3f)] public float correctVolume = 1f;
    [Range(-3f, 3f)] public float correctPitch = 1f;

    [Header("Wrong")]
    public AudioClip wrong;
    [Range(0f, 3f)] public float wrongVolume = 1f;
    [Range(-3f, 3f)] public float wrongPitch = 1f;

    [Header("Coin")]
    public AudioClip coin;
    [Range(0f, 3f)] public float coinVolume = 1f;
    [Range(-3f, 3f)] public float coinPitch = 1f;

    [Header("Pages")]
    public AudioClip pages;
    [Range(0f, 3f)] public float pagesVolume = 1f;
    [Range(-3f, 3f)] public float pagesPitch = 1f;

    [Header("Parts Items")]
    public AudioClip partsitems;
    [Range(0f, 3f)] public float partsitemsVolume = 1f;
    [Range(-3f, 3f)] public float partsitemsPitch = 1f;

    [Header("Vanish")]
    public AudioClip vanish;
    [Range(0f, 3f)] public float vanishVolume = 1f;
    [Range(-3f, 3f)] public float vanishPitch = 1f;

    public void PlaySFX(AudioClip clip)
    {

        if (clip == null) return;

        if (clip == badge)
        {
            SFXSource.pitch = badgePitch;
            SFXSource.PlayOneShot(badge, badgeVolume);
        }
        else if (clip == clear)
        {
            SFXSource.pitch = clearPitch;
            SFXSource.PlayOneShot(clear, clearVolume);
        }
        else if (clip == click)
        {
            SFXSource.pitch = clickPitch;
            SFXSource.PlayOneShot(click, clickVolume);
        }
        else if (clip == disclick)
        {
            SFXSource.pitch = disclickPitch;
            SFXSource.PlayOneShot(disclick, disclickVolume);
        }
        else if (clip == correct)
        {
            SFXSource.pitch = correctPitch;
            SFXSource.PlayOneShot(correct, correctVolume);
        }
        else if (clip == wrong)
        {
            SFXSource.pitch = wrongPitch;
            SFXSource.PlayOneShot(wrong, wrongVolume);
        }
        else if (clip == coin)
        {
            SFXSource.pitch = coinPitch;
            SFXSource.PlayOneShot(coin, coinVolume);
        }
        else if (clip == pages)
        {
            SFXSource.pitch = pagesPitch;
            SFXSource.PlayOneShot(pages, pagesVolume);
        }
        else if (clip == partsitems)
        {
            SFXSource.pitch = partsitemsPitch;
            SFXSource.PlayOneShot(partsitems, partsitemsVolume);
        }
        else if (clip == vanish)
        {
            SFXSource.pitch = vanishPitch;
            SFXSource.PlayOneShot(vanish, vanishVolume);
        }

        // SFXSource.PlayOneShot(clip);

    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (!musicClip) return;

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty("musicSource"));
            UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty("SFXSource"));
            UnityEditor.EditorGUILayout.Space();

            var fields = typeof(AudioManager)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f =>
                    f.Name.EndsWith("Volume") ||
                    f.Name.EndsWith("Pitch") ||
                    f.FieldType == typeof(AudioClip)
                )
                .ToList();

            var clipGroups = fields
                .GroupBy(field =>
                {

                    if (field.Name.EndsWith("Volume"))
                        return field.Name.Replace("Volume", "");
                    if (field.Name.EndsWith("Pitch"))
                        return field.Name.Replace("Pitch", "");
                    return field.Name;
                })
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var group in clipGroups.OrderBy(k => k.Key))
            {
                string groupName = group.Key;
                var groupFields = group.Value;

                var clipFieldInfo = groupFields.FirstOrDefault(f => f.FieldType == typeof(AudioClip));
                if (clipFieldInfo == null) continue;

                var clipProp = serializedObject.FindProperty(clipFieldInfo.Name);
                if (clipProp == null) continue;

                UnityEditor.EditorGUILayout.LabelField(groupName, UnityEditor.EditorStyles.boldLabel);

                UnityEditor.EditorGUILayout.PropertyField(clipProp);

                var volumeField = groupFields.FirstOrDefault(f => f.Name.EndsWith("Volume"));
                if (volumeField != null)
                {
                    var volumeProp = serializedObject.FindProperty(volumeField.Name);
                    if (volumeProp != null)
                    {
                        UnityEditor.EditorGUILayout.PropertyField(volumeProp);
                    }
                }

                var pitchField = groupFields.FirstOrDefault(f => f.Name.EndsWith("Pitch"));
                if (pitchField != null)
                {
                    var pitchProp = serializedObject.FindProperty(pitchField.Name);
                    if (pitchProp != null)
                    {
                        UnityEditor.EditorGUILayout.PropertyField(pitchProp);
                    }
                }

                if (Application.isPlaying)
                {

                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    UnityEditor.EditorGUILayout.Space();
                    if (GUILayout.Button($"Play {groupName}", GUILayout.Width(100)))
                    {

                        AudioManager am = (AudioManager)target;
                        AudioClip ac = clipFieldInfo.GetValue(am) as AudioClip;
                        am.PlaySFX(ac);
                    }
                    UnityEditor.EditorGUILayout.EndHorizontal();
                }
                else
                {

                    UnityEditor.EditorGUILayout.HelpBox("enter play mode to test the sound", MessageType.Info);
                }

                UnityEditor.EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}