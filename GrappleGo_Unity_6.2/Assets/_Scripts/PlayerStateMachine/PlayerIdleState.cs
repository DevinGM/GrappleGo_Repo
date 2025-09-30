using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2025
/// Handles player idle state behaviour
/// </summary>

public class PlayerIdleState : MonoBehaviour, IPlayerState
{
    // reference to PlayerController
    private PlayerController _playerController;

    // Handle is called when GrappleController switches to down state
    public void Handle(PlayerController playerController)
    {
        // if _bikeController is not set, set it
        if (!_playerController)
            _playerController = playerController;

        // make sure gravity is on for default idle
        _playerController.RBRef.useGravity = true;
    }
}