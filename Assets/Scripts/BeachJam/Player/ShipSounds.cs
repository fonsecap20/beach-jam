using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSounds : MonoBehaviour
{
    private AudioSource audioSource;
    private ShipController shipController;

    public float fadeInTime; //in seconds
    public float speedToPitchCoefficient;   
    public float minPitch;
    public float maxPitch;

    private float originalVolume;

    void Start()
    {
        shipController = GetComponent<ShipController>();
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;
        audioSource.volume = 0;
        StartSoundLoop();
        StartCoroutine(FadeIn(fadeInTime));
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = Mathf.Clamp(shipController.GetMagnitude() * speedToPitchCoefficient, minPitch, maxPitch);
        Debug.Log(audioSource.pitch);
    }

    public void StartSoundLoop()
    {
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSoundLoop()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }

    IEnumerator FadeIn(float time)
    {
        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += Time.deltaTime / time;
            yield return null;
        }

        audioSource.volume = originalVolume;
    }
}
