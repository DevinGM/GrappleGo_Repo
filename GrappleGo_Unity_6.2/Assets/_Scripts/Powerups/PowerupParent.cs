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

    protected abstract void OnEnable();
    protected abstract void OnDisable();
    // called on run end
    protected abstract void OnRunEnd();
    // called upon powerup being activated
    protected abstract void Activate();
    // called upon powerup being disactivated
    protected abstract void Deactivate();

    // preset disable timer
    protected virtual IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(Duration);
        // disable powerup component
        this.enabled = false;
    }
}