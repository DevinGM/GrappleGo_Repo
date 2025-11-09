using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/5/2024
/// handles dash powerup behavior
/// </summary>

public class DashPowerup : PowerupParent
{
    // powerup duration
    [SerializeField] private float _duration = 7.5f;
    // time in seconds of dash invincibilty
    [SerializeField] private float _dashLength = .5f;

    // is the player currently dashing?
    private bool _inDash = false;
    // references to inputs
    private PlayerInputs _playerInputs;
    private InputAction _dashAction;
    // reference to in dash model
    private GameObject _inDashModelRef;
    // reference to have dash model
    private GameObject _haveDashModelRef;

    protected override void OnEnable()
    {
        // add inputs
        _playerInputs = new PlayerInputs();
        _dashAction = _playerInputs.Controls.Dash;
        _dashAction.performed += OnDashPerformed;

        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);

        // get reference to in dash model
        _inDashModelRef = transform.Find("InDashModel").gameObject;
        // get reference to have dash model
        _haveDashModelRef = transform.Find("HaveDashModel").gameObject;

        // activate upon enabling
        Activate();
    }

    protected override void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
        _dashAction.performed -= OnDashPerformed;

        Deactivate();
    }

    // called when run ends
    protected override void OnRunEnd()
    {
        this.enabled = false;
    }

    // called upon powerup being activated
    protected override void Activate()
    {
        // publish DashStart
        EventBus.Publish(EventType.DashStart);

        // enable inputs
        _playerInputs.Enable();

        // set duration
        base.Duration = _duration + GameManager.Instance.dashDuration;

        // turn on have dash model
        _haveDashModelRef.SetActive(true);

        // begin disable timer
        StartCoroutine(AutoDisable());
    }

    // called upon powerup being deactivated
    protected override void Deactivate()
    {
        // publish DashEnd
        EventBus.Publish(EventType.DashEnd);

        // disable inputs
        _playerInputs.Disable();

        // turn off have dash model
        _haveDashModelRef.SetActive(false);
    }

    // trigger dash on button press
    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (!_inDash)
            StartCoroutine(Dash());
    }

    // perform dash
    private IEnumerator Dash()
    {
        // turn on cooldown
        _inDash = true;
        // turn on in dash model
        _inDashModelRef.SetActive(true);
        // publish dash
        EventBus.Publish(EventType.DashPerformed);
        // set player invincible
        PlayerController_Tap.Instance.invincible = true;

        // wait _dashLength seconds
        yield return new WaitForSeconds(_dashLength);

        // set player invincibility off
        PlayerController_Tap.Instance.invincible = false;
        // turn off dash model
        _inDashModelRef.SetActive(false);
        // turn off cooldown
        _inDash = false;
    }
}