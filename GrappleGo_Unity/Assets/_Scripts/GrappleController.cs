using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/6/2025
/// Handles grapple behaviour
/// Holds grapple state machine client
/// </summary>

public class GrappleController : MonoBehaviour
{
    // display current state in the editor, remove for build
    [Header("Current State Display")]
    [SerializeField] private MonoBehaviour _CurrentState;

    // reference to player script
    public PlayerController PlayerRef { get; private set; }
    // speed the grapple moves up and down
    public float GrappleSpeed { get; private set; } = 5f;
    // idle position
    public float IdlePositionY { get; private set; }
    // references to states
    public IGrappleState IdleState { get; private set; }
    public IGrappleState UpState { get; private set; }
    public IGrappleState DownState { get; private set; }
    public IGrappleState CeilingState { get; private set; }
    // reference to current state
    public IGrappleState CurrentState { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        // get player reference
        PlayerRef = gameObject.GetComponentInParent<PlayerController>();

        // add states to this gameobject
        IdleState = gameObject.AddComponent<GrappleIdleState>();
        UpState = gameObject.AddComponent<GrappleUpState>();
        DownState = gameObject.AddComponent<GrappleDownState>();
        CeilingState = gameObject.AddComponent<GrappleCeilingState>();

        // default to idle state
        IdlePositionY = transform.localPosition.y;
        TransitionToState(IdleState);
    }

    // activate specific state's Handle function
    public void TransitionToState(IGrappleState state)
    {
        CurrentState = state;
        _CurrentState = (MonoBehaviour)CurrentState;
        CurrentState.Handle(this);
    }
}