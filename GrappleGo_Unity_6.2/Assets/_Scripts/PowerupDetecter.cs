using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/30/2025
/// Detects player's powerup pickups
/// </summary>

public class PowerupDetecter : MonoBehaviour
{
    // references to powerup scripts
    private PowerupParent _shieldPowerup, _boostPowerup, _dashPowerup, _gunPowerUp, _dynamitePowerUp;

    void OnEnable()
    {
        // get powerup references
        _shieldPowerup = this.GetComponent<ShieldPowerup>();
        _boostPowerup = this.GetComponent<BoostPowerup>();
    }

    // handles trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        // only do logic inside run
        if (GameManager.Instance.InRun)
        {
            // if collide with shield powerup activate it and then destroy the pickup object
            if (other.gameObject.CompareTag("ShieldPowerup"))
            {
                PickUpPowerup(_shieldPowerup);
                Destroy(other.gameObject);
            }
            // if collide with boost powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("BoostPowerup"))
            {
                // don't pick up another boost powerup if the player already has one
                if (!PlayerController.Instance.boosting)
                {
                    PickUpPowerup(_boostPowerup);
                    Destroy(other.gameObject);
                }
            }
            /* ///////////////////////////////////// Add these in when made corresponding powerup
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

    // apply given powerup
    private void PickUpPowerup(PowerupParent powerup)
    {
        if (powerup != null)
            powerup.enabled = true;
        // there is no powerup so throw error
        else
            Debug.LogError("ERROR: no powerup detected!!!");
    }
}