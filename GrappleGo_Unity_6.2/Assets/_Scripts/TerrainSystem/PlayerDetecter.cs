using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/24/2025
/// Handles chunk player detection
/// </summary>

public class PlayerDetecter : MonoBehaviour
{
    // has this chunk been stepped on already?
    private bool _steppedOn = false;

    // detects trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        if (!_steppedOn && other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish(EventType.ChunkSteppedOn);
            _steppedOn = true;
        }
    }
}