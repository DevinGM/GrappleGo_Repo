using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/1/2025
/// Handles chunk spawn timing
/// </summary>

public class SpawnLineDetecter : MonoBehaviour
{
    // has this chunk spawned another yet?
    private bool _spawnedChunk = false;

    private void OnTriggerEnter(Collider other)
    {
        // only do logic during run
        if (GameManager.Instance.InRun)
        {
            // if chunk hits spawn line and hasn't triggered a spawn yet, spawn a chunk
            if (!_spawnedChunk && other.gameObject.CompareTag("SpawnLine"))
            {
                _spawnedChunk = true;
                EventBus.Publish(EventType.SpawnChunk);
            }
        }
    }
}