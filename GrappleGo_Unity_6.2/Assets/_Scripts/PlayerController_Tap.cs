using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Devin G Monaghan
/// 11/13/2025
/// HANDLES PLAYER BEHAVIOR
/// detects touch inputs
/// moves player to held touch on screen
/// moves grapple
/// handles damage
/// </summary>

public class PlayerController_Tap : SingletonNonPersist<PlayerController_Tap>
{
    // reference to Rigidbody
    public Rigidbody rbRef;

    // reference to main camera
    private Camera _camera;

    #region Movement Variables

    [Header("Reference to Grapple object\nUse 'Grapple_Tap' instance inside the scene")]
    // reference to grapple
    [SerializeField] private Transform _grapple;

    // is the player currently moving?
    public bool moving = false;

    // speed player is currently moving at
    private float _moveSpeed = 12f;
    // location player spawns at
    private Vector3 _spawnPos;
    private Vector3 _grappleSpawnPos;
    // position player grapples towards
    private Vector3 _targetPos;

    #endregion

    #region Damage Variables

    // number of player's lives, player dies when lives hits 0
    public int lives = 1;
    // is the player currently invincible?
    public bool invincible = false;

    // length of cooldown after taking damage
    private float _damageCooldownTime = .5f;
    // is the damge cooldown on?
    private bool _damageCooldown = false;

    #endregion

    #region Input Variables

    // references to inputs
    private PlayerInputs _playerInputs;
    public PlayerInputs PlayerInputs { get { return _playerInputs; } }
    private InputAction _tapGrappleAction;

    #endregion

    #region Powerup Variables

    // number of currently available dash charges
    public int DashCharges = 0;
    // number of currently available dynamite charges
    public int DynamiteCharges = 0;
    // is the player currently using the boost powerup?
    public bool usingBoost = false;
    // is the player currently using the gun powerup?
    public bool usingGun = false;
    // is the player currently in a dash?
    public bool inDash = false;

    #endregion

    /// functions

    #region OnEnable, & OnDisable

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Subscribe(EventType.GetDash, OnGetDash);
        EventBus.Subscribe(EventType.UseDash, OnUseDash);
        EventBus.Subscribe(EventType.GetDynamite, OnGetDynamite);
        EventBus.Subscribe(EventType.UseDynamite, OnUseDynamite);

        // get references
        _camera = Camera.main;
        rbRef = this.GetComponent<Rigidbody>();
        _spawnPos = transform.position;
        if (_grapple != null)
            _grappleSpawnPos = _grapple.position;
        else
            Debug.LogError("ERROR: No Grapple reference given to player");

        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        _tapGrappleAction = _playerInputs.Controls.TapGrapple;
        _tapGrappleAction.performed += OnTapGrapplePerformed;
        _tapGrappleAction.canceled += OnTapGrappleCancel;

        // get stats
        _moveSpeed = GameManager.Instance.playerMoveSpeed;
    }

    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Unsubscribe(EventType.GetDash, OnGetDash);
        EventBus.Unsubscribe(EventType.UseDash, OnUseDash);
        EventBus.Unsubscribe(EventType.GetDynamite, OnGetDynamite);
        EventBus.Unsubscribe(EventType.UseDynamite, OnUseDynamite);

        // disable inputs
        _playerInputs.Disable();
        _tapGrappleAction.performed -= OnTapGrapplePerformed;
        _tapGrappleAction.canceled -= OnTapGrappleCancel;
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
        // reset charges
        DashCharges = 0;
        DynamiteCharges = 0;
        // reset velocity
        rbRef.linearVelocity = Vector3.zero;
    }

    // add a dash charge
    private void OnGetDash()
    {
        DashCharges++;
        // make sure DashCharges never goes above max
        if (DashCharges > GameManager.Instance.maxDashCharges)
            DashCharges = GameManager.Instance.maxDashCharges;
    }

    // use a dash charge
    private void OnUseDash()
    {
        DashCharges--;
        // make sure DashCharges never goes below 0
        if (DashCharges < 0)
            DashCharges = 0;
    }

    // add a dynamite charge
    private void OnGetDynamite()
    {
        DynamiteCharges++;
        // make sure DynamiteCharges never goes above max
        if (DynamiteCharges > GameManager.Instance.maxDynamiteCharges)
            DynamiteCharges = GameManager.Instance.maxDynamiteCharges;
    }

    // use a dynamite charge
    private void OnUseDynamite()
    {
        DynamiteCharges--;
        // make sure DynamiteCharges never goes below 0
        if (DynamiteCharges < 0)
            DynamiteCharges = 0;
    }

    #endregion

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
    public IEnumerator DamageCooldown()
    {
        _damageCooldown = true;
        yield return new WaitForSeconds(_damageCooldownTime);
        _damageCooldown = false;
    }

    #endregion

    #region Inputs

    // called when player starts touching the screen
    // starts player moving and turns off gravity
    // if not in run, start run
    public void OnTapGrapplePerformed(InputAction.CallbackContext context)
    {
        // if not in run, start run
        if (!GameManager.Instance.InRun)
            EventBus.Publish(EventType.RunStart);
        // only do logic in run
        else
        {
            // make sure there is a touch screen
            if (Touchscreen.current == null)
            {
                Debug.LogError("ERROR: No touch screen detected");
                return;
            }

            // get position of tap
            Vector2 tapPos = Touchscreen.current.primaryTouch.position.ReadValue();
            
            // create a simulated pointer for the current Event System
            PointerEventData simPointer = new PointerEventData(EventSystem.current);
            // move simulated pointer to tap position
            simPointer.position = tapPos;
            // raycast to ui from position of simulated pointer via Event System
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(simPointer, hits);
            // if the raycast hits anything, abort this tap, otherwise good to go
            if (hits.Count > 0)
                return;

            // start moving
            moving = true;
            // reset velocity and turn off gravity
            // if player is dashing, don't reset velocity
            if (!inDash)
                rbRef.linearVelocity = Vector3.zero;
            rbRef.useGravity = false;

            // convert tap position to world position
            Vector3 targetPos = targetPos = _camera.ScreenToWorldPoint(tapPos);
            targetPos.z = 0f;
            // store world position of tap in _targetPos
            _targetPos = targetPos;
            // move grapple to target position
            targetPos.z = -2f;
            _grapple.position = targetPos;

            // play player grapples audio
            if (PlayerAudioHandler.Instance.playerGrapples != null)
                PlayerAudioHandler.Instance.PlaySound(PlayerAudioHandler.Instance.playerGrapples);
        }
    }

    // called when player stops touching the screen
    // stops player moving and turns on gravity
    // if not in run, start run
    public void OnTapGrappleCancel(InputAction.CallbackContext context)
    {
        // only do logic in run
        if (GameManager.Instance.InRun)
        {
            // stop moving
            moving = false;
            // turn on gravity
            if (!inDash)
                rbRef.useGravity = true;
        }
    }
    
    #endregion

    // Update is called once per frame
    private void Update()
    {
        // logic only happens when in a run
        if (GameManager.Instance.InRun)
        {
            // if player is moving, move
            if (moving)
                Move();
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

                // play get coin audio
                if (PlayerAudioHandler.Instance.getCoin != null)
                    PlayerAudioHandler.Instance.PlaySound(PlayerAudioHandler.Instance.getCoin);
            }

            // if player collides with an obstacle or platform take damage and destroy obstacle
            if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Platform"))
            {
                // don't take damage if invincible
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);
            }

            // if player collides with an enemy take damage and destroy enemy
            if (other.gameObject.CompareTag("Enemy"))
            {
                // don't take damage if invincible
                if (!invincible)
                    TakeDamage();
                Destroy(other.gameObject);

                // play enemy death sound
                if (EnemyAudioHandler.Instance.enemyDeath != null)
                    EnemyAudioHandler.Instance.PlaySound(EnemyAudioHandler.Instance.enemyDeath);
            }
        }
    }
}