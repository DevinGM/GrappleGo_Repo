using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Holds camera movement
/// </summary>

public class CameraController : MonoBehaviour
{
    // camera starting position
    private Vector3 _spawnPos;
    // is the player currently in a run?
    private bool _inRun = false;

    void OnEnable()
    {
        _spawnPos = transform.position;

        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called on run start
    private void StartRun()
    {
        _inRun = true;
    }
    // called on run end
    private void EndRun()
    {
        _inRun = false;
        transform.position = _spawnPos;
    }

    // Update is called once per frame
    void Update()
    {
        // while player is in run, move camera at same speed as player
        if (_inRun)
            transform.position += PlayerController.Instance.MoveSpeed * Time.deltaTime * transform.right;
    }
}