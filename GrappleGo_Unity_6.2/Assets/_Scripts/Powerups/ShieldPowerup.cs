using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2024
/// handles shield powerup behavior
/// </summary>

public class ShieldPowerup : PowerupParent
{
    // reference to shield model
    private GameObject _shieldModelRef;

    protected override void OnEnable()
    {
        // get reference to shield model
        _shieldModelRef = transform.Find("ShieldModel").gameObject;

        // subscribe to events
        EventBus.Subscribe(EventType.PlayerDamaged, OnPlayerDamaged);

        // activate upon enabling
        Activate();
    }
    protected override void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.PlayerDamaged, OnPlayerDamaged);
    }

    protected override void Activate()
    {
        // mark powerup as active
        _active = true;
        // increase player lives by 1
        PlayerController.Instance.lives++;
        // turn on shield model
        _shieldModelRef.SetActive(true);
    }
    protected override void Deactivate()
    {
        // mark powerup as inactive
        _active = false;
        // turn off shield model
        _shieldModelRef.SetActive(false);
        // disable powerup component
        this.enabled = false;
    }

    // called when player takes damage
    private void OnPlayerDamaged()
    {
        // check if player is on last life, in which case deactivate shield
        if (PlayerController.Instance.lives == 1)
            Deactivate();
    }
}