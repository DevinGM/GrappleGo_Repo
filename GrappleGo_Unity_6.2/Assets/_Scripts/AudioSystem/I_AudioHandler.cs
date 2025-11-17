using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// Interface for Audio Handler scripts
/// </summary>

public interface I_AudioHandler
{
    // play the given audio source
    public void PlaySound(AudioSource audioSource);
    // stop playing the given audio source
    public void StopSound(AudioSource audioSource);
}