using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSounds : MonoBehaviour
{
    private AudioSource audioSource;
    private ShipController shipController;
    public float speedToPitchCoefficient;

    void Start()
    {
        shipController = GetComponent<ShipController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = Mathf.Clamp(shipController.GetMagnitude() * speedToPitchCoefficient, 1f, 10f);
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
}
