using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2024
/// handles boost powerup behavior
/// </summary>

public class BoostPowerup : PowerupParent
{
    // boost duration
    [SerializeField] private float _duration = 10f;
    [SerializeField] private float _speedBoost = 10f;

    // speed player is at at beginning of powerup
    private float _startingSpeed;
    // reference to boost model
    private GameObject _boostModelRef;

    protected override void OnEnable()
    {
        // get reference to shield model
        _boostModelRef = transform.Find("BoostModel").gameObject;
        // set duration
        Duration = _duration;

        // activate upon enabling
        Activate();
    }
    protected override void OnDisable()
    {
    }

    protected override void Activate()
    {
        // mark powerup as active
        _active = true;
        // get starting speed
        _startingSpeed = PlayerController.Instance.MoveSpeed;
        // increase player speed
        PlayerController.Instance.MoveSpeed += _speedBoost;
        // set player invincible
        PlayerController.Instance.Invincible = true;
        // turn on shield model
        _boostModelRef.SetActive(true);
        // begin disable duration
        StartCoroutine(AutoDisable());
    }

    protected override void Deactivate()
    {
        // mark powerup as inactive
        _active = false;
        // reset player speed
        PlayerController.Instance.MoveSpeed = _startingSpeed;
        // set player invincibility off
        PlayerController.Instance.Invincible = false;
        // turn off shield model
        _boostModelRef.SetActive(false);
        // disable powerup component
        this.enabled = false;
    }
}