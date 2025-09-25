using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/19/2025
/// Hold enemies' interface
/// </summary>

public interface IEnemy
{
    // is the player currently in a run?
    public bool InRun { get; }

    // called when run begins
    void StartRun();
    // called when run ends
    void EndRun();
    // movement behaviour
    void Movement();
}