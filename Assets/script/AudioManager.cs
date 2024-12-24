using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip badge;
    [Range(0f, 3f)] public float badgeVolume = 1f;
    [Range(-3f, 3f)] public float badgePitch = 1f;

    public AudioClip clear;
    [Range(0f, 3f)] public float clearVolume = 1f;
    [Range(-3f, 3f)] public float clearPitch = 1f;

    public AudioClip click;
    [Range(0f, 3f)] public float clickVolume = 1f;
    [Range(-3f, 3f)] public float clickPitch = 1f;

    public AudioClip disclick;
    [Range(0f, 3f)] public float disclickVolume = 1f;
    [Range(-3f, 3f)] public float disclickPitch = 1f;

    public AudioClip correct;
    [Range(0f, 3f)] public float correctVolume = 1f;
    [Range(-3f, 3f)] public float correctPitch = 1f;

    public AudioClip wrong;
    [Range(0f, 3f)] public float wrongVolume = 1f;
    [Range(-3f, 3f)] public float wrongPitch = 1f;

    public AudioClip coin;
    [Range(0f, 3f)] public float coinVolume = 1f;
    [Range(-3f, 3f)] public float coinPitch = 1f;

    public AudioClip pages;
    [Range(0f, 3f)] public float pagesVolume = 1f;
    [Range(-3f, 3f)] public float pagesPitch = 1f;

    public AudioClip partsitems;
    [Range(0f, 3f)] public float partsitemsVolume = 1f;
    [Range(-3f, 3f)] public float partsitemsPitch = 1f;

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
}