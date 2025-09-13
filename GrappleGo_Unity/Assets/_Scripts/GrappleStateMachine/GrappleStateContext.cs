using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/6/2025
/// Holds grapple state context
/// </summary>

public class GrappleStateContext : MonoBehaviour
{
    // reference to a given state
    public IGrappleState CurrentState { get; set; }

    // reference to GrappleController
    private readonly GrappleController _grappleController;

    // holds the GrappleController
    // allows _grappleController to be passed a Controller upon instantiation but still protects it
    public GrappleStateContext(GrappleController grappleController)
    {
        _grappleController = grappleController;
    }

    // activate specific state's Handle function
    public void TransitionToState(IGrappleState state)
    {
        CurrentState = state;
        CurrentState.Handle(_grappleController);
    }
}