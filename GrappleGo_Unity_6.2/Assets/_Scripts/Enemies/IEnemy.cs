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
    // should this enemy die?
    public bool Dead { get; set; }

    // movement behaviour
    void Movement();
}