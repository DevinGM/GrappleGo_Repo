using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/14/2024
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
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Subscribe(EventType.PlayerDamaged, OnPlayerDamaged);

        // activate upon enabling
        Activate();
    }

    protected override void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
        EventBus.Unsubscribe(EventType.PlayerDamaged, OnPlayerDamaged);

        Deactivate();
    }

    // called when run ends
    protected override void OnRunEnd()
    {
        this.enabled = false;
    }

    protected override void Activate()
    {
        // increase player lives by 1
        PlayerController_Tap.Instance.lives++;
        // turn on shield model
        _shieldModelRef.SetActive(true);
    }

    protected override void Deactivate()
    {
        // turn off shield model
        _shieldModelRef.SetActive(false);
    }

    // called when player takes damage
    private void OnPlayerDamaged()
    {
        // check if player is on last life, in which case deactivate shield
        if (PlayerController_Tap.Instance.lives == 1)
            this.enabled = false;
    }
}