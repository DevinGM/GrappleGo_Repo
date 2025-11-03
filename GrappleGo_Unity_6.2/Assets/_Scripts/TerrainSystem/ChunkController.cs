using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 11/1/2025
/// Handles chunk behaviours
/// </summary>

public class ChunkController : MonoBehaviour
{
    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
    }

    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
    }

    private void Update()
    {
        // only do logic during run
        if (GameManager.Instance.InRun)
        {
            // if chunk passes -20 x in world space, cull it
            if (transform.position.x < -20)
                Destroy(this.gameObject);
        }
    }

    // called when run ends
    private void OnRunEnd()
    {
        Destroy(this.gameObject);
    }
}