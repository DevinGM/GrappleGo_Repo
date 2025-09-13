using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles player running state behaviour
/// </summary>

public class PlayerRunningState : MonoBehaviour, IPlayerState
{
    // reference to PlayerController
    private PlayerController _playerController;

    // Handle is called when GrappleController switches to down state
    public void Handle(PlayerController playerController)
    {
        // if _bikeController is not set, set it
        if (!_playerController)
            _playerController = playerController;

        // make sure gravity is on for default running
        _playerController.RBRef.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if have a _playerController
        if (_playerController != null)
        {
            if (_playerController.InRun)
            {
                // move forward constantly
                transform.position += _playerController.MoveSpeed * Time.deltaTime * transform.right;
            }
        }
    }
}