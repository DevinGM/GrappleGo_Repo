using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2025
/// Handles grapple up state behaviour
/// </summary>

public class GrappleUpState : MonoBehaviour, IGrappleState
{
    // reference to GrappleController
    private GrappleController _grappleController;

    // Handle is called when GrappleController switches to up state
    public void Handle(GrappleController grappleController)
    {/*
        // if _bikeController is not set, set it
        if (!_grappleController)
            _grappleController = grappleController;
    */}

    // Update is called once per frame
    void Update()
    {/*
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if player stops inputting grapple, transition to down state
            if (!PlayerController.Instance.InputtingGrapple)
            {
                _grappleController.TransitionToState(_grappleController.DownState);
                return;
            }

            // move up
            transform.position += _grappleController.GrappleSpeed * Time.deltaTime * transform.up;
        }
    */}

    // handles trigger collision enters
    public void OnTriggerEnter(Collider other)
    {/*
        // only do logic if in this state and have a _grappleController
        if (_grappleController != null && _grappleController.CurrentState == (IGrappleState)this)
        {
            // if collided with ceiling transition to ceiling state
            if (other.CompareTag("Ceiling"))
                _grappleController.TransitionToState(_grappleController.CeilingState);
        }
    */}
}