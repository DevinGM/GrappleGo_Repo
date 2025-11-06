using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2024
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
        PlayerController_Tap.Instance.boosting = true;
        PlayerController_Tap.Instance.invincible = true;

        // set player collider to trigger so they don't collide with anything
        PlayerController_Tap.Instance.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

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
        PlayerController_Tap.Instance.boosting = false;
        PlayerController_Tap.Instance.invincible = false;

        // set collider back to NOT a trigger
        PlayerController_Tap.Instance.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

        // start damage cooldown so player doesn't immediately take damage after coming out of boost
        PlayerController_Tap.Instance.StartCoroutine(PlayerController_Tap.Instance.DamageCooldown());

        // turn off shield model
        _boostModelRef.SetActive(false);
    }
}