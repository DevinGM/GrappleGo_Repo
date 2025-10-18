using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 10/17/2025
/// HANDLES PLAYER BEHAVIOR
/// handles player movement
/// gets inputs
/// handles coin collision and collection
/// handles enemy collision and player damage
/// </summary>

public class PlayerController : SingletonNonPersist<PlayerController>
{
    // speed player begins moving at
    [SerializeField] private float _startSpeed = 10f;
    // amount player accelerates by
    [SerializeField] private float _accelSpeed = 1f;
    // amount of time passed before player accelerates
    [SerializeField] private float _accelTime = 5f;

    // speed player is currently climbing at
    private float _climbSpeed = 12f;
    // length of cooldown after taking damage
    private float _damageCooldownTime = .5f;
    // is the damge cooldown on?
    private bool _damageCooldown = false;
    // is the acceleration cooldown on?
    private bool _accelCooldown = false;
    // is the player currently climbing the grapple?
    private bool _climbing = false;
    // is the player currently on the ceiling?
    private bool _onCeiling = false;
    // position at the start of a run
    private Vector3 _spawnPos;
    // reference to player rigidbody
    private Rigidbody _rbRef;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _grappleAction;

    // number of player's lives
    // player dies when lives hits 0
    public int lives = 1;
    // speed player is currently moving at
    public float currentMoveSpeed;
    // is the player currently boosting
    public bool boosting = false;

    // is the player currently inputting grapple?
    public bool InputtingGrapple { get; private set; } = false;

    #region OnEnable & OnDisable

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
        EventBus.Subscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);
        
        // get references
        _rbRef = this.GetComponent<Rigidbody>();

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        _grappleAction = _playerInputs.Controls.Grapple;
        _grappleAction.performed += OnGrapplePerformed;
        _grappleAction.canceled += OnGrappleCanceled;

        // get stats
        _climbSpeed = GameManager.Instance.playerClimbSpeed;
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
        EventBus.Unsubscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);
    }

    #endregion

    #region Functions Called by Events

    // called when run starts
    private void StartRun()
    {
        _spawnPos = transform.position;
        currentMoveSpeed = _startSpeed;
        _climbSpeed = GameManager.Instance.playerClimbSpeed;
    }

    // called when run ends
    private void EndRun()
    {
        // reset player position
        transform.position = _spawnPos;
        // reset player lives
        lives = 1;

        _climbing = false;
        _onCeiling = false;
    }

    // called when grapple reaches ceiling
    private void OnGrappleHitCeiling()
    {
        // begin climbing
        _climbing = true;
        // turn off gravity so player can climb
        _rbRef.useGravity = false;
        // zero player's downward momentum in case they were previously falling
        Vector3 velocity = _rbRef.linearVelocity;
        velocity.y = 0f;
        _rbRef.linearVelocity = velocity;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        // logic only happens when in a run
        if (GameManager.Instance.InRun)
        {
            // increase speed by 1 every 5 seconds
            if (!_accelCooldown)
                StartCoroutine(Accelerate());

            Move();

            // add to distance score
            // player MUST start at x = 0 for score to be accurate
            GameManager.Instance.distanceScore = (int)transform.position.x;
        }
    }

    // handles trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
        {
            // if collide with coin destroy it and add to score
            if (other.gameObject.CompareTag("Coin"))
            {
                GameManager.Instance.pickupsScore += GameManager.Instance.coinValue;
                Destroy(other.gameObject);
            }

            // if player collides with an obstacle take damage and destroy obstacle
            if (other.gameObject.CompareTag("Obstacle"))
            {
                // don't take damage if invincible
                if (!boosting)
                    TakeDamage();

                Destroy(other.gameObject);
            }
        }
    }

    #region Movement

    // handles movement behaviors
    private void Move()
    {
        // move forward constantly
        transform.Translate(currentMoveSpeed * Time.deltaTime * transform.right);

        // when player is climbing but hasn't reached the ceiling yet
        if (_climbing && !_onCeiling && InputtingGrapple)
        {
            // move up
            transform.Translate(_climbSpeed * Time.deltaTime * transform.up);

            // check if player has reached ceiling
            // detect an object above the player
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 1.1f))
            {
                // check if object is the ceiling and player isn't already on the ceiling and make sure they're inputting grapple
                if (hit.collider.transform.CompareTag("Ceiling") && InputtingGrapple)
                    _onCeiling = true;
            }
        }

        // if player stops inputting grapple stop climbing and reactivate gravity
        if (!InputtingGrapple)
        {
            _onCeiling = false;
            _climbing = false;
            _rbRef.useGravity = true;
        }
    }

    // wait _accelTime amount of seconds before increasing speed by _accelSpeed
    private IEnumerator Accelerate()
    {
        _accelCooldown = true;
        yield return new WaitForSeconds(_accelTime);
        currentMoveSpeed += _accelSpeed;
        _accelCooldown = false;
    }

    #endregion

    #region Damage

    // called when player takes damage
    private void TakeDamage()
    {
        // don't take damage if on cooldown
        if (!_damageCooldown)
        {
            // reduce lives
            lives--;
            // announce player has taken damage
            EventBus.Publish(EventType.PlayerDamaged);
            // if lives drop to 0, end run
            if (lives <= 0)
                EventBus.Publish(EventType.RunEnd);
            // start damage cooldown
            StartCoroutine(DamageCooldown());
        }
    }

    // dissallow damage for _damageCooldownTime seconds
    private IEnumerator DamageCooldown()
    {
        _damageCooldown = true;
        yield return new WaitForSeconds(_damageCooldownTime);
        _damageCooldown = false;
    }

    #endregion

    #region Input Functions

    // called when player inputs grapple
    private void OnGrapplePerformed(InputAction.CallbackContext context)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
            InputtingGrapple = true;
        // player is not in run so start run upon pressing grapple
        else
            EventBus.Publish(EventType.RunStart);
    }

    // called when player stops inputting grapple
    private void OnGrappleCanceled(InputAction.CallbackContext context)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
            InputtingGrapple = false;
    }

    #endregion
}