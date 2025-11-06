using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// Handles enemy audio
/// </summary>

public class EnemyAudioHandler : MonoBehaviour, I_AudioHandler
{
    // references to audio sources
    public AudioSource enemyDeath;
    public AudioSource enemyShoot;

    // private reference to this object
    private static EnemyAudioHandler _instance;
    // public reference to this object
    public static EnemyAudioHandler Instance { get { return _instance; } }

    // awake is called before start
    public virtual void Awake()
    {
        // set private ref to self
        _instance = this;
    }

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