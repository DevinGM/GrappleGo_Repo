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

    void OnEnable()
    {
        _spawnPos = transform.position;

        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // reset to starting position on end of run
    private void EndRun()
    {
        transform.position = _spawnPos;
    }

    // Update is called once per frame
    void Update()
    {
        // while player is in run, move camera at same speed as player
        if (PlayerController.Instance.InRun)
        {
            transform.position += PlayerController.Instance.MoveSpeed * Time.deltaTime * transform.right;
        }
    }
}