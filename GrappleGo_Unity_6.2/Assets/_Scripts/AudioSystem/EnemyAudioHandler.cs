using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// Handles enemy audio
/// </summary>

public class EnemyAudioHandler : SingletonNonPersist<EnemyAudioHandler>, I_AudioHandler
{
    // references to audio sources
    public AudioSource enemyDeath;
    public AudioSource enemyShoot;

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