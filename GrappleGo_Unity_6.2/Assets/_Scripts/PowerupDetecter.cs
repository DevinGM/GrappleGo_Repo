using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2025
/// Detects player's powerup pickups
/// </summary>

public class PowerupDetecter : MonoBehaviour
{
    // is the player currently in a run?
    private bool _inRun = false;
    // references to powerup scripts
    private PowerupParent _shieldPowerup, _boostPowerUp, _dashPowerup, _gunPowerUp, _dynamitePowerUp;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // get powerup references
        _shieldPowerup = this.GetComponent<ShieldPowerup>();
    }
    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run starts
    private void StartRun()
    {
        _inRun = true;
    }

    // called when run ends
    private void EndRun()
    {
        _inRun = false;
    }

    // handles triger collisions
    private void OnTriggerEnter(Collider other)
    {
        // only do logic inside run
        if (_inRun)
        {
            // if collide with shield powerup activate it and then destroy the pickup object
            if (other.gameObject.CompareTag("ShieldPowerup"))
            {
                OnPowerupPickUp(_shieldPowerup);
                Destroy(other.gameObject);
            }
            /* ///////////////////////////////////// Add these in when made corresponding powerup
            // if collide with boost powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("BoostPowerup"))
            {
                OnPowerupPickUp(_boostPowerUp);
                Destroy(other.gameObject);
            }
            // if collide with dash powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("DashPowerup"))
            {
                OnPowerupPickUp(_dashPowerup);
                Destroy(other.gameObject);
            }
            // if collide with gun powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("GunPowerup"))
            {
                OnPowerupPickUp(_gunPowerUp);
                Destroy(other.gameObject);
            }
            // if collide with dynamite powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("DynamitePowerup"))
            {
                OnPowerupPickUp(_dynamitePowerUp);
                Destroy(other.gameObject);
            }*/
        }
    }

    // called when player picks up a powerup
    private void OnPowerupPickUp(PowerupParent powerup)
    {
        if (powerup != null)
            powerup.enabled = true;
        // there is no powerup so throw error
        else
            Debug.LogError("ERROR: no powerup detected!!!");
    }
}