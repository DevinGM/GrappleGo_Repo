using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/7/2025
/// Detects player's powerup pickups
/// </summary>

public class PowerupDetecter : MonoBehaviour
{
    // references to powerups
    private PowerupParent _shieldPowerup, _boostPowerup, _dashPowerup, _gunPowerup;
    private PowerupChargeParent _dynamitePowerup;

    void OnEnable()
    {
        // get powerup references
        _shieldPowerup = this.GetComponent<ShieldPowerup>();
        _boostPowerup = this.GetComponent<BoostPowerup>();
        _dashPowerup = this.GetComponent<DashPowerup>();
        _gunPowerup = this.GetComponent<GunPowerup>();
        _dynamitePowerup = this.GetComponent<DynamitePowerup>();
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
                // don't pick up boost if player already has one
                if (!PlayerController_Tap.Instance.usingBoost)
                {
                    PickUpPowerup(_boostPowerup);
                    Destroy(other.gameObject);
                }
            }
            // if collide with dash powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("DashPowerup"))
            {
                // don't pick up dash if player already has one
                if (!PlayerController_Tap.Instance.hasDash)
                {
                    PickUpPowerup(_dashPowerup);
                    Destroy(other.gameObject);
                }
            }
            
            // if collide with gun powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("GunPowerup"))
            {
                // don't pick up gun if player already has one
                if (!PlayerController_Tap.Instance.usingGun)
                {
                    PickUpPowerup(_gunPowerup);
                    Destroy(other.gameObject);
                }
            }
            // if collide with dynamite powerup activate it and then destroy the pickup object
            else if (other.gameObject.CompareTag("DynamitePowerup"))
            {
                // don't pick up another dynamite if charges are at max
                if (PlayerController_Tap.Instance.DynamiteCharges < GameManager.Instance.maxDynamiteCharges)
                {
                    PickUpPowerup(_dynamitePowerup);
                    Destroy(other.gameObject);
                }
                else
                    print("player is at max dynamite charges");
            }
        }
    }

    // apply given powerup
    private void PickUpPowerup(PowerupParent powerup)
    {
        if (powerup != null)
        {
            powerup.enabled = true;

            // play powerup get audio
            if (PlayerAudioHandler.Instance.powerupGet_A != null)
                PlayerAudioHandler.Instance.PlaySound(PlayerAudioHandler.Instance.powerupGet_A);
        }
        // there is no powerup so throw error
        else
            Debug.LogError("ERROR: no powerup detected!!!");
    }
    // apply given powerup charge
    private void PickUpPowerup(PowerupChargeParent powerup)
    {
        if (powerup != null)
        {
            powerup.Pickup();

            // play powerup get audio
            if (PlayerAudioHandler.Instance.powerupGet_A != null)
                PlayerAudioHandler.Instance.PlaySound(PlayerAudioHandler.Instance.powerupGet_A);
        }
        // there is no powerup so throw error
        else
            Debug.LogError("ERROR: no powerup charge detected!!!");
    }
}