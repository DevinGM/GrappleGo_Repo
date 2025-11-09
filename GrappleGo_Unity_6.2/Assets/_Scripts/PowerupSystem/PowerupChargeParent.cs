using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/7/2025
/// holds parent charge powerup outline
/// </summary>

public abstract class PowerupChargeParent : MonoBehaviour
{
    protected abstract void OnEnable();
    protected abstract void OnDisable();
    // called upon powerup being activated
    public abstract void Pickup();
    // called upon powerup being disactivated
    protected abstract void Use(InputAction.CallbackContext context);
}