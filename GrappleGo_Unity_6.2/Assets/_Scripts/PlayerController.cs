using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 9/26/2025
/// Holds player behaviours
/// Handles player state machine
///     state machine handles movement
/// Gets inputs
/// Handles coin collection
/// Handles enemy collision and player damage
/// </summary>

public class PlayerController : SingletonNonPersisit<PlayerController>
{
    // display current state in the editor, remove for build
    [Header("Current State Display")]
    [SerializeField] private MonoBehaviour _CurrentState;

    // speed player begins moving at
    [SerializeField] private float _startSpeed = 10f;
    // speed player begins climbing at
    [SerializeField] private float _startClimbSpeed = 10f;
    // amount player accelerates by
    [SerializeField] private float _accelSpeed = 1f;
    // amount of time passed before player accelerates
    [SerializeField] private float _accelTime = 5f;
    // amount of score added upon coin pickup
    [SerializeField] private int _coinValue = 10;

    // position at the start of a run
    private Vector3 _spawnPos;
    // is the acceleration cooldown on?
    private bool _accelCooldown = false;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _grappleAction;

    // number of player's lives
    // player dies when lives hits 0
    public int lives = 1;

    // reference to player rigidbody
    public Rigidbody RBRef { get; private set; }
    // reference to grapple
    public GrappleController GrappleRef { get; private set; }
    // speed player is moving at
    public float MoveSpeed { get; set; }
    // speed player climbs at
    public float ClimbSpeed { get; private set; }
    // is the player currently in a run
    public bool InRun { get; private set; } = false;
    // is the player currently moving
    public bool InputtingGrapple { get; private set; } = false;
    // is the player currently invincible
    public bool Invincible { get; set; } = false;
    // references to states
    public IPlayerState IdleState { get; private set; }
    public IPlayerState ClimbingState { get; private set; }
    public IPlayerState CeilingState { get; private set; }
    // reference to current state
    public IPlayerState CurrentState { get; private set; }

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
        EventBus.Subscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);
        EventBus.Subscribe(EventType.PlayerHitCeiling, OnPlayerHitCeiling);
        
        // add states to this gameobject
        IdleState = gameObject.AddComponent<PlayerIdleState>();
        ClimbingState = gameObject.AddComponent<PlayerClimbingState>();
        CeilingState = gameObject.AddComponent<PlayerCeilingState>();
        
        // get references
        RBRef = this.GetComponent<Rigidbody>();
        GrappleRef = this.GetComponentInChildren<GrappleController>();

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();

        _grappleAction = _playerInputs.Controls.Grapple;
        _grappleAction.performed += OnGrapplePerformed;
        _grappleAction.canceled += OnGrappleCanceled;
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
        EventBus.Unsubscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);
        EventBus.Unsubscribe(EventType.PlayerHitCeiling, OnPlayerHitCeiling);
    }

    // called when run starts
    private void StartRun()
    {
        _spawnPos = transform.position;
        InRun = true;
        MoveSpeed = _startSpeed;
        ClimbSpeed = _startClimbSpeed;
        TransitionToState(IdleState);
    }

    // called when run ends
    private void EndRun()
    {
        InRun = false;
        // reset player position
        transform.position = _spawnPos;
        lives = 1;
        InputtingGrapple = false;
    }

    // called when grapple reaches ceiling
    private void OnGrappleHitCeiling()
    {
        TransitionToState(ClimbingState);
    }

    // called when player reaches ceiling
    private void OnPlayerHitCeiling()
    {
        TransitionToState(CeilingState);
    }

    // Update is called once per frame
    void Update()
    {
        // logic only happens when in a run
        if (InRun)
        {
            // increase speed by 1 every 5 seconds
            if (!_accelCooldown)
                StartCoroutine(Accelerate());
            // move forward constantly
            transform.Translate(MoveSpeed * Time.deltaTime * transform.right);

            // handle score
            // player MUST start at x = 0 for score to be accurate
            GameManager.Instance.distanceScore = (int)transform.position.x;

            // if player stops inputting grapple out of idle state, transition to idle state
            if (!InputtingGrapple && CurrentState != IdleState)
                TransitionToState(IdleState);
        }
    }

    // handles collision interactions
    private void OnCollisionEnter(Collision collision)
    {
        // only do logic inside run
        if (InRun)
        {
            // if player collides with an obstacle take damage and destroy obstacle
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                // don't take damage if invincible
                if (!Invincible)
                    TakeDamage();
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
                GameManager.Instance.pickupsScore += _coinValue;
                Destroy(other.gameObject);
            }
        }
    }

    // called when player takes damage
    private void TakeDamage()
    {
        // reduce lives
        lives--;
        // announce player has taken damage
        EventBus.Publish(EventType.PlayerDamaged);
        // if lives drop to 0, end run
        if (lives <= 0)
            EventBus.Publish(EventType.RunEnd);
    }

    // wait _accelTime amount of seconds before increasing speed by _accelSpeed
    private IEnumerator Accelerate()
    {
        _accelCooldown = true;
        yield return new WaitForSeconds(_accelTime);
        MoveSpeed += _accelSpeed;
        _accelCooldown = false;
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
        // only do logic inside run
        if (InRun)
            InputtingGrapple = true;
        // player is not in run so start run upon pressing grapple
        else
            EventBus.Publish(EventType.RunStart);
    }

    // called when player stops inputting grapple
    private void OnGrappleCanceled(InputAction.CallbackContext context)
    {
        // only do logic inside run
        if (InRun)
            InputtingGrapple = false;
    }
}