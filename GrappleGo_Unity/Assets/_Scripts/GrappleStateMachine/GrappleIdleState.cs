using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles grapple idle state behaviour
/// </summary>

public class GrappleIdleState : MonoBehaviour, IGrappleState
{
    // reference to GrappleController
    private GrappleController _grappleController;

    // Handle is called when GrappleController switches to up state
    public void Handle(GrappleController grappleController)
    {
        // if _bikeController is not set, set it
        if (!_grappleController)
            _grappleController = grappleController;

        // reset grapple position
        transform.localPosition = new Vector3(transform.localPosition.x, _grappleController.IdlePositionY, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if player inputs grapple, transition to move up state
            if (_grappleController.PlayerRef.InputtingGrapple)
            {
                _grappleController.TransitionToState(_grappleController.UpState);
            }
        }
    }
}