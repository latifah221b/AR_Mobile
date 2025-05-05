using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneMusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bobaAudioSource;
    [SerializeField] private AudioSource kokaAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bobaMusic;
    [SerializeField] private AudioClip kokaMusic;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 2.0f;
    [SerializeField] private int sceneToFadeAt = 6;

    private static CrossSceneMusicManager instance;
    private int currentSceneIndex;
    private bool isFading = false;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            
            if (bobaAudioSource == null)
            {
                bobaAudioSource = gameObject.AddComponent<AudioSource>();
                bobaAudioSource.loop = true;
                bobaAudioSource.playOnAwake = false;
            }

            if (kokaAudioSource == null)
            {
                kokaAudioSource = gameObject.AddComponent<AudioSource>();
                kokaAudioSource.loop = true;
                kokaAudioSource.playOnAwake = false;
                kokaAudioSource.volume = 0f; 
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        if (bobaMusic != null)
        {
            bobaAudioSource.clip = bobaMusic;
        }

        if (kokaMusic != null)
        {
            kokaAudioSource.clip = kokaMusic;
        }

        
        PlayBobaMusic();

        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;

        
        if (currentSceneIndex == sceneToFadeAt && !isFading)
        {
            StartCoroutine(FadeMusic());
        }
    }

    private void PlayBobaMusic()
    {
        
        if (!bobaAudioSource.isPlaying && bobaAudioSource.clip != null)
        {
            bobaAudioSource.volume = 1f;
            bobaAudioSource.Play();
        }
    }

    private IEnumerator FadeMusic()
    {
        isFading = true;

        
        if (kokaAudioSource.clip != null && !kokaAudioSource.isPlaying)
        {
            kokaAudioSource.volume = 0f;
            kokaAudioSource.Play();
        }

        float timer = 0f;
        float startVolumeKoka = 0f;
        float startVolumeBoba = bobaAudioSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            
            bobaAudioSource.volume = Mathf.Lerp(startVolumeBoba, 0f, t);

            
            kokaAudioSource.volume = Mathf.Lerp(startVolumeKoka, 1f, t);

            yield return null;
        }

        
        bobaAudioSource.volume = 0f;
        kokaAudioSource.volume = 1f;

        
        bobaAudioSource.Stop();

        isFading = false;
    }

    
    public void ForceFadeToKokaMusic()
    {
        if (!isFading)
        {
            StartCoroutine(FadeMusic());
        }
    }

    public void RestartBobaMusic()
    {
        StopAllCoroutines();
        isFading = false;

        kokaAudioSource.Stop();
        kokaAudioSource.volume = 0f;

        bobaAudioSource.volume = 1f;
        PlayBobaMusic();
    }
}