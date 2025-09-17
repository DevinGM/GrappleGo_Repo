using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/16/2025
/// Handles chunk behaviours
/// </summary>

public class ChunkController : MonoBehaviour
{
    // is the player currently in a run?
    private bool _inRun = false;
    // has this chunk been stepped on already?
    private bool _steppedOn = false;

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
        _inRun = true;
    }
    // called when run ends
    private void EndRun()
    {
        _inRun = false;
    }

    // called when object collides with a collider
    public void OnCollisionEnter(Collision collision)
    {
        // only perform logic during a run
        if (_inRun)
        {
            if (!_steppedOn && collision.gameObject.CompareTag("Player"))
            {
                EventBus.Publish(EventType.ChunkSteppedOn);
                print("chunk has been stepped on by player");
                _steppedOn = true;
            }
        }
    }
}