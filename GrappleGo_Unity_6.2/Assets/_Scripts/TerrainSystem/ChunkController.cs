using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/25/2025
/// Handles chunk behaviours
/// </summary>

public class ChunkController : MonoBehaviour
{
    // is the player currently in a run?
    private bool _inRun = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    private void Update()
    {
        if (_inRun)
        {
            // if the player gets too far away cull this chunk
            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > 50f)
                Destroy(this.gameObject);
        }
    }

    // called when run begins
    public void StartRun()
    {
        _inRun = true;
    }
    // called when run ends
    private void EndRun()
    {
        Destroy(this.gameObject);
    }
}