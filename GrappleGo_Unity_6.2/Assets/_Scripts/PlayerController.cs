using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 9/16/2025
/// Holds player behaviours
/// Handles player state machine
///     state machine handles movement
/// Handles coin collection
/// </summary>

public class PlayerController : SingletonNonPersisit<PlayerController>
{
    // turn on to start the run
    [Header("Turn On To Start The Run")]
    [SerializeField] private bool _StartRun = false;
    // display current state in the editor, remove for build
    [Header("Current State Display")]
    [SerializeField] private MonoBehaviour _CurrentState;

    // speed player begins moving at
    [SerializeField] private float _startSpeed = 10f;
    // speed player begins climbing at
    [SerializeField] private float _startClimbSpeed = 5f;
    // speed player climbs the grapple once it's reached the ceiling
    [SerializeField] private float _grappleClimbSpeed;
    // amount player accelerates by
    [SerializeField] private float _accelSpeed = 1f;
    // amount of time passed before player accelerates
    [SerializeField] private float _accelTime = 5f;
    // number of player's lives
    // player dies when lives hits 0
    [SerializeField] private int _lives = 1;
    [SerializeField] private int _coinValue = 10;

    // position at the start of a run
    private Vector3 _spawnPos;
    // is the player currently moving
    private bool _waitingToAccelerate = false;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _grappleActionKeyboard;
    private InputAction _grappleAction;

    // reference to player rigidbody
    public Rigidbody RBRef { get; private set; }
    // reference to grapple
    public GrappleController GrappleRef { get; private set; }
    // speed player is moving at
    public float MoveSpeed { get; private set; }
    // speed player climbs at
    public float ClimbSpeed { get; private set; }
    // is the player currently in a run
    public bool InRun { get; private set; } = false;
    // is the player currently moving
    public bool InputtingGrapple { get; private set; } = false;
    // references to states
    public IPlayerState RunningState { get; private set; }
    public IPlayerState ClimbingState { get; private set; }
    public IPlayerState CeilingState { get; private set; }
    // reference to current state
    public IPlayerState CurrentState { get; private set; }

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
        EventBus.Subscribe(EventType.BeginClimbing, StartClimbing);
        EventBus.Subscribe(EventType.PlayerReachedCeiling, HitCeiling);

        // add states to this gameobject
        RunningState = gameObject.AddComponent<PlayerRunningState>();
        ClimbingState = gameObject.AddComponent<PlayerClimbingState>();
        CeilingState = gameObject.AddComponent<PlayerCeilingState>();

        // get references
        RBRef = this.GetComponent<Rigidbody>();
        GrappleRef = this.GetComponentInChildren<GrappleController>();

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();

        _grappleActionKeyboard = _playerInputs.ControlsTemp.GrappleTemp;
        _grappleActionKeyboard.performed += OnGrapplePerformed;
        _grappleActionKeyboard.canceled += OnGrappleCanceled;
        _grappleAction = _playerInputs.ControlsTemp.Grapple;
        _grappleAction.performed += OnGrapplePerformed;
        _grappleAction.canceled += OnGrappleCanceled;
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
        EventBus.Unsubscribe(EventType.BeginClimbing, StartClimbing);
        EventBus.Unsubscribe(EventType.PlayerReachedCeiling, HitCeiling);
    }

    // called when run starts
    private void StartRun()
    {
        _spawnPos = transform.position;
        InRun = true;
        MoveSpeed = _startSpeed;
        ClimbSpeed = _startClimbSpeed;
        TransitionToState(RunningState);
    }

    // called when run ends
    private void EndRun()
    {
        transform.position = _spawnPos;
        InRun = false;
        _lives = 1;
    }

    // called when grapple reaches ceiling
    private void StartClimbing()
    {
        TransitionToState(ClimbingState);
    }

    // called when player reaches ceiling
    private void HitCeiling()
    {
        TransitionToState(CeilingState);
    }

    // Update is called once per frame
    void Update()
    {
        // start a run when _StartRun is set to true
        // dev tool, remove for build
        if (_StartRun)
        {
            EventBus.Publish(EventType.RunStart);
            _StartRun = false;
        }

        // logic only happens when in a run
        if (InRun)
        {
            // increase speed by 1 every 5 seconds
            if (!_waitingToAccelerate)
                StartCoroutine(Accelerate());

            // handle score
            // player MUST start run at x = 0 for score to be accurate
            GameManager.Instance.score = (int)transform.position.x;

            // if the player runs out of lives end the run
            if (_lives <= 0)
                EventBus.Publish(EventType.RunEnd);
        }
    }

    // handles collision interactions
    private void OnCollisionEnter(Collision collision)
    {
        print("player collided with something");
        // only do logic inside run
        if (InRun)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                print("player collided with obstacle");
                _lives--;
                Destroy(collision.gameObject);
            }
        }
    }

    // handles triger collisions
    private void OnTriggerEnter(Collider other)
    {
        // only do logic inside run
        if (InRun)
        {
            // if collide with coin destroy it and add to score
            if (other.gameObject.CompareTag("Coin"))
            {
                GameManager.Instance.score += _coinValue;
                Destroy(other.gameObject);
            }
        }
    }

    // wait _accelTime amount of seconds before increasing speed by _accelSpeed
    private IEnumerator Accelerate()
    {
        _waitingToAccelerate = true;
        yield return new WaitForSeconds(_accelTime);
        MoveSpeed += _accelSpeed;
        _waitingToAccelerate = false;
    }

    // activate specific state's Handle function
    public void TransitionToState(IPlayerState state)
    {
        CurrentState = state;
        _CurrentState = (MonoBehaviour)CurrentState;
        CurrentState.Handle(this);
    }

    // called when player inputs grapple
    private void OnGrapplePerformed(InputAction.CallbackContext context)
    {
        InputtingGrapple = true;
    }

    // called when player stops inputting grapple
    private void OnGrappleCanceled(InputAction.CallbackContext context)
    {
        InputtingGrapple = false;
    }
}