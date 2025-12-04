using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/11/2024
/// handles dash powerup behavior
/// </summary>

public class DashPowerup : PowerupChargeParent
{
    // time in seconds of dash invincibilty
    [SerializeField] private float _dashLength = .5f;
    // strength of dash
    [SerializeField] private float _dashStrength = 5f;

    // is the dash currently on cooldown?
    private float _cooldownLength = .5f;
    private bool _cooldown = false;
    // references to inputs
    private InputAction _dashAction;
    // reference to in dash model
    private GameObject _dashModelRef;

    protected override void OnEnable()
    {
        // add inputs
        StartCoroutine(AddInputs());

        // get reference to in dash model
        _dashModelRef = transform.Find("InDashModel").gameObject;
    }

    // wait one frame to add inputs to make sure player has correct reference
    private IEnumerator AddInputs()
    {
        yield return null;
        _dashAction = PlayerController_Tap.Instance.PlayerInputs.Controls.Dash;
        _dashAction.performed += Use;
    }

    protected override void OnDisable()
    {
        // disable inputs
        _dashAction.performed -= Use;
    }

    // called upon powerup being picked up
    public override void Pickup()
    {
        // if dash tutorial has not been played, play it
        if (!GameManager.Instance.playedDashTutorial)
        {
            GameManager.Instance.playedDashTutorial = true;
            EventBus.Publish(EventType.PlayDashTutorial);
        }

        // publish GetDash
        EventBus.Publish(EventType.GetDash);
    }

    // trigger dash on button press
    protected override void Use(InputAction.CallbackContext context)
    {
        if (!_cooldown && PlayerController_Tap.Instance.DashCharges > 0)
            StartCoroutine(Dash());
    }

    // perform dash
    private IEnumerator Dash()
    {
        // turn on cooldown
        _cooldown = true;

        // spawn explosion by turning it on
        _dashModelRef.SetActive(true);

        // publish UseDash
        EventBus.Publish(EventType.UseDash);
        // set player invincible
        PlayerController_Tap.Instance.invincible = true;
        PlayerController_Tap.Instance.inDash = true;
        // turn on dash model
        _dashModelRef.SetActive(true);

        // reset velocity
        PlayerController_Tap.Instance.rbRef.linearVelocity = Vector3.zero;
        // turn off gravity for dash
        PlayerController_Tap.Instance.rbRef.useGravity = false;
        // apply force right
        PlayerController_Tap.Instance.rbRef.AddForce(_dashStrength * transform.right, ForceMode.Impulse);

        // wait _dashLength seconds before deactivating dash
        yield return new WaitForSeconds(_dashLength);

        PlayerController_Tap.Instance.inDash = false;
        // turn off dash model
        _dashModelRef.SetActive(false);

        // if player is not moving, then make sure gravity is on
        if (!PlayerController_Tap.Instance.moving)
            PlayerController_Tap.Instance.rbRef.useGravity = true;
        PlayerController_Tap.Instance.rbRef.linearVelocity = Vector3.zero;

        // wait _cooldownLength before turning cooldown off and letting player dash again
        yield return new WaitForSeconds(_cooldownLength);

        // set player invincibility off
        PlayerController_Tap.Instance.invincible = false;
        // turn off cooldown
        _cooldown = false;
    }
}