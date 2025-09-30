using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/19/2025
/// Hold enemy interface
/// </summary>

public interface IEnemy
{
    // called when run ends
    void EndRun();
    // movement behaviour
    void Movement();
}