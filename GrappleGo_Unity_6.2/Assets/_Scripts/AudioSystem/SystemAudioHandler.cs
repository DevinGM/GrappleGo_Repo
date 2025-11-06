using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// Handles system audio
/// </summary>

public class SystemAudioHandler : SingletonNonPersist<SystemAudioHandler>, I_AudioHandler
{
    // references to audio sources
    public AudioSource buttonClick;

    // play the given audio source
    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }

    // stop playing the given audio source
    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}