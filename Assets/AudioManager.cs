using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip normalSong;
    public AudioClip specialSong;
    public string specialSceneName;
    public float fadeTime;

    private AudioSource audioSource;
    private static bool isSpecialSceneLoaded = false;
    private static AudioManager instance = null;

    void Awake()
    {
        // Check if an AudioManager already exists
        if (instance == null)
        {
            // If not, set this as the instance and make it persist across scenes
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an AudioManager already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = normalSong;
        audioSource.loop = true;
        audioSource.Play();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == specialSceneName && !isSpecialSceneLoaded)
        {
            isSpecialSceneLoaded = true;
            StartCoroutine(FadeOutAndIn(specialSong));
        }
        else if (scene.name != specialSceneName && isSpecialSceneLoaded)
        {
            isSpecialSceneLoaded = false;
            StartCoroutine(FadeOutAndIn(normalSong));
        }
    }

    IEnumerator FadeOutAndIn(AudioClip newClip)
    {
        // Fade out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        // Change clip and fade in
        audioSource.clip = newClip;
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
