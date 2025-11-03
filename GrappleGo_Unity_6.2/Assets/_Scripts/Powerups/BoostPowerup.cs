using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/17/2024
/// handles boost powerup behavior
/// </summary>

public class BoostPowerup : PowerupParent
{
    // powerup duration
    [SerializeField] private float _duration = 10f;
    // amount speed increases with powerup
    [SerializeField] private float _speedBoost = 15f;

    // speed player is at at beginning of powerup
    private float _startingSpeed;
    // reference to boost model
    private GameObject _boostModelRef;


    protected override void OnEnable()
    {
        // get reference to boost model
        _boostModelRef = transform.Find("BoostModel").gameObject;

        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);

        // activate upon enabling
        Activate();
    }

    protected override void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);

        Deactivate();
    }

    // called when run ends
    protected override void OnRunEnd()
    {
        this.enabled = false;
    }

    protected override void Activate()
    {
        // set duration
        base.Duration = _duration + GameManager.Instance.boostDuration;

        // get starting speed
        _startingSpeed = GameManager.Instance.currentMoveSpeed;

        // increase world speed
        GameManager.Instance.currentMoveSpeed += _speedBoost;

        // set player invincible
        PlayerController.Instance.boosting = true;
        PlayerController.Instance.invincible = true;

        // turn gravity off so player doesn't fall through floor
        PlayerController.Instance.rbRef.useGravity = false;

        // zero velocity in case player was mid fall
        PlayerController.Instance.rbRef.linearVelocity = Vector3.zero;

        // set player collider to trigger so they don't collide with anything
        PlayerController.Instance.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

        // make sure the player isn't stuck in any grappling state
        PlayerController.Instance.inputtingGrapple = false;

        // turn on shield model
        _boostModelRef.SetActive(true);

        // begin disable duration
        StartCoroutine(AutoDisable());
    }

    // called upon powerup being deactivated
    protected override void Deactivate()
    {
        // reset world speed
        GameManager.Instance.currentMoveSpeed = _startingSpeed;

        // set player invincibility off
        PlayerController.Instance.boosting = false;
        PlayerController.Instance.invincible = false;

        // turn gravity back on
        PlayerController.Instance.rbRef.useGravity = true;

        // set collider back to NOT a trigger
        PlayerController.Instance.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

        // start damage cooldown so player doesn't immediately take damage after coming out of boost
        PlayerController.Instance.StartCoroutine(PlayerController.Instance.DamageCooldown());

        // turn off shield model
        _boostModelRef.SetActive(false);
    }
}