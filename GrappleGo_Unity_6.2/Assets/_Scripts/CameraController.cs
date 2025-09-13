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
    private PlayerController _playerRef;

    void OnEnable()
    {
        _playerRef = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // while player is in run, move camera at same speed as player
        if (_playerRef.InRun)
        {
            transform.position += _playerRef.MoveSpeed * Time.deltaTime * transform.right;
        }
    }
}