using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [Header("Audio Clip")]
    public AudioClip badge;
    public AudioClip clear;
    public AudioClip click;
    public AudioClip disclick;
    public AudioClip correct;
    public AudioClip wrong;
    public AudioClip coin;
    public AudioClip pages;
    public AudioClip partsitems;
    public AudioClip vanish;
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}