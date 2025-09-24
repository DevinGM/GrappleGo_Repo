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
    // is the player currently in a run?
    public bool inRun = false;
    // has this chunk been stepped on already?
    public bool steppedOn = false;

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

    // called when run begins
    public void StartRun()
    {
        inRun = true;
    }
    // called when run ends
    private void EndRun()
    {
        inRun = false;
    }

    // detects trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        // only perform logic during a run
        if (inRun)
        {
            if (!steppedOn && other.gameObject.CompareTag("Player"))
            {
                EventBus.Publish(EventType.ChunkSteppedOn);
                steppedOn = true;
            }
        }
    }
}