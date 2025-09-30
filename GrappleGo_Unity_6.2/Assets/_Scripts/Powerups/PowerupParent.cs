using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2024
/// holds parent powerup outline
/// </summary>

public abstract class PowerupParent : MonoBehaviour
{
    // preset duration of time before powerup is disabled
    public float Duration { get; protected set; }
    // is the powerup currently active
    protected bool _active = false;

    protected abstract void OnEnable();
    protected abstract void OnDisable();
    // called upon powerup being activated
    protected abstract void Activate();
    // called upon powerup being disactivated
    protected abstract void Deactivate();

    // preset disable timer
    protected virtual IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(Duration);
        Deactivate();
    }
}