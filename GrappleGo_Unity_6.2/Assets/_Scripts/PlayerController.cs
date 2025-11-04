using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/3/2025
/// HANDLES PLAYER BEHAVIOR
/// handles player movement
/// gets inputs
/// handles coin collision and collection
/// handles enemy collision and player damage
/// </summary>

public class PlayerController : SingletonNonPersist<PlayerController>
{
    // speed player is currently climbing at
    private float _climbSpeed = 12f;
    // length of cooldown after taking damage
    private float _damageCooldownTime = .5f;
    // is the damge cooldown on?
    private bool _damageCooldown = false;
    // is the player currently climbing the grapple?
    private bool _climbing = false;
    // is the player currently on the ceiling?
    private bool _onCeiling = false;
    // location player spawns at
    private Vector3 _spawnPos;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _grappleAction;
    private InputAction _gunAction; ////////////////////////////////////// move this to gun powerup
    private InputAction _dynamiteAction; //////////////////////////// move this to dynamite powerup
    //reference to grapple
    private GrappleController _grappleRef;

    // number of player's lives
    // player dies when lives hits 0
    public int lives = 1;
    // is the player currently inputting grapple?
    public bool inputtingGrapple = false;
    // is the player currently boosting
    public bool boosting = false;
    // is the player currently invincible?
    public bool invincible = false;
    // is the player currently using a powerup with inputs?
    public bool powerupInputting = false;
    // reference to player rigidbody
    public Rigidbody rbRef;

    #region OnEnable & OnDisable

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Subscribe(EventType.DashStart, TurnOnPowerupInputting);
        EventBus.Subscribe(EventType.DashEnd, TurnOffPowerupInputting);
        EventBus.Subscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);

        // get references
        rbRef = this.GetComponent<Rigidbody>();
        _grappleRef = FindAnyObjectByType<GrappleController>();

        // get spawn position
        _spawnPos = transform.position;

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        _grappleAction = _playerInputs.Controls.Grapple;
        _gunAction = _playerInputs.Controls.Gun; //////////////////////////// move this to gun powerup
        _dynamiteAction = _playerInputs.Controls.Dynamite; ///////////// move this to dynamite powerup
        _grappleAction.performed += OnGrapplePerformed;
        _grappleAction.canceled += OnGrappleCanceled;

        // get stats
        _climbSpeed = GameManager.Instance.playerClimbSpeed;
    }

    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Unsubscribe(EventType.GrappleHitCeiling, OnGrappleHitCeiling);
    }

    #endregion

    #region Event Calls

    // called when run starts
    private void OnRunStart()
    {
        _climbSpeed = GameManager.Instance.playerClimbSpeed;
    }

    // called when run ends
    private void OnRunEnd()
    {
        // reset player lives
        lives = 1;

        // reset postion
        transform.position = _spawnPos;
        // reset velocity
        rbRef.linearVelocity = Vector3.zero;

        // reset climbing
        _climbing = false;
        _onCeiling = false;
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

    // called when grapple reaches ceiling
    private void OnGrappleHitCeiling()
    {
        // begin climbing
        _climbing = true;
        // turn off gravity so player can climb
        rbRef.useGravity = false;
        // zero player's downward momentum in case they were previously falling
        Vector3 velocity = rbRef.linearVelocity;
        velocity.y = 0f;
        rbRef.linearVelocity = velocity;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        // logic only happens when in a run
        if (GameManager.Instance.InRun)
        {
            Grapple();
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
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);
            }

            // if player collides with platform via a trigger, take damage and destroy platform 
            if (other.gameObject.CompareTag("Ceiling"))
            {
                // don't take damage if invincible
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);

                print("Player collided with platform");
            }
        }
    }

    // handles grapple behaviors
    private void Grapple()
    {
        // when player is climbing but hasn't reached the ceiling yet
        if (_climbing && !_onCeiling && inputtingGrapple)
        {
            // move up
            transform.Translate(_climbSpeed * Time.deltaTime * transform.up);

            // check if player has reached ceiling
            // detect an object above the player
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 1.2f))
            {
                // check if object is the ceiling and player isn't already on the ceiling and make sure they're inputting grapple
                if (hit.collider.transform.CompareTag("Ceiling") && inputtingGrapple)
                    _onCeiling = true;
            }
        }

        // if player stops inputting grapple or grapple is not on ceiling stop climbing and reactivate gravity
        if (!inputtingGrapple || !_grappleRef.onCeiling)
        {
            _onCeiling = false;
            _climbing = false;
            // don't turn gravity back on if player is boosting
            if (!boosting)
                rbRef.useGravity = true;
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

    // called when player inputs grapple
    private void OnGrapplePerformed(InputAction.CallbackContext context)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
        {
            // turn off inputs during boost
            if (!boosting)
                inputtingGrapple = true;
        }
        // player is not in run so start run upon pressing grapple
        else
            EventBus.Publish(EventType.RunStart);
    }

    // called when player stops inputting grapple
    private void OnGrappleCanceled(InputAction.CallbackContext context)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
            inputtingGrapple = false;
    }

    #endregion
}