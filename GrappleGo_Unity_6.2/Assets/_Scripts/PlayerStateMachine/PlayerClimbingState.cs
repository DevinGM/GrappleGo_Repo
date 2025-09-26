using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles player climbing state behaviour
/// </summary>

public class PlayerClimbingState : MonoBehaviour, IPlayerState
{
    // reference to PlayerController
    private PlayerController _playerController;

    // Handle is called when GrappleController switches to down state
    public void Handle(PlayerController playerController)
    {
        // if _bikeController is not set, set it
        if (!_playerController)
            _playerController = playerController;

        // turn gravity off for climbing
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
            {
                _playerController.TransitionToState(_playerController.IdleState);
                return;
            }

            // move up
            transform.Translate(_playerController.ClimbSpeed * Time.deltaTime * transform.up);
            // move grapple down at same rate so it stays in the same place
            _playerController.GrappleRef.transform.Translate(_playerController.ClimbSpeed * Time.deltaTime * -transform.up);
        }
    }
}