using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles grapple down state behaviour
/// </summary>

public class GrappleDownState : MonoBehaviour, IGrappleState
{
    // reference to GrappleController
    private GrappleController _grappleController;

    // Handle is called when GrappleController switches to down state
    public void Handle(GrappleController grappleController)
    {
        // if _bikeController is not set, set it
        if (!_grappleController)
            _grappleController = grappleController;

    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if grapple reaches idle position, transition to idle state
            if (transform.localPosition.y < _grappleController.IdlePositionY)
            {
                _grappleController.TransitionToState(_grappleController.IdleState);
                return;
            }

            // if player inputs grapple, transition to move up state
            if (_grappleController.PlayerRef.InputtingGrapple)
            {
                _grappleController.TransitionToState(_grappleController.UpState);
                return;
            }

            // move down
            transform.position -= _grappleController.GrappleSpeed * Time.deltaTime * transform.up;
        }        
    }
}