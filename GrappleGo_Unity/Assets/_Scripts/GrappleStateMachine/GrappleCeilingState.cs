using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/7/2025
/// Handles grapple ceiling state behaviour
/// </summary>

public class GrappleCeilingState : MonoBehaviour, IGrappleState
{
    // reference to GrappleController
    private GrappleController _grappleController;

    // Handle is called when GrappleController switches to ceiling state
    public void Handle(GrappleController grappleController)
    {
        // if _bikeController is not set, set it
        if (!_grappleController)
            _grappleController = grappleController;

        // publish GrappleOnCeiling event
        EventBus.Publish(EventType.BeginClimbing);
    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if player stops inputting grapple, transition to move down state
            if (!_grappleController.PlayerRef.InputtingGrapple)
            {
                _grappleController.TransitionToState(_grappleController.DownState);
                return;
            }

            // if grapple reaches idlePosition tell player
            if (_grappleController.transform.localPosition.y < _grappleController.IdlePositionY)
            {
                EventBus.Publish(EventType.PlayerReachedCeiling);
            }
        }
    }
    
    // handles trigger collision exits
    public void OnTriggerExit(Collider other)
    {
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if stopped colliding with ceiling,
            // transition to up or down state depending on if player is still inputting grapple
            if (other.CompareTag("Ceiling"))
            {
                if (_grappleController.PlayerRef.InputtingGrapple)
                    _grappleController.TransitionToState(_grappleController.UpState);
                else
                    _grappleController.TransitionToState(_grappleController.DownState);
            }
        }
    }
}