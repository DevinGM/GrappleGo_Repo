using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// HANDLES PLAYER BEHAVIOR
/// detects touch inputs
/// moves player to held touch on screen
/// moves grapple
/// handles damage
/// </summary>

public class PlayerController_Tap : SingletonNonPersist<PlayerController_Tap>
{
    [Header("Reference to Grapple object\nUse 'Grapple_Tap' instance inside the scene")]
    // reference to grapple
    [SerializeField] private Transform _grapple;

    // speed player is currently moving at
    private float _moveSpeed = 12f;
    // strength of gravity
    private float _gravityStrength = 9.81f;
    // length of cooldown after taking damage
    private float _damageCooldownTime = .5f;
    // is the damge cooldown on?
    private bool _damageCooldown = false;
    // is the player currently moving?
    public bool _moving = false;
    // is gravity currently on?
    public bool _gravityOn = false;
    // velocity of gravity currently being applied to player
    private Vector3 _gravityVelocity;
    // location player spawns at
    private Vector3 _spawnPos;
    private Vector3 _grappleSpawnPos;
    // position player grapples towards
    private Vector3 _targetPos;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _tapGrapplePositionAction;
    private InputAction _tapGrapplePressAction;
    // reference to main camera
    private Camera _camera;

    // number of player's lives
    // player dies when lives hits 0
    public int lives = 1;
    // is the player currently boosting
    public bool boosting = false;
    // is the player currently invincible?
    public bool invincible = false;
    // is the player currently using a powerup with inputs?
    public bool powerupInputting = false;
    // reference to rigidbody
    public Rigidbody rbRef;

    #region OnEnable & OnDisable

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Subscribe(EventType.DashStart, TurnOnPowerupInputting);
        EventBus.Subscribe(EventType.DashEnd, TurnOffPowerupInputting);

        // get references
        _camera = Camera.main;
        _spawnPos = transform.position;
        if (_grapple != null)
            _grappleSpawnPos = _grapple.position;
        else
            Debug.LogError("ERROR: No Grapple reference given to player");
        rbRef = GetComponent<Rigidbody>();

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        _tapGrapplePositionAction = _playerInputs.Controls.TapGrapplePosition;
        _tapGrapplePressAction = _playerInputs.Controls.TapGrapplePress;
        _tapGrapplePositionAction.performed += OnTapGrapplePosition;
        _tapGrapplePressAction.performed += OnTapGrapplePress;
        _tapGrapplePressAction.canceled += OnTapGrapplePress;

        // get stats
        _moveSpeed = GameManager.Instance.playerMoveSpeed;
    }

    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Unsubscribe(EventType.DashStart, TurnOnPowerupInputting);
        EventBus.Unsubscribe(EventType.DashEnd, TurnOffPowerupInputting);

        // unsubscribe to inputs
        _tapGrapplePositionAction.performed -= OnTapGrapplePosition;
        _tapGrapplePressAction.performed -= OnTapGrapplePress;
        _tapGrapplePressAction.canceled -= OnTapGrapplePress;
    }

    #endregion

    #region Event Calls

    // called when run starts
    private void OnRunStart()
    {
        _moveSpeed = GameManager.Instance.playerMoveSpeed;
    }

    // called when run ends
    private void OnRunEnd()
    {
        // reset player lives
        lives = 1;
        // reset postion
        transform.position = _spawnPos;
        _grapple.position = _grappleSpawnPos;
    }

    // mark player as using powerup inputs
    private void TurnOnPowerupInputting()
    {
        powerupInputting = true;
    }

    // mark player as not using powerup inputs
    private void TurnOffPowerupInputting()
    {
        powerupInputting = false;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        // logic only happens when in a run
        if (GameManager.Instance.InRun)
        {
            // if player is moving, move
            if (_moving)
                Move();

            // if gravity is on, enact gravity
            if (_gravityOn)
                Gravity();
            
        }
    }

    #region Movement

    // move to target position
    private void Move()
    {
        // only move if player is inbetween 1.1 and 9.9 in the y axis
        if (transform.position.y >= 1.1f && transform.position.y <= 9.9f)
            // move to target position
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);

        // if player is beneath 1.1 on the y, move to 1.1
        if (transform.position.y < 1.1f)
            transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);

        // if player is above 9.9 on the y, move to 9.9
        if (transform.position.y > 9.9f)
            transform.position = new Vector3(transform.position.x, 9.9f, transform.position.z);
    }

    // manually enacts gravity on the player
    // using manual gravity because having a rigidbody breaks Vector3.MoveTowards for some reason
    public void Gravity()
    {
        _gravityVelocity.y -= _gravityStrength * Time.deltaTime;
        transform.position += _gravityVelocity * Time.deltaTime;
        // if player moves below 1.1 on the y axis, turn off gravity
        if (transform.position.y <= 1.1f)
        {
            _gravityOn = false;
            // reset position to 1.1
            transform.position = new Vector3(transform.position.x, 1.1f, 0f);
        }
    }

    #endregion

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
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);
            }

            // if player collides with platform via a trigger, take damage and destroy platform 
            if (other.gameObject.CompareTag("Platform"))
            {
                // don't take damage if invincible
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);

                print("Player collided with platform");
            }
        }
    }

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
    public IEnumerator DamageCooldown()
    {
        _damageCooldown = true;
        yield return new WaitForSeconds(_damageCooldownTime);
        _damageCooldown = false;
    }

    #endregion

    #region Input Functions

    // called when player touches the screen
    // gets location of where the player tapped the screen for the player to move to
    public void OnTapGrapplePosition(InputAction.CallbackContext context)
    {
        // only do logic inside
        if (GameManager.Instance.InRun)
        {
            // convert tap position from screen space to world space
            Vector3 targetPos = _camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            targetPos.z = 0f;
            // store world position of tap in _targetPos
            _targetPos = targetPos;

            // move grapple to target position
            targetPos.z = -2f;
            _grapple.position = targetPos;
        }
    }
    
    // called when player starts and stops touching the screen
    // starts and stops player moving
    // turns off gravity while moving and back on when not moving
    // if not in run, start run
    public void OnTapGrapplePress(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.InRun)
            EventBus.Publish(EventType.RunStart);
        else
        {
            _moving = context.ReadValueAsButton();
            // if stopped moving, turn on gravity
            if (!_moving)
            {
                _gravityVelocity = Vector3.zero;
                _gravityOn = true;
            }
            // if started moving, turn off gravity
            else
                _gravityOn = false;
        }
    }

    #endregion
}