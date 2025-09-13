using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles player ceiling state behaviour
/// </summary>

public class PlayerCeilingState : MonoBehaviour, IPlayerState
{
    // reference to PlayerController
    private PlayerController _playerController;

    // Handle is called when GrappleController switches to down state
    public void Handle(PlayerController playerController)
    {
        // if _bikeController is not set, set it
        if (!_playerController)
            _playerController = playerController;

        // make sure gravity is off on ceiling
        _playerController.RBRef.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if in this state and have a _playerController
        if (_playerController != null && _playerController.CurrentState == (IPlayerState)this)
        {
            // if player stops inputting grapple transition to running state
            if (!_playerController.InputtingGrapple)
                _playerController.TransitionToState(_playerController.RunningState);
        }
    }
}